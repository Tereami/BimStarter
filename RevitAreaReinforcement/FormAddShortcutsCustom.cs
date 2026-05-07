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
    public partial class FormAddShortcutsCustom : Form
    {
        private readonly string xmlFileRestoreRebar;
        private readonly string url;

        public string userXmlPath = string.Empty;

        public FormAddShortcutsCustom(string xmlFileRestoreRebar, string url)
        {
            InitializeComponent();
            this.xmlFileRestoreRebar = xmlFileRestoreRebar;
            this.url = url;
            textBoxXmlPath.Text = xmlFileRestoreRebar;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(new ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true
            });
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string folder = Path.GetDirectoryName(xmlFileRestoreRebar);
            System.Diagnostics.Process.Start(new ProcessStartInfo
            {
                FileName = folder,
                UseShellExecute = true
            });
        }

        private void buttonSelectXml_Click(object sender, EventArgs e)
        {
            OpenFileDialog od = new OpenFileDialog()
            {
                Filter = "XML files (*.xml)|*.xml|All Files (*.*)|*.*",
                Multiselect = false
            };
            if(od.ShowDialog() == DialogResult.OK)
            {
                userXmlPath = od.FileName;
            }

            this.DialogResult = DialogResult.Yes;
        }
    }
}
