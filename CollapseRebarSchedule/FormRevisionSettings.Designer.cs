namespace SchedulesTools
{
    partial class FormRevisionSettings
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
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOk = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.checkBoxUseGroupingRevisions = new System.Windows.Forms.CheckBox();
            this.checkBoxUseRevisionsOnThisSheet = new System.Windows.Forms.CheckBox();
            this.checkBoxUseCloudsCount = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.comboBoxSheetNumberParameter = new System.Windows.Forms.ComboBox();
            this.comboBoxRevisionParameter = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.radioUseParamForDescription = new System.Windows.Forms.RadioButton();
            this.radioUseStandartDescription = new System.Windows.Forms.RadioButton();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBoxSheetNote = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.comboBoxSheetsForLastRevision = new System.Windows.Forms.ComboBox();
            this.textBoxSheetRevDescParam = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(235, 409);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 0;
            this.buttonCancel.Text = "Отмена";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonOk
            // 
            this.buttonOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOk.Location = new System.Drawing.Point(154, 409);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(75, 23);
            this.buttonOk.TabIndex = 0;
            this.buttonOk.Text = "ОК";
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.checkBoxUseGroupingRevisions);
            this.groupBox1.Controls.Add(this.checkBoxUseRevisionsOnThisSheet);
            this.groupBox1.Controls.Add(this.checkBoxUseCloudsCount);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(298, 91);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Действия";
            // 
            // checkBoxUseGroupingRevisions
            // 
            this.checkBoxUseGroupingRevisions.AutoSize = true;
            this.checkBoxUseGroupingRevisions.Location = new System.Drawing.Point(6, 65);
            this.checkBoxUseGroupingRevisions.Name = "checkBoxUseGroupingRevisions";
            this.checkBoxUseGroupingRevisions.Size = new System.Drawing.Size(281, 17);
            this.checkBoxUseGroupingRevisions.TabIndex = 2;
            this.checkBoxUseGroupingRevisions.Text = "Заполнить \"Разрешение на внесение изменений\"";
            this.checkBoxUseGroupingRevisions.UseVisualStyleBackColor = true;
            // 
            // checkBoxUseRevisionsOnThisSheet
            // 
            this.checkBoxUseRevisionsOnThisSheet.AutoSize = true;
            this.checkBoxUseRevisionsOnThisSheet.Location = new System.Drawing.Point(6, 42);
            this.checkBoxUseRevisionsOnThisSheet.Name = "checkBoxUseRevisionsOnThisSheet";
            this.checkBoxUseRevisionsOnThisSheet.Size = new System.Drawing.Size(287, 17);
            this.checkBoxUseRevisionsOnThisSheet.TabIndex = 2;
            this.checkBoxUseRevisionsOnThisSheet.Text = "Перечисление изменений в \"Ведомости чертежей\"";
            this.checkBoxUseRevisionsOnThisSheet.UseVisualStyleBackColor = true;
            // 
            // checkBoxUseCloudsCount
            // 
            this.checkBoxUseCloudsCount.AutoSize = true;
            this.checkBoxUseCloudsCount.Location = new System.Drawing.Point(6, 19);
            this.checkBoxUseCloudsCount.Name = "checkBoxUseCloudsCount";
            this.checkBoxUseCloudsCount.Size = new System.Drawing.Size(176, 17);
            this.checkBoxUseCloudsCount.TabIndex = 2;
            this.checkBoxUseCloudsCount.Text = "Заполнить \"Кол.уч\" в штампе";
            this.checkBoxUseCloudsCount.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.comboBoxSheetsForLastRevision);
            this.groupBox2.Controls.Add(this.comboBoxSheetNote);
            this.groupBox2.Controls.Add(this.comboBoxSheetNumberParameter);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.comboBoxRevisionParameter);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Location = new System.Drawing.Point(12, 114);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(296, 183);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Параметры";
            // 
            // comboBoxSheetNumberParameter
            // 
            this.comboBoxSheetNumberParameter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxSheetNumberParameter.FormattingEnabled = true;
            this.comboBoxSheetNumberParameter.Location = new System.Drawing.Point(6, 72);
            this.comboBoxSheetNumberParameter.Name = "comboBoxSheetNumberParameter";
            this.comboBoxSheetNumberParameter.Size = new System.Drawing.Size(277, 21);
            this.comboBoxSheetNumberParameter.TabIndex = 1;
            // 
            // comboBoxRevisionParameter
            // 
            this.comboBoxRevisionParameter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxRevisionParameter.FormattingEnabled = true;
            this.comboBoxRevisionParameter.Location = new System.Drawing.Point(6, 32);
            this.comboBoxRevisionParameter.Name = "comboBoxRevisionParameter";
            this.comboBoxRevisionParameter.Size = new System.Drawing.Size(277, 21);
            this.comboBoxRevisionParameter.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 56);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Номер листа:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(103, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Номер изменения:";
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.textBoxSheetRevDescParam);
            this.groupBox3.Controls.Add(this.radioUseParamForDescription);
            this.groupBox3.Controls.Add(this.radioUseStandartDescription);
            this.groupBox3.Location = new System.Drawing.Point(12, 303);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(296, 96);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Разрешение на изменения, использовать:";
            // 
            // radioUseParamForDescription
            // 
            this.radioUseParamForDescription.AutoSize = true;
            this.radioUseParamForDescription.Location = new System.Drawing.Point(9, 42);
            this.radioUseParamForDescription.Name = "radioUseParamForDescription";
            this.radioUseParamForDescription.Size = new System.Drawing.Size(111, 17);
            this.radioUseParamForDescription.TabIndex = 4;
            this.radioUseParamForDescription.Text = "Параметр листа:";
            this.radioUseParamForDescription.UseVisualStyleBackColor = true;
            this.radioUseParamForDescription.CheckedChanged += new System.EventHandler(this.radioUseParamForDescription_CheckedChanged);
            // 
            // radioUseStandartDescription
            // 
            this.radioUseStandartDescription.AutoSize = true;
            this.radioUseStandartDescription.Checked = true;
            this.radioUseStandartDescription.Location = new System.Drawing.Point(9, 19);
            this.radioUseStandartDescription.Name = "radioUseStandartDescription";
            this.radioUseStandartDescription.Size = new System.Drawing.Size(285, 17);
            this.radioUseStandartDescription.TabIndex = 4;
            this.radioUseStandartDescription.TabStop = true;
            this.radioUseStandartDescription.Text = "\"Описание изменения\" в настройках \"Изменений\"";
            this.radioUseStandartDescription.UseVisualStyleBackColor = true;
            this.radioUseStandartDescription.CheckedChanged += new System.EventHandler(this.radioUseStandartDescription_CheckedChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 96);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(269, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "\"Примечание\" - перечисление номеров изменений:";
            // 
            // comboBoxSheetNote
            // 
            this.comboBoxSheetNote.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxSheetNote.FormattingEnabled = true;
            this.comboBoxSheetNote.Location = new System.Drawing.Point(6, 112);
            this.comboBoxSheetNote.Name = "comboBoxSheetNote";
            this.comboBoxSheetNote.Size = new System.Drawing.Size(277, 21);
            this.comboBoxSheetNote.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 136);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(248, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Перечисление листов в последнем изменении:";
            // 
            // comboBoxSheetsForLastRevision
            // 
            this.comboBoxSheetsForLastRevision.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxSheetsForLastRevision.FormattingEnabled = true;
            this.comboBoxSheetsForLastRevision.Location = new System.Drawing.Point(6, 152);
            this.comboBoxSheetsForLastRevision.Name = "comboBoxSheetsForLastRevision";
            this.comboBoxSheetsForLastRevision.Size = new System.Drawing.Size(277, 21);
            this.comboBoxSheetsForLastRevision.TabIndex = 1;
            // 
            // textBoxSheetRevDescParam
            // 
            this.textBoxSheetRevDescParam.Enabled = false;
            this.textBoxSheetRevDescParam.Location = new System.Drawing.Point(20, 65);
            this.textBoxSheetRevDescParam.Name = "textBoxSheetRevDescParam";
            this.textBoxSheetRevDescParam.Size = new System.Drawing.Size(263, 20);
            this.textBoxSheetRevDescParam.TabIndex = 5;
            // 
            // FormSettings
            // 
            this.AcceptButton = this.buttonOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(322, 444);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.buttonOk);
            this.Controls.Add(this.buttonCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FormSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Настройки";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox checkBoxUseGroupingRevisions;
        private System.Windows.Forms.CheckBox checkBoxUseRevisionsOnThisSheet;
        private System.Windows.Forms.CheckBox checkBoxUseCloudsCount;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox comboBoxSheetNumberParameter;
        private System.Windows.Forms.ComboBox comboBoxRevisionParameter;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RadioButton radioUseParamForDescription;
        private System.Windows.Forms.RadioButton radioUseStandartDescription;
        private System.Windows.Forms.ComboBox comboBoxSheetNote;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboBoxSheetsForLastRevision;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxSheetRevDescParam;
    }
}