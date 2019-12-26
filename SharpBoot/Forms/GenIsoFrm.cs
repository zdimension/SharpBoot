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

        private string tmpdir;

        public override void DoWork()
        {
            tmpdir = FileIO.GetTemporaryDirectory();

            ChangeStatus(Strings.Init);

            try
            {
                using (var ext = new SevenZipExtractor())
                {

                    var isodir = _usb ? OutputFilepath : Path.Combine(tmpdir, "iso");

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

                    var archs = Path.Combine(tmpdir, "arch");
                    Directory.CreateDirectory(archs);

                    File.WriteAllBytes(Path.Combine(archs, "basedisk.7z"), Resources.basedisk);

                    ChangeProgress(0, 100, Strings.ExtractBaseDisk + " 1/6");
                    ext.Extract(Path.Combine(archs, "basedisk.7z"), isodir);

                    if (IsCancelled)
                    {
                        return;
                    }

                    ProcessBackgroundImage(workingDir);

                    ChangeProgressBar(60, 100);

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
                                Grub2.GetCode(t, Res), Utils.GetEnc());
                            main.Items.Add(new BootMenuItem(c, c, EntryType.Category, Hash.CRC32(c), false));
                        }

                        ii++;

                        if (IsCancelled)
                        {
                            return;
                        }
                    }

                    File.WriteAllText(Path.Combine(workingDir, "grub.cfg"), Grub2.GetCode(main, Res));

                    if (IsCancelled)
                    {
                        return;
                    }

                    if (_usb)
                    {
                        InstallBootloader();
                    }
                    else
                    {
                        GenerateISO(archs, ext, isodir);
                    }
                }
            }
            catch (OperationCanceledException)
            {
                CancelWork();
            }
            finally
            {
                Cleanup(tmpdir);
            }
        }

        private void CopyFile(string source, string dest)
        {
            var started = DateTime.Now;
            XCopy.Copy(source, dest, true,
                true,
                (o, pce) =>
                {
                    ChangeProgressBarEstimate(pce.ProgressPercentage, 100, started);
                }, CancellationTokenSource.Token);
        }

        private void Cleanup(string f)
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

            FileIO.ClrTmp();
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

        private void InstallBootloader()
        {
            ChangeProgress(23, 100, string.Format(Strings.InstallingBoot, "Grub2", OutputFilepath));
            Grub2.Install(OutputFilepath);
        }

        private void GenerateISO(string archs, SevenZipExtractor ext, string isodir)
        {
            File.WriteAllBytes(Path.Combine(archs, "mkisofs.7z"), Resources.mkisofs);
            ChangeProgress(22, 100, Strings.Extracting.FormatEx("Mkisofs"));
            ext.Extract(Path.Combine(archs, "mkisofs.7z"), Path.Combine(tmpdir, "mkisofs"));

            var mkisofsexe = Path.Combine(tmpdir, "mkisofs", "mkisofs.exe");
            ChangeProgress(30, 100, Strings.CreatingISO);
            var p = new Process
            {
                StartInfo =
                {
                    UseShellExecute = false,
                    FileName = mkisofsexe,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                }
            };
            p.StartInfo.Arguments +=
                " -publisher \"SharpBoot\" -iso-level 3 -no-emul-boot -boot-load-size 4 -boot-info-table -r -J -b boot/grub/eltorito.img";
            p.StartInfo.Arguments += " -o \"" + OutputFilepath + "\" \"" + isodir + "\"";
            p.EnableRaisingEvents = true;

            Utils.WaitWhile(() => !File.Exists(p.StartInfo.FileName));

            if (IsCancelled)
            {
                return;
            }

            ChangeProgress(43, 100, Strings.CreatingISO);
            try
            {
                var started = DateTime.Now;
                p.ErrorDataReceived += (sender, args) =>
                {
                    Localization.UpdateThreadCulture();

                    try
                    {
                        if (args?.Data == null) return;
                        var o = args.Data.Trim();
                        if (!o.Contains('%')) return;
                        var pp = o.Substring(0, 5).Replace("%", "").Trim();
                        if (double.TryParse(pp, NumberStyles.Float, CultureInfo.InvariantCulture, out var d))
                            if (d >= 0 && d <= 100)
                            {
                                ChangeStatus(Strings.CreatingISO + " " + (d / 100).ToString("P"));

                                ChangeProgressBarEstimate(
                                    (int)Math.Round(d, 0, MidpointRounding.AwayFromZero), 100, started);
                            }
                    }
                    catch
                    {
                        // ignore
                    }
                };
                p.Start();
                p.BeginErrorReadLine();
                p.WaitForExit();
            }
            catch (Win32Exception e) when (e.NativeErrorCode == WinError.ERROR_CANCELLED)
            {
                throw new OperationCanceledException(Strings.OpCancelled, e);
            }
        }
    }
}