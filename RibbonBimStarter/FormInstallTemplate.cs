using System;
using System.Windows.Forms;

namespace RibbonBimStarter
{
    public partial class FormInstallTemplate : Form
    {
        public FormInstallTemplate()
        {
            InitializeComponent();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.No;
            this.Close();
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://weandrevit.ru/");
        }
    }
}
