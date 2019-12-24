using System;
using System.IO;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;
using SharpBoot.AdminProcess.Properties;

namespace SharpBoot.AdminProcess
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 2) return;

            var type = args[0].ToLower();
            var letter = args[1].ToUpper();

            switch (type)
            {
                case "syslinux":
                    WriteMBR(letter, Resources.syslinux_mbr);
                    break;

                case "grub4dos":
                    WriteMBR(letter, Resources.g4d_grldr);
                    break;

                case "grub2":
                    WriteMBR(letter, Resources.grub2_boot, Resources.grub2_core);
                    break;
            }
        }

        public const uint GENERIC_READ = 0x80000000;
        public const uint GENERIC_WRITE = 0x40000000;
        public const int FILE_SHARE_READ = 1;
        public const int FILE_SHARE_WRITE = 2;
        public const int OPEN_EXISTING = 3;
        public const uint FSCTL_LOCK_VOLUME = 0x00090018;
        public const uint FSCTL_UNLOCK_VOLUME = 0x0009001c;
        public const uint FSCTL_DISMOUNT_VOLUME = 0x00090020;

        public static void WriteMBR(string l, byte[] mbr, byte[] extra = null)
        {
            GetDiskFreeSpace(l, out _, out var sectorSize, out _, out _);

            var path = GetPhysicalPath(l.ToLower().Substring(0, 2));
            Console.WriteLine($"{l} => {path}");

            using (var volume = CreateFile(@"\\.\" + l,
                GENERIC_READ | GENERIC_WRITE,
                FILE_SHARE_READ | FILE_SHARE_WRITE, IntPtr.Zero, OPEN_EXISTING,
                FILE_FLAG_NO_BUFFERING, IntPtr.Zero))
            {
                if (volume.IsInvalid)
                {
                    Console.WriteLine("Failed to open volume");
                    Environment.Exit(1);
                }

                if (!Attempt(() => DeviceIoControl(volume, FSCTL_DISMOUNT_VOLUME, null, 0, null,
                    0, out _, IntPtr.Zero)))
                {
                    Console.WriteLine("Failed to dismount volume");
                    Environment.Exit(1);
                }

                if (!Attempt(() => DeviceIoControl(volume, FSCTL_LOCK_VOLUME, null, 0, null, 0, out _,
                    IntPtr.Zero)))
                {
                    Console.WriteLine("Failed to lock device");
                    Environment.Exit(1);
                }

                using (var drive = CreateFile(path,
                    GENERIC_READ | GENERIC_WRITE,
                    FILE_SHARE_READ | FILE_SHARE_WRITE, IntPtr.Zero, OPEN_EXISTING,
                    FILE_FLAG_NO_BUFFERING, IntPtr.Zero))

                {
                    if (drive.IsInvalid)
                    {
                        Console.WriteLine("Failed to open device");
                        Environment.Exit(1);
                    }

                    Console.WriteLine($"Writing MBR");

                    using (var src = new FileStream(drive, FileAccess.ReadWrite))
                    {
                        WriteAtPos(src, 0, mbr.Take(432).ToArray(), sectorSize);

                        if (extra != null)
                            WriteAtPos(src, 512, extra, sectorSize);
                    }

                    Console.WriteLine("Writing done");
                }

                DeviceIoControl(volume, FSCTL_UNLOCK_VOLUME, null, 0, null, 0, out _, IntPtr.Zero);
            }
        }

        public static bool Attempt(Func<bool> func, int n = 5)
        {
            for (var i = 0; i < n; i++)
            {
                if (func())
                    return true;
            }

            return false;
        }

        public static void WriteAtPos(FileStream src, int pos, byte[] buf, uint SECTOR_SIZE)
        {
            var startSector = pos / SECTOR_SIZE;
            var endSector = (pos + buf.Length + SECTOR_SIZE - 1) / SECTOR_SIZE;
            var num = endSector - startSector;

            var tmp = new byte[num * SECTOR_SIZE];

            var begin = startSector * SECTOR_SIZE;

            src.Position = begin;
            src.Read(tmp, 0, tmp.Length);

            Array.Copy(buf, 0, tmp, pos - startSector * SECTOR_SIZE, buf.Length);

            src.Position = begin;
            src.Write(tmp, 0, tmp.Length);
        }

        public static string GetPhysicalPath(string letter)
        {
            letter = letter.Substring(0, 2);

            foreach (var partition in new ManagementObjectSearcher(
                $"ASSOCIATORS OF {{Win32_LogicalDisk.DeviceID='{letter}'}} WHERE AssocClass = Win32_LogicalDiskToPartition").Get())
            {
                foreach (var drive in new ManagementObjectSearcher($"ASSOCIATORS OF {{Win32_DiskPartition.DeviceID='{partition["DeviceID"]}'}} WHERE AssocClass = Win32_DiskDriveToDiskPartition").Get())
                    return drive["DeviceID"].ToString();
            }

            throw new DriveNotFoundException();
        }

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern bool GetDiskFreeSpace(string lpRootPathName,
            out uint lpSectorsPerCluster,
            out uint lpBytesPerSector,
            out uint lpNumberOfFreeClusters,
            out uint lpTotalNumberOfClusters);

        [DllImport("kernel32.dll", ExactSpelling = true, SetLastError = true)]
        static extern bool DeviceIoControl(SafeFileHandle hDevice, uint dwIoControlCode, byte[] lpInBuffer, int nInBufferSize, byte[] lpOutBuffer, int nOutBufferSize, out int lpBytesReturned, IntPtr lpOverlapped);

        public const int FILE_FLAG_NO_BUFFERING = 0x20000000;

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        static extern SafeFileHandle CreateFile(string lpFileName, uint dwDesiredAccess, uint dwShareMode, IntPtr lpSecurityAttributes, uint dwCreationDisposition, uint dwFlagsAndAttributes, IntPtr hTemplateFile);
    }
}
