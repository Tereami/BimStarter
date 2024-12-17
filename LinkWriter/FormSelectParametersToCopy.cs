using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace LinkWriter
{
    public partial class FormSelectParametersToCopy : Form
    {
        public WriteLinkSettings ValuesSettings;

        public FormSelectParametersToCopy(WriteLinkSettings settings)
        {
            InitializeComponent();

            checkedListBoxSheet.Items.AddRange(settings.SheetParams.ToArray());
            checkedListBoxProject.Items.AddRange(settings.ProjectParams.ToArray());
            checkedListBoxTitleblock.Items.AddRange(settings.TitleblockParams.ToArray());
            checkedListBoxType.Items.AddRange(settings.TypeParams.ToArray());
        }

        private void buttonCancel_Click(object sender, System.EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void buttonNext_Click(object sender, System.EventArgs e)
        {
            List<MyParameterValue> sheetParameters = checkedListBoxSheet.SelectedItems.Cast<MyParameterValue>().ToList();
            List<MyParameterValue> projectParameters = checkedListBoxProject.SelectedItems.Cast<MyParameterValue>().ToList();
            List<MyParameterValue> titleblockParams = checkedListBoxTitleblock.SelectedItems.Cast<MyParameterValue>().ToList();
            List<MyParameterValue> typeParams = checkedListBoxType.SelectedItems.Cast<MyParameterValue>().ToList();

            ValuesSettings = new WriteLinkSettings(sheetParameters, projectParameters, titleblockParams, typeParams);

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
