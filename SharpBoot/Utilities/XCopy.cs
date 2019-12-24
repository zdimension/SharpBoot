using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Threading;

namespace SharpBoot.Utilities
{
    // http://stackoverflow.com/a/8341945/2196124
    /// <summary>
    ///     PInvoke wrapper for CopyEx
    ///     http://msdn.microsoft.com/en-us/library/windows/desktop/aa363852.aspx
    /// </summary>
    public class XCopy
    {
        private string Destination;
        private int FilePercentCompleted;
        private bool IsCancelled;

        private string Source;

        public static void Copy(string source, string destination, bool overwrite, bool nobuffering,
            EventHandler<ProgressChangedEventArgs> handler, CancellationToken token = default)
        {
            new XCopy().CopyInternal(source, destination, overwrite, nobuffering, handler, token);
        }

        private event EventHandler Completed;
        private event EventHandler<ProgressChangedEventArgs> ProgressChanged;

        private void CopyInternal(string source, string destination, bool overwrite, bool nobuffering,
            EventHandler<ProgressChangedEventArgs> handler, CancellationToken token = default)
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

                IsCancelled = false;

                token.Register(() => IsCancelled = true);

                if (!CopyFileEx(Source, Destination, CopyProgressHandler, IntPtr.Zero, ref IsCancelled,
                    copyFileFlags))
                {
                    switch (Marshal.GetLastWin32Error())
                    {
                        case WinError.ERROR_CANCELLED:
                        case WinError.ERROR_REQUEST_ABORTED:
                            throw new OperationCanceledException(token);

                        default:
                            throw new Win32Exception(Marshal.GetLastWin32Error());
                    }
                }
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

                ProgressChanged?.Invoke(this, new ProgressChangedEventArgs(FilePercentCompleted, null));
            }
        }

        private void OnCompleted()
        {
            Completed?.Invoke(this, EventArgs.Empty);
        }

        #region PInvoke

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool CopyFileEx(string lpExistingFileName, string lpNewFileName,
            CopyProgressRoutine lpProgressRoutine, IntPtr lpData, ref bool pbCancel, CopyFileFlags dwCopyFlags);

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
            COPY_FILE_RESTARTABLE = 0x00000002,
            COPY_FILE_OPEN_SOURCE_FOR_WRITE = 0x00000004,
            COPY_FILE_ALLOW_DECRYPTED_DESTINATION = 0x00000008,
            COPY_FILE_NO_BUFFERING = 0x00001000,
        }

        private CopyProgressResult CopyProgressHandler(long total, long transferred, long streamSize,
            long streamByteTrans, uint dwStreamNumber,
            CopyProgressCallbackReason reason, IntPtr hSourceFile, IntPtr hDestinationFile, IntPtr lpData)
        {
            if (reason == CopyProgressCallbackReason.CALLBACK_CHUNK_FINISHED)
                OnProgressChanged(transferred / (double) total * 100.0);

            if (transferred >= total)
                OnCompleted();

            if (IsCancelled)
                return CopyProgressResult.PROGRESS_CANCEL;

            return CopyProgressResult.PROGRESS_CONTINUE;
        }

        #endregion
    }
}