using System;
using System.Linq;
using System.Windows.Forms;

namespace RibbonBimStarter
{
    public partial class FormVersionInfo : Form
    {
        public FormVersionInfo(string[] text)
        {
            InitializeComponent();

            labelVersion.Text = text[0];
            labelNews.Text = string.Join(Environment.NewLine, text.Skip(1));
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void linkLabelBimstarter_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(@"https://t.me/bimstarter");
        }
    }
}
