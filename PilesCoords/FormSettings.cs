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
using System.Windows.Forms;
#endregion

namespace PilesCoords
{
    public partial class FormSettings : Form
    {
        public Settings newSets = null;

        public FormSettings(Settings sets)
        {
            InitializeComponent();

            newSets = sets;

            radioNumberingDown.Checked = sets.numberingUpDown;
            radioNumberingUp.Checked = !sets.numberingUpDown;
            numericFirstNumber.Value = (decimal)sets.firstNumber;

            txtPileFamilyName.Text = sets.pileFamilyName;
            numPileDepth.Value = (decimal)sets.pileDepth;

            checkBoxSortByPileType_FirstTable.Checked = sets.sortByPileType_Table1;
            checkBoxSortByUses_FirstTable.Checked = sets.sortByPileUses_Table1;
            checkBoxSortByBottomElev_FirstTable.Checked = sets.sortByBottomElev_Table1;
            checkBoxSortByTopElev_FirstTable.Checked = sets.sortByTopElev_Table1;
            checkBoxSortByCutLength_FirstTable.Checked = sets.sortByCutLength_Table1;
            checkBoxSortBySlabElev_FirstTable.Checked = sets.sortBySlabElev_Table1;

            checkBoxSortByPileType_Table2.Checked = sets.sortByPileType_Table2;
            checkBoxSortByUses_Table2.Checked = sets.sortByPileUses_Table2;
            checkBoxSortByBottomElev_Table2.Checked = sets.sortByBottomElev_Table2;
            checkBoxSortByTopElev_Table2.Checked = sets.sortByTopElev_Table2;
            checkBoxSortByCutLength_Table2.Checked = sets.sortByCutLength_Table2;
            checkBoxSortBySlabElev_Table2.Checked = sets.sortBySlabElev_Table2;

            textBoxPilePosition.Text = sets.paramPilePosition;
            textBoxPileLength.Text = sets.paramPileLength;
            textBoxLengthAfterCut.Text = sets.paramPileLengthAfterCut;
            textBoxRange.Text = sets.paramRange;
            textBoxRangeWithElevation.Text = sets.paramRangeWithElevation;
            textBoxSlabBottomElev.Text = sets.paramSlabBottomElev;
            textBoxPileCutLength.Text = sets.paramPileCutHeigth;
            textBoxPlacementElevation.Text = sets.paramPlacementElevation;
            textBoxPileTypeNumber.Text = sets.paramPileTypeNumber;
        }


        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            newSets.numberingUpDown = radioNumberingDown.Checked;
            newSets.pileFamilyName = txtPileFamilyName.Text;
            newSets.firstNumber = (int)numericFirstNumber.Value;

            newSets.sortByPileType_Table1 = checkBoxSortByPileType_FirstTable.Checked;
            newSets.sortByPileUses_Table1 = checkBoxSortByUses_FirstTable.Checked;
            newSets.sortByBottomElev_Table1 = checkBoxSortByBottomElev_FirstTable.Checked;
            newSets.sortByTopElev_Table1 = checkBoxSortByTopElev_FirstTable.Checked;
            newSets.sortByCutLength_Table1 = checkBoxSortByCutLength_FirstTable.Checked;
            newSets.sortBySlabElev_Table1 = checkBoxSortBySlabElev_FirstTable.Checked;

            newSets.sortByPileType_Table2 = checkBoxSortByPileType_Table2.Checked;
            newSets.sortByPileUses_Table2 = checkBoxSortByUses_Table2.Checked;
            newSets.sortByBottomElev_Table2 = checkBoxSortByBottomElev_Table2.Checked;
            newSets.sortByTopElev_Table2 = checkBoxSortByTopElev_Table2.Checked;
            newSets.sortByCutLength_Table2 = checkBoxSortByCutLength_Table2.Checked;
            newSets.sortBySlabElev_Table2 = checkBoxSortBySlabElev_Table2.Checked;
            newSets.pileDepth = (double)numPileDepth.Value;

            newSets.paramPilePosition = textBoxPilePosition.Text;
            newSets.paramPileLength = textBoxPileLength.Text;
            newSets.paramPileLengthAfterCut = textBoxLengthAfterCut.Text;
            newSets.paramRange = textBoxRange.Text;
            newSets.paramRangeWithElevation = textBoxRangeWithElevation.Text;
            newSets.paramSlabBottomElev = textBoxSlabBottomElev.Text;
            newSets.paramPileCutHeigth = textBoxPileCutLength.Text;
            newSets.paramPlacementElevation = textBoxPlacementElevation.Text;
            newSets.paramPileTypeNumber = textBoxPileTypeNumber.Text;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }


        private void buttonHelp_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://bim-starter.com/plugins/piles/");
        }
    }
}
