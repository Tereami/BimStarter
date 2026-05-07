using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RevitAreaReinforcement
{
    public partial class FormAddShortcutsDefault : Form
    {
        private readonly string xmlPath;
        private readonly string hotkeyRemindePath;
        private readonly string url;

        public FormAddShortcutsDefault(string xmlPath, string hotkeyRemindePath, string url)
        {
            InitializeComponent();
            textBoxXmlPath.Text = xmlPath;
            this.xmlPath = xmlPath;
            this.hotkeyRemindePath = hotkeyRemindePath;
            this.url = url;
        }

        private void buttonCopy_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(xmlPath);
            buttonCopy.Text = "Copied!";
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(new ProcessStartInfo
            {
                FileName = hotkeyRemindePath,
                UseShellExecute = true
            });
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(new ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true
            });
        }
    }
}
