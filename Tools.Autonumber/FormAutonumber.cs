using System;
using System.Windows.Forms;

namespace Tools.Autonumber
{
    public partial class FormAutonumber : Form
    {
        public int StartNumber;
        public int EndNumber;
        public bool SuppressTooltips;
        public int Speed;

        public FormAutonumber()
        {
            InitializeComponent();

            string appVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            this.Text = $"{this.Text} v. {appVersion}";
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            StartNumber = (int)numericStart.Value;
            EndNumber = (int)numericEnd.Value;
            SuppressTooltips = checkBoxSuppressTooltips.Checked;
            Speed = trackBarSpeed.Value;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
