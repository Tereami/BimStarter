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
#region Usings
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Tools.Model;
#endregion

namespace RevitAreaReinforcement
{
    public partial class DialogWindowWall : Form
    {
        public RebarInfoWall wri;

        public DialogWindowWall(RebarInfoWall reinfInfo, List<string> rebarTypes1, List<string> rebarTypes2)
        {
            InitializeComponent();

            wri = reinfInfo;

            checkBoxGenerateHorizontal.Checked = reinfInfo.generateHorizontal;
            txtBoxHorizArmSection.Text = reinfInfo.horizontalSectionText;
            checkBoxGenerateVertical.Checked = reinfInfo.generateVertical;
            txtBoxVertArmSection.Text = reinfInfo.verticalSectionText;

            txtBackOffset.Text = reinfInfo.backOffset.InchesToStringMillimeters();
            txtBottomOffset.Text = reinfInfo.bottomOffset.InchesToStringMillimeters();
            txtTopOffset.Text = reinfInfo.topOffset.InchesToStringMillimeters();

            txtHorizontalInterval.Text = reinfInfo.horizontalRebarInterval.InchesToStringMillimeters();
            txtVerticalInterval.Text = reinfInfo.verticalRebarInterval.InchesToStringMillimeters();

            txtRebarCover.Text = reinfInfo.rebarCover.InchesToStringMillimeters();

            txtVerticalFreeLength.Text = reinfInfo.verticalFreeLength.InchesToStringMillimeters();
            checkBoxAutoVerticalFreeLengh.Checked = wri.autoVerticalFreeLength;
            checkBox_AsymVertFreeLength.Checked = wri.verticalAsymmOffset;
            checkBox_AsymVertFreeLengthTop.Checked = wri.verticalAsymmOffsetTop;
            textBoxVertAsymmLength.Text = wri.verticalAsymmManualLength.InchesToStringMillimeters();
            radioButtonForceUp.Checked = wri.verticalRebarStretched;
            numericUpDownVertFreeLengthRound.Value = (decimal)(wri.verticalFreeLengthRound.InchesToMillimeters());

            txtHorizontalFreeLength.Text = reinfInfo.horizontalFreeLength.InchesToStringMillimeters();
            cmbHorizonalType.DataSource = rebarTypes1;
            cmbVerticalType.DataSource = rebarTypes2;
            cmbHorizonalType.SelectedItem = reinfInfo.horizontalRebarTypeName;
            cmbVerticalType.SelectedItem = reinfInfo.verticalRebarTypeName;

            checkBoxHorizAddInterval.Checked = reinfInfo.horizontalAddInterval;

            checkBoxUnificateLength.Checked = reinfInfo.useUnification;
            textBoxLengths.Text = string.Join(";",
                reinfInfo.lengthsUnification.Select(i => i.InchesToStringMillimeters()));

            checkBoxSkipReinforced.Checked = reinfInfo.SkipAlreadyReinforcedWalls;

            string appVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            this.Text = $"{this.Text} v. {appVersion}";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            wri.generateHorizontal = checkBoxGenerateHorizontal.Checked;
            wri.verticalSectionText = txtBoxVertArmSection.Text;
            wri.generateVertical = checkBoxGenerateVertical.Checked;
            wri.horizontalSectionText = txtBoxHorizArmSection.Text;

            wri.backOffset = txtBackOffset.Text.ParseToInches();
            wri.bottomOffset = txtBottomOffset.Text.ParseToInches();
            wri.topOffset = txtTopOffset.Text.ParseToInches();

            wri.horizontalRebarInterval = txtHorizontalInterval.Text.ParseToInches();
            wri.verticalRebarInterval = txtVerticalInterval.Text.ParseToInches();

            wri.rebarCover = txtRebarCover.Text.ParseToInches();

            wri.verticalFreeLength = txtVerticalFreeLength.Text.ParseToInches();
            wri.autoVerticalFreeLength = checkBoxAutoVerticalFreeLengh.Checked;
            wri.verticalAsymmOffset = checkBox_AsymVertFreeLength.Checked;
            wri.verticalAsymmOffsetTop = checkBox_AsymVertFreeLengthTop.Checked;
            wri.verticalAsymmManualLength = textBoxVertAsymmLength.Text.ParseToInches();
            wri.verticalRebarStretched = radioButtonForceUp.Checked;
            wri.verticalFreeLengthRound = ((double)numericUpDownVertFreeLengthRound.Value) / 304.8;


            wri.horizontalFreeLength = txtHorizontalFreeLength.Text.ParseToInches();
            wri.horizontalRebarTypeName = cmbHorizonalType.Text;
            wri.verticalRebarTypeName = cmbVerticalType.Text;

            wri.horizontalAddInterval = checkBoxHorizAddInterval.Checked;

            wri.useUnification = checkBoxUnificateLength.Checked;
            wri.lengthsUnification = textBoxLengths.Text.Split(';').Select(i => i.ParseToInches()).ToList();

            wri.SkipAlreadyReinforcedWalls = checkBoxSkipReinforced.Checked;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }


        private void checkBoxGenerateVertical_CheckedChanged(object sender, EventArgs e)
        {
            bool chkd = checkBoxGenerateVertical.Checked;

            cmbVerticalType.Enabled = chkd;
            txtBoxVertArmSection.Enabled = chkd;
            txtVerticalFreeLength.Enabled = chkd;
            txtVerticalInterval.Enabled = chkd;
        }

        private void checkBoxGenerateHorizontal_CheckedChanged(object sender, EventArgs e)
        {
            bool chkd = checkBoxGenerateHorizontal.Checked;

            cmbHorizonalType.Enabled = chkd;
            txtBoxHorizArmSection.Enabled = chkd;
            txtTopOffset.Enabled = chkd;
            txtBottomOffset.Enabled = chkd;
            txtHorizontalInterval.Enabled = chkd;
        }

        private void checkBoxUnificateLength_CheckedChanged(object sender, EventArgs e)
        {
            textBoxLengths.Enabled = checkBoxUnificateLength.Checked;
        }

        private void checkBoxAutoVerticalFreeLengh_CheckedChanged(object sender, EventArgs e)
        {
            bool chkd = checkBoxAutoVerticalFreeLengh.Checked;
            txtVerticalFreeLength.Enabled = !chkd;
            radioButtonForceUp.Enabled = chkd;
            radioButtonForceDown.Enabled = chkd;
            numericUpDownVertFreeLengthRound.Enabled = chkd;
            label1.Enabled = chkd;

            if (checkBox_AsymVertFreeLength.Checked)
                textBoxVertAsymmLength.Enabled = !chkd;
        }

        private void checkBox_AsymVertFreeLength_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            bool chkd = checkBox.Checked;
            checkBox_AsymVertFreeLengthTop.Checked = chkd;
            checkBox_AsymVertFreeLengthTop.Enabled = chkd;

            if (!checkBoxAutoVerticalFreeLengh.Checked)
                textBoxVertAsymmLength.Enabled = chkd;
        }
    }
}