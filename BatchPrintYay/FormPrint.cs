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

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Tools.LinksManager;


namespace BatchPrintYay
{
    public partial class FormPrint : System.Windows.Forms.Form
    {
        private YayPrintSettings _printSettings;
        public YayPrintSettings printSettings
        {
            get { return _printSettings; }
        }

        //public List<MySheet> sheetsToPrint = new List<MySheet>();
        //private Dictionary<string, List<MySheet>> sheetsBaseToPrint =
        //    new Dictionary<string, List<MySheet>>();
        List<MyRevitDocument> AllDocuments;

        public List<MyRevitDocument> PrintableDocuments;

        public bool printToFile = false;


        public FormPrint(List<MyRevitDocument> allDocuments, YayPrintSettings printSettings)
        {
            InitializeComponent();
            this.Text += " ver. " + System.IO.File.GetLastWriteTime(System.Reflection.Assembly.GetExecutingAssembly().Location).ToString();
            this.AcceptButton = btnOk;
            this.CancelButton = btnCancel;

            AllDocuments = allDocuments;

            BuildTreeView(AllDocuments);

            //заполняю параметры печати
            _printSettings = printSettings;
            textBoxNameConstructor.Text = printSettings.nameConstructor;
            txtBoxOutputFolder.Text = printSettings.outputFolder;
            checkBoxMergePdfs.Checked = printSettings.mergePdfs;
            checkBoxHideWorkplanes.Checked = printSettings.hideWorkplanes;
            checkBoxHideBoundaries.Checked = printSettings.hideCropBoundaries;
            textBoxNonPrintedLinesPrefix.Text = printSettings.nonPrintedLinesPrefix;


            List<string> printers = new List<string>();
            foreach (string printer in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
            {
                printers.Add(printer);
            }
            if (printers.Count == 0) throw new Exception("Cant find any installed printers");
            comboBoxPrinters.DataSource = printers;

            if (printers.Contains(_printSettings.printerName))
            {
                comboBoxPrinters.SelectedItem = printers.Where(i => i.Equals(_printSettings.printerName)).First();
            }
            else
            {
                string selectedPrinterName = PrinterUtility.GetDefaultPrinter();
                if (!printers.Contains(selectedPrinterName)) throw new Exception("Cant find printer " + selectedPrinterName);
                comboBoxPrinters.SelectedItem = printers.Where(i => i.Equals(selectedPrinterName)).First();
            }



            radioButtonPDF.Checked = comboBoxPrinters.SelectedItem.ToString().Contains("PDF");
            radioButtonPaper.Checked = !radioButtonPDF.Checked;


            if (_printSettings.hiddenLineProcessing == Autodesk.Revit.DB.HiddenLineViewsType.VectorProcessing)
            {
                radioButtonVector.Checked = true;
                radioButtonRastr.Checked = false;
            }
            else
            {
                radioButtonVector.Checked = false;
                radioButtonRastr.Checked = true;
            }

            List<Autodesk.Revit.DB.RasterQualityType> rasterTypes =
                Enum.GetValues(typeof(Autodesk.Revit.DB.RasterQualityType))
                .Cast<Autodesk.Revit.DB.RasterQualityType>()
                .ToList();
            comboBoxRasterQuality.DataSource = rasterTypes;
            try
            {
                comboBoxRasterQuality.SelectedItem = _printSettings.rasterQuality;
                //rasterQualityTypes.Where(i => Enum.GetName(typeof(Autodesk.Revit.DB.RasterQualityType), i).Equals()).First();
            }
            catch { }

            List<ColorType> colorTypes = Enum.GetValues(typeof(ColorType))
                .Cast<ColorType>()
                .ToList();
            comboBoxColors.DataSource = colorTypes;
            comboBoxColors.SelectedItem = _printSettings.colorsType;
            textBoxAlwaysColorParamName.Text = _printSettings.alwaysColorParamName;

            checkBoxExportDwg.Checked = _printSettings.exportToDwg;
            textBoxDwgNameConstructor.Text = _printSettings.dwgNameConstructor;
            List<string> dwgExportSettingsList = _printSettings.dwgProfiles.Select(i => i.Name).ToList();
            comboBoxDwgProfiles.DataSource = dwgExportSettingsList;
            if (dwgExportSettingsList.Contains(_printSettings.selectedDwgExportProfileName))
            {
                comboBoxDwgProfiles.SelectedItem = _printSettings.selectedDwgExportProfileName;
            }
        }

        private void BuildTreeView(List<MyRevitDocument> docs)
        {
            foreach (MyRevitDocument myDoc in docs)
            {
                TreeNode docNode = new TreeNode(myDoc.Name);
                docNode.Tag = myDoc;
                bool haveCheckedSheets = false;
                foreach (MySheet sheet in myDoc.Sheets)
                {
                    string sheetTitle = sheet.ToString();
                    TreeNode sheetNode = new TreeNode(sheetTitle);
                    sheetNode.Tag = sheet;
                    sheetNode.Checked = sheet.IsPrintable;
                    if (sheet.IsPrintable) haveCheckedSheets = true;
                    docNode.Nodes.Add(sheetNode);
                }
                if (haveCheckedSheets) docNode.Expand();
                treeView1.Nodes.Add(docNode);
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;

            PrintableDocuments = new List<MyRevitDocument>();
            foreach (TreeNode docNode in treeView1.Nodes)
            {
                MyRevitDocument myDoc = docNode.Tag as MyRevitDocument;
                //string revitDocTitle = sheetsBaseToPrint.Keys.Where(d => d == docNodeTitle).First();
                myDoc.Sheets.Clear();
                foreach (TreeNode sheetNode in docNode.Nodes)
                {
                    if (!sheetNode.Checked) continue;

                    MySheet sheet = sheetNode.Tag as MySheet;
                    myDoc.Sheets.Add(sheet);
                }
                if (myDoc.Sheets.Count == 0) continue;

                PrintableDocuments.Add(myDoc);
            }


            if (radioButtonVector.Checked)
                _printSettings.hiddenLineProcessing = Autodesk.Revit.DB.HiddenLineViewsType.VectorProcessing;
            //Enum.GetName(typeof(Autodesk.Revit.DB.HiddenLineViewsType), Autodesk.Revit.DB.HiddenLineViewsType.VectorProcessing);
            else
                _printSettings.hiddenLineProcessing = Autodesk.Revit.DB.HiddenLineViewsType.RasterProcessing;
            //Enum.GetName(typeof(Autodesk.Revit.DB.HiddenLineViewsType), Autodesk.Revit.DB.HiddenLineViewsType.RasterProcessing);

            _printSettings.rasterQuality = (Autodesk.Revit.DB.RasterQualityType)comboBoxRasterQuality.SelectedValue;
            //Enum.GetName(typeof(Autodesk.Revit.DB.RasterQualityType), comboBoxRasterQuality.SelectedValue);
            _printSettings.outputFolder = txtBoxOutputFolder.Text;
            _printSettings.printerName = comboBoxPrinters.SelectedItem.ToString();


            bool checkConstructor = false;
            string tempConstr = textBoxNameConstructor.Text;
            if (tempConstr.Split('<').Length > 1)
            {
                if (tempConstr.Split('<')[1].Contains(">"))
                    checkConstructor = true;
            }
            if (checkConstructor)
            {
                _printSettings.nameConstructor = textBoxNameConstructor.Text;
            }
            else
            {
                _printSettings.nameConstructor = MyStrings.DefaultPDFfilename;
            }
            _printSettings.mergePdfs = checkBoxMergePdfs.Checked;
            _printSettings.printToPaper = radioButtonPaper.Checked;
            this.printToFile = radioButtonPDF.Checked;

            //_printSettings.colorStamp = checkBoxColorStamp.Checked;
            //_printSettings.excludeColors = textBoxExcludeColors.Text;

            _printSettings.colorsType = (ColorType)comboBoxColors.SelectedItem;
            _printSettings.alwaysColorParamName = textBoxAlwaysColorParamName.Text;
            _printSettings.useOrientation = checkBoxOrientation.Checked;
            _printSettings.refreshSchedules = checkBoxRefresh.Checked;
            _printSettings.hideWorkplanes = checkBoxHideWorkplanes.Checked;
            _printSettings.hideCropBoundaries = checkBoxHideBoundaries.Checked;
            _printSettings.nonPrintedLinesPrefix = textBoxNonPrintedLinesPrefix.Text;

            _printSettings.exportToDwg = checkBoxExportDwg.Checked;
            _printSettings.dwgNameConstructor = textBoxDwgNameConstructor.Text;
            if (comboBoxDwgProfiles.SelectedItem != null)
                _printSettings.selectedDwgExportProfileName = comboBoxDwgProfiles.SelectedItem.ToString();

            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void buttonBrowse_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbDialog = new FolderBrowserDialog();
            fbDialog.ShowNewFolderButton = true;
            if (fbDialog.ShowDialog() == DialogResult.OK)
            {
                string path = fbDialog.SelectedPath;
                this.txtBoxOutputFolder.Text = path;
            }
        }

        private void btnOpenNameConstructor_Click(object sender, EventArgs e)
        {
            formNameConstructor formName = new BatchPrintYay.formNameConstructor(textBoxNameConstructor.Text);

            if (formName.ShowDialog(this) == DialogResult.OK)
            {
                textBoxNameConstructor.Text = formName.nameConstructor;
            }
            formName.Dispose();
        }

        //private void buttonFormatsSetup_Click(object sender, EventArgs e)
        //{

        //}

        private void radioButtonPDF_CheckedChanged(object sender, EventArgs e)
        {
            bool chkPdf = radioButtonPDF.Checked;

            txtBoxOutputFolder.Enabled = chkPdf;
            buttonBrowse.Enabled = chkPdf;
            textBoxNameConstructor.Enabled = chkPdf;
            btnOpenNameConstructor.Enabled = chkPdf;
            label5.Enabled = chkPdf;
            label6.Enabled = chkPdf;
            checkBoxMergePdfs.Enabled = chkPdf;
            checkBoxOrientation.Enabled = chkPdf;

            if (chkPdf)
            {
                List<ColorType> colorTypes = Enum.GetValues(typeof(ColorType))
                    .Cast<ColorType>()
                    .ToList();
                comboBoxColors.DataSource = colorTypes;
                comboBoxColors.SelectedItem = _printSettings.colorsType;
            }
            else
            {
                List<ColorType> colorTypes = new List<ColorType> { ColorType.Color, ColorType.GrayScale, ColorType.Monochrome };
                comboBoxColors.DataSource = colorTypes;
                comboBoxColors.SelectedItem = ColorType.Monochrome;
            }
        }

        private void checkBoxMergePdfs_CheckedChanged(object sender, EventArgs e)
        {
            bool chk = !checkBoxMergePdfs.Checked;

            textBoxNameConstructor.Enabled = chk;
            btnOpenNameConstructor.Enabled = chk;
            label6.Enabled = chk;

        }

        private void comboBoxColors_SelectedIndexChanged(object sender, EventArgs e)
        {
            ColorType curColorType = (ColorType)comboBoxColors.SelectedItem;
            if (curColorType == ColorType.MonochromeWithExcludes)
            {
                buttonExcludesColor.Enabled = true;
                textBoxAlwaysColorParamName.Enabled = true;
            }
            else
            {
                buttonExcludesColor.Enabled = false;
                textBoxAlwaysColorParamName.Enabled = false;
            }
        }

        private void comboBoxPrinters_SelectedIndexChanged(object sender, EventArgs e)
        {
            radioButtonPDF.Checked = comboBoxPrinters.SelectedItem.ToString().Contains("PDF");
            radioButtonPaper.Checked = !radioButtonPDF.Checked;
        }

        private void buttonHelp_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://bim-starter.com/plugins/batchprint");
        }

        private void radioButtonRastr_CheckedChanged(object sender, EventArgs e)
        {
            comboBoxRasterQuality.Enabled = true;
        }

        private void radioButtonVector_CheckedChanged(object sender, EventArgs e)
        {
            comboBoxRasterQuality.Enabled = false;
        }

        private void buttonExcludesColor_Click(object sender, EventArgs e)
        {
            FormExcludeColors form = new FormExcludeColors(_printSettings.excludeColors);
            if (form.ShowDialog() != DialogResult.OK) return;

            printSettings.excludeColors = form.Colors;
        }

        private void treeView1_AfterCheck(object sender, TreeViewEventArgs e)
        {
            TreeNode parentNode = e.Node;
            foreach (TreeNode child in parentNode.Nodes)
            {
                child.Checked = parentNode.Checked;
            }
        }

        private void checkBoxExportDwg_CheckedChanged(object sender, EventArgs e)
        {
            comboBoxDwgProfiles.Enabled = checkBoxExportDwg.Checked;
            label1.Enabled = checkBoxExportDwg.Checked;
            textBoxDwgNameConstructor.Enabled = checkBoxExportDwg.Checked;
            label7.Enabled = checkBoxExportDwg.Checked;
        }
    }
}
