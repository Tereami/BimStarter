using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RebarSketch
{
    public partial class FormErrorNoParameter : Form
    {
        public FormErrorNoParameter(string parameter, string family)
        {
            InitializeComponent();

            labelParameters.Text = parameter;
            labelFamilies.Text = family;
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void linkLabelHelp_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(new ProcessStartInfo
            {
                FileName = "https://bim-starter.com/plugins/rebarsketch/",
                UseShellExecute = true
            });
        }
    }
}
