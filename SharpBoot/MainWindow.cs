using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Resources;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Linq;
using SharpBoot.Properties;
using W7R;

namespace SharpBoot
{
    public enum Bootloader
    {
        Syslinux = 0,
        Grub4Dos = 1
    }

    [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
    public partial class MainWindow : Form
    {
        public void SetSize()
        {
            tbxSize.Text = Program.GetSizeString(CurImages.Sum(x => x.SizeB) + 988);

            menuStrip.Renderer = Windows7Renderer.Instance;

            cmsChecksum.Renderer = Windows7Renderer.Instance;
        }

        private void loadlng()
        {
            List<CultureInfo> result = fromresx(typeof (Strings));

            result.AddRange(fromresx(typeof (ISOCat)));

            result.Distinct().ToList().ForEach(x => cbxLng.Items.Add(new {Value = x, Name = x.NativeName}));
        }

        private static List<CultureInfo> fromresx(Type t)
        {
            List<CultureInfo> result = new List<CultureInfo>();
            ResourceManager rm = new ResourceManager(t);

            CultureInfo[] cultures = CultureInfo.GetCultures(CultureTypes.AllCultures);
            foreach (CultureInfo culture in cultures)
            {
                try
                {
                    if (culture.Equals(CultureInfo.InvariantCulture)) continue; //do not use "==", won't work

                    ResourceSet rs = rm.GetResourceSet(culture, true, false);
                    if (rs != null)
                        result.Add(culture);
                }
                catch (CultureNotFoundException)
                {
                    //NOP
                }
            }
            return result;
        }

        public List<ImageLine> CurImages = new List<ImageLine>();

        public Bootloader SelectedBootloader => (Bootloader) cbxBootloader.SelectedIndex;


        public void AddImage(string filePath, ISOV ver = null)
        {
            if (CurImages.Count(x => x.FilePath == filePath) != 0)
                return;

            var name = Path.GetFileNameWithoutExtension(filePath);
            var desc = "";
            var cat = "";


            if (automaticallyAddISOInfoToolStripMenuItem.Checked && ver?.Hash != "other")
            {
                ver = ver ?? (new FileInfo(filePath).Length > 750000000 ? null : ISOInfo.GetFromFile(filePath));
                if (ver == null)
                {
                    MessageBox.Show(Strings.CouldntDetect, "SharpBoot", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    name = ver.Name;
                    desc = ver.Parent.Description;
                    cat = ver.Parent.Category;
                }
            }


            var im = new ImageLine(name, filePath, desc, cat);
            CurImages.Add(im);

            SetSize();


            lvIsos.Rows.Add(name, Program.GetFileSizeString(filePath), cat, desc, filePath);
        }

        [DllImport("uxtheme.dll", ExactSpelling = true, CharSet = CharSet.Unicode)]
        private static extern int SetWindowTheme(IntPtr hwnd, string pszSubAppName, string pszSubIdList);

        private CultureInfo getselectedlng()
        {
            dynamic item = cbxLng.SelectedItem;
            return item.Value as CultureInfo;
        }

        public MainWindow()
        {
            DoubleBuffered = true;
            SetStyle(
                ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw,
                true);

            InitializeComponent();
            changing = true;
            loadlng();
            var c = Program.GetCulture();
            cbxLng.SelectedItem = cbxLng.Items.Cast<object>()
                .Select(it => new { it, its = it })
                .Where(t => (((dynamic)t.its).Value as CultureInfo).Equals(c))
                .Select(t => t.it)
                .First();
            automaticallyAddISOInfoToolStripMenuItem.Checked = Settings.Default.AutoAddInfo;

            SetSize();
            SetWindowTheme(lvIsos.Handle, "EXPLORER", null);

            cbxBootloader.SelectedIndex = 0;
            cbxRes.SelectedIndex = 0;
            cbxBackType.SelectedIndex = 0;
        }


        private static void g_GenerationFinished(GenIsoFrm g)
        {
            Program.ClrTmp();

            Thread.CurrentThread.CurrentCulture = new CultureInfo(Settings.Default.Lang);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(Settings.Default.Lang);

            if (
                MessageBox.Show(Strings.IsoCreated.Replace(@"\n", "\n"), Strings.IsoCreatedTitle,
                    MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
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
                        !(files.Any(
                            x => Path.GetExtension(x).ToLower() == ".iso" || Path.GetExtension(x).ToLower() == ".img")))
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
            ((string[]) e.Data.GetData(DataFormats.FileDrop)).All(x =>
            {
                AddImage(x);
                return true;
            });
        }

        private void btnGen_Click(object sender, EventArgs e)
        {
            launchgeniso(false);
        }

        private void launchgeniso(bool usb)
        {
            AskPForm ask = usb ? (AskPForm)new AskUSB() : (AskPForm)new AskPath();
            if (ask.ShowDialog() == DialogResult.OK)
            {
                var g = new GenIsoFrm(ask.FileName, usb);
                g.GenerationFinished += delegate { g_GenerationFinished(g); };
                g.Images = CurImages;
                g.Title = txtTitle.Text;
                if (usb) g.filesystem = ((AskUSB) ask).FileSystem;
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
                var selsize = cbxRes.SelectedItem.ToString();
                selsize = selsize.Replace("x", " ");
                var ssize = selsize.Split(' ');

                var selload = cbxBootloader.SelectedItem.ToString().ToLower().Trim();
                if (selload.StartsWith("syslinux")) selload = "syslinux";

                IBootloader bl = null;
                if (selload == "syslinux") bl = new Syslinux();
                if (selload == "grub4dos")
                {
                    bl = new Grub4Dos();
                }

                g.bloader = bl;

                g.Res = new Size(int.Parse(ssize[0]), int.Parse(ssize[1]));
                g.ShowDialog(this);

                Program.ClrTmp();
            }
        }

        public void CheckFields()
        {
            btnGen.Enabled = btnUSB.Enabled = !(lvIsos.Rows.Count == 0 ||
                               (cbxBackType.SelectedIndex == 1 && !File.Exists(txtBackFile.Text)));
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
                        Path.GetExtension(files[0]).ToLower() != ".img" && !files[0].EndsWith("\\"))
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
            var t = ((string[]) e.Data.GetData(DataFormats.FileDrop));
            var a = t[0];
            QEMUISO.LaunchQemu(a, a.EndsWith("\\"));
        }

        private void lvIsos_SelectionChanged(object sender, EventArgs e)
        {
            btnRemISO.Enabled = lvIsos.SelectedRows.Count == 1;
            btnChecksum.Enabled = lvIsos.SelectedRows.Count == 1;
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
                CurImages.IndexOf(CurImages.Single(x => x.FilePath == lvIsos.SelectedRows[0].Cells[4].Value.ToString()));
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
                {
                    cbxRes.SelectedIndex = 0;
                }
                else if (img.Width >= 720 && img.Width < 912)
                {
                    cbxRes.SelectedIndex = 1;
                }
                else
                {
                    cbxRes.SelectedIndex = 2;
                }

                txtBackFile.Text = ofpI.FileName;

                CheckFields();
            }
        }

        private void btnChecksum_Click(object sender, EventArgs e)
        {
            chksum("MD5", () =>
            {
                var file = new FileStream(lvIsos.SelectedRows[0].Cells[4].Value.ToString(), FileMode.Open);
                MD5 md5 = new MD5CryptoServiceProvider();
                var retVal = md5.ComputeHash(file);
                file.Close();

                var sb = new StringBuilder();
                foreach (var t1 in retVal)
                {
                    sb.Append(t1.ToString("x2"));
                }

                return sb.ToString();
            });
        }

        private void chksum(string n, Func<string> f)
        {
            var d = DateTime.Now;
            Cursor = Cursors.WaitCursor;


            var sb = f();

            var a = DateTime.Now;
            var t = a - d;
            txImInfo.Text = string.Format(Strings.ChkOf, n) + " " +
                            Path.GetFileName(lvIsos.SelectedRows[0].Cells[4].Value.ToString()) + " :\r\n";
            txImInfo.Text += sb + "\r\n";
            txImInfo.Text += Strings.CalcIn + " " + t.Hours + "h " + t.Minutes + "m " + (t.TotalMilliseconds / 1000.0) +
                             "s";
            Cursor = Cursors.Default;
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
        }

        private bool changing;

        public bool FieldsEmpty()
        {
            return lvIsos.Rows.Count == 0 && txtTitle.Text == "SharpBoot" && txtBackFile.Text.Length == 0;
        }

        private void cbxLng_SelectedIndexChanged(object sender, EventArgs e)
        {
            Program.SetAppLng(getselectedlng());
            if (changing && FieldsEmpty())
            {
                Controls.Clear();
                InitializeComponent();
                SetSize();
                cbxLng.Items.Clear();
                loadlng();
                cbxBootloader.SelectedIndex = 0;
                cbxRes.SelectedIndex = 0;
                cbxBackType.SelectedIndex = 0;
            }
            else if (!FieldsEmpty())
            {
                MessageBox.Show(Strings.ChangesNeedRestart);
            }

            changing = false;
            var c = Program.GetCulture();
            /*foreach (var it in
                cbxLng.Items.Cast<object>()
                    .Select(it => new {it, its = it})
                    .Where(t => (((dynamic) t.its).Value as CultureInfo).Equals(c))
                    .Select(t => t.it))
            {
                cbxLng.SelectedItem = it;
                break;
            }*/
            cbxLng.SelectedItem = cbxLng.Items.Cast<object>()
                .Select(it => new {it, its = it})
                .Where(t => (((dynamic) t.its).Value as CultureInfo).Equals(c))
                .Select(t => t.it)
                .First();
            changing = true;
        }

        public void CheckGrub4Dos()
        {
            cbxRes.Enabled = !(cbxBackType.SelectedIndex == 2 && cbxBootloader.SelectedIndex == 1);
            if (cbxBackType.SelectedIndex == 1 && File.Exists(txtBackFile.Text) && cbxBootloader.SelectedIndex == 1)
            {
                var img = Image.FromFile(txtBackFile.Text);
                switch (img.Width)
                {
                    case 800:
                        cbxRes.SelectedIndex = 1;
                        break;
                    case 1024:
                        cbxRes.SelectedIndex = 2;
                        break;
                    default:
                        cbxRes.SelectedIndex = 0;
                        break;
                }
            }
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
            if (fr.ShowDialog() == DialogResult.OK)
            {
                AddImage(fr.ISOPath, fr.IsoV);
            }
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
                cbxBootloader.SelectedIndex = Convert.ToInt32(c.Element("Bootloader").Value);
                cbxRes.SelectedIndex = Convert.ToInt32(c.Element("Resolution").Value);
                cbxBackType.SelectedIndex = Convert.ToInt32(c.Element("Backtype").Value);
                txtBackFile.Text = c.Element("Backpath").Value;

                foreach (XElement a in c.Elements("ISOs").Nodes())
                {
                    CurImages.Add(new ImageLine(a.Element("Nom").Value, a.Element("Path").Value, a.Element("Desc").Value,
                        a.Element("Cat").Value));
                    lvIsos.Rows.Add(a.Element("Nom").Value, Program.GetFileSizeString(a.Element("Path").Value),
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
                        new XElement("Bootloader", cbxBootloader.SelectedIndex),
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
                /*if(File.Exists(saveFileDialog.FileName)) File.Delete(saveFileDialog.FileName);
                using (var ms = File.OpenWrite(saveFileDialog.FileName))
                {
                    var fmt = new BinaryFormatter();
                    fmt.Serialize(ms, doc);
                    ms.Flush();
                    ms.Close();
                }*/
            }
        }

        private void txtTitle_TextChanged(object sender, EventArgs e)
        {
            var pos = txtTitle.SelectionStart;
            txtTitle.Text = txtTitle.Text.RemoveAccent();
            txtTitle.SelectionStart = pos;
        }

        private void btnInstBoot_Click(object sender, EventArgs e)
        {
            new InstallBoot().ShowDialog(this);
        }

        private void btnSha1_Click(object sender, EventArgs e)
        {
            chksum("SHA-1", () => Utils.FileSHA1(lvIsos.SelectedRows[0].Cells[4].Value.ToString()));
        }

        private void btnSha256_Click(object sender, EventArgs e)
        {
            chksum("SHA-256", () => Utils.FileSHA256(lvIsos.SelectedRows[0].Cells[4].Value.ToString()));
        }

        private void btnSha512_Click(object sender, EventArgs e)
        {
            chksum("SHA-512", () => Utils.FileSHA512(lvIsos.SelectedRows[0].Cells[4].Value.ToString()));
        }

        private void btnSha384_Click(object sender, EventArgs e)
        {
            chksum("SHA-384", () => Utils.FileSHA384(lvIsos.SelectedRows[0].Cells[4].Value.ToString()));
        }

        private void lvIsos_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            CheckFields();
        }

        private void cbxBootloader_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckGrub4Dos();
        }

        private void cbxBackType_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtBackFile.Enabled = btnBackBrowse.Enabled = cbxBackType.SelectedIndex == 1;
            if (cbxBackType.SelectedIndex != 1) txtBackFile.Text = "";
            CheckFields();
            CheckGrub4Dos();
        }

        private void lvIsos_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            CheckFields();
        }

        private void btnUSB_Click(object sender, EventArgs e)
        {
            launchgeniso(true);
        }
    }
}