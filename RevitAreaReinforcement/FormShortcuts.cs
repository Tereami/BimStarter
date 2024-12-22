using System;
using System.Windows.Forms;

namespace RevitAreaReinforcement
{
    public partial class FormShortcuts : Form
    {
        public FormShortcuts()
        {
            InitializeComponent();
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
