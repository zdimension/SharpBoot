using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace SharpBoot.Utilities
{
    public static class DriveIO
    {
        public const long SIZE_BASEDISK = 0;
        public const int FILE_ATTRIBUTE_SYSTEM = 0x4;
        public const int FILE_FLAG_SEQUENTIAL_SCAN = 0x8;
        public const int FILE_FLAG_NO_BUFFERING = 0x20000000;

        public static void InstallMBR(string l, byte[] theMbr)
        {
            var mbr = new byte[512];

            using (
                var device = Kernel32.CreateFile(GetPhysicalPath(l.ToLower().Substring(0, 2)),
                    0x80000000 | 0x40000000,
                    1 | 2, IntPtr.Zero, 3,
                    /*Utils.FILE_ATTRIBUTE_SYSTEM | Utils.FILE_FLAG_SEQUENTIAL_SCAN*/
                    FILE_FLAG_NO_BUFFERING,
                    IntPtr.Zero))
            {
                if (device.IsInvalid)
                    throw new IOException("Unable to access drive. Win32 Error Code " + Marshal.GetLastWin32Error());

                using (var src = new FileStream(device, FileAccess.ReadWrite))
                {
                    src.Read(mbr, 0, 512);
                    Array.Copy(theMbr, mbr, theMbr.Length);
                    src.Position = 0;
                    src.Write(mbr, 0, 512);
                }
            }
        }

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

        public enum FormatResult
        {
            Success = 0,
            GenericError = 1,
            UserCancelled = 2,
            AccessDenied = 3,
            PartitionTooBig = 4
        }

        public static FormatResult FormatDrive(string driveLetter,
            string fileSystem = "NTFS", bool quickFormat = true,
            int clusterSize = 8192, string label = "SHARPBOOT", bool enableCompression = false)
        {
            if (driveLetter.Length != 2 || driveLetter[1] != ':' || !char.IsLetter(driveLetter[0]))
                return FormatResult.GenericError;

            if (enableCompression && fileSystem != "NTFS")
                return FormatResult.GenericError;

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
                long maxsize;

                switch (fileSystem)
                {
                    case "FAT12":
                        maxsize = 16777216;
                        break;
                    case "FAT16":
                        maxsize = 4294967296;
                        break;
                    default:
                        maxsize = -1;
                        break;
                }

                if (maxsize != -1 && di.TotalSize >= maxsize)
                {
                    return FormatResult.PartitionTooBig;
                }
            }
            catch
            {
                return FormatResult.GenericError;
            }

            p.StartInfo.Arguments += $" /k format /FS:{fileSystem} /V:{label} /A:{clusterSize} {(quickFormat ? "/Q" : "")} {(enableCompression ? "/C" : "")} /Y {driveLetter} & exit";

            try
            {
                p.Start();
            }
            catch (Win32Exception e) 
                when (e.NativeErrorCode == WinError.ERROR_CANCELLED)
            {
                return FormatResult.UserCancelled;
            }
            catch (Win32Exception e)
                when (e.NativeErrorCode == WinError.ERROR_ACCESS_DENIED)
            {
                return FormatResult.AccessDenied;
            }
            catch
            {
                return FormatResult.GenericError;
            }

            return p.WaitForExit(20000) ? FormatResult.Success : FormatResult.GenericError;
        }
    }
}
