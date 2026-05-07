using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tools.Shortcuts
{
    public partial class FormAddShortcutsSelect : Form
    {
        public FormAddShortcutsSelect()
        {
            InitializeComponent();
        }

        private void buttonDefaultShortcuts_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Yes;
            this.Close();
        }


        private void buttonCustomShortcuts_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.No;
            this.Close();
        }
    }
}
