using System.Windows.Forms;

namespace RibbonBimStarter
{
    public partial class FormUpdateTemplate : Form
    {
        public FormUpdateTemplate()
        {
            InitializeComponent();
        }

        private void buttonCancel_Click(object sender, System.EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void buttonNext_Click(object sender, System.EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
