using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RevitAreaReinforcement
{
    public partial class DialogWindowRestoreAreaRebar : Form
    {
        public string _xmlPath = "error path";
        public int speed = 1;
        public DialogWindowRestoreAreaRebar(string XmlPath, int speed)
        {
            InitializeComponent();
            _xmlPath = XmlPath;
            trackBarSpeed.Value = speed;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(@"https://forums.autodesk.com/t5/revit-api-forum/why-the-boundary-curve-of-area-reinforcement-is-not-connect-to/td-p/8382121");
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            speed = trackBarSpeed.Value;
            this.Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void buttonShowHelp_Click(object sender, EventArgs e)
        {
            Height = 657;
            buttonHideHelp.Visible = true;
            label1.Visible = true;
            label2.Visible = true;
            buttonShowHelp.Visible = false;
        }

        private void buttonHideHelp_Click(object sender, EventArgs e)
        {
            Height = 160;
            buttonHideHelp.Visible = false;
            label1.Visible = false;
            label2.Visible = false;
            buttonShowHelp.Visible = true;
        }
    }
}
