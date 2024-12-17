using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RibbonBimStarter
{
    public enum UploadOption { AddVersion, FirstLoad, LoadAsNew }

    public partial class FormUploadReplaceOrNewFamily : Form
    {
        private string _guid;
        public UploadOption selectedOption;
        public string versionDescription;


        public FormUploadReplaceOrNewFamily(string guid)
        {
            InitializeComponent();
            _guid = guid;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(App.settings.Website + "family?guid=" + _guid);
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            if(radioButtonAddVersion.Checked)
            {
                selectedOption = UploadOption.AddVersion;
                versionDescription = richTextBox1.Text;
            }
            else if (radioButtonNewFamily.Checked)
            {
                selectedOption = UploadOption.LoadAsNew;
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void radioButtonAddVersion_CheckedChanged(object sender, EventArgs e)
        {
            richTextBox1.Enabled = radioButtonAddVersion.Checked;
        }
    }
}
