using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Win32.SafeHandles;

namespace SharpBoot.Utilities
{
    public static class WinApi
    {
        public const int IDC_HAND = 32649;
    }

    public static class WindowMessage
    {
        public const int WM_EXITMENULOOP = 0x0212;
        public const int WM_SETCURSOR = 0x0020;
        public const int WM_SETFOCUS = 0x07;
        public const int WM_ENABLE = 0x0A;
    }

    public static class WinError
    {
        public const int ERROR_ACCESS_DENIED = 5;
        public const int ERROR_CANCELLED = 1223;
        public const int ERROR_REQUEST_ABORTED = 1235;
    }

    public static class UxTheme
    {
        [DllImport("uxtheme.dll", ExactSpelling = true, CharSet = CharSet.Unicode)]
        public static extern int SetWindowTheme(IntPtr hwnd, string pszSubAppName, string pszSubIdList);
    }

    public static class Kernel32
    {
        [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern SafeFileHandle CreateFile([MarshalAs(UnmanagedType.LPTStr)] string fileName,
            uint fileAccess, uint fileShare, IntPtr securityAttributes, uint creationDisposition, uint flags,
            IntPtr template);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern uint QueryDosDevice(string DeviceName, IntPtr TargetPath, uint ucchMax);

        [DllImport("kernel32.dll", ExactSpelling = true, SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool DeviceIoControl(IntPtr hDevice, uint dwIoControlCode, IntPtr lpInBuffer,
            uint nInBufferSize, IntPtr lpOutBuffer, uint nOutBufferSize, out uint lpBytesReturned, IntPtr lpOverlapped);
    }
}
