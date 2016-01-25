using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Win32.SafeHandles;

namespace SharpBoot
{
    public class DiskStream : Stream
    {
        public const int DEFAULT_SECTOR_SIZE = 512;
        private const int BUFFER_SIZE = 4096;

        private string diskID;
        private FileAccess desiredAccess;
        private SafeFileHandle fileHandle;


        public DiskStream(string diskID, FileAccess desiredAccess)
        {
            this.diskID = diskID;
            this.desiredAccess = desiredAccess;

            // if desiredAccess is Write or Read/Write
            //   find volumes on this disk
            //   lock the volumes using FSCTL_LOCK_VOLUME
            //     unlock the volumes on Close() or in destructor


            this.fileHandle = this.openFile(diskID, desiredAccess);
        }

        private SafeFileHandle openFile(string id, FileAccess desiredAccess)
        {
            uint access;
            switch (desiredAccess)
            {
                case FileAccess.Read:
                    access = DeviceIO.GENERIC_READ;
                    break;
                case FileAccess.Write:
                    access = DeviceIO.GENERIC_WRITE;
                    break;
                case FileAccess.ReadWrite:
                    access = DeviceIO.GENERIC_READ | DeviceIO.GENERIC_WRITE;
                    break;
                default:
                    access = DeviceIO.GENERIC_READ;
                    break;
            }

            SafeFileHandle ptr = DeviceIO.CreateFile(
                id,
                access,
                DeviceIO.FILE_SHARE_WRITE,
                IntPtr.Zero,
                DeviceIO.OPEN_EXISTING,
                DeviceIO.FILE_FLAG_WRITE_THROUGH,
                IntPtr.Zero);

            if (ptr.IsInvalid)
            {
                Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
            }

            return ptr;
        }

        public override bool CanRead
        {
            get
            {
                return (this.desiredAccess == FileAccess.Read || this.desiredAccess == FileAccess.ReadWrite) ? true : false;
            }
        }
        public override bool CanWrite
        {
            get
            {
                return (this.desiredAccess == FileAccess.Write || this.desiredAccess == FileAccess.ReadWrite) ? true : false;
            }
        }

        /// <summary>
        /// En cas de substitution dans une classe dérivée, obtient la longueur du flux en octets. 
        /// </summary>
        /// <returns>
        /// Longue valeur représentant la longueur du flux en octets.
        /// </returns>
        /// <exception cref="T:System.NotSupportedException">Une classe dérivée de Stream ne prend pas en charge la recherche. </exception><exception cref="T:System.ObjectDisposedException">Des méthodes ont été appelées après que le flux a été fermé. </exception>
        public override long Length { get; }

        public override bool CanSeek
        {
            get
            {
                return true;
            }
        }

        public override long Position
        {
            get
            {
                return (long)PositionU;
            }
            set
            {
                PositionU = (ulong)value;
            }
        }

        public ulong PositionU
        {
            get
            {
                ulong n = 0;
                if (!DeviceIO.SetFilePointerEx(this.fileHandle, 0, out n, (uint)SeekOrigin.Current))
                    Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
                return n;
            }
            set
            {
                /*if (value > (this.LengthU - 1))
                    throw new EndOfStreamException("Cannot set position beyond the end of the disk.");*/

                ulong n = 0;
                if (!DeviceIO.SetFilePointerEx(this.fileHandle, value, out n, (uint)SeekOrigin.Begin))
                    Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
            }
        }

        public override void Flush()
        {
            // not required, since FILE_FLAG_WRITE_THROUGH and FILE_FLAG_NO_BUFFERING are used
            //if (!Unmanaged.FlushFileBuffers(this.fileHandle))
            //    Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
        }
        public override void Close()
        {
            if (this.fileHandle != null)
            {
                DeviceIO.CloseHandle(this.fileHandle);
                this.fileHandle.SetHandleAsInvalid();
                this.fileHandle = null;
            }
            base.Close();
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException("Setting the length is not supported with DiskStream objects.");
        }
        public override int Read(byte[] buffer, int offset, int count)
        {
            return (int)Read(buffer, (uint)offset, (uint)count);
        }

        public unsafe uint Read(byte[] buffer, uint offset, uint count)
        {
            uint n = 0;
            fixed (byte* p = buffer)
            {
                if (!DeviceIO.ReadFile(this.fileHandle, p + offset, count, &n, IntPtr.Zero))
                    Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
            }
            return n;
        }
        public override void Write(byte[] buffer, int offset, int count)
        {
            Write(buffer, (uint)offset, (uint)count);
        }
        public unsafe void Write(byte[] buffer, uint offset, uint count)
        {
            uint n = 0;
            fixed (byte* p = buffer)
            {
                if (!DeviceIO.WriteFile(this.fileHandle, p + offset, count, &n, IntPtr.Zero))
                    Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
            }
        }
        public override long Seek(long offset, SeekOrigin origin)
        {
            return (long)SeekU((ulong)offset, origin);
        }
        public ulong SeekU(ulong offset, SeekOrigin origin)
        {
            ulong n = 0;
            if (!DeviceIO.SetFilePointerEx(this.fileHandle, offset, out n, (uint)origin))
                Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
            return n;
        }
    }

    /// <summary>
    /// P/Invoke wrappers around Win32 functions and constants.
    /// </summary>
    internal partial class DeviceIO
    {

        #region Constants used in unmanaged functions

        public const uint FILE_SHARE_READ = 0x00000001;
        public const uint FILE_SHARE_WRITE = 0x00000002;
        public const uint FILE_SHARE_DELETE = 0x00000004;
        public const uint OPEN_EXISTING = 3;

        public const uint GENERIC_READ = (0x80000000);
        public const uint GENERIC_WRITE = (0x40000000);

        public const uint FILE_FLAG_NO_BUFFERING = 0x20000000;
        public const uint FILE_FLAG_WRITE_THROUGH = 0x80000000;
        public const uint FILE_READ_ATTRIBUTES = (0x0080);
        public const uint FILE_WRITE_ATTRIBUTES = 0x0100;
        public const uint ERROR_INSUFFICIENT_BUFFER = 122;

        #endregion

        #region Unamanged function declarations

        [DllImport("kernel32.dll", SetLastError = true)]
        public static unsafe extern SafeFileHandle CreateFile(
            string FileName,
            uint DesiredAccess,
            uint ShareMode,
            IntPtr SecurityAttributes,
            uint CreationDisposition,
            uint FlagsAndAttributes,
            IntPtr hTemplateFile);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool CloseHandle(SafeFileHandle hHandle);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool DeviceIoControl(
            SafeFileHandle hDevice,
            uint dwIoControlCode,
            IntPtr lpInBuffer,
            uint nInBufferSize,
            [Out] IntPtr lpOutBuffer,
            uint nOutBufferSize,
            ref uint lpBytesReturned,
            IntPtr lpOverlapped);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern unsafe bool WriteFile(
            SafeFileHandle hFile,
            byte* pBuffer,
            uint NumberOfBytesToWrite,
            uint* pNumberOfBytesWritten,
            IntPtr Overlapped);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern unsafe bool ReadFile(
            SafeFileHandle hFile,
            byte* pBuffer,
            uint NumberOfBytesToRead,
            uint* pNumberOfBytesRead,
            IntPtr Overlapped);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool SetFilePointerEx(
            SafeFileHandle hFile,
            ulong liDistanceToMove,
            out ulong lpNewFilePointer,
            uint dwMoveMethod);

        [DllImport("kernel32.dll")]
        public static extern bool FlushFileBuffers(
            SafeFileHandle hFile);

        #endregion

    }
}
