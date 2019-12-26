namespace SharpBoot.Forms
{
    partial class CustomFileFrm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CustomFileFrm));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.lblHeader = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnAnnul = new System.Windows.Forms.Button();
            this.btnRemove = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lvFiles = new System.Windows.Forms.DataGridView();
            this.clmnLocal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmnRemote = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.ofpFile = new System.Windows.Forms.OpenFileDialog();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lvFiles)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblHeader
            // 
            resources.ApplyResources(this.lblHeader, "lblHeader");
            this.lblHeader.Name = "lblHeader";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Control;
            this.panel1.Controls.Add(this.btnOK);
            this.panel1.Controls.Add(this.btnAnnul);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // btnOK
            // 
            resources.ApplyResources(this.btnOK, "btnOK");
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Name = "btnOK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // btnAnnul
            // 
            resources.ApplyResources(this.btnAnnul, "btnAnnul");
            this.btnAnnul.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnAnnul.Name = "btnAnnul";
            this.btnAnnul.UseVisualStyleBackColor = true;
            // 
            // btnRemove
            // 
            resources.ApplyResources(this.btnRemove, "btnRemove");
            this.btnRemove.Image = global::SharpBoot.Properties.Resources.delete;
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // btnAdd
            // 
            resources.ApplyResources(this.btnAdd, "btnAdd");
            this.btnAdd.Image = global::SharpBoot.Properties.Resources.add;
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // panel2
            // 
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.BackColor = System.Drawing.SystemColors.ControlDark;
            this.panel2.Controls.Add(this.lvFiles);
            this.panel2.Name = "panel2";
            // 
            // lvFiles
            // 
            this.lvFiles.AllowDrop = true;
            this.lvFiles.AllowUserToAddRows = false;
            this.lvFiles.AllowUserToResizeRows = false;
            this.lvFiles.BackgroundColor = System.Drawing.SystemColors.Window;
            this.lvFiles.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lvFiles.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleVertical;
            this.lvFiles.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 9F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.lvFiles.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.lvFiles.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.lvFiles.ColumnHeadersVisible = false;
            this.lvFiles.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.clmnLocal,
            this.clmnRemote});
            resources.ApplyResources(this.lvFiles, "lvFiles");
            this.lvFiles.GridColor = System.Drawing.SystemColors.Window;
            this.lvFiles.Name = "lvFiles";
            this.lvFiles.RowHeadersVisible = false;
            this.lvFiles.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.lvFiles.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.lvFiles_CellDoubleClick);
            this.lvFiles.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.lvFiles_CellDoubleClick);
            this.lvFiles.CellValidated += new System.Windows.Forms.DataGridViewCellEventHandler(this.lvFiles_CellValidated);
            this.lvFiles.SelectionChanged += new System.EventHandler(this.lvFiles_SelectionChanged);
            this.lvFiles.DragDrop += new System.Windows.Forms.DragEventHandler(this.lvFiles_DragDrop);
            this.lvFiles.DragEnter += new System.Windows.Forms.DragEventHandler(this.lvFiles_DragEnter);
            // 
            // clmnLocal
            // 
            this.clmnLocal.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.clmnLocal.FillWeight = 50F;
            this.clmnLocal.Name = "clmnLocal";
            this.clmnLocal.ReadOnly = true;
            this.clmnLocal.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // clmnRemote
            // 
            this.clmnRemote.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.clmnRemote.FillWeight = 50F;
            this.clmnRemote.Name = "clmnRemote";
            this.clmnRemote.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.btnAdd, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnRemove, 2, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // ofpFile
            // 
            this.ofpFile.Multiselect = true;
            // 
            // CustomFileFrm
            // 
            this.AcceptButton = this.btnOK;
            resources.ApplyResources(this, "$this");
                        this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.btnAnnul;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.lblHeader);
            this.Controls.Add(this.panel1);
            this.Name = "CustomFileFrm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Load += new System.EventHandler(this.CustomFileFrm_Load);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.lvFiles)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblHeader;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnAnnul;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.OpenFileDialog ofpFile;
        private System.Windows.Forms.DataGridView lvFiles;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmnRemote;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmnLocal;
    }
}