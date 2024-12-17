using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Tools.LinksManager;

namespace LinkWriter
{
    public partial class FormSelectLinks : System.Windows.Forms.Form
    {
        public List<MyRevitLinkDocument> selectedLinks;
        public FormSelectLinks(List<MyRevitLinkDocument> links)
        {
            InitializeComponent();

            checkedListBox1.Items.Clear();
            checkedListBox1.Items.AddRange(links.ToArray());
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void buttonNext_Click(object sender, EventArgs e)
        {
            selectedLinks = checkedListBox1.CheckedItems.Cast<MyRevitLinkDocument>().ToList();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}