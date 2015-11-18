using System;
using System.Diagnostics;
using System.IO;
using SharpBoot.Properties;

namespace SharpBoot
{
    public class SevenZipExtractor
    {
        public string SevenZipPath;

        public SevenZipExtractor()
        {
            var d = Program.GetTemporaryDirectory();
            SevenZipPath = Path.Combine(d, "7za.exe");
            File.WriteAllBytes(SevenZipPath, Resources._7za);
        }

        public event EventHandler ExtractionFinished;

        protected void OnFinished()
        {
            ExtractionFinished?.Invoke(this, EventArgs.Empty);
        }

        public void Close()
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            Program.SafeDel(Path.GetDirectoryName(SevenZipPath));
        }

        public void Extract(string arch, string output)
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

            p.Exited += delegate { OnFinished(); };

            p.Start();
        }
    }
}