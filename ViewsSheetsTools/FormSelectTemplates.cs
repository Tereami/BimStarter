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

namespace ViewsSheetsTools
{
    public partial class FormSelectTemplates : Form
    {
        public List<MyView> templates;
        public List<MyView> selectedTemplates;

        public FormSelectTemplates(List<MyView> Templates)
        {
            InitializeComponent();
            templates = Templates;
            listBox1.DataSource = templates;

        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            selectedTemplates = new List<MyView>();
            foreach (object item in listBox1.SelectedItems)
            {
                MyView selView = item as MyView;
                if (selView == null) throw new Exception("Это не вид!");

                selectedTemplates.Add(selView);
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
