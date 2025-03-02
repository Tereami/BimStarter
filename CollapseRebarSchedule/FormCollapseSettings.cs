using System.Windows.Forms;

namespace SchedulesTools
{
    public partial class FormCollapseSettings : Form
    {
        public CollapseScheduleSettings NewSets { get; set; }
        public FormCollapseSettings(CollapseScheduleSettings sets)
        {
            InitializeComponent();

            numericRowsCount.Value = (decimal)sets.HeaderRowsCount;
            textBoxSign.Text = sets.LastColumnSign;
        }

        private void buttonOk_Click(object sender, System.EventArgs e)
        {
            NewSets = new CollapseScheduleSettings
            {
                HeaderRowsCount = (int)numericRowsCount.Value,
                LastColumnSign = textBoxSign.Text,
            };
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void buttonCancel_Click(object sender, System.EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
