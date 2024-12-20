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
using SchedulesTools;
using System;
using System.Windows.Forms;
#endregion

namespace SchedulesTable
{
    public partial class FormTableSettings : Form
    {
        public TableSettings sets;
        public FormTableSettings(TableSettings s)
        {
            InitializeComponent();

            sets = s;

            radioButtonUseComplects.Checked = s.useComplects;
            textBoxComplectParamName.Text = s.sheetComplectParamName;
            checkBoxGetLinkFiles.Checked = s.getLinkFiles;

            radioButtonUseStandardSheetNumber.Checked = s.useStandardSheetNumber;
            textBoxAltSheetParamName.Text = s.altSheetNumberParam;
            numericRowHeight.Value = (decimal)(s.rowHeight * 304.8);
            numericMaxChars.Value = s.maxCharsInOneLine;
            textBoxNewLineSymbol.Text = s.newLineSymbol;
            numericRowCoeff.Value = (decimal)s.rowHeightCoeff;
        }

        private void radioButtonUseComplects_CheckedChanged(object sender, EventArgs e)
        {
            textBoxComplectParamName.Enabled = radioButtonUseComplects.Checked;
        }

        private void radioButtonCustomSheetNumber_CheckedChanged(object sender, EventArgs e)
        {
            textBoxAltSheetParamName.Enabled = radioButtonCustomSheetNumber.Checked;
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            sets.useComplects = radioButtonUseComplects.Checked;
            sets.sheetComplectParamName = textBoxComplectParamName.Text;
            sets.getLinkFiles = checkBoxGetLinkFiles.Checked;

            sets.useStandardSheetNumber = radioButtonUseStandardSheetNumber.Checked;
            sets.altSheetNumberParam = textBoxAltSheetParamName.Text;
            sets.rowHeight = ((double)numericRowHeight.Value) / 304.8;
            sets.maxCharsInOneLine = (int)numericMaxChars.Value;
            sets.newLineSymbol = textBoxNewLineSymbol.Text;
            sets.rowHeightCoeff = (double)numericRowCoeff.Value;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://bim-starter.com/plugins/schedulestable/");
        }
    }
}
