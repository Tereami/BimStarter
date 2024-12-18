using System;
using System.Windows.Forms;

namespace Tools.Forms
{
    public partial class FormLog : Form
    {
        public FormLog(string text)
        {
            InitializeComponent();
            richTextBox1.Text = text;
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
