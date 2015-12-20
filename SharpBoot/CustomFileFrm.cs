using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace SharpBoot
{
    public partial class CustomFileFrm : Form
    {
        public CustomFileFrm()
        {
            InitializeComponent();

            if (Program.IsWin)
            {
                Utils.SetWindowTheme(lvFiles.Handle, "EXPLORER", null);
            }

            lblHeader.Text = Strings.AddFiles;
            btnOK.Text = Strings.OK;
            btnAnnul.Text = Strings.Cancel;
        }

        public Dictionary<string, string> CFiles
        {
            get
            {
                return (from r in lvFiles.Rows.Cast<DataGridViewRow>()
                        select new {Local = r.Cells[0].Value.ToString(), Remote = r.Cells[1].Value.ToString()})
                    .ToDictionary(x => x.Local, x => x.Remote);
            }
            set
            {
                lvFiles.Rows.Clear();
                value.All(x =>
                {
                    lvFiles.Rows.Add(x.Key, x.Value);
                    return true;
                });
            }
        }


        private void CustomFileFrm_Load(object sender, EventArgs e)
        {
            lblHeader.Text = Strings.AddFiles;
        }

        private void lvFiles_SelectionChanged(object sender, EventArgs e)
        {
            btnRemove.Enabled = lvFiles.SelectedRows.Count != 0;
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow r in lvFiles.SelectedRows)
            {
                lvFiles.Rows.Remove(r);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (ofpFile.ShowDialog() == DialogResult.OK)
            {
                foreach (var f in ofpFile.FileNames)
                {
                    AddFile(f);
                }
            }
        }

        private void AddFile(string local, string remote)
        {
            if (File.Exists(local))
                lvFiles.Rows.Add(local, remote);
        }

        private void AddFile(string local)
        {
            AddFile(local, GetPath(local));
        }

        private static string GetPath(string local)
        {
            return "/" + Path.GetFileName(local).RemoveAccent();
        }

        private void lvFiles_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
        }

        private void lvFiles_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                ((string[]) e.Data.GetData(DataFormats.FileDrop)).ToList().ForEach(AddFile);
            }
        }

        private void lvFiles_CellValidated(object sender, DataGridViewCellEventArgs e)
        {
            var cell = lvFiles.Rows[e.RowIndex].Cells[e.ColumnIndex];
            var cv = cell.Value?.ToString() ?? "";
            if (string.IsNullOrWhiteSpace(cv) || cv.EndsWith("/"))
            {
                var c2 = lvFiles.Rows[e.RowIndex].Cells[0];
                cell.Value = GetPath(c2.Value.ToString());
            }
            else
            {
                if (!cv.StartsWith("/"))
                {
                    cell.Value = "/" + cv;
                }
            }
        }
    }
}