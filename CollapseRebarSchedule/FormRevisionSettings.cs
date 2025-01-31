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
using System.Linq;
using System.Windows.Forms;
#endregion

namespace SchedulesTools
{
    public partial class FormRevisionSettings : Form
    {
        public FormRevisionSettings()
        {
            InitializeComponent();

            checkBoxUseCloudsCount.Checked = Settings.UseCloudsCount;
            checkBoxUseRevisionsOnThisSheet.Checked = Settings.UseRevisionsOnThisSheet;
            checkBoxUseGroupingRevisions.Checked = Settings.UseGroupingRevisions;

            comboBoxRevisionParameter.DataSource = Settings.revParams;
            comboBoxRevisionParameter.SelectedItem = Settings.revParams
                .Where(i => i.ToString() == Settings.revisionParam.ToString())
                .First();

            comboBoxSheetNumberParameter.DataSource = Settings.SheetNumberParameters;
            comboBoxSheetNumberParameter.Text = Settings.SheetNumberParameter;

            comboBoxSheetNote.Text = Settings.SheetNoteParam;
            comboBoxSheetsForLastRevision.Text = Settings.SheetsForLastRevision;

            radioUseStandartDescription.Checked = Settings.UseStandartRevisionDescription;
            radioUseParamForDescription.Checked = Settings.UseParameterForRevisionDescription;
            textBoxSheetRevDescParam.Text = Settings.SheetRevisionDescriptionParameter;

            string appVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            this.Text = $"{this.Text} v.{appVersion}";
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            Settings.UseCloudsCount = checkBoxUseCloudsCount.Checked;
            Settings.UseRevisionsOnThisSheet = checkBoxUseRevisionsOnThisSheet.Checked;
            Settings.UseGroupingRevisions = checkBoxUseGroupingRevisions.Checked;

            Settings.revisionParam = comboBoxRevisionParameter.SelectedItem as RevisionParameter;
            Settings.SheetNumberParameter = comboBoxSheetNumberParameter.Text;

            Settings.SheetNoteParam = comboBoxSheetNote.Text;
            Settings.SheetsForLastRevision = comboBoxSheetsForLastRevision.Text;

            Settings.UseStandartRevisionDescription = radioUseStandartDescription.Checked;
            Settings.UseParameterForRevisionDescription = radioUseParamForDescription.Checked;
            Settings.SheetRevisionDescriptionParameter = textBoxSheetRevDescParam.Text;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void radioUseParamForDescription_CheckedChanged(object sender, EventArgs e)
        {
            textBoxSheetRevDescParam.Enabled = true;
        }

        private void radioUseStandartDescription_CheckedChanged(object sender, EventArgs e)
        {
            textBoxSheetRevDescParam.Enabled = false;
        }
    }
}
