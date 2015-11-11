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
using SharpBoot.Properties;

namespace SharpBoot
{
    [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
    public partial class GenIsoFrm : Form
    {
        public List<ImageLine> Images { get; set; }


        public List<string> Categories
        {
            get { return Images.Select(x => x.Category.RemoveAccent()).Distinct().ToList(); }
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

        public void AddImage(ImageLine i)
        {
            Images.Add(i);

            if (!Categories.Contains(i.Category))
                Categories.Add(i.Category);
        }

        public delegate void ChangeProgressBarDelegate(int val, int max);

        public GenIsoFrm(string output)
        {
            InitializeComponent();

            OutputFilepath = output;
        }

        public delegate void ChangeStatusDelegate(string stat);

        public void ChangeProgressBar(int val, int max)
        {
            pbx.Maximum = max;
            pbx.Value = val;
        }

        public void ChangeStatus(string stat)
        {
            lblStatus.Text = stat;
        }

        public void ChangeProgress(int val, int max, string stat)
        {
            pbx.BeginInvoke(new ChangeProgressBarDelegate(ChangeProgressBar), val, max);

            lblStatus.BeginInvoke(new ChangeStatusDelegate(ChangeStatus), stat);
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
            Show();
            if (usethread)
                bwkISO.RunWorkerAsync();
            else
                Generate();
        }

        private void btnAnnul_Click(object sender, EventArgs e)
        {
            bwkISO.CancelAsync();
        }

        public IBootloader bloader { get; set; }

        public Size Res { get; set; }

        public void Generate()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo(Settings.Default.Lang);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(Settings.Default.Lang);

            var f = Program.GetTemporaryDirectory();

            lblStatus.Text = Strings.Init;
            Thread.Sleep(1000);

            var ext = new SevenZipExtractor();

            var sylp = Path.Combine(f, "iso", "boot", bloader.FolderName);

            bloader.WorkingDir = sylp;
            bloader.Resolution = Res;

            if (!Directory.Exists(sylp))
                Directory.CreateDirectory(sylp);

            var isoroot = Path.Combine(f, "iso", "images");
            var isodir = Path.Combine(f, "iso");

            var mkisofsexe = Path.Combine(f, "mkisofs", "mkisofs.exe");
            var archs = Path.Combine(f, "arch");
            Directory.CreateDirectory(archs);

            File.WriteAllBytes(Path.Combine(archs, "basedisk.7z"), Resources.basedisk);
            File.WriteAllBytes(Path.Combine(archs, "bloader.7z"), bloader.Archive);
            File.WriteAllBytes(Path.Combine(archs, "mkisofs.7z"), Resources.mkisofs);

            ChangeProgress(0, 100, Strings.ExtractBaseDisk);
            ext.Extract(Path.Combine(archs, "basedisk.7z"), Path.Combine(f, "iso"));
            ext.Extract(Path.Combine(archs, "bloader.7z"), Path.Combine(f, "iso"));

            if (bloader is Syslinux)
            {
                var theme = new SyslinuxTheme
                {
                    Resolution = Res,
                    noback = IsoBackgroundImage == "$$NONE$$"
                };
                File.WriteAllText(Path.Combine(sylp, "theme.cfg"), theme.GetCode());
            }

            Image img = null;

            if (IsoBackgroundImage == "")
            {
                var ms = new MemoryStream(Resources.sharpboot);
                img = Image.FromStream(ms);
            }
            else if (IsoBackgroundImage != "$$NONE$$") img = Image.FromFile(IsoBackgroundImage);

            bloader.SetImage(img, Res);

            ext.ExtractionFinished += delegate { ChangeProgress(50, 100, Strings.ExtractMkisofs); };

            ext.Extract(Path.Combine(archs, "mkisofs.7z"), Path.Combine(f, "mkisofs"));

            // copier les fichiers dans le rep temporaire
            ChangeProgress(0, Images.Count, Strings.CopyISOfiles);
            for (var i = 0; i < Images.Count; i++)
            {
                ChangeProgress(i, Images.Count, Strings.Copying + " " + Path.GetFileName(Images[i].FilePath));
                try
                {
                    File.Copy(Images[i].FilePath, Path.Combine(isoroot, Path.GetFileName(Images[i].FilePath)));
                }
                catch (DirectoryNotFoundException)
                {
                    Directory.CreateDirectory(isoroot);
                    File.Copy(Images[i].FilePath, Path.Combine(isoroot, Path.GetFileName(Images[i].FilePath)));
                }
            }

            ChangeProgress(0, Categories.Count, Strings.GenMenus);


            var main = new BootMenu(Title, true);
            main.Items.Add(new BootMenuItem(Strings.HDD.RemoveAccent(), Strings.BootFromHDD.RemoveAccent(),
                MenuItemType.BootHDD));

            var ii = 0;

            foreach (var c in Categories)
            {
                if (string.IsNullOrWhiteSpace(c))
                {
                    ChangeProgress(ii, Categories.Count, Strings.GenMainMenu);
                    Images.Where(x => x.Category == c).All(x =>
                    {
                        main.Items.Add(new BootMenuItem(x.Name.RemoveAccent(), x.Description.RemoveAccent(),
                            MenuItemType.ISO, x.FilePath, false));
                        return true;
                    });
                }
                else
                {
                    ChangeProgress(ii, Categories.Count, Strings.GenMenu + " " + c + "\"");
                    var t = new BootMenu(c, false);
                    Images.Where(x => x.Category == c).All(x =>
                    {
                        t.Items.Add(new BootMenuItem(x.Name.RemoveAccent(), x.Description.RemoveAccent(),
                            MenuItemType.ISO, x.FilePath, false));
                        return true;
                    });

                    File.WriteAllText(Path.Combine(sylp, c.ToLower().Replace(" ", "")) + bloader.FileExt,
                        bloader.GetCode(t));
                    main.Items.Add(new BootMenuItem(c, c, MenuItemType.Category, "", false));
                }

                ii++;
            }
            if (bloader is Syslinux)
                File.WriteAllText(Path.Combine(sylp, "syslinux.cfg"), bloader.GetCode(main));
            else if (bloader is Grub4Dos)
                File.WriteAllText(Path.Combine(isodir, "menu.lst"), bloader.GetCode(main));


            // TODO: Implement working progress printing (I can't get OutputDataReceived to work on my computer)
            ChangeProgress(23, 100, Strings.CreatingISO);

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
            p.StartInfo.Arguments += " -o \"" + OutputFilepath + "\" \"" + Path.Combine(f, "iso") + "\"";
            p.EnableRaisingEvents = true;

            /*var a = new Action<object, DataReceivedEventArgs>((o, args) => MessageBox.Show(args.Data));

            p.OutputDataReceived += new DataReceivedEventHandler(a);
            p.ErrorDataReceived += new DataReceivedEventHandler(a);*/

            p.Exited += delegate { GenF(f); };

            Thread.Sleep(500);
            p.Start();
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


            ext.Close();
        }

        private void bwkISO_DoWork(object sender, DoWorkEventArgs e)
        {
            Generate();
        }

        private void GenF(string f)
        {
            if (Directory.Exists(f))
                new DirectoryInfo(f).Delete(true);

            OnFinished(EventArgs.Empty);

            Program.ClrTmp();
        }
    }

    public class BootMenu
    {
        public string Title { get; set; }

        public List<BootMenuItem> Items { get; set; }

        public bool MainMenu { get; set; }

        public BootMenu(string title, bool main)
        {
            Title = title;
            MainMenu = main;
            Items = new List<BootMenuItem>();
        }
    }

    public class BootMenuItem
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public MenuItemType Type { get; set; }

        public bool Start { get; set; }

        public string CustomCode { get; set; }


        public string IsoName { get; set; }

        public BootMenuItem(string n, string d, MenuItemType t, string ison = "", bool st = true, string code = "")
        {
            Name = n;
            Description = d;
            Type = t;
            Start = st;
            IsoName = Path.GetFileName(ison);
            CustomCode = code;
        }
    }

    public enum MenuItemType
    {
        ISO = 0,
        Category = 1,
        BootHDD = 2,
        IMG = 3
    }
}