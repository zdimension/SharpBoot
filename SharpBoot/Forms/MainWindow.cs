using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Resources;
using System.Security.Cryptography;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Linq;
using SharpBoot.Models;
using SharpBoot.Properties;
using SharpBoot.Utilities;
using OperationCanceledException = System.OperationCanceledException;

// ReSharper disable PossibleNullReferenceException

namespace SharpBoot.Forms
{
    [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
    public partial class MainWindow : Form
    {
        private bool changing;

        public List<ImageLine> CurImages = new List<ImageLine>();

        private Dictionary<string, string> CustomFiles = new Dictionary<string, string>();

        private readonly bool dev_FirstLaunch = false;

        private readonly Dictionary<string, Tuple<CultureInfo, bool>> lngs =
            new Dictionary<string, Tuple<CultureInfo, bool>>();

        private bool update_available;

        public MainWindow()
        {
            if (Settings.Default.FirstLaunch && dev_FirstLaunch)
                Hide();

            SetStyle(
                ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.DoubleBuffer |
                ControlStyles.ResizeRedraw,
                true);

            InitAfterLng();
            changing = true;
            loadlng();
            var c = Utils.GetCulture();
            setlngitem(c);
            automaticallyAddISOInfoToolStripMenuItem.Checked = Settings.Default.AutoAddInfo;
            cbxBackType.SelectedIndex = 2;

            SetSize();
            if (Utils.IsWin) UxTheme.SetWindowTheme(lvIsos.Handle, "EXPLORER", null);


            ISOInfo.UpdateFinished += (o, args) =>
            {
                try
                {
                    if (InvokeRequired)
                        Invoke((MethodInvoker) (() => mniUpdate.Visible = false));
                    else mniUpdate.Visible = false;
                }
                catch
                {
                    // ignored
                }
            };


            if (Settings.Default.FirstLaunch && dev_FirstLaunch)
            {
                var firstlaunch = new FirstLaunch();
                firstlaunch.ShowDialog();

                Show();
            }
        }

        public byte[] SelectedBackground
            =>
                cbxBackType.SelectedIndex == 0
                    ? Resources.sharpboot
                    : cbxBackType.SelectedIndex == 1
                        ? File.Exists(txtBackFile.Text) ? Image.FromFile(txtBackFile.Text).ToByteArray() : new byte[0]
                        : new byte[0];

        protected override CreateParams CreateParams
        {
            get
            {
                var handleParam = base.CreateParams;
                handleParam.ExStyle |= 0x02000000; // WS_EX_COMPOSITED       
                return handleParam;
            }
        }

        public void SetSize()
        {
            tbxSize.Text = Utils.GetSizeString(CurImages.Sum(x => x.SizeB) + 8787466 + DriveIO.SIZE_BASEDISK +
                                                 SelectedBackground.Length); // TODO: Update the bloader size
        }


        private void loadlng()
        {
            languageToolStripMenuItem.DropDownItems.Clear();
            var result = fromresx(typeof(Strings));

            result.AddRange(fromresx(typeof(ISOCat)));

            var systemLng = CultureInfo.InstalledUICulture;
            if (!systemLng.IsNeutralCulture)
                systemLng = systemLng.Parent;

            if (result.All(x => x.ThreeLetterISOLanguageName != systemLng.ThreeLetterISOLanguageName))
                result.Add(systemLng);

            result = result.Distinct().ToList();
            result.Sort((x, y) => string.Compare(x.NativeName, y.NativeName, StringComparison.Ordinal));

            lngs.Clear();
            foreach (var x in result)
            {
                var mnit = new ToolStripMenuItem(x.NativeName, Utils.GetFlag(x.Name)) {Tag = x.Name};
                mnit.Click += (sender, args) => LngItemClick(mnit);
                languageToolStripMenuItem.DropDownItems.Add(mnit);

                lngs.Add(x.Name, new Tuple<CultureInfo, bool>(x, !Equals(x, systemLng)));
            }
        }

        private void InitAfterLng()
        {
            Controls.Clear();
            InitializeComponent();
            SetSize();
            centerDragndrop();
            lngs.Clear();
            loadlng();
            LoadResolutions();
            cbxRes.SelectedIndex = 0;
            cbxBackType.SelectedIndex = 0;
            updateAvailableToolStripMenuItem.Visible = update_available;
            addFilesToolStripMenuItem.Text = Strings.AddFiles;
            btnCustomEntry.Text = Strings.AddCustomEntry;
            btnBackBrowse.Text = Strings.Browse;
            btnAbout.Text = " " + Strings.AboutSharpBoot;
            btnChecksum.Text = " " + btnChecksum.Text;
            btnCustomCode.Text = " " + btnCustomCode.Text;
            btnRemISO.Text = " " + btnRemISO.Text;
            Program.editcode = btnCustomCode.Text.Substring(1);
            Program.fpath = lvIsos.Columns[4].HeaderText;
        }

        private void LngItemClick(ToolStripItem it)
        {
            var tmp = lngs[it.Tag.ToString()];

            if (Utils.GetCulture().Equals(tmp.Item1)) return;

            if (!tmp.Item2)
            {
                Process.Start("https://poeditor.com/join/project/GDNqzsHFSk");
                setlngitem(Utils.GetCulture());
                return;
            }

            Utils.SetAppLng(tmp.Item1);

            if (changing && FieldsEmpty())
                InitAfterLng();
            else if (!FieldsEmpty()) MessageBox.Show(Strings.ChangesNeedRestart);

            changing = false;

            setlngitem(tmp.Item1);

            changing = true;
        }

        private static List<CultureInfo> fromresx(Type t)
        {
            var result = new List<CultureInfo>();
            var rm = new ResourceManager(t);

            var cultures = CultureInfo.GetCultures(CultureTypes.AllCultures);
            foreach (var culture in cultures)
                try
                {
                    if (culture.Equals(CultureInfo.InvariantCulture)) continue; //do not use "==", won't work

                    var rs = rm.GetResourceSet(culture, true, false);
                    if (rs != null)
                        result.Add(culture);
                }
                catch (CultureNotFoundException)
                {
                    //NOP
                }

            return result;
        }


        public void AddImage(string filePath, ISOV ver = null)
        {
            if (CurImages.Count(x => x.FilePath == filePath) != 0)
                return;

            var name = Path.GetFileNameWithoutExtension(filePath);
            var desc = "";
            var cat = "";

            if (ver?.Hash == "nover")
            {
                name = ver.Parent.Name;
                desc = ver.Parent.Description;
                cat = ver.Parent.CategoryTxt;
            }
            else
            {
                if (automaticallyAddISOInfoToolStripMenuItem.Checked && ver?.Hash != "other")
                {
                    ver = ver ?? ISOInfo.GetFromFile(filePath, new FileInfo(filePath).Length > 10000000);
                    if (ver == null)
                    {
                        MessageBox.Show(Path.GetFileName(filePath) + "\n\n" + Strings.CouldntDetect, "SharpBoot",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    }
                    else
                    {
                        name = ver.Name;
                        desc = ver.Parent.Description;
                        cat = ver.Parent.CategoryTxt;
                    }
                }
            }


            var im = new ImageLine(name, filePath, desc, cat,
                typ: filePath.ToLower().EndsWith("img") ? EntryType.IMG : EntryType.ISO);
            CurImages.Add(im);

            SetSize();


            lvIsos.Rows.Add(name, Utils.GetFileSizeString(filePath), cat, desc, filePath);
        }

        public void setlngitem(CultureInfo ci)
        {
            var found = false;

            foreach (ToolStripMenuItem mni in languageToolStripMenuItem.DropDownItems)
                if (lngs[mni.Tag.ToString()].Item1.Equals(ci))
                {
                    found = true;
                    mni.Checked = true;
                    languageToolStripMenuItem.Image = mni.Image;
                    break;
                }
                else
                {
                    mni.Checked = false;
                }

            // ReSharper disable once TailRecursiveCall
            if (!found) setlngitem(new CultureInfo("en"));
        }

        private void checkForUpdates()
        {
            try
            {
                using (var wb = new WebClient())
                {
                    wb.Headers["User-Agent"] =
                        "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/47.0.2526.58 Safari/537.36";
                    var ct = wb.DownloadString("https://api.github.com/repos/zdimension/SharpBoot/releases/latest");

                    var lnid = ct.IndexOf("tag_name");
                    ct = ct.Substring(lnid + 13);
                    ct = ct.Substring(0, ct.IndexOf('"'));

                    var v = Version.Parse(ct);
                    //v = new Version(3, 7);
                    updateAvailableToolStripMenuItem.Visible =
                        update_available = v > Assembly.GetEntryAssembly().GetName().Version;
                }
            }
            catch
            {
                // ignored
            }

            //mniUpdate.Visible = false;
        }

        private void g_GenerationFinished(GenIsoFrm g)
        {
            Utils.ClrTmp();

            Thread.CurrentThread.CurrentCulture = new CultureInfo(Settings.Default.Lang);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(Settings.Default.Lang);

            if (g.abort)
                return;

            if (
                MessageBox.Show(this, Strings.IsoCreated.Replace(@"\n", "\n"), Strings.IsoCreatedTitle,
                    MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                if (g.filesystem == "NTFS")
                    Thread.Sleep(1500); // let Windows notice the FS changes

                QEMUISO.LaunchQemu(g.OutputFilepath, g._usb);
            }
        }

        private void lvIsos_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                var files = ((string[]) e.Data.GetData(DataFormats.FileDrop)).ToList();
                if (files.Count == 1)
                {
                    if (Path.GetExtension(files[0]).ToLower() != ".iso" &&
                        Path.GetExtension(files[0]).ToLower() != ".img")
                    {
                        e.Effect = DragDropEffects.None;
                        return;
                    }
                }
                else
                {
                    if (
                        !files.Any(
                            x => Path.GetExtension(x).ToLower() == ".iso" || Path.GetExtension(x).ToLower() == ".img"))
                    {
                        e.Effect = DragDropEffects.None;
                        return;
                    }
                }

                e.Effect = DragDropEffects.Copy;
            }
        }

        private void lvIsos_DragDrop(object sender, DragEventArgs e)
        {
            foreach (var x in (string[]) e.Data.GetData(DataFormats.FileDrop))
            {
                AddImage(x);
            }
        }

        private void btnGen_Click(object sender, EventArgs e)
        {
            launchgeniso(false);
        }

        private void launchgeniso(bool usb)
        {
            Form ask = null;
            if (usb)
            {
                var fs =
                    new[] {"NTFS", "FAT32", "FAT16", "FAT12"}.AddRecommended(
                        CurImages.Any(x => x.SizeB >= uint.MaxValue)
                            ? 0
                            : 1);
                ask = new USBFrm(Strings.CreateMultibootUsb, Strings.Filesystem, Strings.OK, true, fs);
            }
            else
            {
                ask = new AskPath();
            }

            if (ask.ShowDialog() == DialogResult.OK)
            {
                var fn = usb ? ((USBFrm) ask).SelectedUSB.Name.ToUpper().Substring(0, 3) : ((AskPath) ask).FileName;
                var g = new GenIsoFrm(fn, usb);
                g.WorkFinished += delegate { g_GenerationFinished(g); };

                g.Title = txtTitle.Text;
                if (usb) g.filesystem = ((USBFrm) ask).TheComboBox.SelectedItem.ToString().RemoveRecommended();
                switch (cbxBackType.SelectedIndex)
                {
                    case 0:
                        g.IsoBackgroundImage = "";
                        break;
                    case 1:
                        g.IsoBackgroundImage = txtBackFile.Text;
                        break;
                    default:
                        g.IsoBackgroundImage = "$$NONE$$";
                        break;
                }

                g.Res = ((dynamic) cbxRes.SelectedItem).Val;
                g.Images =
                    CurImages.Select(
                        x =>
                            new ImageLine(x.Name, x.FilePath, x.Description,
                                x.Category, x.CustomCode, x.EntryType)).ToList();
                g.CustomFiles = CustomFiles;
                g.ShowDialog(this);

                Utils.ClrTmp();
            }
        }

        public void CheckFields()
        {
            lblDragHere.Visible = lvIsos.Rows.Count == 0;
            btnGen.Enabled = btnUSB.Enabled = !(lvIsos.Rows.Count == 0 ||
                                                cbxBackType.SelectedIndex == 1 && !File.Exists(txtBackFile.Text));
        }

        private void btnRemISO_Click(object sender, EventArgs e)
        {
            var fp = lvIsos.SelectedRows[0].Cells[4].Value.ToString();
            CurImages.RemoveAll(x => x.FilePath == fp);
            lvIsos.Rows.Remove(lvIsos.Rows.OfType<DataGridViewRow>().Single(x => x.Cells[4].Value.ToString() == fp));

            SetSize();
        }

        private void gbxTest_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                var files = ((string[]) e.Data.GetData(DataFormats.FileDrop)).ToList();
                if (files.Count == 1)
                {
                    if (Path.GetExtension(files[0]).ToLower() != ".iso" &&
                        Path.GetExtension(files[0]).ToLower() != ".img" && !files[0].EndsWith(Program.DirCharStr))
                        return;
                }
                else
                {
                    return;
                }

                e.Effect = DragDropEffects.Copy;
            }
        }

        private void gbxTest_DragDrop(object sender, DragEventArgs e)
        {
            var t = (string[]) e.Data.GetData(DataFormats.FileDrop);
            var a = t[0];
            QEMUISO.LaunchQemu(a, a.EndsWith(Program.DirCharStr));
        }

        private void lvIsos_SelectionChanged(object sender, EventArgs e)
        {
            btnRemISO.Enabled = btnChecksum.Enabled = btnCustomCode.Enabled = lvIsos.SelectedRows.Count == 1;
        }

        private void lvIsos_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            btnRemISO_Click(this, EventArgs.Empty);
        }

        private void lvIsos_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (lvIsos.SelectedRows.Count != 1)
                return;

            var newname = lvIsos.SelectedRows[0].Cells[0].Value?.ToString() ?? "";
            var newcat = lvIsos.SelectedRows[0].Cells[2].Value?.ToString() ?? "";
            var newdesc = lvIsos.SelectedRows[0].Cells[3].Value?.ToString() ?? "";

            var ind =
                CurImages.IndexOf(CurImages.Single(x =>
                    x.FilePath == lvIsos.SelectedRows[0].Cells[4].Value.ToString()));
            var nw = new ImageLine(newname, lvIsos.SelectedRows[0].Cells[4].Value.ToString(), newdesc, newcat);
            CurImages.RemoveAt(ind);
            CurImages.Insert(ind, nw);
        }

        private void btnBackBrowse_Click(object sender, EventArgs e)
        {
            var ofpI = new OpenFileDialog
            {
                Filter = Strings.PicFilter + " (*.png, *.jpg, *.jpeg, *.bmp)|*.png;*.jpg;*.jpeg;*.bmp"
            };

            if (ofpI.ShowDialog() == DialogResult.OK)
            {
                var img = Image.FromFile(ofpI.FileName);
                if (img.Width < 720)
                    cbxRes.SelectedIndex = 0;
                else if (img.Width >= 720 && img.Width < 912)
                    cbxRes.SelectedIndex = 1;
                else if (img.Width >= 912 && img.Width < 1152)
                    cbxRes.SelectedIndex = 2;
                else
                    cbxRes.SelectedIndex = 3;

                txtBackFile.Text = ofpI.FileName;

                CheckFields();
            }
        }

        private void btnChecksum_Click(object sender, EventArgs e)
        {
            btnChecksum.ShowContextMenuStrip();
        }

        private void chksum<T>(string n)
            where T : HashAlgorithm, new()
        {
            Cursor = Cursors.WaitCursor;

            var frm = new GenericTask(n);
            var res = "";
            frm.WorkHandler = () =>
            {
                var d = DateTime.Now;

                var sb = Hash.FileHash<T>(
                    lvIsos.SelectedRows[0].Cells[4].Value.ToString(), 
                    frm.CancellationTokenSource.Token,
                    (prog, size) =>
                    {
                        frm.ChangeProgressBarEstimate((int)(prog * 100 / size), 100, d);
                    });

                var a = DateTime.Now;
                var t = a - d;

                res = string.Format(Strings.ChkOf, n,
                                    Path.GetFileName(lvIsos.SelectedRows[0].Cells[4].Value.ToString())) + "\r\n";
                res += sb + "\r\n";
                /*txImInfo.Text += Strings.CalcIn + " " + t.Hours + "h " + t.Minutes + "m " + (t.TotalMilliseconds / 1000.0) +
                                 "s";*/
                res += string.Format(Strings.CalcIn, t);
            };

            frm.WorkFinished += delegate
            {
                this.InvokeIfRequired(() =>
                {
                    Cursor = Cursors.Default;
                    txImInfo.Text = res;
                });
            };

            frm.ShowDialog(this);
        }

        private void theupdate()
        {
            mniUpdate.Visible = true;
            checkForUpdates();
            mniUpdate.Visible = true;
            ISOInfo.RefreshISOs();
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            SetSize();
            centerDragndrop();
            theupdate();
        }

        public bool FieldsEmpty()
        {
            return lvIsos.Rows.Count == 0 && txtTitle.Text == "SharpBoot" && txtBackFile.Text.Length == 0;
        }


        private void centerDragndrop()
        {
            lblDragHere.Location = new Point(
                lvIsos.Width / 2 - lblDragHere.Width / 2 + lvIsos.Location.X,
                lvIsos.Height / 2 - lblDragHere.Height / 2 + lvIsos.Location.Y
            );
        }

        private void button1_Click(object sender, EventArgs e)
        {
            new About().ShowDialog(this);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void addISOToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var fr = new AddIso();
            if (fr.ShowDialog() == DialogResult.OK) AddImage(fr.ISOPath, fr.IsoV);
        }

        private void automaticallyAddISOInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            automaticallyAddISOInfoToolStripMenuItem.Checked = !automaticallyAddISOInfoToolStripMenuItem.Checked;
            Settings.Default.AutoAddInfo = automaticallyAddISOInfoToolStripMenuItem.Checked;
            Settings.Default.Save();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                lvIsos.Rows.Clear();
                txtTitle.Text = "";

                var d = XDocument.Load(new FileStream(openFileDialog.FileName, FileMode.Open));

                var c = d.Element("SharpBoot");

                txtTitle.Text = c.Element("Name").Value;
                cbxRes.SelectedIndex = Convert.ToInt32(c.Element("Resolution").Value);
                cbxBackType.SelectedIndex = Convert.ToInt32(c.Element("Backtype").Value);
                txtBackFile.Text = c.Element("Backpath").Value;

                foreach (XElement a in c.Elements("ISOs").Nodes())
                {
                    CurImages.Add(new ImageLine(a.Element("Nom").Value, a.Element("Path").Value,
                        a.Element("Desc").Value,
                        a.Element("Cat").Value));
                    lvIsos.Rows.Add(a.Element("Nom").Value, Utils.GetFileSizeString(a.Element("Path").Value),
                        a.Element("Cat").Value, a.Element("Desc").Value, a.Element("Path").Value);
                }
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                var doc =
                    new XDocument(new XElement("SharpBoot",
                        new XElement("Name", txtTitle.Text),
                        new XElement("Resolution", cbxRes.SelectedIndex),
                        new XElement("Backtype", cbxBackType.SelectedIndex),
                        new XElement("Backpath", txtBackFile.Text),
                        new XElement("ISOs",
                            lvIsos.Rows.OfType<DataGridViewRow>().Select(x => new XElement("ISO",
                                new XElement("Nom", x.Cells[0].Value),
                                new XElement("Cat", x.Cells[2].Value),
                                new XElement("Desc", x.Cells[3].Value),
                                new XElement("Path", x.Cells[4].Value))))));

                doc.Save(saveFileDialog.FileName);
            }
        }

        private void txtTitle_TextChanged(object sender, EventArgs e)
        {
            /*if (changTitle) return;
            var pos = txtTitle.SelectionStart - 1;
            var t = new Dictionary<int, int>();
            changTitle = true;
            var old = txtTitle.Text;
            txtTitle.Text = txtTitle.Text.RemoveAccent(out t);
            changTitle = false;
            if(pos == old.Length)
            {
                txtTitle.SelectionStart = txtTitle.Text.Length;
            }
            else txtTitle.SelectionStart = t.ContainsKey(pos) ? t[pos] : 0;
            txtTitle.SelectionStart++;*/
        }

        private void btnInstBoot_Click(object sender, EventArgs e)
        {
            var frm = new USBFrm(Strings.InstallABootLoader, Strings.ChooseBootloader, Strings.Install, false, "Grub2", "Grub4DOS", "Syslinux");
            frm.BtnClicked += (o, args) =>
            {
                frm.ProgressVisible = true;
                frm.SetProgress(5);
                try
                {
                    Utils.CallAdminProcess(frm.TheComboBox.SelectedItem.ToString().ToLower(), frm.SelectedUSB.Name);
                    frm.SetProgress(100);
                    MessageBox.Show(
                        string.Format(Strings.BootloaderInstalled,
                            frm.TheComboBox.SelectedItem.ToString(),
                            frm.SelectedUSB.Name), "SharpBoot", 0, MessageBoxIcon.Information);
                }
                catch(OperationCanceledException)
                {
                    return;
                }
            };
            frm.ShowDialog(this);
        }

        private void btnSha1_Click(object sender, EventArgs e)
        {
            chksum<SHA1CryptoServiceProvider>("SHA-1");
        }

        private void btnSha256_Click(object sender, EventArgs e)
        {
            chksum<SHA256CryptoServiceProvider>("SHA-256");
        }

        private void btnSha512_Click(object sender, EventArgs e)
        {
            chksum<SHA512CryptoServiceProvider>("SHA-512");
        }

        private void btnSha384_Click(object sender, EventArgs e)
        {
            chksum<SHA384CryptoServiceProvider>("SHA-384");
        }

        private void lvIsos_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            CheckFields();
        }

        private void cbxBackType_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtBackFile.Enabled = btnBackBrowse.Enabled = cbxBackType.SelectedIndex == 1;
            if (cbxBackType.SelectedIndex != 1) txtBackFile.Text = "";
            CheckFields();
            SetSize();
        }

        private void lvIsos_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            CheckFields();
        }

        private void btnUSB_Click(object sender, EventArgs e)
        {
            launchgeniso(true);
        }

        private void mD5ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            chksum<MD5CryptoServiceProvider>("MD5");
        }

        private void updateAvailableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/zdimension/SharpBoot/releases/latest");
        }

        private void MainWindow_SizeChanged(object sender, EventArgs e)
        {
            centerDragndrop();
        }

        private ImageLine selectediline()
        {
            var cit = lvIsos.SelectedRows[0];
            return CurImages.First(x => x.Name == cit.Cells[0].Value?.ToString());
        }

        private void btnCustomCode_Click(object sender, EventArgs e)
        {
            var cit = lvIsos.SelectedRows[0];
            var bmi = new BootMenuItem(
                cit.Cells[0].Value?.ToString() ?? "",
                cit.Cells[3].Value?.ToString() ?? "",
                selectediline().EntryType,
                cit.Cells[4].Value?.ToString() ?? "") {Start = false};

            var cod = Grub2.GetCode(bmi);

            var edfrm = new EditCodeFrm("/images/" + Path.GetFileName(cit.Cells[4].Value.ToString())) {Code = cod};
            if (edfrm.ShowDialog(this) == DialogResult.OK)
            {
                lvIsos.SelectedRows[0].Cells[5].Value = edfrm.Code == cod ? "" : edfrm.Code;
                CurImages.First(x => x.Name == cit.Cells[0].Value.ToString()).CustomCode =
                    lvIsos.SelectedRows[0].Cells[5].Value.ToString();
            }
        }

        private void addFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var filesfrm = new CustomFileFrm {CFiles = CustomFiles};
            filesfrm.ShowDialog(this);
            CustomFiles = filesfrm.CFiles;
        }

        private void btnCustomEntry_Click(object sender, EventArgs e)
        {
            var entryfrm = new CustomEntryFrm();
            if (entryfrm.ShowDialog(this) == DialogResult.OK)
            {
                var im = new ImageLine(Path.GetFileNameWithoutExtension(entryfrm.FilePath), entryfrm.FilePath, "", "",
                    "", entryfrm.SelectedType);
                CurImages.Add(im);
                SetSize();
                lvIsos.Rows.Add(im.Name, Utils.GetFileSizeString(entryfrm.FilePath), "", "", entryfrm.FilePath);
            }
        }

        private void LoadResolutions()
        {
            var res = new[]
            {
                new {Val = new Size(640, 480), Disp = "640x480", Ratio = "4/3 (1.33)"},
                new {Val = new Size(800, 600), Disp = "800x600", Ratio = "4/3 (1.33)"},
                new {Val = new Size(1024, 768), Disp = "1024x768", Ratio = "4/3 (1.33)"},
                new {Val = new Size(1280, 1024), Disp = "1280x1024", Ratio = "5/4 (1.25)"}
            };
            cbxRes.Sorted = false;
            cbxRes.DataSource = res;
        }

        private void txtBackFile_TextChanged(object sender, EventArgs e)
        {
            SetSize();
        }

        private void editThemeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var ted = new ThemeEditor();
            ted.ShowDialog(this);
        }
    }
}