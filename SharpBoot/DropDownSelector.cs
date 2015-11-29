using System;

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
