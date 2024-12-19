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
using System.Windows.Forms;
#endregion

namespace RevitViewFilters
{

    public partial class FormSelectParameterForFilters : Form
    {
        public List<MyParameter> parameters;
        public MyParameter selectedParameter;
        public CriteriaType criteriaType;
        public ColorizeMode colorizeMode;
        public int startSymbols;
        public bool colorLines;
        public bool colorFill;


        public FormSelectParameterForFilters()
        {
            InitializeComponent();

            string ver = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            this.Text += " ver." + ver;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void FormSelectParameterForFilters_Load(object sender, EventArgs e)
        {
            comboBoxParameters.DataSource = parameters;
        }

        private void buttonNext_Click(object sender, EventArgs e)
        {
            if (radioButtonCheckHostMark.Checked)
            {
                this.colorizeMode = ColorizeMode.CheckHostmark;
            }
            else if (radioButtonResetColors.Checked)
            {
                this.colorizeMode = ColorizeMode.ResetColors;
            }
            else if (radioButtonUserParameter.Checked)
            {
                this.colorizeMode = ColorizeMode.ByParameter;
                selectedParameter = comboBoxParameters.SelectedItem as MyParameter;
                if (selectedParameter == null) throw new Exception("Выбран не MyParameter");


                if (radioButtonEquals.Checked) criteriaType = CriteriaType.Equals;
                if (radioButtonStartsWith.Checked) criteriaType = CriteriaType.StartsWith;
                startSymbols = (int)numericStartSymbols.Value;
            }

            this.colorLines = checkBoxColorLines.Checked;
            this.colorFill = checkBoxColorFill.Checked;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void radioButtonEquals_CheckedChanged(object sender, EventArgs e)
        {
            numericStartSymbols.Enabled = false;
            labelSymbols.Enabled = false;
        }

        private void radioButtonStartsWith_CheckedChanged(object sender, EventArgs e)
        {
            numericStartSymbols.Enabled = true;
            labelSymbols.Enabled = true;

        }

        private void radioButtonUserParameter_CheckedChanged(object sender, EventArgs e)
        {
            groupBox1.Enabled = true;
        }

        private void radioButtonCheckHostMark_CheckedChanged(object sender, EventArgs e)
        {
            groupBox1.Enabled = false;
        }
    }
}
