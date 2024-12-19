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
    public partial class FormInputText : Form
    {
        public string UserText;

        public FormInputText(string headerText)
        {
            InitializeComponent();
            label1.Text = headerText;
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            UserText = textBox1.Text;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            UserText = null;
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
