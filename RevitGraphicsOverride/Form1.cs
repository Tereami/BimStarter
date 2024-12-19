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
using System.Windows.Forms;

namespace RevitGraphicsOverride
{
    public partial class Form1 : Form
    {
        public bool searchAllProject = false;
        public bool selectElems = true;
        public bool showResults = false;

        public Form1()
        {
            InitializeComponent();
            this.AcceptButton = buttonOk;
            this.CancelButton = buttonCancel;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            searchAllProject = radioButtonAllProject.Checked;
            selectElems = checkBoxSelectElements.Checked;
            showResults = checkBoxShowResults.Checked;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
