using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SharpBoot
{
    public partial class DropDownSelector : SelectorCtrl
    {
        public DropDownSelector()
        {
            InitializeComponent();
        }

        public DropDownSelector(string text, params object[] items) : base(text)
        {
            comboBox.DataSource = items;
        }

        public new dynamic Value => comboBox.SelectedItem;

        public EventHandler ValueChanged = delegate { };
    }
}
