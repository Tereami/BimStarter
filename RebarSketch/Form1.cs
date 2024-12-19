using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using System.IO;
using System.Drawing;

namespace RebarSketch
{
    public partial class Form1 : Form
    {
        public const int IconWidth = 150;
        public const int IconHeight = 90;

        public string executionFolder;
        //public string templateImagePath;
        public string curTempImagePath = "";

        private GlobalSettings sets;
        private XmlSketchItem activeTemplate;
        private List<XmlSketchItem> allTemplates;

        public Form1(string libraryPath)
        {
            InitializeComponent();
            imageList1.ImageSize = new Size(IconWidth, IconHeight);

            ScetchLibrary lib = new ScetchLibrary();
            lib.Activate(App.libraryPath);

            allTemplates = lib.templates;

            for(int i = 0; i< allTemplates.Count; i++)
            {
                XmlSketchItem xsi = allTemplates[i];
                AddScetchIconToList(xsi);
            }

            sets = GlobalSettings.Read();
            executionFolder = libraryPath;
            string dllVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            this.Text = $"{MyStrings.TitleSketchConstructor} {dllVersion}";
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            UpdateTemplateByForm();

            foreach (XmlSketchItem xsi in allTemplates)
            {
                xsi.Save();
            }

            CloseAndDispose();
            this.Close();
        }

        private void UpdateTemplateByForm()
        {
            if (activeTemplate == null) return;
            activeTemplate.parameters = new List<ScetchParameter>();

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.IsNewRow) continue;
                var cells = row.Cells;

                if (cells[0].Value == null) cells[0].Value = MyStrings.RebarAparam; //Rbr_A
                if (cells[1].Value == null) cells[1].Value = MyStrings.RebarAparam;
                if (cells[2].Value == null) cells[2].Value = sets.defaultFontSize;
                if (cells[3].Value == null) cells[3].Value = 100;
                if (cells[4].Value == null) cells[4].Value = 200;
                if (cells[5].Value == null) cells[5].Value = 0;
                if (cells[6].Value == null) cells[6].Value = false;
                if (cells[7].Value == null) cells[7].Value = sets.defautLengthAccuracy;
                if (cells[8].Value == null) cells[8].Value = true;
                if (cells[9].Value == null) cells[9].Value = "";
                if (cells[10].Value == null) cells[10].Value = "";


                ScetchParameter sparam = new ScetchParameter();

                sparam.Name = row.Cells[0].Value.ToString();
                sparam.value = row.Cells[1].Value.ToString();
                sparam.FontSize = float.Parse(cells[2].Value.ToString());
                sparam.PositionX = float.Parse(cells[3].Value.ToString());
                sparam.PositionY = float.Parse(cells[4].Value.ToString());
                sparam.Rotation = float.Parse(cells[5].Value.ToString());
                sparam.IsNarrow = (bool)cells[6].Value;
                sparam.LengthAccuracy = double.Parse(cells[7].Value.ToString());
                sparam.ShowVariableLengthInterval = (bool)cells[8].Value;
                sparam.Prefix = row.Cells[9].Value.ToString();
                sparam.Suffix = row.Cells[10].Value.ToString();

                activeTemplate.parameters.Add(sparam);
            }

            activeTemplate.families = new List<string>();

            foreach (string fam in richTextBoxFamilies.Lines)
            {
                activeTemplate.families.Add(fam);
            }
        }


        private void RefreshImage()
        {
            if (activeTemplate == null) return;
            
            string newTempImage = ScetchImage.GenerateTemporary(sets, activeTemplate.templateImagePath, activeTemplate.parameters);

            pictureBox1.Load(newTempImage);

            if (!string.IsNullOrEmpty(curTempImagePath))
            {
                System.IO.File.Delete(curTempImagePath);
            }

            curTempImagePath = newTempImage;
        }

        private void ActivateControls()
        {
            dataGridView1.Enabled = true;
            richTextBoxFamilies.Enabled = true;
            btnSave.Enabled = true;
        }

        private void CloseAndDispose()
        {
            pictureBox1.Dispose();
            if (!string.IsNullOrEmpty(curTempImagePath))
            {
                System.IO.File.Delete(curTempImagePath);
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            CloseAndDispose();
        }

        private void buttonOpenLibraryFolder_Click(object sender, EventArgs e)
        {
            if (System.IO.Directory.Exists(App.libraryPath))
            {
                System.Diagnostics.Process.Start(App.libraryPath);
            }
            else
            {
                MessageBox.Show($"{MyStrings.ErrorFolderNotFound}: {App.libraryPath}");
            }
        }


        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            UpdateTemplateByForm();
            this.RefreshImage();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://bim-starter.com/plugins/rebarsketch/");
        }

        private void buttonSettings_Click(object sender, EventArgs e)
        {
            FormGlobalSettings formSettings = new FormGlobalSettings(sets);
            if (formSettings.ShowDialog() != DialogResult.OK)
                return;

            sets = formSettings.newSettings;
            GlobalSettings.Save(sets);
            if (activeTemplate != null)
            {
                UpdateTemplateByForm();
                RefreshImage();
            }
        }

        private void dataGridView1_UserDeletedRow(object sender, DataGridViewRowEventArgs e)
        {
            UpdateTemplateByForm();
            RefreshImage();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            CloseAndDispose();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListView lv = sender as ListView;
            if (lv.SelectedItems.Count == 0) return;

            UpdateTemplateByForm();

            ListViewItem item = lv.SelectedItems[0];

            string path = item.ImageKey;

            activeTemplate = item.Tag as XmlSketchItem;

            this.ActivateControls();
            dataGridView1.Rows.Clear();

            for (int i = 0; i < activeTemplate.parameters.Count; i++)
            {
                ScetchParameter sp = activeTemplate.parameters[i];
                sp.value = sp.Name;
                dataGridView1.Rows.Add(sp.Name, sp.Name, sp.FontSize,
                    sp.PositionX, sp.PositionY, sp.Rotation, sp.IsNarrow, sp.LengthAccuracy, sp.ShowVariableLengthInterval, sp.Prefix, sp.Suffix);
            }

            richTextBoxFamilies.Lines = activeTemplate.families.ToArray();

            this.RefreshImage();
        }

        private void buttonNewForm_Click(object sender, EventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.InitialDirectory = executionFolder;
            openDialog.Title = MyStrings.SelectImage;
            openDialog.Multiselect = false;
            openDialog.Filter = "PNG images(*.png)| *.png";
            if (openDialog.ShowDialog() != DialogResult.OK)
                return;
            string templateImagePath = openDialog.FileName;

            FormInputText inputForm = new FormInputText(MyStrings.NewFormName);
            if (inputForm.ShowDialog() != DialogResult.OK) return;
            string newFormName = inputForm.UserText;

            foreach(XmlSketchItem xsi in allTemplates)
            {
                if(xsi.formName == newFormName)
                {
                    MessageBox.Show(MyStrings.NameIsAlreadyInUse);
                    return;
                }
            }

            string newFormDirectory = Path.Combine(executionFolder, newFormName);

            try
            {
                Directory.CreateDirectory(newFormDirectory);
            }
            catch
            {
                throw new Exception("FAILED TO CREATE FOLDER! CHECK USER PERMISSIONS " + newFormDirectory);
            }

            Bitmap bmp = new Bitmap(templateImagePath);
            Bitmap newBmp = new Bitmap(bmp.Width, bmp.Height, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
            using (Graphics gr = Graphics.FromImage(newBmp))
            {
                gr.DrawImage(bmp, new Rectangle(0, 0, bmp.Width, bmp.Height));
            }

            string newFormImagePath = Path.Combine(newFormDirectory, "scetch.png");
            newBmp.Save(newFormImagePath, System.Drawing.Imaging.ImageFormat.Png);

            XmlSketchItem newTemplate = new XmlSketchItem();
            newTemplate.families = new List<string> { "Rebar form name", "Rebar family name" };
            newTemplate.folder = newFormDirectory;
            newTemplate.formName = newFormName;
            
            ScetchParameter newParam = new ScetchParameter();
            newParam.FontSize = sets.defaultFontSize;
            newParam.LengthAccuracy = sets.defautLengthAccuracy;
            newParam.value = newParam.Name;
            newTemplate.parameters = new List<ScetchParameter> { newParam };

            newTemplate.templateImagePath = newFormImagePath;
            allTemplates.Add(newTemplate);
            AddScetchIconToList(newTemplate);

            newTemplate.Save();

            int newItemNumber = listView1.Items.Count - 1;
            listView1.Select();
            listView1.Items[newItemNumber].Focused = true;
            listView1.Items[newItemNumber].Selected = true;
            listView1.Items[newItemNumber].EnsureVisible();
        }

        private Bitmap ResizeBitmap(Bitmap sourceBitmap)
        {
            Bitmap newImage = new Bitmap(IconWidth, IconHeight);
            using (Graphics gr = Graphics.FromImage(newImage))
            {
                gr.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                gr.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                gr.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                gr.DrawImage(sourceBitmap, new Rectangle(0, 0, IconWidth, IconHeight));
            }
            return newImage;
        }

        private void AddScetchIconToList(XmlSketchItem xsi)
        {
            string imagepath = Path.Combine(xsi.folder, "scetch.png");
            string folderName = xsi.formName;

            Bitmap sourceBitmap = new Bitmap(imagepath);

            Bitmap newImage = ResizeBitmap(sourceBitmap);
            imageList1.Images.Add(imagepath, newImage);

            string title = folderName;
            if (xsi.IsSubtype) title += " (subtype)";

            ListViewItem newRow = listView1.Items.Add(imagepath, title, imagepath);
            newRow.ToolTipText = imagepath;
            newRow.Tag = xsi;
        }
    }
}
