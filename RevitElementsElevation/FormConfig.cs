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
using System.Windows.Forms;
#endregion

namespace RevitElementsElevation
{
    public partial class FormConfig : Form
    {
        private Config config;
        public FormConfig(ref Config cfg)
        {
            InitializeComponent();

            config = cfg;

            txtBoxLevelElevation.Text = cfg.paramBaseLevel;
            txtBoxElevationFromLevel.Text = cfg.paramElevOnLevel;
            checkBoxWallAndColumns.Checked = config.useWallAndColumns;
            textBoxTopElevParamName.Text = config.paramTopElevName;
            textBoxBottomElevParamName.Text = config.paramBottomElevName;
            checkBoxElevIsCurrency.Checked = config.elevIsCurrency;

            this.AcceptButton = btnOk;
            this.CancelButton = btnCancel;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            config.paramBaseLevel = txtBoxLevelElevation.Text;
            config.paramElevOnLevel = txtBoxElevationFromLevel.Text;
            config.useWallAndColumns = checkBoxWallAndColumns.Checked;
            config.paramTopElevName = textBoxTopElevParamName.Text;
            config.paramBottomElevName = textBoxBottomElevParamName.Text;
            config.elevIsCurrency = checkBoxElevIsCurrency.Checked;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void checkBoxWallAndColumns_CheckedChanged(object sender, EventArgs e)
        {
            textBoxTopElevParamName.Enabled = checkBoxWallAndColumns.Checked;
            textBoxBottomElevParamName.Enabled = checkBoxWallAndColumns.Checked;
        }
    }
}
