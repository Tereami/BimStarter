#region License
/*Данный код опубликован под лицензией Creative Commons Attribution-ShareAlike.
Разрешено использовать, распространять, изменять и брать данный код за основу для производных в коммерческих и
некоммерческих целях, при условии указания авторства и если производные лицензируются на тех же условиях.
Код поставляется "как есть". Автор не несет ответственности за возможные последствия использования.
Зуев Александр, 2020, все права защищены.
This code is listed under the Creative Commons Attribution-ShareAlike license.
You may use, redistribute, remix, tweak, and build upon this work non-commercially and commercially,
as long as you credit the author by linking back and license your new creations under the same terms.
This code is provided 'as is'. Author disclaims any implied warranty.
Zuev Aleksandr, 2020, all rigths reserved.*/
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Tools.Model.CategoryTools;

namespace Tools.Forms
{
    public partial class FormSelectCategories : Form
    {
        public List<Autodesk.Revit.DB.BuiltInCategory> SelectedCategories;
        List<RevitCategory> AllCategories;

        public FormSelectCategories(List<Autodesk.Revit.DB.BuiltInCategory> selectedCategories, List<RevitCategory> allCategories)
        {
            InitializeComponent();

            AllCategories = allCategories;
            SelectedCategories = selectedCategories;

            WriteRows(AllCategories, selectedCategories);
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            bool succeed = ReadSelectedRows();

            if (succeed)
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private bool ReadSelectedRows()
        {
            SelectedCategories = checkedListBoxCats
                .CheckedItems
                .Cast<RevitCategory>()
                .Select(c => c.InternalCategory)
                .ToList();

            if (SelectedCategories.Count == 0)
            {
                MessageBox.Show(MyStrings.MessageNoCheckboxesChecked);
                return false;
            }
            return true;
        }

        private void WriteRows(List<RevitCategory> VisibleCategories, List<Autodesk.Revit.DB.BuiltInCategory> SelectedCategories)
        {
            checkedListBoxCats.Items.Clear();
            for (int i = 0; i < VisibleCategories.Count; i++)
            {
                RevitCategory item = VisibleCategories[i];
                checkedListBoxCats.Items.Add(item);

                if (SelectedCategories != null && SelectedCategories.Contains(item.InternalCategory))
                {
                    checkedListBoxCats.SetItemChecked(i, true);
                }
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void checkBoxHide_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox check = (CheckBox)sender;
            ReadSelectedRows();

            if (check.Checked)
            {
                List<RevitCategory> visibleCategories = AllCategories
                    .Where(i => SelectedCategories.Contains(i.InternalCategory))
                    .ToList();
                WriteRows(visibleCategories, SelectedCategories);
            }
            else
            {
                WriteRows(AllCategories, SelectedCategories);
            }
        }
    }
}
