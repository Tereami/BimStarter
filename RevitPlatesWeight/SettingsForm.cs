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
Zuev Aleksandr, 2020, all rigths reserved.

More about solution / Подробнее: http://weandrevit.ru/plagin-massa-plastin-km/
*/
#endregion

using System;
using System.Windows.Forms;

namespace RevitPlatesWeight
{
    public partial class SettingsForm : Form
    {
        public Settings newSets;
        public SettingsForm(Settings set, string xmlpath)
        {
            InitializeComponent();

            this.textBoxProfileName.Text = set.ProfileNameValue;
            this.numGroupConstr.Value = set.GroupConstParamValue;
            this.numElemType.Value = set.ElementTypeParamValue;
            this.numWeightType.Value = set.ElementWeightTypeValue;
            this.checkBoxRewrite.Checked = set.Rewrite;
            this.checkBoxWriteThickName.Checked = set.writeThickName;
            this.checkBoxWriteThickness.Checked = set.writeThickvalue;
            this.textBoxPlatePrefix.Text = set.platePrefix;
            this.checkBoxWritePlateLengthWidth.Checked = set.writePlatesLengthWidth;
            this.checkBoxEnablePlateNumbering.Checked = set.enablePlatesNumbering;
            this.textBoxPlateNumberingParamName.Text = set.plateNumberingParamName;
            this.numericUpDownNumberingStartWith.Value = set.plateNumberingStartWith;
            this.checkBoxWriteBeamsLength.Checked = set.writeBeamLength;
            this.checkBoxWriteColumnsLength.Checked = set.writeColumnLength;
            this.radioOnlyCurrentView.Checked = set.useOnlyVisibleOnCurrentView;

#if R2019
            checkBoxWritePlateLengthWidth.Enabled = false;
            checkBoxWritePlateLengthWidth.Enabled = false;
#endif

            this.textBoxGroupConstParamName.Text = set.GroupConstParamName;
            this.textBoxElementTypeParamName.Text = set.ElementTypeParamName;
            this.textBoxElementWeightTypeParamName.Text = set.ElementWeightTypeParamName;
            this.textBoxWeightParamName.Text = set.WeightParamName;
            this.textBoxMaterialNameParam.Text = set.MaterialNameParam;
            this.textBoxVolumeParamName.Text = set.VolumeParamName;
            this.textBoxProfileNameParamName.Text = set.ProfileNameParamName;
            this.textBoxPlateNameParamName.Text = set.PlateNameParamName;
            this.textBoxThicknessParamName.Text = set.ThicknessParamName;
            this.textBoxPlateLengthParamName.Text = set.PlateLengthParamName;
            this.textBoxPlateWidthParamName.Text = set.PlateWidthParamName;
            this.textBoxLengthCorrectedParamName.Text = set.LengthCorrectedParamName;


            this.labelPath.Text = xmlpath;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            newSets = new Settings()
            {
                ProfileNameValue = textBoxProfileName.Text,
                GroupConstParamValue = (int)numGroupConstr.Value,
                ElementTypeParamValue = (int)numElemType.Value,
                ElementWeightTypeValue = (int)numWeightType.Value,
                writeThickName = checkBoxWriteThickName.Checked,
                writeThickvalue = checkBoxWriteThickness.Checked,
                platePrefix = textBoxPlatePrefix.Text,
                writePlatesLengthWidth = checkBoxWritePlateLengthWidth.Checked,
                enablePlatesNumbering = checkBoxEnablePlateNumbering.Checked,
                plateNumberingParamName = textBoxPlateNumberingParamName.Text,
                plateNumberingStartWith = (int)numericUpDownNumberingStartWith.Value,
                writeBeamLength = checkBoxWriteBeamsLength.Checked,
                writeColumnLength = checkBoxWriteColumnsLength.Checked,
                Rewrite = checkBoxRewrite.Checked,
                useOnlyVisibleOnCurrentView = radioOnlyCurrentView.Checked,

                GroupConstParamName = textBoxGroupConstParamName.Text,
                ElementTypeParamName = textBoxElementTypeParamName.Text,
                ElementWeightTypeParamName = textBoxElementWeightTypeParamName.Text,
                WeightParamName = textBoxWeightParamName.Text,
                MaterialNameParam = textBoxMaterialNameParam.Text,
                VolumeParamName = textBoxVolumeParamName.Text,
                ProfileNameParamName = textBoxProfileNameParamName.Text,
                PlateNameParamName = textBoxPlateNameParamName.Text,
                ThicknessParamName = textBoxThicknessParamName.Text,
                PlateLengthParamName = textBoxPlateLengthParamName.Text,
                PlateWidthParamName = textBoxPlateWidthParamName.Text,
                LengthCorrectedParamName = textBoxLengthCorrectedParamName.Text
            };



            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void buttonReset_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Retry;
            this.Close();
        }
    }
}
