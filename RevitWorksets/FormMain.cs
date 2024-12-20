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
using RevitWorksets.WorksetWorkers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Tools.Model.CategoryTools;

namespace RevitWorksets
{
    public partial class FormMain : Form
    {
        public InfosStorage Model { get; set; }

        List<RevitCategory> AllCategories { get; set; }
        public string XmlPath { get; set; }

        private bool flagLinkWorksetEnabled = false;

        public FormMain(InfosStorage newSettings, string xmlPath, List<RevitCategory> allCategories)
        {
            Debug.WriteLine("Initialize main window");
            InitializeComponent();

            string appVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            this.Text = $"{this.Text} v. {appVersion}";

            XmlPath = xmlPath;
            AllCategories = allCategories;
            Model = newSettings;

            BuildWindow();

            CreateButtonColumn(dataGridViewCategories, MyStrings.ColumnCategories, nameof(WorksetByCategory.CategoriesText), Model.worksetsByCategory);
            CreateButtonColumn(dataGridViewFamilies, MyStrings.ColumnFamilies, nameof(WorksetByFamily.FamiliesText), Model.worksetsByFamily);
            CreateButtonColumn(dataGridViewTypes, MyStrings.ColumnTypes, nameof(WorksetByType.TypesText), Model.worksetsByType);

            Debug.WriteLine("Initialized main window");
            flagLinkWorksetEnabled = true;
        }

        private void CreateButtonColumn(DataGridView table, string header, string name, object source)
        {
            DataGridViewButtonColumn buttonColumn = new DataGridViewButtonColumn()
            {
                DataPropertyName = name,
                HeaderText = header,
                Name = name,
                FillWeight = 200,
            };
            table.Columns.Add(buttonColumn);
            table.DataSource = source;
            table.Columns[name].DisplayIndex = table.Columns.Count - 1;
        }


        private void chkbxInGroup_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox curChkBox = sender as CheckBox;
            GroupBox grbx = curChkBox.Parent as GroupBox;
            foreach (Control child in grbx.Controls)
            {
                if (child == curChkBox) continue;
                child.Enabled = curChkBox.Checked;
            }
        }

        private void dataGridView_EnabledChanged(object sender, EventArgs e)
        {
            DataGridView dgv = sender as DataGridView;
            if (!dgv.Enabled)
            {
                dgv.DefaultCellStyle.BackColor = SystemColors.Control;
                dgv.DefaultCellStyle.ForeColor = SystemColors.GrayText;
                dgv.ColumnHeadersDefaultCellStyle.BackColor = SystemColors.Control;
                dgv.ColumnHeadersDefaultCellStyle.ForeColor = SystemColors.GrayText;
                dgv.CurrentCell = null;
                dgv.ReadOnly = true;
                dgv.EnableHeadersVisualStyles = false;
            }
            else
            {
                dgv.DefaultCellStyle.BackColor = SystemColors.Window;
                dgv.DefaultCellStyle.ForeColor = SystemColors.ControlText;
                dgv.ColumnHeadersDefaultCellStyle.BackColor = SystemColors.Window;
                dgv.ColumnHeadersDefaultCellStyle.ForeColor = SystemColors.ControlText;
                dgv.ReadOnly = false;
                dgv.EnableHeadersVisualStyles = true;
            }
        }

        private void FormMain_HelpButtonClicked(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://bim-starter.com/plugins/worksets");
        }


        private void BuildWindow()
        {
            if (string.IsNullOrEmpty(XmlPath))
            {
                toolStripStatusLabel1.Text = MyStrings.ToolStripSettingsFileNotFound;
                XmlPath = Path.Combine(InfosStorage.GetDefaultStorageFolder(), "worksets.xml");
                MakeHelpLabelsVisible(true);
            }
            else
            {
                toolStripStatusLabel1.Text = $"{MyStrings.ToolStripLoadedFromFile}: {XmlPath}";
            }


            checkBoxEnabledByCategory.Checked = Model.WorksetByCategoryEnabled;
            checkBoxEnableByFamilyName.Checked = Model.WorksetByFamilyEnabled;
            checkBoxEnableByType.Checked = Model.WorksetByTypeEnabled;

            checkBoxEnabledByParameter.Checked = Model.WorksetByParameterEnabled;
            textBoxWorksetNameParameter.Text = Model.worksetByParameter.ParameterName;

            checkBoxEnabledForLinkedFiles.Checked = Model.WorksetByLinkEnabled;
            textBoxLinkSeparator.Text = Model.worksetByLink.separator;
            numericLinkPartNumber.Value = Model.worksetByLink.partNumberAfterSeparator;
            numericLinkIgnoreFirstChars.Value = Model.worksetByLink.ignoreFirstCharsAfterSeparation;
            numericLinkIgnoreLastChars.Value = Model.worksetByLink.ignoreLastCharsAfterSeparation;
            textBoxLinkPrefix.Text = Model.worksetByLink.prefixForLinkWorksets;

            checkBoxEnabledForDwgLinks.Checked = Model.WorksetByDwgEnabled;
            textBoxDwgWorksetName.Text = Model.worksetByDwg.WorksetName;

            checkBoxNoEmptyWorksets.Checked = Model.NoEmptyWorksets;

            this.Refresh();
        }

        private void UpdateModel()
        {
            Model.WorksetByCategoryEnabled = checkBoxEnabledByCategory.Checked;
            Model.WorksetByFamilyEnabled = checkBoxEnableByFamilyName.Checked;
            Model.WorksetByTypeEnabled = checkBoxEnableByType.Checked;

            Model.WorksetByParameterEnabled = checkBoxEnabledByParameter.Checked;
            Model.worksetByParameter.ParameterName = textBoxWorksetNameParameter.Text;

            Model.WorksetByLinkEnabled = checkBoxEnabledForLinkedFiles.Checked;
            Model.worksetByLink.separator = textBoxLinkSeparator.Text;
            Model.worksetByLink.partNumberAfterSeparator = (int)numericLinkPartNumber.Value;
            Model.worksetByLink.ignoreFirstCharsAfterSeparation = (int)numericLinkIgnoreFirstChars.Value;
            Model.worksetByLink.ignoreLastCharsAfterSeparation = (int)numericLinkIgnoreLastChars.Value;
            Model.worksetByLink.prefixForLinkWorksets = textBoxLinkPrefix.Text;

            Model.WorksetByDwgEnabled = checkBoxEnabledForDwgLinks.Checked;
            Model.worksetByDwg.WorksetName = textBoxDwgWorksetName.Text;

            Model.NoEmptyWorksets = checkBoxNoEmptyWorksets.Checked;
        }

        private void buttonLoad_Click(object sender, EventArgs e)
        {
            flagLinkWorksetEnabled = false;
            string lastUsedFolder = Path.GetDirectoryName(XmlPath);

            System.Windows.Forms.OpenFileDialog dialog = new System.Windows.Forms.OpenFileDialog();
            dialog.InitialDirectory = lastUsedFolder;
            dialog.Multiselect = false;
            dialog.Filter = "xml files (*.xml)|*.xml|All files (*.*)|*.*";
            if (dialog.ShowDialog() != System.Windows.Forms.DialogResult.OK) return;

            XmlPath = dialog.FileName;

            Model = InfosStorage.LoadFromFile(XmlPath);

            BuildWindow();
            ReloadTablesDatasource();

            InfosStorage.SaveLastUsedXmlPath(XmlPath);
            Debug.WriteLine("Xml path: " + XmlPath);
            flagLinkWorksetEnabled = true;
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            string fileName = Path.GetFileName(XmlPath);
            string lastUsedFolder = Path.GetDirectoryName(XmlPath);

            System.Windows.Forms.SaveFileDialog dialog = new System.Windows.Forms.SaveFileDialog();
            dialog.InitialDirectory = lastUsedFolder;
            dialog.FileName = fileName;
            dialog.Filter = "xml files (*.xml)|*.xml|All files (*.*)|*.*";
            if (dialog.ShowDialog() != System.Windows.Forms.DialogResult.OK) return;

            XmlPath = dialog.FileName;
            UpdateModel();
            Model.Save(XmlPath);
            toolStripStatusLabel1.Text = $"{MyStrings.ToolStripSavedToFile}: {XmlPath}";
        }

        private void buttonReset_Click(object sender, EventArgs e)
        {
            flagLinkWorksetEnabled = false;
            XmlPath = null;
            Model = InfosStorage.GetDefault();
            BuildWindow();
            ReloadTablesDatasource();
            InfosStorage.ClearLastXmlPath();
            MakeHelpLabelsVisible(true);
            toolStripStatusLabel1.Text = MyStrings.ToolStripSettingsReset;
            flagLinkWorksetEnabled = true;
        }

        private void ReloadTablesDatasource()
        {
            dataGridViewCategories.DataSource = Model.worksetsByCategory;
            dataGridViewFamilies.DataSource = Model.worksetsByFamily;
            dataGridViewTypes.DataSource = Model.worksetsByType;
        }


        private void buttonOk_Click(object sender, EventArgs e)
        {
            Debug.WriteLine("OK clicked");

            if (checkBoxEnabledByCategory.Checked
                || checkBoxEnableByType.Checked
                || checkBoxEnableByFamilyName.Checked
                || checkBoxEnabledByParameter.Checked
                || checkBoxEnabledForLinkedFiles.Checked
                || checkBoxEnabledForDwgLinks.Checked)
            {
                UpdateModel();
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show(MyStrings.MessageToCheckboxesChecked);
                return;
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Debug.WriteLine("CANCEL clicked");
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }


        private void textBoxLink_TextChanged(object sender, EventArgs e)
        {
            if (!flagLinkWorksetEnabled) return;
            UpdateModel();
            string sampleFilename = textBoxLinkTestFilename.Text;
            string worksetName = Model.worksetByLink.GetWorksetName(sampleFilename);
            labelLinkTestResult.Text = worksetName;
        }


        private void dataGridViewCategories_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int rowNumber = datagridViewCellClickHandler(sender, e);
            if (rowNumber == -1) return;

            WorksetByCategory clickedRow = Model.worksetsByCategory[rowNumber];
            if (clickedRow.revitCategories == null)
                clickedRow.revitCategories = new List<Autodesk.Revit.DB.BuiltInCategory>();


            Tools.Forms.FormSelectCategories formSelect = new Tools.Forms.FormSelectCategories(clickedRow.revitCategories, AllCategories);
            if (formSelect.ShowDialog() != DialogResult.OK)
                return;

            clickedRow.revitCategories = formSelect.SelectedCategories;
            this.Refresh();
        }


        private void dataGridViewFamilies_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int rowNumber = datagridViewCellClickHandler(sender, e);
            if (rowNumber == -1) return;

            WorksetByFamily clickedRow = Model.worksetsByFamily[rowNumber];
            if (clickedRow.FamilyNames == null) clickedRow.FamilyNames = new List<string>();

            Tools.Forms.FormSelectTextValues formSelect =
                new Tools.Forms.FormSelectTextValues(MyStrings.FormTitleSetFamilyPrefixes, clickedRow.FamilyNames);
            if (formSelect.ShowDialog() != DialogResult.OK) return;

            clickedRow.FamilyNames = formSelect.ResultValues;
            this.Refresh();
        }

        private void dataGridViewByType_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int rowNumber = datagridViewCellClickHandler(sender, e);
            if (rowNumber == -1) return;

            WorksetByType clickedRow = Model.worksetsByType[rowNumber];
            if (clickedRow.TypeNames == null) clickedRow.TypeNames = new List<string>();

            Tools.Forms.FormSelectTextValues formSelect =
                new Tools.Forms.FormSelectTextValues(MyStrings.FormTitleSetTypePrefixes, clickedRow.TypeNames);

            if (formSelect.ShowDialog() != DialogResult.OK) return;

            clickedRow.TypeNames = formSelect.ResultValues;
            this.Refresh();
        }

        private int datagridViewCellClickHandler(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dgv = sender as DataGridView ?? throw new Exception("DataGridView cast error");
            if (dgv.Columns[e.ColumnIndex] is DataGridViewButtonColumn)
            {
                int rowNumber = e.RowIndex;
                return rowNumber;
            }
            return -1;
        }

        private void labelHelp_Click(object sender, EventArgs e)
        {
            MakeHelpLabelsVisible(false);
        }

        private void MakeHelpLabelsVisible(bool visible)
        {
            labelHelp0.Visible = visible;
            labelHelp1.Visible = visible;
            labelHelp2.Visible = visible;
            labelHelp3.Visible = visible;
            labelHelp4.Visible = visible;
            labelHelp5.Visible = visible;
            labelHelp6.Visible = visible;
        }
    }
}
