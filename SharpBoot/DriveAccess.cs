using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace SharpBoot
{
    public class DriveAccess : IDisposable
    {
        #region imports from kernel32.dll

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern SafeFileHandle CreateFile(string lpFileName, uint dwDesiredAccess, uint dwShareMode,
            IntPtr pSecurityAttributes, uint dwCreationDisposition, uint dwFlagsAndAttributes,
            IntPtr hTemplateFile);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern uint QueryDosDevice(string DeviceName, IntPtr TargetPath, uint ucchMax);

        [DllImport("kernel32.dll", ExactSpelling = true, SetLastError = true, CharSet = CharSet.Auto)]
        private static extern bool DeviceIoControl(IntPtr hDevice, uint dwIoControlCode, IntPtr lpInBuffer,
            uint nInBufferSize,
            IntPtr lpOutBuffer, uint nOutBufferSize, out uint lpBytesReturned, IntPtr lpOverlapped);

        #endregion

        #region protected members

        protected SafeFileHandle driveHandle;
        public FileStream driveStream;
        protected DriveGeometry driveGeometry;

        #endregion

        #region public members

        public enum MediaType
        {
            Unknown,
            F5_1Pt2_512,
            F3_1Pt44_512,
            F3_2Pt88_512,
            F3_20Pt8_512,
            F3_720_512,
            F5_360_512,
            F5_320_512,
            F5_320_1024,
            F5_180_512,
            F5_160_512,
            RemovableMedia,
            FixedMedia,
            F3_120M_512,
            F3_640_512,
            F5_640_512,
            F5_720_512,
            F3_1Pt2_512,
            F3_1Pt23_1024,
            F5_1Pt23_1024,
            F3_128Mb_512,
            F3_230Mb_512,
            F8_256_128,
            F3_200Mb_512,
            F3_240M_512,
            F3_32M_512
        }

        public class DriveGeometry
        {
            public unsafe DriveGeometry(byte* fromBuffer)
            {
                Cylinders = getUint64(fromBuffer, 0);
                Media = (MediaType) getUint32(fromBuffer, 8);
                TracksPerCylinder = getUint32(fromBuffer, 12);
                SectorsPerTrack = getUint32(fromBuffer, 16);
                BytesPerSector = getUint32(fromBuffer, 20);
            }

            public MediaType Media { get; }

            public uint TracksPerCylinder { get; }

            public uint SectorsPerTrack { get; }

            public uint BytesPerSector { get; }

            public ulong Cylinders { get; }

            public ulong TotalSectors => Cylinders * TracksPerCylinder * SectorsPerTrack;

            private static unsafe uint getUint32(byte* buf, int offset)
            {
                uint res = 0;
                for (var i = 0; i < 4; i++)
                    res += buf[i + offset] * (uint) Math.Pow(256, i);
                return res;
            }

            private static unsafe ulong getUint64(byte* buf, int offset)
            {
                ulong res = 0;
                for (var i = 0; i < 8; i++)
                    res += buf[i + offset] * (uint) Math.Pow(256, i);
                return res;
            }
        }

        public DriveGeometry Geometry => driveGeometry;

        /// <summary>
        ///     Returns a list of all Drives on the mahine
        /// </summary>
        /// <param name="DeviceName">Can be null</param>
        /// <returns>List of all Drives</returns>
        public static List<string> GetAllDrives(string DeviceName)
        {
            const int maxSize = 60000;
            var auxBuffer = Marshal.AllocHGlobal(maxSize);
            var result = QueryDosDevice(DeviceName, auxBuffer, maxSize);
            if (result == 0)
            {
                Marshal.FreeHGlobal(auxBuffer);
                throw new Exception("Win 32 Exception : 0x" +
                                    Convert.ToString(Marshal.GetHRForLastWin32Error(), 16).PadLeft(8, '0'));
            }

            var retDrives = new List<string>();
            unsafe
            {
                var startPtr = (byte*) auxBuffer.ToPointer();
                var aux = "";
                for (var i = 0; i < maxSize; i++)
                    if (startPtr[i] == 0)
                    {
                        if (aux.StartsWith("PhysicalDrive") || aux.StartsWith("CdRom") ||
                            aux.Length == 2 && aux[1] == ':')
                            retDrives.Add(aux);
                        aux = "";
                        if (startPtr[i + 1] == 0) break;
                    }
                    else
                    {
                        aux += Convert.ToChar(startPtr[i]);
                    }
            }

            Marshal.FreeHGlobal(auxBuffer);
            retDrives.Sort(delegate(string a, string b)
            {
                if (a.Length == b.Length) return a.CompareTo(b);
                return a.Length.CompareTo(b.Length);
            });
            return retDrives;
        }

        /// <summary>
        ///     Reads a number of sectors from the opened drive
        /// </summary>
        /// <param name="startSector">Address of the start sector</param>
        /// <param name="sectorCount">Number of sectors to read</param>
        /// <param name="Buffer">The buffer to put data in</param>
        /// <param name="offset">Offset of data ub the buffer</param>
        /// <returns>Number of sectors read</returns>
        public int ReadSectors(ulong startSector, ulong sectorCount, byte[] Buffer, int offset)
        {
            if (sectorCount == 0) return 0;
            if (startSector + sectorCount > driveGeometry.TotalSectors) return 0;

            if ((ulong) driveStream.Position != startSector * driveGeometry.BytesPerSector)
                driveStream.Seek((long) (startSector * driveGeometry.BytesPerSector), SeekOrigin.Begin);

            var count = driveStream.Read(Buffer, offset, (int) (sectorCount * driveGeometry.BytesPerSector));
            return count / (int) driveGeometry.BytesPerSector;
        }

        /// <summary>
        ///     Writes a number of sectors on the opened drive (Untested!!!)
        /// </summary>
        /// <param name="startSector">Address of the start sector</param>
        /// <param name="sectorCount">Number of sectors to write</param>
        /// <param name="Buffer">The buffer the data is taken from</param>
        /// <param name="offset">Offset of data in the buffer</param>
        public void WriteSectors(ulong startSector, ulong sectorCount, byte[] Buffer, int offset)
        {
            if (sectorCount == 0) return;
            if (startSector + sectorCount > driveGeometry.TotalSectors) return;

            if ((ulong) driveStream.Position != startSector * driveGeometry.BytesPerSector)
                driveStream.Seek((long) (startSector * driveGeometry.BytesPerSector), SeekOrigin.Begin);

            driveStream.Write(Buffer, offset, (int) (sectorCount * driveGeometry.BytesPerSector));
        }

        public DriveAccess(string Path)
        {
            if (Path == null || Path.Length == 0)
                throw new ArgumentNullException("Path");

            driveHandle = CreateFile(Path, 0xC0000000, 0x03, IntPtr.Zero, 0x03, 0x80, IntPtr.Zero);
            if (driveHandle.IsInvalid)
            {
                driveHandle.Close();
                driveHandle.Dispose();
                driveHandle = null;
                throw new Exception("Win32 Exception : 0x" +
                                    Convert.ToString(Marshal.GetHRForLastWin32Error(), 16).PadLeft(8, '0'));
            }

            driveStream = new FileStream(driveHandle, FileAccess.ReadWrite);

            var p = Marshal.AllocHGlobal(24);
            uint returned;
            if (DeviceIoControl(driveHandle.DangerousGetHandle(), 0x00070000, IntPtr.Zero, 0, p, 40, out returned,
                IntPtr.Zero))
            {
                unsafe
                {
                    driveGeometry = new DriveGeometry((byte*) p.ToPointer());
                }
            }
            else
            {
                Marshal.FreeHGlobal(p);
                throw new Exception("Could not get the drive geometry information!");
            }

            Marshal.FreeHGlobal(p);
        }

        public void Dispose()
        {
            if (driveHandle != null)
            {
                if (driveStream != null)
                {
                    driveStream.Close();
                    driveStream.Dispose();
                }

                if (!driveHandle.IsClosed) driveHandle.Close();
                driveHandle.Dispose();
                driveHandle = null;
            }
        }

        #endregion
    }
}