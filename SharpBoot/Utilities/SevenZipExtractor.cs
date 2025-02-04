﻿using System;
using System.Diagnostics;
using System.IO;
using SharpBoot.Properties;

namespace SharpBoot.Utilities
{
    public class SevenZipExtractor : IDisposable
    {
        public string SevenZipPath;

        public SevenZipExtractor()
        {
            var d = FileIO.GetTemporaryDirectory();
            SevenZipPath = Path.Combine(d, "7za.exe");
            File.WriteAllBytes(SevenZipPath, Resources._7za);
        }

        public event Action ExtractionFinished;

        protected void OnFinished()
        {
            ExtractionFinished?.Invoke();
        }

        public void Close()
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            FileIO.SafeDel(Path.GetDirectoryName(SevenZipPath));
        }

        public void Extract(string arch, string output, bool wait = true, int maxDelay = -1)
        {
            var p = new Process
            {
                StartInfo =
                {
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    FileName = SevenZipPath,
                    Arguments = "x \"" + arch + "\" -o\"" + output + "\""
                }
            };

            Utils.WaitWhile(() => !File.Exists(p.StartInfo.FileName));

            p.Exited += delegate { OnFinished(); };

            p.Start();

            if (wait)
                p.WaitForExit(maxDelay);
        }

        public void Dispose()
        {
            Close();
        }
    }
}