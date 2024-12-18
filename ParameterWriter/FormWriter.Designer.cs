namespace ParameterWriter
{
    partial class FormWriter
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormWriter));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioAllElements = new System.Windows.Forms.RadioButton();
            this.radioViewElements = new System.Windows.Forms.RadioButton();
            this.radioSelectedElements = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBoxTargetParameter = new System.Windows.Forms.ComboBox();
            this.groupBoxSourceData = new System.Windows.Forms.GroupBox();
            this.comboBox_Constructor = new System.Windows.Forms.ComboBox();
            this.radioButton_Constructor = new System.Windows.Forms.RadioButton();
            this.comboBox_Level = new System.Windows.Forms.ComboBox();
            this.radioButton_Level = new System.Windows.Forms.RadioButton();
            this.comboBox_OtherParameter = new System.Windows.Forms.ComboBox();
            this.radioButton_OtherParameter = new System.Windows.Forms.RadioButton();
            this.radioButton_ConstValue = new System.Windows.Forms.RadioButton();
            this.comboBox_ConstValue = new System.Windows.Forms.ComboBox();
            this.buttonOk = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonLoad = new System.Windows.Forms.Button();
            this.buttonSave = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.buttonExecute = new System.Windows.Forms.Button();
            this.checkBox_ShowLog = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.groupBoxSourceData.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Controls.Add(this.radioAllElements);
            this.groupBox1.Controls.Add(this.radioViewElements);
            this.groupBox1.Controls.Add(this.radioSelectedElements);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.comboBoxTargetParameter);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // radioAllElements
            // 
            resources.ApplyResources(this.radioAllElements, "radioAllElements");
            this.radioAllElements.Name = "radioAllElements";
            this.radioAllElements.TabStop = true;
            this.radioAllElements.UseVisualStyleBackColor = true;
            // 
            // radioViewElements
            // 
            resources.ApplyResources(this.radioViewElements, "radioViewElements");
            this.radioViewElements.Checked = true;
            this.radioViewElements.Name = "radioViewElements";
            this.radioViewElements.TabStop = true;
            this.radioViewElements.UseVisualStyleBackColor = true;
            // 
            // radioSelectedElements
            // 
            resources.ApplyResources(this.radioSelectedElements, "radioSelectedElements");
            this.radioSelectedElements.Name = "radioSelectedElements";
            this.radioSelectedElements.TabStop = true;
            this.radioSelectedElements.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // comboBoxTargetParameter
            // 
            resources.ApplyResources(this.comboBoxTargetParameter, "comboBoxTargetParameter");
            this.comboBoxTargetParameter.FormattingEnabled = true;
            this.comboBoxTargetParameter.Name = "comboBoxTargetParameter";
            this.comboBoxTargetParameter.SelectedIndexChanged += new System.EventHandler(this.comboBoxParameter_SelectedIndexChanged);
            // 
            // groupBoxSourceData
            // 
            resources.ApplyResources(this.groupBoxSourceData, "groupBoxSourceData");
            this.groupBoxSourceData.Controls.Add(this.comboBox_Constructor);
            this.groupBoxSourceData.Controls.Add(this.radioButton_Constructor);
            this.groupBoxSourceData.Controls.Add(this.comboBox_Level);
            this.groupBoxSourceData.Controls.Add(this.radioButton_Level);
            this.groupBoxSourceData.Controls.Add(this.comboBox_OtherParameter);
            this.groupBoxSourceData.Controls.Add(this.radioButton_OtherParameter);
            this.groupBoxSourceData.Controls.Add(this.radioButton_ConstValue);
            this.groupBoxSourceData.Controls.Add(this.comboBox_ConstValue);
            this.groupBoxSourceData.Name = "groupBoxSourceData";
            this.groupBoxSourceData.TabStop = false;
            // 
            // comboBox_Constructor
            // 
            resources.ApplyResources(this.comboBox_Constructor, "comboBox_Constructor");
            this.comboBox_Constructor.FormattingEnabled = true;
            this.comboBox_Constructor.Name = "comboBox_Constructor";
            // 
            // radioButton_Constructor
            // 
            resources.ApplyResources(this.radioButton_Constructor, "radioButton_Constructor");
            this.radioButton_Constructor.Name = "radioButton_Constructor";
            this.radioButton_Constructor.TabStop = true;
            this.radioButton_Constructor.UseVisualStyleBackColor = true;
            this.radioButton_Constructor.CheckedChanged += new System.EventHandler(this.radioButtonWriteValue_CheckedChanged);
            // 
            // comboBox_Level
            // 
            resources.ApplyResources(this.comboBox_Level, "comboBox_Level");
            this.comboBox_Level.FormattingEnabled = true;
            this.comboBox_Level.Name = "comboBox_Level";
            // 
            // radioButton_Level
            // 
            resources.ApplyResources(this.radioButton_Level, "radioButton_Level");
            this.radioButton_Level.Name = "radioButton_Level";
            this.radioButton_Level.UseVisualStyleBackColor = true;
            this.radioButton_Level.CheckedChanged += new System.EventHandler(this.radioButtonWriteValue_CheckedChanged);
            // 
            // comboBox_OtherParameter
            // 
            resources.ApplyResources(this.comboBox_OtherParameter, "comboBox_OtherParameter");
            this.comboBox_OtherParameter.FormattingEnabled = true;
            this.comboBox_OtherParameter.Name = "comboBox_OtherParameter";
            // 
            // radioButton_OtherParameter
            // 
            resources.ApplyResources(this.radioButton_OtherParameter, "radioButton_OtherParameter");
            this.radioButton_OtherParameter.Name = "radioButton_OtherParameter";
            this.radioButton_OtherParameter.UseVisualStyleBackColor = true;
            this.radioButton_OtherParameter.CheckedChanged += new System.EventHandler(this.radioButtonWriteValue_CheckedChanged);
            // 
            // radioButton_ConstValue
            // 
            resources.ApplyResources(this.radioButton_ConstValue, "radioButton_ConstValue");
            this.radioButton_ConstValue.Checked = true;
            this.radioButton_ConstValue.Name = "radioButton_ConstValue";
            this.radioButton_ConstValue.TabStop = true;
            this.radioButton_ConstValue.UseVisualStyleBackColor = true;
            this.radioButton_ConstValue.CheckedChanged += new System.EventHandler(this.radioButtonWriteValue_CheckedChanged);
            // 
            // comboBox_ConstValue
            // 
            resources.ApplyResources(this.comboBox_ConstValue, "comboBox_ConstValue");
            this.comboBox_ConstValue.FormattingEnabled = true;
            this.comboBox_ConstValue.Name = "comboBox_ConstValue";
            // 
            // buttonOk
            // 
            resources.ApplyResources(this.buttonOk, "buttonOk");
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
            // 
            // buttonCancel
            // 
            resources.ApplyResources(this.buttonCancel, "buttonCancel");
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonLoad
            // 
            resources.ApplyResources(this.buttonLoad, "buttonLoad");
            this.buttonLoad.Name = "buttonLoad";
            this.buttonLoad.UseVisualStyleBackColor = true;
            this.buttonLoad.Click += new System.EventHandler(this.buttonLoad_Click);
            // 
            // buttonSave
            // 
            resources.ApplyResources(this.buttonSave, "buttonSave");
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // groupBox2
            // 
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Controls.Add(this.buttonLoad);
            this.groupBox2.Controls.Add(this.buttonExecute);
            this.groupBox2.Controls.Add(this.buttonSave);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // buttonExecute
            // 
            resources.ApplyResources(this.buttonExecute, "buttonExecute");
            this.buttonExecute.Name = "buttonExecute";
            this.buttonExecute.UseVisualStyleBackColor = true;
            this.buttonExecute.Click += new System.EventHandler(this.buttonExecute_Click);
            // 
            // checkBox_ShowLog
            // 
            resources.ApplyResources(this.checkBox_ShowLog, "checkBox_ShowLog");
            this.checkBox_ShowLog.Name = "checkBox_ShowLog";
            this.checkBox_ShowLog.UseVisualStyleBackColor = true;
            // 
            // FormWriter
            // 
            this.AcceptButton = this.buttonOk;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.Controls.Add(this.checkBox_ShowLog);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOk);
            this.Controls.Add(this.groupBoxSourceData);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FormWriter";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBoxSourceData.ResumeLayout(false);
            this.groupBoxSourceData.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioAllElements;
        private System.Windows.Forms.RadioButton radioViewElements;
        private System.Windows.Forms.RadioButton radioSelectedElements;
        private System.Windows.Forms.GroupBox groupBoxSourceData;
        private System.Windows.Forms.ComboBox comboBox_ConstValue;
        private System.Windows.Forms.ComboBox comboBoxTargetParameter;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.ComboBox comboBox_OtherParameter;
        private System.Windows.Forms.RadioButton radioButton_OtherParameter;
        private System.Windows.Forms.RadioButton radioButton_ConstValue;
        private System.Windows.Forms.ComboBox comboBox_Constructor;
        private System.Windows.Forms.RadioButton radioButton_Constructor;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBox_Level;
        private System.Windows.Forms.RadioButton radioButton_Level;
        private System.Windows.Forms.Button buttonLoad;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button buttonExecute;
        private System.Windows.Forms.CheckBox checkBox_ShowLog;
    }
}