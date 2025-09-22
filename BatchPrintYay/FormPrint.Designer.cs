namespace BatchPrintYay
{
    partial class FormPrint
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPrint));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textBoxNonPrintedLinesPrefix = new System.Windows.Forms.TextBox();
            this.checkBoxHideBoundaries = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.checkBoxHideWorkplanes = new System.Windows.Forms.CheckBox();
            this.checkBoxRefresh = new System.Windows.Forms.CheckBox();
            this.checkBoxOrientation = new System.Windows.Forms.CheckBox();
            this.checkBoxMergePdfs = new System.Windows.Forms.CheckBox();
            this.radioButtonPDF = new System.Windows.Forms.RadioButton();
            this.radioButtonPaper = new System.Windows.Forms.RadioButton();
            this.btnOpenNameConstructor = new System.Windows.Forms.Button();
            this.textBoxNameConstructor = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.buttonBrowse = new System.Windows.Forms.Button();
            this.txtBoxOutputFolder = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBoxPrinters = new System.Windows.Forms.ComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.radioButtonRastr = new System.Windows.Forms.RadioButton();
            this.radioButtonVector = new System.Windows.Forms.RadioButton();
            this.comboBoxRasterQuality = new System.Windows.Forms.ComboBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.textBoxAlwaysColorParamName = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.buttonExcludesColor = new System.Windows.Forms.Button();
            this.comboBoxColors = new System.Windows.Forms.ComboBox();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.buttonHelp = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.textBoxDwgNameConstructor = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.comboBoxDwgProfiles = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.checkBoxExportDwg = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Controls.Add(this.textBoxNonPrintedLinesPrefix);
            this.groupBox1.Controls.Add(this.checkBoxHideBoundaries);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.checkBoxHideWorkplanes);
            this.groupBox1.Controls.Add(this.checkBoxRefresh);
            this.groupBox1.Controls.Add(this.checkBoxOrientation);
            this.groupBox1.Controls.Add(this.checkBoxMergePdfs);
            this.groupBox1.Controls.Add(this.radioButtonPDF);
            this.groupBox1.Controls.Add(this.radioButtonPaper);
            this.groupBox1.Controls.Add(this.btnOpenNameConstructor);
            this.groupBox1.Controls.Add(this.textBoxNameConstructor);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.buttonBrowse);
            this.groupBox1.Controls.Add(this.txtBoxOutputFolder);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.comboBoxPrinters);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // textBoxNonPrintedLinesPrefix
            // 
            resources.ApplyResources(this.textBoxNonPrintedLinesPrefix, "textBoxNonPrintedLinesPrefix");
            this.textBoxNonPrintedLinesPrefix.Name = "textBoxNonPrintedLinesPrefix";
            // 
            // checkBoxHideBoundaries
            // 
            resources.ApplyResources(this.checkBoxHideBoundaries, "checkBoxHideBoundaries");
            this.checkBoxHideBoundaries.Checked = true;
            this.checkBoxHideBoundaries.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxHideBoundaries.Name = "checkBoxHideBoundaries";
            this.checkBoxHideBoundaries.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // checkBoxHideWorkplanes
            // 
            resources.ApplyResources(this.checkBoxHideWorkplanes, "checkBoxHideWorkplanes");
            this.checkBoxHideWorkplanes.Checked = true;
            this.checkBoxHideWorkplanes.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxHideWorkplanes.Name = "checkBoxHideWorkplanes";
            this.checkBoxHideWorkplanes.UseVisualStyleBackColor = true;
            // 
            // checkBoxRefresh
            // 
            resources.ApplyResources(this.checkBoxRefresh, "checkBoxRefresh");
            this.checkBoxRefresh.Checked = true;
            this.checkBoxRefresh.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxRefresh.Name = "checkBoxRefresh";
            this.checkBoxRefresh.UseVisualStyleBackColor = true;
            // 
            // checkBoxOrientation
            // 
            resources.ApplyResources(this.checkBoxOrientation, "checkBoxOrientation");
            this.checkBoxOrientation.Name = "checkBoxOrientation";
            this.checkBoxOrientation.UseVisualStyleBackColor = true;
            this.checkBoxOrientation.CheckedChanged += new System.EventHandler(this.checkBoxMergePdfs_CheckedChanged);
            // 
            // checkBoxMergePdfs
            // 
            resources.ApplyResources(this.checkBoxMergePdfs, "checkBoxMergePdfs");
            this.checkBoxMergePdfs.Name = "checkBoxMergePdfs";
            this.checkBoxMergePdfs.UseVisualStyleBackColor = true;
            this.checkBoxMergePdfs.CheckedChanged += new System.EventHandler(this.checkBoxMergePdfs_CheckedChanged);
            // 
            // radioButtonPDF
            // 
            resources.ApplyResources(this.radioButtonPDF, "radioButtonPDF");
            this.radioButtonPDF.Checked = true;
            this.radioButtonPDF.Name = "radioButtonPDF";
            this.radioButtonPDF.TabStop = true;
            this.radioButtonPDF.UseVisualStyleBackColor = true;
            this.radioButtonPDF.CheckedChanged += new System.EventHandler(this.radioButtonPDF_CheckedChanged);
            // 
            // radioButtonPaper
            // 
            resources.ApplyResources(this.radioButtonPaper, "radioButtonPaper");
            this.radioButtonPaper.Name = "radioButtonPaper";
            this.radioButtonPaper.UseVisualStyleBackColor = true;
            // 
            // btnOpenNameConstructor
            // 
            resources.ApplyResources(this.btnOpenNameConstructor, "btnOpenNameConstructor");
            this.btnOpenNameConstructor.Name = "btnOpenNameConstructor";
            this.btnOpenNameConstructor.UseVisualStyleBackColor = true;
            this.btnOpenNameConstructor.Click += new System.EventHandler(this.btnOpenNameConstructor_Click);
            // 
            // textBoxNameConstructor
            // 
            resources.ApplyResources(this.textBoxNameConstructor, "textBoxNameConstructor");
            this.textBoxNameConstructor.Name = "textBoxNameConstructor";
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // buttonBrowse
            // 
            resources.ApplyResources(this.buttonBrowse, "buttonBrowse");
            this.buttonBrowse.Name = "buttonBrowse";
            this.buttonBrowse.UseVisualStyleBackColor = true;
            this.buttonBrowse.Click += new System.EventHandler(this.buttonBrowse_Click);
            // 
            // txtBoxOutputFolder
            // 
            resources.ApplyResources(this.txtBoxOutputFolder, "txtBoxOutputFolder");
            this.txtBoxOutputFolder.Name = "txtBoxOutputFolder";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // comboBoxPrinters
            // 
            resources.ApplyResources(this.comboBoxPrinters, "comboBoxPrinters");
            this.comboBoxPrinters.FormattingEnabled = true;
            this.comboBoxPrinters.Name = "comboBoxPrinters";
            this.comboBoxPrinters.SelectedIndexChanged += new System.EventHandler(this.comboBoxPrinters_SelectedIndexChanged);
            // 
            // groupBox2
            // 
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Controls.Add(this.radioButtonRastr);
            this.groupBox2.Controls.Add(this.radioButtonVector);
            this.groupBox2.Controls.Add(this.comboBoxRasterQuality);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // radioButtonRastr
            // 
            resources.ApplyResources(this.radioButtonRastr, "radioButtonRastr");
            this.radioButtonRastr.Name = "radioButtonRastr";
            this.radioButtonRastr.UseVisualStyleBackColor = true;
            this.radioButtonRastr.CheckedChanged += new System.EventHandler(this.radioButtonRastr_CheckedChanged);
            // 
            // radioButtonVector
            // 
            resources.ApplyResources(this.radioButtonVector, "radioButtonVector");
            this.radioButtonVector.Checked = true;
            this.radioButtonVector.Name = "radioButtonVector";
            this.radioButtonVector.TabStop = true;
            this.radioButtonVector.UseVisualStyleBackColor = true;
            this.radioButtonVector.CheckedChanged += new System.EventHandler(this.radioButtonVector_CheckedChanged);
            // 
            // comboBoxRasterQuality
            // 
            resources.ApplyResources(this.comboBoxRasterQuality, "comboBoxRasterQuality");
            this.comboBoxRasterQuality.FormattingEnabled = true;
            this.comboBoxRasterQuality.Items.AddRange(new object[] {
            resources.GetString("comboBoxRasterQuality.Items"),
            resources.GetString("comboBoxRasterQuality.Items1"),
            resources.GetString("comboBoxRasterQuality.Items2"),
            resources.GetString("comboBoxRasterQuality.Items3")});
            this.comboBoxRasterQuality.Name = "comboBoxRasterQuality";
            // 
            // groupBox3
            // 
            resources.ApplyResources(this.groupBox3, "groupBox3");
            this.groupBox3.Controls.Add(this.textBoxAlwaysColorParamName);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.buttonExcludesColor);
            this.groupBox3.Controls.Add(this.comboBoxColors);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.TabStop = false;
            // 
            // textBoxAlwaysColorParamName
            // 
            resources.ApplyResources(this.textBoxAlwaysColorParamName, "textBoxAlwaysColorParamName");
            this.textBoxAlwaysColorParamName.Name = "textBoxAlwaysColorParamName";
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // buttonExcludesColor
            // 
            resources.ApplyResources(this.buttonExcludesColor, "buttonExcludesColor");
            this.buttonExcludesColor.Name = "buttonExcludesColor";
            this.buttonExcludesColor.UseVisualStyleBackColor = true;
            this.buttonExcludesColor.Click += new System.EventHandler(this.buttonExcludesColor_Click);
            // 
            // comboBoxColors
            // 
            resources.ApplyResources(this.comboBoxColors, "comboBoxColors");
            this.comboBoxColors.FormattingEnabled = true;
            this.comboBoxColors.Items.AddRange(new object[] {
            resources.GetString("comboBoxColors.Items"),
            resources.GetString("comboBoxColors.Items1"),
            resources.GetString("comboBoxColors.Items2")});
            this.comboBoxColors.Name = "comboBoxColors";
            this.comboBoxColors.SelectedIndexChanged += new System.EventHandler(this.comboBoxColors_SelectedIndexChanged);
            // 
            // btnOk
            // 
            resources.ApplyResources(this.btnOk, "btnOk");
            this.btnOk.Name = "btnOk";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // treeView1
            // 
            resources.ApplyResources(this.treeView1, "treeView1");
            this.treeView1.CheckBoxes = true;
            this.treeView1.FullRowSelect = true;
            this.treeView1.Name = "treeView1";
            this.treeView1.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterCheck);
            // 
            // buttonHelp
            // 
            resources.ApplyResources(this.buttonHelp, "buttonHelp");
            this.buttonHelp.Name = "buttonHelp";
            this.buttonHelp.UseVisualStyleBackColor = true;
            this.buttonHelp.Click += new System.EventHandler(this.buttonHelp_Click);
            // 
            // groupBox4
            // 
            resources.ApplyResources(this.groupBox4, "groupBox4");
            this.groupBox4.Controls.Add(this.textBoxDwgNameConstructor);
            this.groupBox4.Controls.Add(this.label7);
            this.groupBox4.Controls.Add(this.comboBoxDwgProfiles);
            this.groupBox4.Controls.Add(this.label1);
            this.groupBox4.Controls.Add(this.checkBoxExportDwg);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.TabStop = false;
            // 
            // textBoxDwgNameConstructor
            // 
            resources.ApplyResources(this.textBoxDwgNameConstructor, "textBoxDwgNameConstructor");
            this.textBoxDwgNameConstructor.Name = "textBoxDwgNameConstructor";
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // comboBoxDwgProfiles
            // 
            resources.ApplyResources(this.comboBoxDwgProfiles, "comboBoxDwgProfiles");
            this.comboBoxDwgProfiles.FormattingEnabled = true;
            this.comboBoxDwgProfiles.Name = "comboBoxDwgProfiles";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // checkBoxExportDwg
            // 
            resources.ApplyResources(this.checkBoxExportDwg, "checkBoxExportDwg");
            this.checkBoxExportDwg.Name = "checkBoxExportDwg";
            this.checkBoxExportDwg.UseVisualStyleBackColor = true;
            this.checkBoxExportDwg.CheckedChanged += new System.EventHandler(this.checkBoxExportDwg_CheckedChanged);
            // 
            // FormPrint
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.treeView1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.buttonHelp);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FormPrint";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboBoxPrinters;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton radioButtonRastr;
        private System.Windows.Forms.RadioButton radioButtonVector;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ComboBox comboBoxColors;
        private System.Windows.Forms.ComboBox comboBoxRasterQuality;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button buttonBrowse;
        private System.Windows.Forms.TextBox txtBoxOutputFolder;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnOpenNameConstructor;
        private System.Windows.Forms.TextBox textBoxNameConstructor;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.RadioButton radioButtonPDF;
        private System.Windows.Forms.RadioButton radioButtonPaper;
        private System.Windows.Forms.CheckBox checkBoxMergePdfs;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.CheckBox checkBoxOrientation;
        private System.Windows.Forms.Button buttonHelp;
        private System.Windows.Forms.CheckBox checkBoxRefresh;
        private System.Windows.Forms.Button buttonExcludesColor;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox comboBoxDwgProfiles;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox checkBoxExportDwg;
        private System.Windows.Forms.TextBox textBoxDwgNameConstructor;
        private System.Windows.Forms.TextBox textBoxAlwaysColorParamName;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.CheckBox checkBoxHideBoundaries;
        private System.Windows.Forms.CheckBox checkBoxHideWorkplanes;
        private System.Windows.Forms.TextBox textBoxNonPrintedLinesPrefix;
        private System.Windows.Forms.Label label2;
    }
}