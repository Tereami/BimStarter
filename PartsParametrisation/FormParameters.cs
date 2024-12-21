using System;
using System.Windows.Forms;

namespace PartsParametrisation
{
    public partial class FormParameters : Form
    {
        public PartSettings userSettings;
        public FormParameters(PartSettings sets)
        {
            InitializeComponent();
            userSettings = sets;
            dataGridView1.DataSource = sets.Parameters;

            string appVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            this.Text = $"{this.Text} v. {appVersion}";
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
