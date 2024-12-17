using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RibbonBimStarter
{
    public partial class FormAddFamily : Form
    {
        public string Imagepath;
        public string FamilyName;
        public string Group;
        public string Munufacturer;
        public string Document;
        public string Description;

        public FormAddFamily(string name, string description, string imagepath, List<string> groups)
        {
            InitializeComponent();

            textBoxFamName.Text = name;
            textBoxDescription.Text = description;
            Imagepath = imagepath;
            pictureBox1.ImageLocation = imagepath;
            comboBoxGroup.DataSource = groups;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            FamilyName = textBoxFamName.Text;
            Group = (string)comboBoxGroup.SelectedValue;
            Munufacturer = textBoxMunufacturer.Text;
            Document = textBoxDocument.Text;
            Description = textBoxDescription.Text;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void buttonReplaceImage_Click(object sender, EventArgs e)
        {
            using (System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog())
            {
                openFileDialog.Multiselect = false;
                openFileDialog.Filter = "JPG files (*.jpg)|*.jpg";

                if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    Imagepath = openFileDialog.FileName;
                    pictureBox1.ImageLocation = openFileDialog.FileName;
                }
            }
        }
    }
}
