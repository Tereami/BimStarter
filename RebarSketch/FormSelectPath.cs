using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RebarSketch
{
    public partial class FormSelectPath : Form
    {
        public bool UseServerPath;
        public string ServerPath;
        public FormSelectPath(string configFilePath, string defaultConfigFolder)
        {
            InitializeComponent();
            labelConfigIniPath.Text = configFilePath;
            labelAppDataConfigPath.Text = defaultConfigFolder;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            textBoxPath.Enabled = false;
            buttonBrowse.Enabled = false;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            textBoxPath.Enabled = true;
            buttonBrowse.Enabled = true;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.UseServerPath = radioButton1.Checked;
            this.ServerPath = textBoxPath.Text;
            this.Close();
        }

        private void buttonBrowse_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.ShowNewFolderButton = false;
            if (dialog.ShowDialog() != DialogResult.OK) return;
            textBoxPath.Text = dialog.SelectedPath;
        }

        private void buttonHelp_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://bim-starter.com/plugins/rebarsketch/");
        }
    }
}
