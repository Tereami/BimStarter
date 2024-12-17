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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
#endregion

namespace ClearUnusedGUIDs
{
    public partial class FormAddParameters : Form
    {
        public List<MyExParamDefinition> defs;
        public bool isInstance;

        public MyParameterGroup selectedGroup;


        public FormAddParameters(List<MyExParamDefinition> Defs, List<MyParameterGroup> Groups)
        {
            InitializeComponent();
            defs = Defs;
            listBoxParams.DataSource = defs;
#if R2017 || R2018 || R2019 || R2020 || R2021 || R2022 || R2023
            comboBoxGrouping.DataSource = Groups;
            comboBoxGrouping.SelectedItem = Groups.Find(i => i.Group == Autodesk.Revit.DB.BuiltInParameterGroup.PG_DATA);
#else
            comboBoxGrouping.SelectedItem = Groups.Find(i => i.Group == Autodesk.Revit.DB.GroupTypeId.Data);
#endif
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            defs = listBoxParams.SelectedItems.Cast<MyExParamDefinition>().ToList();
            isInstance = radioButtonByInstance.Checked;

            selectedGroup = (MyParameterGroup)comboBoxGrouping.SelectedItem;

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
