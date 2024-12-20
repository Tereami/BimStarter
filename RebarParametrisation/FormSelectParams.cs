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
#region Usings
using System;
using System.Windows.Forms;
#endregion

namespace RebarParametrisation
{
    public partial class FormSelectParams : Form
    {
        public RebarParametrisationSettings userSettings;
        public FormSelectParams(RebarParametrisationSettings sets)
        {
            InitializeComponent();
            chkboxHostid.Checked = sets.UseHostId;
            chkboxUniqId.Checked = sets.UseUniqueHostId;
            chkboxHostMark.Checked = sets.UseHostMark;
            chkboxUseThickness.Checked = sets.UseHostThickness;

            chkboxRebarWeight.Checked = sets.UseRebarWeight;
            chkboxRebarLength.Checked = sets.UseRebarLength;
            chkboxRebarDiameter.Checked = sets.UseRebarDiameter;

            cmbboxConcreteClass.Text = sets.DefaultConcreteClass.ToString("F0");

            switch (sets.LinkFilesSetting)
            {
                case ProcessedLinkFiles.NoLinks:
                    radioNoLinks.Checked = true;
                    break;
                case ProcessedLinkFiles.OnlyLibs:
                    radioOnlyLibs.Checked = true;
                    break;
                case ProcessedLinkFiles.AllKrFiles:
                    radioAllKrLinks.Checked = true;
                    break;
            }

        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            double concreteClass = double.Parse(cmbboxConcreteClass.Text);
            userSettings = new RebarParametrisationSettings
            {
                UseHostId = chkboxHostid.Checked,
                UseUniqueHostId = chkboxUniqId.Checked,
                UseHostMark = chkboxHostMark.Checked,
                UseHostThickness = chkboxUseThickness.Checked,

                UseRebarWeight = chkboxRebarWeight.Checked,
                UseRebarLength = chkboxRebarLength.Checked,
                UseRebarDiameter = chkboxRebarDiameter.Checked,

                DefaultConcreteClass = concreteClass
            };

            if (radioNoLinks.Checked == true) userSettings.LinkFilesSetting = ProcessedLinkFiles.NoLinks;
            if (radioOnlyLibs.Checked == true) userSettings.LinkFilesSetting = ProcessedLinkFiles.OnlyLibs;
            if (radioAllKrLinks.Checked == true) userSettings.LinkFilesSetting = ProcessedLinkFiles.AllKrFiles;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void chkboxHostid_CheckedChanged(object sender, EventArgs e)
        {
            textBoxHostId.Enabled = chkboxHostid.Checked;
        }

        private void chkboxUniqId_CheckedChanged(object sender, EventArgs e)
        {
            textBoxHostUniqId.Enabled = chkboxUniqId.Checked;
        }

        private void chkboxRebarWeight_CheckedChanged(object sender, EventArgs e)
        {
            textBoxWeight.Enabled = chkboxRebarWeight.Checked;
        }

        private void chkboxRebarLength_CheckedChanged(object sender, EventArgs e)
        {
            textBoxLength.Enabled = chkboxRebarLength.Checked;
        }

        private void chkboxRebarDiameter_CheckedChanged(object sender, EventArgs e)
        {
            textBoxDiameter.Enabled = chkboxRebarDiameter.Checked;
        }

        private void chkboxHostMark_CheckedChanged(object sender, EventArgs e)
        {
            textBoxHostMark.Enabled = chkboxHostMark.Checked;
        }

        private void chkboxUseThickness_CheckedChanged(object sender, EventArgs e)
        {
            textBoxHostThickness.Enabled = chkboxUseThickness.Checked;
        }
    }
}
