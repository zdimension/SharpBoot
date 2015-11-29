using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SharpBoot
{
    partial class DropDownSelector
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
            this.comboBox = new ComboBox();
            this.SuspendLayout();
            // 
            // comboBox
            // 
            this.comboBox.DisplayMember = "Disp";
            this.comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBox.FormattingEnabled = true;
            this.comboBox.Location = new Point(0, 28);
            this.comboBox.Name = "comboBox";
            this.comboBox.Size = new Size(432, 23);
            this.comboBox.TabIndex = 9;
            this.comboBox.ValueMember = "Value";
            // 
            // DropDownSelector
            // 
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.Controls.Add(this.comboBox);
            this.Name = "DropDownSelector";
            this.Controls.SetChildIndex(this.comboBox, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ComboBox comboBox;
    }
}
