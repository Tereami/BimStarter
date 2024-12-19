using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ElementId = Autodesk.Revit.DB.ElementId;

namespace RevitViewFilters
{
    public partial class FormSelectCategories : Form
    {
        public List<ElementId> checkedCategoriesIds;

        public FormSelectCategories(Autodesk.Revit.DB.Document doc, List<MyCategory> categories)
        {
            InitializeComponent();

            foreach (MyCategory mycat in categories)
            {
                checkedListBox1.Items.Add(mycat, CheckState.Checked);
            }

            string ver = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            this.Text += " ver." + ver;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            checkedCategoriesIds = new List<ElementId>();

            foreach (var checkedItem in checkedListBox1.CheckedItems)
            {
                MyCategory mycat = (MyCategory)checkedItem;

                checkedCategoriesIds.Add(new ElementId(mycat.BuiltinCat));
            }

            this.DialogResult = DialogResult.OK;
        }
    }
}
