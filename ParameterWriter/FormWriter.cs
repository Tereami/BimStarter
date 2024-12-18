using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ParameterWriter
{
    public enum WriterMode
    {
        AllInProject,
        OnlySelected,
        OnCurrentView
    }

    public enum SourceMode
    {
        FixValue,
        OtherParameter,
        Constructor,
        Level
    }

    public partial class FormWriter : Form
    {
        //public Dictionary<string, HashSet<object>> valuesBase;
        //public HashSet<string> parametersList;

        //public string paramName;
        //public string source;
        public WriterMode writerMode;
        //public SourceMode sourceMode;
        Dictionary<string, HashSet<string>> values;
        List<string> paramsList = new List<string>();

        private WriterSettings currentSettings;
        public List<WriterSettings> allSettings;
        public bool ShowLog = false;


        public FormWriter(bool haveSelectedElements, Dictionary<string, HashSet<string>> ValuesBase, List<string> levelParameters)
        {
            InitializeComponent();

            values = ValuesBase;
            paramsList = ValuesBase.Keys.ToList(); //parameters.ToList();
            paramsList.Sort();

            currentSettings = new WriterSettings();

            if (haveSelectedElements)
            {
                radioSelectedElements.Checked = true;
                radioSelectedElements.Enabled = false;
                radioViewElements.Enabled = false;
                radioAllElements.Enabled = false;
            }

            comboBox_OtherParameter.DataSource = paramsList.ToList();
            comboBoxTargetParameter.DataSource = paramsList;
            comboBox_Level.DataSource = levelParameters;

            RefreshControls();
        }

        private void RefreshControls()
        {
            comboBoxTargetParameter.Text = currentSettings.targetParamName;
            comboBox_ConstValue.Text = currentSettings.ConstValue;
            comboBox_OtherParameter.Text = currentSettings.sourceParameterName;
            comboBox_Level.Text = currentSettings.levelParamName;
            comboBox_Constructor.Text = currentSettings.constructor;

            switch (currentSettings.sourceMode)
            {
                case SourceMode.FixValue:
                    radioButton_ConstValue.Checked = true;
                    break;
                case SourceMode.OtherParameter:
                    radioButton_OtherParameter.Checked = true;
                    break;
                case SourceMode.Constructor:
                    radioButton_Constructor.Checked = true;
                    break;
                case SourceMode.Level:
                    radioButton_Level.Checked = true;
                    break;
            }
        }

        private void ApplySettings()
        {
            currentSettings.targetParamName = comboBoxTargetParameter.Text;

            if (radioSelectedElements.Checked) writerMode = WriterMode.OnlySelected;
            if (radioViewElements.Checked) writerMode = WriterMode.OnCurrentView;
            if (radioAllElements.Checked) writerMode = WriterMode.AllInProject;

            if (radioButton_ConstValue.Checked)
            {
                currentSettings.sourceMode = SourceMode.FixValue;
                currentSettings.sourceParameterName = comboBox_ConstValue.Text;
            }
            else if (radioButton_OtherParameter.Checked)
            {
                currentSettings.sourceMode = SourceMode.OtherParameter;
                currentSettings.sourceParameterName = comboBox_OtherParameter.Text;
            }
            else if (radioButton_Constructor.Checked)
            {
                currentSettings.sourceMode = SourceMode.Constructor;
                currentSettings.sourceParameterName = comboBox_Constructor.Text;
            }
            else if (radioButton_Level.Checked)
            {
                currentSettings.sourceMode = SourceMode.Level;
                currentSettings.sourceParameterName = comboBox_Level.Text;
            }

            currentSettings.targetParamName = comboBoxTargetParameter.Text;
            currentSettings.ConstValue = comboBox_ConstValue.Text;
            currentSettings.sourceParameterName = comboBox_OtherParameter.Text;
            currentSettings.levelParamName = comboBox_Level.Text;
            currentSettings.constructor = comboBox_Constructor.Text;

            ShowLog = checkBox_ShowLog.Checked;

            //if (!sets.constructorHistory.Contains(comboBox_Constructor.Text))
            //{
            //    sets.constructorHistory.Add(comboBox_Constructor.Text);
            //}
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            ApplySettings();
            allSettings = new List<WriterSettings>();
            allSettings.Add(currentSettings);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void comboBoxParameter_SelectedIndexChanged(object sender, EventArgs e)
        {
            string curParam = comboBoxTargetParameter.SelectedValue.ToString();
            List<string> curValues = values[curParam].ToList(); ;
            curValues.Sort();
            comboBox_ConstValue.DataSource = curValues;
        }

        private void radioButtonWriteValue_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rbtn = sender as RadioButton;
            string suffix = rbtn.Name.Split('_')[1];

            foreach (Control child in groupBoxSourceData.Controls)
            {
                if (child.Name.StartsWith("radio"))
                    continue;
                if (child.Name.EndsWith(suffix))
                    child.Enabled = rbtn.Enabled;
                else
                    child.Enabled = !rbtn.Enabled;
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            ApplySettings();

            SaveFileDialog savedialog = new SaveFileDialog();
            savedialog.Filter = "Config files (*.xml)|*.xml";
            savedialog.FileName = currentSettings.ToString();
            savedialog.RestoreDirectory = true;
            if (savedialog.ShowDialog() != DialogResult.OK)
                return;

            string xmlFileName = savedialog.FileName;
            WriterSettings.Save(currentSettings, xmlFileName);
        }

        private void buttonLoad_Click(object sender, EventArgs e)
        {
            OpenFileDialog opendialog = new OpenFileDialog();
            opendialog.Multiselect = false;
            opendialog.Filter = "Config files (*.xml)|*.xml";
            opendialog.RestoreDirectory = true;
            if (opendialog.ShowDialog() != DialogResult.OK)
                return;

            string xmlFilePath = opendialog.FileName;
            currentSettings = WriterSettings.Load(xmlFilePath);
            RefreshControls();
        }

        private void buttonExecute_Click(object sender, EventArgs e)
        {
            ApplySettings();

            OpenFileDialog opendialog = new OpenFileDialog();
            opendialog.Multiselect = true;
            opendialog.Filter = "Config files (*.xml)|*.xml";
            opendialog.RestoreDirectory = true;
            if (opendialog.ShowDialog() != DialogResult.OK)
                return;

            allSettings = new List<WriterSettings>();
            foreach (string file in opendialog.FileNames)
            {
                WriterSettings ws = WriterSettings.Load(file);
                allSettings.Add(ws);
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
