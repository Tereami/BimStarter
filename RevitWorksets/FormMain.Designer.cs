namespace RevitWorksets
{
    partial class FormMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.buttonOk = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonSave = new System.Windows.Forms.Button();
            this.buttonLoad = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.labelHelp1 = new System.Windows.Forms.Label();
            this.dataGridViewCategories = new System.Windows.Forms.DataGridView();
            this.checkBoxEnabledByCategory = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.labelHelp2 = new System.Windows.Forms.Label();
            this.dataGridViewFamilies = new System.Windows.Forms.DataGridView();
            this.checkBoxEnableByFamilyName = new System.Windows.Forms.CheckBox();
            this.labelHelp6 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxWorksetNameParameter = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.checkBoxEnabledByParameter = new System.Windows.Forms.CheckBox();
            this.buttonHelp = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.labelHelp5 = new System.Windows.Forms.Label();
            this.labelHelp4 = new System.Windows.Forms.Label();
            this.labelLinkTestResult = new System.Windows.Forms.Label();
            this.numericLinkIgnoreLastChars = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.numericLinkIgnoreFirstChars = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.numericLinkPartNumber = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxLinkTestFilename = new System.Windows.Forms.TextBox();
            this.textBoxLinkPrefix = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.textBoxLinkSeparator = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.checkBoxEnabledForLinkedFiles = new System.Windows.Forms.CheckBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.textBoxDwgWorksetName = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.checkBoxEnabledForDwgLinks = new System.Windows.Forms.CheckBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.dataGridViewTypes = new System.Windows.Forms.DataGridView();
            this.checkBoxEnableByType = new System.Windows.Forms.CheckBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.labelHelp3 = new System.Windows.Forms.Label();
            this.buttonReset = new System.Windows.Forms.Button();
            this.labelHelp0 = new System.Windows.Forms.Label();
            this.checkBoxNoEmptyWorksets = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewCategories)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewFamilies)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericLinkIgnoreLastChars)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericLinkIgnoreFirstChars)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericLinkPartNumber)).BeginInit();
            this.groupBox5.SuspendLayout();
            this.groupBox6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTypes)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonOk
            // 
            resources.ApplyResources(this.buttonOk, "buttonOk");
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.UseVisualStyleBackColor = true;
            // 
            // buttonCancel
            // 
            resources.ApplyResources(this.buttonCancel, "buttonCancel");
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // buttonSave
            // 
            resources.ApplyResources(this.buttonSave, "buttonSave");
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.UseVisualStyleBackColor = true;
            // 
            // buttonLoad
            // 
            resources.ApplyResources(this.buttonLoad, "buttonLoad");
            this.buttonLoad.Name = "buttonLoad";
            this.buttonLoad.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Controls.Add(this.labelHelp1);
            this.groupBox1.Controls.Add(this.dataGridViewCategories);
            this.groupBox1.Controls.Add(this.checkBoxEnabledByCategory);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // labelHelp1
            // 
            resources.ApplyResources(this.labelHelp1, "labelHelp1");
            this.labelHelp1.AutoEllipsis = true;
            this.labelHelp1.BackColor = System.Drawing.Color.Yellow;
            this.labelHelp1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelHelp1.Name = "labelHelp1";
            // 
            // dataGridViewCategories
            // 
            resources.ApplyResources(this.dataGridViewCategories, "dataGridViewCategories");
            this.dataGridViewCategories.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewCategories.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewCategories.Name = "dataGridViewCategories";
            // 
            // checkBoxEnabledByCategory
            // 
            resources.ApplyResources(this.checkBoxEnabledByCategory, "checkBoxEnabledByCategory");
            this.checkBoxEnabledByCategory.Checked = true;
            this.checkBoxEnabledByCategory.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxEnabledByCategory.Name = "checkBoxEnabledByCategory";
            this.checkBoxEnabledByCategory.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Controls.Add(this.labelHelp2);
            this.groupBox2.Controls.Add(this.dataGridViewFamilies);
            this.groupBox2.Controls.Add(this.checkBoxEnableByFamilyName);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // labelHelp2
            // 
            resources.ApplyResources(this.labelHelp2, "labelHelp2");
            this.labelHelp2.AutoEllipsis = true;
            this.labelHelp2.BackColor = System.Drawing.Color.Yellow;
            this.labelHelp2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelHelp2.Name = "labelHelp2";
            // 
            // dataGridViewFamilies
            // 
            resources.ApplyResources(this.dataGridViewFamilies, "dataGridViewFamilies");
            this.dataGridViewFamilies.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewFamilies.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewFamilies.Name = "dataGridViewFamilies";
            // 
            // checkBoxEnableByFamilyName
            // 
            resources.ApplyResources(this.checkBoxEnableByFamilyName, "checkBoxEnableByFamilyName");
            this.checkBoxEnableByFamilyName.Checked = true;
            this.checkBoxEnableByFamilyName.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxEnableByFamilyName.Name = "checkBoxEnableByFamilyName";
            this.checkBoxEnableByFamilyName.UseVisualStyleBackColor = true;
            // 
            // labelHelp6
            // 
            resources.ApplyResources(this.labelHelp6, "labelHelp6");
            this.labelHelp6.AutoEllipsis = true;
            this.labelHelp6.BackColor = System.Drawing.Color.Yellow;
            this.labelHelp6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelHelp6.Name = "labelHelp6";
            // 
            // groupBox3
            // 
            resources.ApplyResources(this.groupBox3, "groupBox3");
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.textBoxWorksetNameParameter);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Controls.Add(this.checkBoxEnabledByParameter);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.TabStop = false;
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // textBoxWorksetNameParameter
            // 
            resources.ApplyResources(this.textBoxWorksetNameParameter, "textBoxWorksetNameParameter");
            this.textBoxWorksetNameParameter.Name = "textBoxWorksetNameParameter";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // checkBoxEnabledByParameter
            // 
            resources.ApplyResources(this.checkBoxEnabledByParameter, "checkBoxEnabledByParameter");
            this.checkBoxEnabledByParameter.Checked = true;
            this.checkBoxEnabledByParameter.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxEnabledByParameter.Name = "checkBoxEnabledByParameter";
            this.checkBoxEnabledByParameter.UseVisualStyleBackColor = true;
            // 
            // buttonHelp
            // 
            resources.ApplyResources(this.buttonHelp, "buttonHelp");
            this.buttonHelp.Name = "buttonHelp";
            this.buttonHelp.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            resources.ApplyResources(this.groupBox4, "groupBox4");
            this.groupBox4.Controls.Add(this.labelHelp5);
            this.groupBox4.Controls.Add(this.labelHelp4);
            this.groupBox4.Controls.Add(this.labelLinkTestResult);
            this.groupBox4.Controls.Add(this.numericLinkIgnoreLastChars);
            this.groupBox4.Controls.Add(this.label7);
            this.groupBox4.Controls.Add(this.numericLinkIgnoreFirstChars);
            this.groupBox4.Controls.Add(this.label6);
            this.groupBox4.Controls.Add(this.numericLinkPartNumber);
            this.groupBox4.Controls.Add(this.label4);
            this.groupBox4.Controls.Add(this.textBoxLinkTestFilename);
            this.groupBox4.Controls.Add(this.textBoxLinkPrefix);
            this.groupBox4.Controls.Add(this.label10);
            this.groupBox4.Controls.Add(this.label9);
            this.groupBox4.Controls.Add(this.label8);
            this.groupBox4.Controls.Add(this.textBoxLinkSeparator);
            this.groupBox4.Controls.Add(this.label5);
            this.groupBox4.Controls.Add(this.checkBoxEnabledForLinkedFiles);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.TabStop = false;
            // 
            // labelHelp5
            // 
            resources.ApplyResources(this.labelHelp5, "labelHelp5");
            this.labelHelp5.AutoEllipsis = true;
            this.labelHelp5.BackColor = System.Drawing.Color.Yellow;
            this.labelHelp5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelHelp5.Name = "labelHelp5";
            // 
            // labelHelp4
            // 
            resources.ApplyResources(this.labelHelp4, "labelHelp4");
            this.labelHelp4.AutoEllipsis = true;
            this.labelHelp4.BackColor = System.Drawing.Color.Yellow;
            this.labelHelp4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelHelp4.Name = "labelHelp4";
            // 
            // labelLinkTestResult
            // 
            resources.ApplyResources(this.labelLinkTestResult, "labelLinkTestResult");
            this.labelLinkTestResult.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelLinkTestResult.Name = "labelLinkTestResult";
            // 
            // numericLinkIgnoreLastChars
            // 
            resources.ApplyResources(this.numericLinkIgnoreLastChars, "numericLinkIgnoreLastChars");
            this.numericLinkIgnoreLastChars.Name = "numericLinkIgnoreLastChars";
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // numericLinkIgnoreFirstChars
            // 
            resources.ApplyResources(this.numericLinkIgnoreFirstChars, "numericLinkIgnoreFirstChars");
            this.numericLinkIgnoreFirstChars.Name = "numericLinkIgnoreFirstChars";
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // numericLinkPartNumber
            // 
            resources.ApplyResources(this.numericLinkPartNumber, "numericLinkPartNumber");
            this.numericLinkPartNumber.Name = "numericLinkPartNumber";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // textBoxLinkTestFilename
            // 
            resources.ApplyResources(this.textBoxLinkTestFilename, "textBoxLinkTestFilename");
            this.textBoxLinkTestFilename.Name = "textBoxLinkTestFilename";
            // 
            // textBoxLinkPrefix
            // 
            resources.ApplyResources(this.textBoxLinkPrefix, "textBoxLinkPrefix");
            this.textBoxLinkPrefix.Name = "textBoxLinkPrefix";
            // 
            // label10
            // 
            resources.ApplyResources(this.label10, "label10");
            this.label10.Name = "label10";
            // 
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // textBoxLinkSeparator
            // 
            resources.ApplyResources(this.textBoxLinkSeparator, "textBoxLinkSeparator");
            this.textBoxLinkSeparator.Name = "textBoxLinkSeparator";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // checkBoxEnabledForLinkedFiles
            // 
            resources.ApplyResources(this.checkBoxEnabledForLinkedFiles, "checkBoxEnabledForLinkedFiles");
            this.checkBoxEnabledForLinkedFiles.Checked = true;
            this.checkBoxEnabledForLinkedFiles.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxEnabledForLinkedFiles.Name = "checkBoxEnabledForLinkedFiles";
            this.checkBoxEnabledForLinkedFiles.UseVisualStyleBackColor = true;
            // 
            // groupBox5
            // 
            resources.ApplyResources(this.groupBox5, "groupBox5");
            this.groupBox5.Controls.Add(this.textBoxDwgWorksetName);
            this.groupBox5.Controls.Add(this.label13);
            this.groupBox5.Controls.Add(this.checkBoxEnabledForDwgLinks);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.TabStop = false;
            // 
            // textBoxDwgWorksetName
            // 
            resources.ApplyResources(this.textBoxDwgWorksetName, "textBoxDwgWorksetName");
            this.textBoxDwgWorksetName.Name = "textBoxDwgWorksetName";
            // 
            // label13
            // 
            resources.ApplyResources(this.label13, "label13");
            this.label13.Name = "label13";
            // 
            // checkBoxEnabledForDwgLinks
            // 
            resources.ApplyResources(this.checkBoxEnabledForDwgLinks, "checkBoxEnabledForDwgLinks");
            this.checkBoxEnabledForDwgLinks.Checked = true;
            this.checkBoxEnabledForDwgLinks.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxEnabledForDwgLinks.Name = "checkBoxEnabledForDwgLinks";
            this.checkBoxEnabledForDwgLinks.UseVisualStyleBackColor = true;
            // 
            // groupBox6
            // 
            resources.ApplyResources(this.groupBox6, "groupBox6");
            this.groupBox6.Controls.Add(this.dataGridViewTypes);
            this.groupBox6.Controls.Add(this.checkBoxEnableByType);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.TabStop = false;
            // 
            // dataGridViewTypes
            // 
            resources.ApplyResources(this.dataGridViewTypes, "dataGridViewTypes");
            this.dataGridViewTypes.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewTypes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewTypes.Name = "dataGridViewTypes";
            // 
            // checkBoxEnableByType
            // 
            resources.ApplyResources(this.checkBoxEnableByType, "checkBoxEnableByType");
            this.checkBoxEnableByType.Checked = true;
            this.checkBoxEnableByType.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxEnableByType.Name = "checkBoxEnableByType";
            this.checkBoxEnableByType.UseVisualStyleBackColor = true;
            // 
            // statusStrip1
            // 
            resources.ApplyResources(this.statusStrip1, "statusStrip1");
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Name = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            resources.ApplyResources(this.toolStripStatusLabel1, "toolStripStatusLabel1");
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            // 
            // labelHelp3
            // 
            resources.ApplyResources(this.labelHelp3, "labelHelp3");
            this.labelHelp3.AutoEllipsis = true;
            this.labelHelp3.BackColor = System.Drawing.Color.Yellow;
            this.labelHelp3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelHelp3.Name = "labelHelp3";
            // 
            // buttonReset
            // 
            resources.ApplyResources(this.buttonReset, "buttonReset");
            this.buttonReset.Name = "buttonReset";
            this.buttonReset.UseVisualStyleBackColor = true;
            // 
            // labelHelp0
            // 
            resources.ApplyResources(this.labelHelp0, "labelHelp0");
            this.labelHelp0.AutoEllipsis = true;
            this.labelHelp0.BackColor = System.Drawing.Color.Yellow;
            this.labelHelp0.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelHelp0.Name = "labelHelp0";
            // 
            // checkBoxNoEmptyWorksets
            // 
            resources.ApplyResources(this.checkBoxNoEmptyWorksets, "checkBoxNoEmptyWorksets");
            this.checkBoxNoEmptyWorksets.Name = "checkBoxNoEmptyWorksets";
            this.checkBoxNoEmptyWorksets.UseVisualStyleBackColor = true;
            // 
            // FormMain
            // 
            this.AcceptButton = this.buttonOk;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.Controls.Add(this.checkBoxNoEmptyWorksets);
            this.Controls.Add(this.labelHelp0);
            this.Controls.Add(this.labelHelp6);
            this.Controls.Add(this.labelHelp3);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.buttonHelp);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.buttonReset);
            this.Controls.Add(this.buttonLoad);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOk);
            this.HelpButton = true;
            this.Name = "FormMain";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewCategories)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewFamilies)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericLinkIgnoreLastChars)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericLinkIgnoreFirstChars)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericLinkPartNumber)).EndInit();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTypes)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.Button buttonLoad;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox checkBoxEnabledByCategory;
        private System.Windows.Forms.DataGridView dataGridViewCategories;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DataGridView dataGridViewFamilies;
        private System.Windows.Forms.CheckBox checkBoxEnableByFamilyName;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox checkBoxEnabledByParameter;
        private System.Windows.Forms.Button buttonHelp;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxWorksetNameParameter;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.TextBox textBoxLinkSeparator;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox checkBoxEnabledForLinkedFiles;
        private System.Windows.Forms.Label labelLinkTestResult;
        private System.Windows.Forms.NumericUpDown numericLinkIgnoreLastChars;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown numericLinkIgnoreFirstChars;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown numericLinkPartNumber;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxLinkTestFilename;
        private System.Windows.Forms.TextBox textBoxLinkPrefix;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.TextBox textBoxDwgWorksetName;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.CheckBox checkBoxEnabledForDwgLinks;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.DataGridView dataGridViewTypes;
        private System.Windows.Forms.CheckBox checkBoxEnableByType;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.Label labelHelp1;
        private System.Windows.Forms.Label labelHelp2;
        private System.Windows.Forms.Label labelHelp3;
        private System.Windows.Forms.Label labelHelp4;
        private System.Windows.Forms.Label labelHelp5;
        private System.Windows.Forms.Label labelHelp6;
        private System.Windows.Forms.Button buttonReset;
        private System.Windows.Forms.Label labelHelp0;
        private System.Windows.Forms.CheckBox checkBoxNoEmptyWorksets;
    }
}