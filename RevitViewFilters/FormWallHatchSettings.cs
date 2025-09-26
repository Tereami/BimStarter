using System;
using System.Windows.Forms;

namespace RevitViewFilters
{
    public partial class FormWallHatchSettings : Form
    {
        public WallHatchSettings Sets;
        public FormWallHatchSettings(WallHatchSettings sets)
        {
            InitializeComponent();

            Sets = sets;

            checkBoxUseBaseLevel.Checked = sets.UseBaseLevel;
            checkBoxUseHeight.Checked = sets.UseHeight;
            checkBoxUseType.Checked = sets.UseType;
            checkBoxUseThickness.Checked = sets.UseThickness;

            textBoxHatchPrefix.Text = sets.HatchPrefix;
            textBoxImagePrefix.Text = sets.ImagePrefix;
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            Sets.UseBaseLevel = checkBoxUseBaseLevel.Checked;
            Sets.UseHeight = checkBoxUseHeight.Checked;
            Sets.UseType = checkBoxUseType.Checked;
            Sets.UseThickness = checkBoxUseThickness.Checked;

            Sets.HatchPrefix = textBoxHatchPrefix.Text;
            Sets.ImagePrefix = textBoxImagePrefix.Text;

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