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

namespace RebarTools
{
    public partial class FormDialog : Form
    {
        public bool rebarAsBodyActivate = true;
        public bool rebarAsbodyOn = true;
        public bool rebarOverlayActivate = false;
        public bool rebarIsUnobsqured = false;

        public FormDialog()
        {
            InitializeComponent();
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            rebarAsBodyActivate = checkBoxBody.Checked;
            rebarAsbodyOn = radioButtonAsBodyOn.Checked;
            rebarOverlayActivate = checkBoxOverlay.Checked;
            rebarIsUnobsqured = radioButtonOverlayOn.Checked;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void checkBoxBody_CheckedChanged(object sender, EventArgs e)
        {
            radioButtonAsBodyOn.Enabled = checkBoxBody.Checked;
            radioButtonAsBodyOff.Enabled = checkBoxBody.Checked;
        }

        private void checkBoxOverlay_CheckedChanged(object sender, EventArgs e)
        {
            radioButtonOverlayOff.Enabled = checkBoxOverlay.Checked;
            radioButtonOverlayOn.Enabled = checkBoxOverlay.Checked;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
