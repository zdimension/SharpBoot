using System.Windows.Forms;

namespace SharpBoot
{
    public partial class SelectorCtrl : UserControl
    {
        public SelectorCtrl()
        {
            InitializeComponent();
        }

        public SelectorCtrl(string text) : this()
        {
            label1.Text = text;
        }

        public object Value => null;

        public bool IsValid { get; set; } = false;
    }
}
