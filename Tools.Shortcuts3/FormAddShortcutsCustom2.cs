using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tools.Shortcuts
{
    public partial class FormAddShortcutsCustom2 : Form
    {
        string xmlPath;
        public FormAddShortcutsCustom2(string xmlPath)
        {
            InitializeComponent();
            textBoxXmlPath.Text = xmlPath;
            this.xmlPath = xmlPath;
        }

        private void buttonCopy_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(xmlPath);
            buttonCopy.Text = "Copied!";
        }
    }
}
