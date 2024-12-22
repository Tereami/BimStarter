using System;
using System.Windows.Forms;

namespace RevitAreaReinforcement
{
    public partial class FormShortcuts2 : Form
    {
        public FormShortcuts2(string xmlpath, string backupxml)
        {
            InitializeComponent();

            labelFilename.Text = xmlpath;
            labelBackup.Text = backupxml;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
