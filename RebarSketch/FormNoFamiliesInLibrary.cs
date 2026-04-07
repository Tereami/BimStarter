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
    public partial class FormNoFamiliesInLibrary : Form
    {
        public FormNoFamiliesInLibrary(IEnumerable<string> families)
        {
            InitializeComponent();

            string text = string.Join(Environment.NewLine, families);
            richTextBox1.Text = text;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(new ProcessStartInfo
            {
                FileName = "https://bim-starter.com/plugins/rebarsketch/",
                UseShellExecute = true
            });
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}