using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.RightsManagement;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32.SafeHandles;
using SharpBoot.Properties;
// ReSharper disable UnusedMember.Local
// ReSharper disable EventNeverSubscribedTo.Local

namespace SharpBoot
{
    public static class Utils
    {
        public const long SIZE_BASEDISK = 0;

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern SafeFileHandle CreateFile(string lpFileName, UInt32 dwDesiredAccess, UInt32 dwShareMode, IntPtr pSecurityAttributes, UInt32 dwCreationDisposition, UInt32 dwFlagsAndAttributes, IntPtr hTemplateFile);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern UInt32 QueryDosDevice(string DeviceName, IntPtr TargetPath, UInt32 ucchMax);

        [DllImport("kernel32.dll", ExactSpelling = true, SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool DeviceIoControl(IntPtr hDevice, uint dwIoControlCode, IntPtr lpInBuffer, uint nInBufferSize, IntPtr lpOutBuffer, uint nOutBufferSize, out uint lpBytesReturned, IntPtr lpOverlapped);


        public static string GetPhysicalPath(string letter)
        {
            letter = letter.Substring(0, 2);

            var deviceId = "";

            var queryResults = new ManagementObjectSearcher(
                    $"ASSOCIATORS OF {{Win32_LogicalDisk.DeviceID='{letter}'}} WHERE AssocClass = Win32_LogicalDiskToPartition");
            var partitions = queryResults.Get();
            foreach (var partition in partitions)
            {
                queryResults = new ManagementObjectSearcher(
                    $"ASSOCIATORS OF {{Win32_DiskPartition.DeviceID='{partition["DeviceID"]}'}} WHERE AssocClass = Win32_DiskDriveToDiskPartition");
                var drives = queryResults.Get();
                foreach (var drive in drives)
                    deviceId = drive["DeviceID"].ToString();
            }

            return deviceId;
        }

        public static byte[] ToByteArray(this Image img)
        {
            var ms = new MemoryStream();
            img.Save(ms, ImageFormat.Jpeg);
            return ms.ToArray();
        }

        public static Bitmap DrawFilledRectangle(int w, int h, Color c)
        {
            var bmp = new Bitmap(w, h);
            using (var g = Graphics.FromImage(bmp))
            {
                g.FillRectangle(new SolidBrush(c), 0, 0, w, h);
            }
            return bmp;
        }

        public static bool Is64 => Environment.GetEnvironmentVariable("PROCESSOR_ARCHITECTURE").IndexOf("64") > 0;

        public static string FormatEx(this string s, params object[] args)
        {
            return string.Format(s, args);
        }

        public static Image GetFlag(string twocode)
        {
            if (twocode == "en") return Resources.flag_usa;
            var dc = new List<string> {"de", "fr", "ro", "zh-Hans", "zh-Hant", "ru", "uk", "es", "cs", "it"};
            var index = dc.IndexOf(twocode);
            return index == -1 ? null : About.Flags[index];
        }


        public static List<string> Wrap(this string text, int maxLength)
        {
     
            // Return empty list of strings if the text was empty
            if (text.Length == 0) return new List<string>();
     
            var words = text.Split(' ');
            var lines = new List<string>();
            var currentLine = "";
     
            foreach (var currentWord in words)
            {
     
                if ((currentLine.Length > maxLength) ||
                    ((currentLine.Length + currentWord.Length) > maxLength))
                {
                    lines.Add(currentLine);
                    currentLine = "";
                }
     
                if (currentLine.Length > 0)
                    currentLine += " " + currentWord;
                else
                    currentLine += currentWord;
     
            }
     
            if (currentLine.Length > 0)
                lines.Add(currentLine);
     
            
            return lines;
        }

        public static string FileMD5(string fileName)
        {
            StringBuilder formatted;
            using (var fs = new FileStream(fileName, FileMode.Open))
            using (var bs = new BufferedStream(fs))
            {
                using (MD5 md5 = new MD5CryptoServiceProvider())
                {
                    var hash = md5.ComputeHash(bs);
                    formatted = new StringBuilder(2 * hash.Length);
                    foreach (var b in hash)
                    {
                        formatted.AppendFormat("{0:x2}", b);
                    }
                }
            }
            return formatted.ToString();
        }

        public static string FileSHA1(string fileName)
        {
            StringBuilder formatted;
            using (var fs = new FileStream(fileName, FileMode.Open))
            using (var bs = new BufferedStream(fs))
            {
                using (var sha1 = new SHA1Managed())
                {
                    var hash = sha1.ComputeHash(bs);
                    formatted = new StringBuilder(2 * hash.Length);
                    foreach (var b in hash)
                    {
                        formatted.AppendFormat("{0:x2}", b);
                    }
                }
            }
            return formatted.ToString();
        }

        public static string FileSHA256(string fileName)
        {
            StringBuilder formatted;
            using (var fs = new FileStream(fileName, FileMode.Open))
            using (var bs = new BufferedStream(fs))
            {
                using (SHA256 sha256 = new SHA256Managed())
                {
                    var hash = sha256.ComputeHash(bs);
                    formatted = new StringBuilder(2 * hash.Length);
                    foreach (var b in hash)
                    {
                        formatted.AppendFormat("{0:x2}", b);
                    }
                }
            }
            return formatted.ToString();
        }

        public static string FileSHA384(string fileName)
        {
            StringBuilder formatted;
            using (var fs = new FileStream(fileName, FileMode.Open))
            using (var bs = new BufferedStream(fs))
            {
                using (SHA384 sha384 = new SHA384Managed())
                {
                    var hash = sha384.ComputeHash(bs);
                    formatted = new StringBuilder(2 * hash.Length);
                    foreach (var b in hash)
                    {
                        formatted.AppendFormat("{0:x2}", b);
                    }
                }
            }
            return formatted.ToString();
        }

        public static string FileSHA512(string fileName)
        {
            StringBuilder formatted;
            using (var fs = new FileStream(fileName, FileMode.Open))
            using (var bs = new BufferedStream(fs))
            {
                using (SHA512 sha512 = new SHA512Managed())
                {
                    var hash = sha512.ComputeHash(bs);
                    formatted = new StringBuilder(2 * hash.Length);
                    foreach (var b in hash)
                    {
                        formatted.AppendFormat("{0:x2}", b);
                    }
                }
            }
            return formatted.ToString();
        }

        public static string CRC32(string ct)
        {
            return CRC32(Program.GetEnc().GetBytes(ct));
        }

        public static string CRC32(byte[] ct)
        {
            var table = new uint[256];
            for (var i = 0; i < 256; i++)
            {
                var cur = (uint) i;
                for (var j = 0; j < 8; j++)
                    if ((cur & 1) == 1)
                        cur = (cur >> 1) ^ 0xedb88320;
                    else
                        cur = cur >> 1;
                table[i] = cur;
            }
            return (~ct.Aggregate(0xffffffff, (current, t) => (current >> 8) ^ table[t])).ToString("x8");
        }


        // http://stackoverflow.com/a/10018438/2196124
        public static bool IsExternalDisk(string driveLetter)
        {
            var retVal = false;
            driveLetter = driveLetter.TrimEnd('\\');

            // browse all USB WMI physical disks
            foreach (
                ManagementObject drive in
                    new ManagementObjectSearcher("select DeviceID, MediaType,InterfaceType from Win32_DiskDrive").Get())
            {
                // associate physical disks with partitions
                var partitionCollection =
                    new ManagementObjectSearcher(
                        $"associators of {{Win32_DiskDrive.DeviceID='{drive["DeviceID"]}'}} " +
                        "where AssocClass = Win32_DiskDriveToDiskPartition").Get();

                foreach (var logicalCollection in from ManagementObject partition in partitionCollection
                                                  where partition != null
                                                  select new ManagementObjectSearcher(
                                                      $"associators of {{Win32_DiskPartition.DeviceID='{partition["DeviceID"]}'}} " +
                                                      "where AssocClass= Win32_LogicalDiskToPartition").Get())
                {
                    foreach (var volumeEnumerator in from ManagementObject logical in logicalCollection
                                                     where logical != null
                                                     select new ManagementObjectSearcher(
                                                         $"select DeviceID from Win32_LogicalDisk where Name='{logical["Name"]}'")
                                                         .Get().GetEnumerator())
                    {
                        volumeEnumerator.MoveNext();

                        var volume = (ManagementObject) volumeEnumerator.Current;

                        if (
                            driveLetter.ToLowerInvariant()
                                .Equals(volume["DeviceID"].ToString().ToLowerInvariant()) &&
                            (drive["MediaType"].ToString().ToLowerInvariant().Contains("external") ||
                             drive["InterfaceType"].ToString().ToLowerInvariant().Contains("usb")))
                        {
                            retVal = true;
                            break;
                        }
                    }
                }
            }

            return retVal;
        }

        [DllImport("uxtheme.dll", ExactSpelling = true, CharSet = CharSet.Unicode)]
        public static extern int SetWindowTheme(IntPtr hwnd, string pszSubAppName, string pszSubIdList);

        public static string FileHash(string fileName, string hash)
        {
            switch (hash)
            {
                case "md5":
                    return FileMD5(fileName);
                case "sha1":
                    return FileSHA1(fileName);
                case "sha256":
                    return FileSHA256(fileName);
            }

            return "";
        }

        // thanks http://www.codeproject.com/Articles/115598/Formatting-a-Drive-using-C-and-WMI
        public static uint FormatDrive(string driveLetter,
            string fileSystem = "NTFS", bool quickFormat = true,
            int clusterSize = 8192, string label = "SHARPBOOT", bool enableCompression = false)
        {
            if (driveLetter.Length != 2 || driveLetter[1] != ':' || !char.IsLetter(driveLetter[0]))
                return 1;

            /*//query and format given drive         
            ManagementObjectSearcher searcher = new ManagementObjectSearcher
                (@"select * from Win32_Volume WHERE DriveLetter = '" + driveLetter + "'");
            uint result = 1;
            foreach (ManagementObject vi in searcher.Get())
            {
                result = (uint)vi.InvokeMethod("Format", new object[]
                {fileSystem, quickFormat, clusterSize, label, enableCompression});
            }

            return result;*/

            var p = new Process
            {
                StartInfo =
                {
                    CreateNoWindow = true,
                    UseShellExecute = true,
                    FileName = "cmd",
                    Verb = "runas"
                }
            };

            try
            {
                var di = new DriveInfo(driveLetter);
                long maxsize = -1;
                if (fileSystem == "FAT12") maxsize = 16777216;
                if (fileSystem == "FAT16") maxsize = 4000000000;
                if (di.TotalSize >= maxsize && maxsize != -1)
                {
                    MessageBox.Show(string.Format(Strings.PartitionTooBig, fileSystem), "SharpBoot",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 2;
                }
            }
            catch
            {
                return 1;
            }

            p.StartInfo.Arguments += " /k format /FS:" + fileSystem + " /V:" + label + " /Q /Y " + driveLetter +
                                     " & exit";
            try
            {
                p.Start();
            }
            catch (Win32Exception e)
            {
                switch (e.NativeErrorCode)
                {
                    case 1223:
                        return 2;
                    case 5:
                        return 3;
                    default:
                        return 1;
                }
            }
            catch
            {
                return 1;
            }
            var finished = p.WaitForExit(20000);
            return (uint) (finished ? 0 : 1);
        }
    }

    // Thanks :D http://stackoverflow.com/a/8341945/2196124
    /// <summary>
    ///     PInvoke wrapper for CopyEx
    ///     http://msdn.microsoft.com/en-us/library/windows/desktop/aa363852.aspx
    /// </summary>
    public class XCopy
    {
        public static void Copy(string source, string destination, bool overwrite, bool nobuffering)
        {
            new XCopy().CopyInternal(source, destination, overwrite, nobuffering, null);
        }

        public static void Copy(string source, string destination, bool overwrite, bool nobuffering,
            EventHandler<ProgressChangedEventArgs> handler)
        {
            new XCopy().CopyInternal(source, destination, overwrite, nobuffering, handler);
        }

        private event EventHandler Completed;
        private event EventHandler<ProgressChangedEventArgs> ProgressChanged;

        private int IsCancelled;
        private int FilePercentCompleted;
        private string Source;
        private string Destination;

        private XCopy()
        {
            IsCancelled = 0;
        }

        private void CopyInternal(string source, string destination, bool overwrite, bool nobuffering,
            EventHandler<ProgressChangedEventArgs> handler)
        {
            try
            {
                var copyFileFlags = CopyFileFlags.COPY_FILE_RESTARTABLE;
                if (!overwrite)
                    copyFileFlags |= CopyFileFlags.COPY_FILE_FAIL_IF_EXISTS;

                if (nobuffering)
                    copyFileFlags |= CopyFileFlags.COPY_FILE_NO_BUFFERING;

                Source = source;
                Destination = destination;

                if (handler != null)
                    ProgressChanged += handler;

                var result = CopyFileEx(Source, Destination, CopyProgressHandler, IntPtr.Zero, ref IsCancelled,
                    copyFileFlags);
                if (!result)
                    throw new Win32Exception(Marshal.GetLastWin32Error());
            }
            catch (Exception)
            {
                if (handler != null)
                    ProgressChanged -= handler;

                throw;
            }
        }

        private void OnProgressChanged(double percent)
        {
            // only raise an event when progress has changed
            if ((int) percent > FilePercentCompleted)
            {
                FilePercentCompleted = (int) percent;

                var handler = ProgressChanged;
                handler?.Invoke(this, new ProgressChangedEventArgs((int) FilePercentCompleted, null));
            }
        }

        private void OnCompleted()
        {
            var handler = Completed;
            handler?.Invoke(this, EventArgs.Empty);
        }

        #region PInvoke

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool CopyFileEx(string lpExistingFileName, string lpNewFileName,
            CopyProgressRoutine lpProgressRoutine, IntPtr lpData, ref int pbCancel, CopyFileFlags dwCopyFlags);

        private delegate CopyProgressResult CopyProgressRoutine(
            long TotalFileSize, long TotalBytesTransferred, long StreamSize, long StreamBytesTransferred,
            uint dwStreamNumber, CopyProgressCallbackReason dwCallbackReason,
            IntPtr hSourceFile, IntPtr hDestinationFile, IntPtr lpData);

        private enum CopyProgressResult : uint
        {
            PROGRESS_CONTINUE = 0,
            PROGRESS_CANCEL = 1,
            PROGRESS_STOP = 2,
            PROGRESS_QUIET = 3
        }

        private enum CopyProgressCallbackReason : uint
        {
            CALLBACK_CHUNK_FINISHED = 0x00000000,
            CALLBACK_STREAM_SWITCH = 0x00000001
        }

        [Flags]
        private enum CopyFileFlags : uint
        {
            COPY_FILE_FAIL_IF_EXISTS = 0x00000001,
            COPY_FILE_NO_BUFFERING = 0x00001000,
            COPY_FILE_RESTARTABLE = 0x00000002,
            COPY_FILE_OPEN_SOURCE_FOR_WRITE = 0x00000004,
            COPY_FILE_ALLOW_DECRYPTED_DESTINATION = 0x00000008
        }

        private CopyProgressResult CopyProgressHandler(long total, long transferred, long streamSize,
            long streamByteTrans, uint dwStreamNumber,
            CopyProgressCallbackReason reason, IntPtr hSourceFile, IntPtr hDestinationFile, IntPtr lpData)
        {
            if (reason == CopyProgressCallbackReason.CALLBACK_CHUNK_FINISHED)
                OnProgressChanged((transferred / (double) total) * 100.0);

            if (transferred >= total)
                OnCompleted();

            return CopyProgressResult.PROGRESS_CONTINUE;
        }

        #endregion
    }
}