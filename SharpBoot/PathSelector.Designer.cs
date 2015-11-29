using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using SharpBoot.Properties;

namespace SharpBoot
{
    partial class PathSelector
    {
        /// <summary> 
        /// Variable nécessaire au concepteur.
        /// </summary>
        private IContainer components = null;

        /// <summary> 
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur de composants

        /// <summary> 
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas 
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnBrowse = new Button();
            this.tbxDest = new TextBox();
            this.SuspendLayout();
            // 
            // btnBrowse
            // 
            this.btnBrowse.Anchor = ((AnchorStyles)((AnchorStyles.Bottom | AnchorStyles.Right)));
            this.btnBrowse.Image = Resources.folder;
            this.btnBrowse.ImeMode = ImeMode.NoControl;
            this.btnBrowse.Location = new Point(402, 27);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new Size(31, 25);
            this.btnBrowse.TabIndex = 7;
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new EventHandler(this.btnBrowse_Click);
            // 
            // tbxDest
            // 
            this.tbxDest.Anchor = ((AnchorStyles)(((AnchorStyles.Bottom | AnchorStyles.Left) 
            | AnchorStyles.Right)));
            this.tbxDest.Location = new Point(0, 28);
            this.tbxDest.Name = "tbxDest";
            this.tbxDest.Size = new Size(396, 23);
            this.tbxDest.TabIndex = 6;
            this.tbxDest.TextChanged += new EventHandler(this.tbxDest_TextChanged);
            // 
            // PathSelector
            // 
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.tbxDest);
            this.Name = "PathSelector";
            this.Controls.SetChildIndex(this.tbxDest, 0);
            this.Controls.SetChildIndex(this.btnBrowse, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Button btnBrowse;
        private TextBox tbxDest;
    }
}
