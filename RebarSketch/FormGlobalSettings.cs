using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RebarSketch
{
    public partial class FormGlobalSettings : Form
    {
        public GlobalSettings newSettings;

        public FormGlobalSettings(GlobalSettings sets)
        {
            InitializeComponent();

            textBoxTempPath.Text = sets.tempPath;

            using (System.Drawing.Text.InstalledFontCollection col 
                = new System.Drawing.Text.InstalledFontCollection())
            {
                foreach (FontFamily fa in col.Families)
                {
                    comboBoxFontName.Items.Add(fa.Name);
                }
            }
            comboBoxFontName.Text = sets.fontName;

            comboBoxFontStyle.DataSource = Enum.GetValues(typeof(FontStyle));
            comboBoxFontStyle.SelectedItem = sets.fontStyle;

            numericFontSize.Value = (decimal)sets.defaultFontSize;
            numericLinesSpacing.Value = (decimal)sets.linesSpacing;

            numericRound.Value = (decimal)sets.defautLengthAccuracy;
            textBoxImageParamName.Text = sets.imageParamName;
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            newSettings = new GlobalSettings();

            newSettings.tempPath = textBoxTempPath.Text;
            newSettings.fontName = comboBoxFontName.Text;
            newSettings.fontStyle =  (FontStyle)comboBoxFontStyle.SelectedItem;
            newSettings.defaultFontSize = (int)numericFontSize.Value;
            newSettings.linesSpacing = (float)numericLinesSpacing.Value;
            newSettings.defautLengthAccuracy = (double)numericRound.Value;
            newSettings.imageParamName = textBoxImageParamName.Text;

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
