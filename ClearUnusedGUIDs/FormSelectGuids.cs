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
#region usings
using System;
using System.Collections.Generic;
using System.Windows.Forms;
#endregion

namespace ClearUnusedGUIDs
{
    public partial class FormSelectGuids : Form
    {
        public List<int> selectedIds = new List<int>();

        public FormSelectGuids(List<MyParameterDefinition> myParams)
        {
            InitializeComponent();
            this.AcceptButton = buttonOk;
            this.CancelButton = buttonCancel;
            //this.listBox1.DataSource = myParams;

            foreach (MyParameterDefinition myparam in myParams)
            {
                dataGridView1.Rows.Add(myparam.id, myparam.paramName, myparam.guid);
            }
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView1.SelectedRows)
            {
                DataGridViewCell cell = row.Cells[0];
                int selId = (int)cell.Value;
                selectedIds.Add(selId);
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
