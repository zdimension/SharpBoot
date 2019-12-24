using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using SharpBoot.Models;
using SharpBoot.Properties;
using SharpBoot.Utilities;
using OperationCanceledException = System.OperationCanceledException;

namespace SharpBoot.Forms
{
    [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
    public class GenIsoFrm : WorkerFrm
    {
        public bool _usb;

        public string filesystem = "";

        public GenIsoFrm(string output, bool usb)
        {
            Load += GenIsoFrm_Load;
            OutputFilepath = output;
            _usb = usb;
        }

        public List<ImageLine> Images { get; set; }


        public List<string> Categories
        {
            get { return Images.Select(x => x.Category).Distinct().ToList(); }
        }

        public long TotalSize
        {
            get { return Images.Sum(x => x.SizeB); }
        }


        public string Title { get; set; }

        public string IsoBackgroundImage { get; set; } = "";

        public string OutputFilepath { get; set; }

        public Size Res { get; set; }

        public Dictionary<string, string> CustomFiles { get; set; }

        public void AddImage(ImageLine i)
        {
            Images.Add(i);

            if (!Categories.Contains(i.Category))
                Categories.Add(i.Category);
        }

        private void GenIsoFrm_Load(object sender, EventArgs e)
        {
            Text = _usb ? Strings.CreatingUSB : Strings.CreatingISO;
        }

        public override void DoWork()
        {
            var f = Utils.GetTemporaryDirectory();

            ChangeStatus(Strings.Init);
            Thread.Sleep(1000);

            try
            {
                using (var ext = new SevenZipExtractor())
                {

                    var isodir = _usb ? OutputFilepath : Path.Combine(f, "iso");

                    if (_usb)
                    {
                        FormatDrive();
                    }

                    if (IsCancelled)
                    {
                        return;
                    }

                    var workingDir = Path.Combine(isodir, "boot", "grub");

                    if (!Directory.Exists(workingDir))
                        Directory.CreateDirectory(workingDir);

                    var isoroot = Path.Combine(isodir, "images");

                    var mkisofsexe = Path.Combine(f, "mkisofs", "mkisofs.exe");
                    var archs = Path.Combine(f, "arch");
                    Directory.CreateDirectory(archs);

                    File.WriteAllBytes(Path.Combine(archs, "basedisk.7z"), Resources.basedisk);
                    if (!_usb) File.WriteAllBytes(Path.Combine(archs, "mkisofs.7z"), Resources.mkisofs);

                    ChangeProgress(0, 100, Strings.ExtractBaseDisk + " 1/6");
                    ext.Extract(Path.Combine(archs, "basedisk.7z"), isodir);

                    if (IsCancelled)
                    {
                        return;
                    }

                    ProcessBackgroundImage(workingDir);

                    if (!_usb)
                    {
                        ChangeProgress(50, 100, Strings.Extracting.FormatEx("Mkisofs"));

                        ext.Extract(Path.Combine(archs, "mkisofs.7z"), Path.Combine(f, "mkisofs"));
                    }

                    ChangeProgressBar(60, 100);
                    Utils.SafeDel(archs);

                    if (IsCancelled)
                    {
                        return;
                    }

                    CopyFiles(isoroot, isodir);

                    if (IsCancelled)
                    {
                        return;
                    }

                    ChangeAdditional("");
                    ChangeProgress(0, Categories.Count, Strings.GenMenus);


                    var main = new BootMenu(Title, true);
                    main.Items.Add(new BootMenuItem(Strings.BootFromHDD, Strings.BootFromHDD,
                        EntryType.BootHDD));

                    var ii = 0;

                    //var itype = new Func<string, EntryType>(fn => Path.GetExtension(fn).ToLower() == ".img" ? EntryType.IMG : EntryType.ISO);

                    if (IsCancelled)
                    {
                        return;
                    }

                    foreach (var c in Categories)
                    {
                        if (string.IsNullOrWhiteSpace(c))
                        {
                            ChangeProgress(ii, Categories.Count, Strings.GenMainMenu);
                            foreach (var x in Images.Where(x => x.Category == c))
                            {
                                main.Items.Add(new BootMenuItem(x.Name, x.Description,
                                    x.EntryType, x.FilePath, false, x.CustomCode));
                            }
                        }
                        else
                        {
                            ChangeProgress(ii, Categories.Count, string.Format(Strings.GenMenu, c));
                            var t = new BootMenu(c, false);
                            foreach (var x in Images.Where(x => x.Category == c))
                            {
                                t.Items.Add(new BootMenuItem(x.Name, x.Description,
                                    x.EntryType, x.FilePath, false, x.CustomCode));
                            }

                            File.WriteAllText(Path.Combine(workingDir, Hash.CRC32(c)) + ".cfg",
                                Grub2.GetCode(t), Utils.GetEnc());
                            main.Items.Add(new BootMenuItem(c, c, EntryType.Category, Hash.CRC32(c), false));
                        }

                        ii++;

                        if (IsCancelled)
                        {
                            return;
                        }
                    }

                    File.WriteAllText(Path.Combine(workingDir, "grub.cfg"), Grub2.GetCode(main));

                    if (IsCancelled)
                    {
                        return;
                    }

                    if (_usb)
                    {
                        InstallBootloader(f);
                    }
                    else
                    {
                        GenerateISO(f, mkisofsexe, archs, ext, isodir);
                    }
                }
            }
            catch (OperationCanceledException)
            {
                CancelWork();
            }
            finally
            {
                GenF(f);
            }
        }

        private void CopyFile(string source, string dest)
        {
            var tok = new CancellationTokenSource();

            void handler(object sender, EventArgs e)
            {
                tok.Cancel();
            }

            WorkCancelled += handler;
            var started = DateTime.Now;
            XCopy.Copy(source, dest, true,
                true,
                (o, pce) =>
                {
                    var rem = TimeSpan.FromSeconds((DateTime.Now - started).TotalSeconds / pce.ProgressPercentage *
                                                   (100 - pce.ProgressPercentage));

                    ChangeProgressBar(pce.ProgressPercentage, 100);
                    ChangeAdditional(string.Format(Strings.RemainingTime, rem));
                }, tok.Token);
            WorkCancelled -= handler;
        }

        private void GenF(string f)
        {
            var iter = 0;
            while (Directory.Exists(f) && iter < 10)
            {
                try
                {
                    new DirectoryInfo(f).Delete(true);
                }
                catch
                {
                    // ignored
                }

                iter++;
            }

            Utils.ClrTmp();
        }

        private void FormatDrive()
        {
            // format
            if (
                MessageBox.Show(Strings.FormatWillErase.Replace(@"\n", "\n"), "SharpBoot",
                    MessageBoxButtons.YesNo) !=
                DialogResult.Yes)
            {
                CancelWork();
                return;
            }

            var tries = 1;
            while (true)
            {
                if (tries == 5)
                {
                    MessageBox.Show(Strings.FormatError, "SharpBoot", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    CancelWork();
                    return;
                }

                ChangeProgress(0, 100, Strings.Formatting);
                var askform = new AskPath();
                askform.SetTextMode(Text, Strings.VolumeLabel, string.Concat(Title.Where(char.IsLetter)));
                var volumeLabel = "";
                if (askform.ShowDialog() == DialogResult.OK)
                {
                    volumeLabel = askform.FileName;
                }
                else
                {
                    CancelWork();
                    return;
                }

                var res = DriveIO.FormatDrive(OutputFilepath.Substring(0, 2), filesystem,
                    label: volumeLabel);

                if (res == DriveIO.FormatResult.Success)
                {
                    ChangeProgress(100, 100, Strings.Formatting);

                    break;
                }

                switch (res)
                {
                    case DriveIO.FormatResult.AccessDenied:
                        MessageBox.Show(Strings.NeedAdmin, "SharpBoot", MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        CancelWork();
                        return;
                    case DriveIO.FormatResult.PartitionTooBig:
                        MessageBox.Show(string.Format(Strings.PartitionTooBig, filesystem), "SharpBoot",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        CancelWork();
                        return;
                    case DriveIO.FormatResult.UserCancelled:
                        CancelWork();
                        return;
                    default:
                        if (
                            MessageBox.Show(Strings.FormatError, "SharpBoot", MessageBoxButtons.RetryCancel,
                                MessageBoxIcon.Error) == DialogResult.Cancel)
                        {
                            CancelWork();
                            return;
                        }

                        tries++;
                        break;
                }
            }
        }

        private void ProcessBackgroundImage(string workingDir)
        {
            Image img = null;
            ChangeProgress(30, 100, Strings.ExtractBaseDisk + " 4/6");
            if (IsoBackgroundImage == "")
            {
                var ms = new MemoryStream(Resources.sharpboot);
                img = Image.FromStream(ms);
            }
            else if (IsoBackgroundImage != "$$NONE$$")
            {
                img = Image.FromFile(IsoBackgroundImage);
            }

            ChangeProgress(35, 100, Strings.ExtractBaseDisk + " 5/6");
            Grub2.SetImage(img, Res, workingDir);
            ChangeProgress(45, 100, Strings.ExtractBaseDisk + " 6/6");
        }

        private void CopyFiles(string isoroot, string isodir)
        {
            ChangeProgress(0, Images.Count, Strings.CopyISOfiles);
            for (var i = 0; i < Images.Count; i++)
            {
                var current = Images[i].FilePath;
                ChangeProgress(i, Images.Count, string.Format(Strings.Copying, Path.GetFileName(current)));
                while (!Directory.Exists(isoroot))
                    Directory.CreateDirectory(isoroot);
                Utils.AttemptTry(() => CopyFile(current, Path.Combine(isoroot, Path.GetFileName(current))));
            }

            ChangeProgressBar(0, CustomFiles.Count);

            for (var i = 0; i < CustomFiles.Count; i++)
            {
                var current = CustomFiles.ToList()[i];
                var local = current.Key;
                var remote = current.Value;
                if (remote.StartsWith("/")) remote = remote.Substring(1);
                ChangeProgress(i, CustomFiles.Count, string.Format(Strings.Copying, Path.GetFileName(local)));
                while (!Directory.Exists(isodir))
                    Directory.CreateDirectory(isodir);
                Utils.AttemptTry(() => CopyFile(local, Path.Combine(isodir, remote)));
            }
        }

        private void InstallBootloader(string f)
        {
            ChangeProgress(23, 100, string.Format(Strings.InstallingBoot, "Grub2", OutputFilepath));
            Grub2.Install(OutputFilepath);
            GenF(f);
        }

        private void GenerateISO(string f, string mkisofsexe, string archs, SevenZipExtractor ext, string isodir)
        {
            // TODO: Implement working progress printing (I can't get OutputDataReceived to work on my computer)
            ChangeProgress(23, 100, Strings.CreatingISO);
            Thread.Sleep(500);
            var p = new Process
            {
                StartInfo =
                {
                    UseShellExecute = false,
                    FileName = mkisofsexe,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };
            p.StartInfo.Arguments +=
                " -publisher \"SharpBoot\" -iso-level 3 -no-emul-boot -boot-load-size 4 -boot-info-table -r -J -b boot/grub/eltorito.img";
            p.StartInfo.Arguments += " -o \"" + OutputFilepath + "\" \"" + isodir + "\"";
            p.EnableRaisingEvents = true;

            /*var a = new Action<object, DataReceivedEventArgs>((o, args) => MessageBox.Show(args.Data));

        p.OutputDataReceived += new DataReceivedEventHandler(a);
        p.ErrorDataReceived += new DataReceivedEventHandler(a);*/
            var exitCaught = false;
            p.Exited += delegate
            {
                exitCaught = true;
                GenF(f);
            };

            Thread.Sleep(500);

            if (IsCancelled)
            {
                return;
            }

            ChangeProgress(33, 100, string.Format(Strings.Extracting, "Mkisofs"));
            var iter = 0;
            while (true)
            {
                if (iter == 5)
                {
                    MessageBox.Show("Extraction of Mkisofs failed after: 5 attempts. Aborting.");
                    abort = true;
                    return;
                }

                if (!File.Exists(mkisofsexe))
                {
                    if (!Directory.Exists(archs)) Directory.CreateDirectory(archs);
                    File.WriteAllBytes(Path.Combine(archs, "mkisofs.7z"), Resources.mkisofs);
                    ext.Extract(Path.Combine(archs, "mkisofs.7z"), Path.Combine(f, "mkisofs"));
                }
                else
                {
                    break;
                }

                Thread.Sleep(500);
                iter++;
            }

            ChangeProgress(43, 100, Strings.CreatingISO);
            try
            {
                p.OutputDataReceived += (sender, args) =>
                {
                    try
                    {
                        if (args?.Data == null) return;
                        var o = args.Data.Trim();
                        if (!o.Contains('%')) return;
                        var pp = o.Substring(1, 5).Trim();
                        if (decimal.TryParse(pp, out var d))
                            if (o[0] == ' ' && o[3] == '.' && o[6] == '%')
                                ChangeProgress(
                                    Convert.ToInt32(Math.Round(d, 0, MidpointRounding.AwayFromZero)),
                                    100, Strings.CreatingISO + "\t" + pp + "%");
                    }
                    catch
                    {
                    }
                };
                p.Start();
                p.BeginOutputReadLine();
                p.WaitForExit();
                /*while (!p.WaitForExit(1))
                {
                    System.Threading.Thread.Sleep(500);
                    p.Refresh();
                    Application.DoEvents();
                }*/
            }
            catch (FileNotFoundException)
            {
            }
            /*p.BeginOutputReadLine();

        using (var reader = p.StandardOutput)
        {
            while (!reader.EndOfStream)
            {
                var o = reader.ReadLine().Trim();
                var pp = o.Substring(1, 5).Trim();
                var d = decimal.Parse(pp);
                if (o[0] == ' ' && o[3] == '.' && o[6] == '%')
                {
                    ChangeProgress(Convert.ToInt32(Math.Round(d, 0, MidpointRounding.AwayFromZero)), 100, Strings.CopyISOfiles + "\t" + pp + "%");
                }
            }
        }*/

            Thread.Sleep(500);
            if (!exitCaught)
                GenF(f);
        }
    }
}