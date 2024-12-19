using System;
using System.Windows.Forms;

namespace RevitAreaReinforcement
{
    public partial class DialogWindowWallAuto : Form
    {
        public RebarInfoWall rebarInfo;
        public DialogWindowWallAuto(RebarInfoWall riw)
        {
            InitializeComponent();

            rebarInfo = riw;

            radioButtonFreeLengthAuto.Checked = riw.autoVerticalFreeLength;
            numericUpDownVertFreeLengthRound.Value = (decimal)(304.8 * riw.verticalFreeLengthRound);
            checkBox_AsymmVertFreeLength.Checked = rebarInfo.verticalAsymmOffset;
            checkBox_VertRebarStretched.Checked = rebarInfo.verticalRebarStretched;

            numericUpDownHorizOffset.Value = (decimal)(304.8 * riw.bottomOffset);
            checkBoxIncreasedIntervalTopOrBottom.Checked = riw.horizontalIntervalIncreasedTopOrBottom;

            textBoxRazdelVert.Text = riw.verticalSectionText;
            textBoxRazdelHoris.Text = riw.horizontalSectionText;

            checkBoxSkipAlreadyReinforced.Checked = riw.SkipAlreadyReinforcedWalls;
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            rebarInfo.autoVerticalFreeLength = radioButtonFreeLengthAuto.Checked;
            rebarInfo.verticalFreeLengthRound = ((double)numericUpDownVertFreeLengthRound.Value) / 304.8;
            rebarInfo.verticalAsymmOffset = checkBox_AsymmVertFreeLength.Checked;
            rebarInfo.verticalRebarStretched = checkBox_VertRebarStretched.Checked;

            rebarInfo.bottomOffset = ((double)numericUpDownHorizOffset.Value) / 304.8;
            rebarInfo.horizontalIntervalIncreasedTopOrBottom = checkBoxIncreasedIntervalTopOrBottom.Checked;

            rebarInfo.verticalSectionText = textBoxRazdelVert.Text;
            rebarInfo.horizontalSectionText = textBoxRazdelHoris.Text;

            rebarInfo.SkipAlreadyReinforcedWalls = checkBoxSkipAlreadyReinforced.Checked;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void radioButtonFreeLengthAuto_CheckedChanged(object sender, EventArgs e)
        {
            bool chkd = radioButtonFreeLengthAuto.Checked;
            checkBox_AsymmVertFreeLength.Enabled = chkd;
            checkBox_VertRebarStretched.Enabled = chkd;
        }
    }
}
