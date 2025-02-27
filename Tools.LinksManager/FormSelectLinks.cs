using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Tools.LinksManager
{
    public partial class FormSelectLinks : System.Windows.Forms.Form
    {
        public List<MyRevitLinkDocument> selectedLinks;
        private bool AllowMultiselect = false;
        public FormSelectLinks(List<MyRevitLinkDocument> links, bool allowMultiselect)
        {
            InitializeComponent();
            AllowMultiselect = allowMultiselect;
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
            if (checkedListBox1.CheckedItems.Count == 0)
            {
                MessageBox.Show("Please choose links / Выберите файлы");
                return;
            }
            selectedLinks = checkedListBox1.CheckedItems.Cast<MyRevitLinkDocument>().ToList();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void checkedListBox1_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (AllowMultiselect) return;

            for (int i = 0; i < checkedListBox1.Items.Count; ++i)
            {
                if (i != e.Index) checkedListBox1.SetItemChecked(i, false);
            }
        }
    }
}