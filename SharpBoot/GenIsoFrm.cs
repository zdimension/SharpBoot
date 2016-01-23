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
using Microsoft.VisualBasic;
using SharpBoot.Properties;

namespace SharpBoot
{
    [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
    public partial class GenIsoFrm : Form
    {
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

        public event EventHandler GenerationFinished;

        protected virtual void OnFinished(EventArgs e)
        {
            ChangeProgress(100, 100, "");
            GenerationFinished?.Invoke(this, e);
            BeginInvoke(new CloseDelegate(CloseD));
        }

        public delegate void CloseDelegate();

        public void CloseD()
        {
            Close();
        }

        public string filesystem = "";

        public void AddImage(ImageLine i)
        {
            Images.Add(i);

            if (!Categories.Contains(i.Category))
                Categories.Add(i.Category);
        }

        public delegate void ChangeProgressBarDelegate(int val, int max);

        public bool _usb;

        public GenIsoFrm(string output, bool usb)
        {
            InitializeComponent();
            lblStatus.Text = Strings.Init;
            btnAnnul.Text = Strings.Cancel;
            OutputFilepath = output;
            _usb = usb;
        }

        public delegate void ChangeStatusDelegate(string stat);

        public void ChangeProgressBar(int val, int max)
        {
            if (pbx.InvokeRequired)
                pbx.Invoke((MethodInvoker) (() =>
                {
                    pbx.Maximum = max;
                    pbx.Value = val;
                }));
            else
            {
                pbx.Maximum = max;
                pbx.Value = val;
            }
        }

        public void ChangeStatus(string stat)
        {
            if (lblStatus.InvokeRequired) lblStatus.Invoke((MethodInvoker) (() => lblStatus.Text = stat));
            else lblStatus.Text = stat;
        }

        public void ChangeProgress(int val, int max, string stat)
        {
            ChangeProgressBar(val, max);

            ChangeStatus(stat);
        }


        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);


            if (bwkISO.IsBusy)
                bwkISO.CancelAsync();
        }


        private bool usethread = true;

        private void GenIsoFrm_Load(object sender, EventArgs e)
        {
            if (_usb) Text = Strings.CreatingUSB;
            Show();
            if (usethread)
                bwkISO.RunWorkerAsync();
            else
                Generate();
        }

        private bool closeonclick;

        private void btnAnnul_Click(object sender, EventArgs e)
        {
            if (closeonclick)
            {
                DialogResult = DialogResult.Cancel;
                Close();
            }
            else
            {
                bwkISO.CancelAsync();
                SetCancel();
            }
        }

        public IBootloader bloader { get; set; }

        public Size Res { get; set; }

        public bool abort;

        public Dictionary<string, string> CustomFiles { get; set; }

        public void Generate()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo(Settings.Default.Lang);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(Settings.Default.Lang);


            var f = Program.GetTemporaryDirectory();

            lblStatus.Text = Strings.Init;
            Thread.Sleep(1000);

            var ext = new SevenZipExtractor();

            var isodir = _usb ? OutputFilepath : Path.Combine(f, "iso");

            if (_usb)
            {
                // format
                if (
                    MessageBox.Show(Strings.FormatWillErase.Replace(@"\n", "\n"), "SharpBoot", MessageBoxButtons.YesNo) !=
                    DialogResult.Yes)
                {
                    abort = true;
                    return;
                }
                var tries = 1;
                while (true)
                {
                    if (tries == 5)
                    {
                        MessageBox.Show(Strings.FormatError, "SharpBoot", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        abort = true;
                        return;
                    }
                    ChangeProgress(0, 100, Strings.Formatting);
                    var askform = new AskPath();
                    askform.SetTextMode(Text, Strings.VolumeLabel, string.Concat(Title.Where(char.IsLetter)));
                    var volumeLabel = "";
                    if(askform.ShowDialog() == DialogResult.OK)
                    {
                        volumeLabel = askform.FileName;
                    }
                    else
                    {
                        abort = true;
                        return;
                    }
                    uint res = 1;
                    if ((res = Utils.FormatDrive(OutputFilepath.Substring(0, 2), filesystem,
                        label: volumeLabel)) == 0)
                    {
                        ChangeProgress(100, 100, Strings.Formatting);

                        break;
                    }
                    else
                    {
                        switch (res)
                        {
                            case 3:
                                MessageBox.Show(Strings.NeedAdmin, "SharpBoot", MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                                abort = true;
                                return;
                            case 2:
                                abort = true;
                                return;
                        }
                        if (
                            MessageBox.Show(Strings.FormatError, "SharpBoot", MessageBoxButtons.RetryCancel,
                                MessageBoxIcon.Error) == DialogResult.Cancel)
                        {
                            abort = true;
                            return;
                        }
                    }
                    tries++;
                }
            }

            if (bwkISO.CancellationPending)
            {
                abort = true;
                return;
            }

            var sylp = Path.Combine(isodir, "boot", bloader.FolderName);

            bloader.WorkingDir = sylp;
            bloader.Resolution = Res;

            if (!Directory.Exists(sylp))
                Directory.CreateDirectory(sylp);

            var isoroot = Path.Combine(isodir, "images");


            var mkisofsexe = Path.Combine(f, "mkisofs", "mkisofs.exe");
            var archs = Path.Combine(f, "arch");
            Directory.CreateDirectory(archs);

            File.WriteAllBytes(Path.Combine(archs, "basedisk.7z"), Resources.basedisk);
            File.WriteAllBytes(Path.Combine(archs, "bloader.7z"), bloader.Archive);
            if (!_usb) File.WriteAllBytes(Path.Combine(archs, "mkisofs.7z"), Resources.mkisofs);

            ChangeProgress(0, 100, Strings.ExtractBaseDisk + " 1/6");
            ext.Extract(Path.Combine(archs, "basedisk.7z"), isodir);
            ChangeProgress(10, 100, Strings.ExtractBaseDisk + " 2/6");
            ext.Extract(Path.Combine(archs, "bloader.7z"), isodir);
            if (bloader is Syslinux)
            {
                var theme = new SyslinuxTheme
                {
                    Resolution = Res,
                    noback = IsoBackgroundImage == "$$NONE$$"
                };
                ChangeProgress(20, 100, Strings.ExtractBaseDisk + " 3/6");
                File.WriteAllText(Path.Combine(sylp, "theme.cfg"), theme.GetCode());
                if (Program.UseCyrillicFont)
                {
                    File.WriteAllBytes(Path.Combine(sylp, "cyrillic_cp866.psf"), Resources._866_8x16);
                }
            }

            if (bwkISO.CancellationPending)
            {
                abort = true;
                return;
            }

            Image img = null;
            ChangeProgress(30, 100, Strings.ExtractBaseDisk + " 4/6");
            if (IsoBackgroundImage == "")
            {
                var ms = new MemoryStream(Resources.sharpboot);
                img = Image.FromStream(ms);
            }
            else if (IsoBackgroundImage != "$$NONE$$") img = Image.FromFile(IsoBackgroundImage);
            ChangeProgress(35, 100, Strings.ExtractBaseDisk + " 5/6");
            bloader.SetImage(img, Res);
            ChangeProgress(45, 100, Strings.ExtractBaseDisk + " 6/6");
            if (!_usb)
            {
                ChangeProgress(50, 100, Strings.Extracting.FormatEx("Mkisofs"));

                ext.Extract(Path.Combine(archs, "mkisofs.7z"), Path.Combine(f, "mkisofs"));
            }
            ChangeProgressBar(60, 100);
            Program.SafeDel(archs);

            if (bwkISO.CancellationPending)
            {
                abort = true;
                return;
            }

            // copier les fichiers dans le rep temporaire
            ChangeProgress(0, Images.Count, Strings.CopyISOfiles);
            for (var i = 0; i < Images.Count; i++)
            {
                var current = Images[i].FilePath;
                ChangeProgress(i, Images.Count, string.Format(Strings.Copying, Path.GetFileName(current)));
                while (!Directory.Exists(isoroot))
                    Directory.CreateDirectory(isoroot);
                for (var j = 0; j < 5; j++)
                {
                    try
                    {
                        XCopy.Copy(current, Path.Combine(isoroot, Path.GetFileName(current)), true,
                            true,
                            (o, pce) => { ChangeProgressBar(pce.ProgressPercentage, 100); });
                        break;
                    }
                    catch
                    {
                    }
                }
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
                for (var j = 0; j < 5; j++)
                {
                    try
                    {
                        XCopy.Copy(local, Path.Combine(isodir, remote), true,
                            true,
                            (o, pce) => { ChangeProgressBar(pce.ProgressPercentage, 100); });
                        break;
                    }
                    catch
                    {
                    }
                }
            }

            if (bwkISO.CancellationPending)
            {
                abort = true;
                return;
            }

            ChangeProgress(0, Categories.Count, Strings.GenMenus);


            var main = new BootMenu(Title, true);
            main.Items.Add(new BootMenuItem(Strings.BootFromHDD.RemoveAccent(), Strings.BootFromHDD.RemoveAccent(),
                EntryType.BootHDD));

            var ii = 0;

            //var itype = new Func<string, EntryType>(fn => Path.GetExtension(fn).ToLower() == ".img" ? EntryType.IMG : EntryType.ISO);

            if (bwkISO.CancellationPending)
            {
                abort = true;
                return;
            }

            foreach (var c in Categories)
            {
                if (string.IsNullOrWhiteSpace(c))
                {
                    ChangeProgress(ii, Categories.Count, Strings.GenMainMenu);
                    Images.Where(x => x.Category == c).All(x =>
                    {
                        main.Items.Add(new BootMenuItem(x.Name.RemoveAccent(), x.Description.RemoveAccent(),
                            x.EntryType, x.FilePath, false, x.CustomCode));
                        return true;
                    });
                }
                else
                {
                    ChangeProgress(ii, Categories.Count, string.Format(Strings.GenMenu, c));
                    var t = new BootMenu(c, false);
                    Images.Where(x => x.Category == c).All(x =>
                    {
                        t.Items.Add(new BootMenuItem(x.Name.RemoveAccent(), x.Description.RemoveAccent(),
                            x.EntryType, x.FilePath, false, x.CustomCode));
                        return true;
                    });

                    File.WriteAllText(Path.Combine(sylp, Utils.CRC32(c)) + bloader.FileExt,
                        bloader.GetCode(t), Program.GetEnc());
                    main.Items.Add(new BootMenuItem(c, c, EntryType.Category, Utils.CRC32(c), false));
                }

                ii++;

                if (bwkISO.CancellationPending)
                {
                    abort = true;
                    return;
                }
            }
            if (bloader is Syslinux)
                File.WriteAllText(Path.Combine(sylp, "syslinux.cfg"), bloader.GetCode(main), Program.GetEnc());
            else if (bloader is Grub4DOS)
                File.WriteAllText(Path.Combine(isodir, "menu.lst"), bloader.GetCode(main));

            if (bwkISO.CancellationPending)
            {
                abort = true;
                return;
            }

            if (_usb)
            {
                ChangeProgress(23, 100, string.Format(Strings.InstallingBoot, bloader.Name, OutputFilepath));
                BootloaderInst.Install(OutputFilepath, bloader.FolderName);
                GenF(f);
            }
            else
            {
                // TODO: Implement working progress printing (I can't get OutputDataReceived to work on my computer)
                ChangeProgress(23, 100, Strings.CreatingISO);
                Thread.Sleep(500);
                var p = new Process
                {
                    StartInfo =
                    {
                        UseShellExecute = false,
                        FileName = mkisofsexe
                        /*RedirectStandardOutput = true,
                    RedirectStandardError = true*/
                    }
                };
                p.StartInfo.Arguments += " " + bloader.CmdArgs +
                                         " -publisher \"SharpBoot\" -no-emul-boot -boot-load-size 4 -boot-info-table -r -J -b " +
                                         bloader.BinFile;
                p.StartInfo.Arguments += " -o \"" + OutputFilepath + "\" \"" + isodir + "\"";
                p.EnableRaisingEvents = true;

                /*var a = new Action<object, DataReceivedEventArgs>((o, args) => MessageBox.Show(args.Data));

            p.OutputDataReceived += new DataReceivedEventHandler(a);
            p.ErrorDataReceived += new DataReceivedEventHandler(a);*/

                p.Exited += delegate { GenF(f); };

                Thread.Sleep(500);

                if (bwkISO.CancellationPending)
                {
                    abort = true;
                    return;
                }

                while (true)
                {
                    if (!File.Exists(mkisofsexe))
                    {
                        if (!Directory.Exists(archs)) Directory.CreateDirectory(archs);
                        File.WriteAllBytes(Path.Combine(archs, "mkisofs.7z"), Resources.mkisofs);
                        ext.Extract(Path.Combine(archs, "mkisofs.7z"), Path.Combine(f, "mkisofs"));
                    }
                    else break;
                    Thread.Sleep(500);
                }
                try
                {
                    p.Start();
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
                if (!p.HasExited)
                    p.WaitForExit();
            }
            Program.SupportAccent = false;

            ext.Close();
        }

        private void bwkISO_DoWork(object sender, DoWorkEventArgs e)
        {
            Generate();
            if (abort)
            {
                closeonclick = true;
                SetCancel();
            }
        }

        private void SetCancel()
        {
            ChangeProgress(0, 100, Strings.OpCancelled);
            btnAnnul.Invoke((MethodInvoker) (() =>
            {
                btnAnnul.Text = Strings.Close;
                btnAnnul.DialogResult = DialogResult.Cancel;
            }));
        }

        private void GenF(string f)
        {
            while (Directory.Exists(f))
            {
                try
                {
                    new DirectoryInfo(f).Delete(true);
                }
                catch
                {
                }
            }

            Invoke((MethodInvoker) (() => OnFinished(EventArgs.Empty)));

            Program.ClrTmp();
        }

        private void bwkISO_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                abort = true;
                SetCancel();
            }
            if (e.Error != null)
            {
                if (e.Error is FileNotFoundException)
                {
                    MessageBox.Show("File not found: " + ((FileNotFoundException) e.Error).FileName);
                }
                else throw new Exception("Error: " + e.Error.Message + "\n", e.Error);
            }
        }
    }
}