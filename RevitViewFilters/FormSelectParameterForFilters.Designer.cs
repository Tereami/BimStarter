namespace RevitViewFilters
{
    partial class FormSelectParameterForFilters
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSelectParameterForFilters));
            this.comboBoxParameters = new System.Windows.Forms.ComboBox();
            this.buttonNext = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.labelSymbols = new System.Windows.Forms.Label();
            this.numericStartSymbols = new System.Windows.Forms.NumericUpDown();
            this.radioButtonStartsWith = new System.Windows.Forms.RadioButton();
            this.radioButtonEquals = new System.Windows.Forms.RadioButton();
            this.radioButtonUserParameter = new System.Windows.Forms.RadioButton();
            this.radioButtonCheckHostMark = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.radioButtonResetColors = new System.Windows.Forms.RadioButton();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.checkBoxColorFill = new System.Windows.Forms.CheckBox();
            this.checkBoxColorLines = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericStartSymbols)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // comboBoxParameters
            // 
            resources.ApplyResources(this.comboBoxParameters, "comboBoxParameters");
            this.comboBoxParameters.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxParameters.FormattingEnabled = true;
            this.comboBoxParameters.Name = "comboBoxParameters";
            // 
            // buttonNext
            // 
            resources.ApplyResources(this.buttonNext, "buttonNext");
            this.buttonNext.Name = "buttonNext";
            this.buttonNext.UseVisualStyleBackColor = true;
            this.buttonNext.Click += new System.EventHandler(this.buttonNext_Click);
            // 
            // buttonCancel
            // 
            resources.ApplyResources(this.buttonCancel, "buttonCancel");
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Controls.Add(this.labelSymbols);
            this.groupBox1.Controls.Add(this.numericStartSymbols);
            this.groupBox1.Controls.Add(this.comboBoxParameters);
            this.groupBox1.Controls.Add(this.radioButtonStartsWith);
            this.groupBox1.Controls.Add(this.radioButtonEquals);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // labelSymbols
            // 
            resources.ApplyResources(this.labelSymbols, "labelSymbols");
            this.labelSymbols.Name = "labelSymbols";
            // 
            // numericStartSymbols
            // 
            resources.ApplyResources(this.numericStartSymbols, "numericStartSymbols");
            this.numericStartSymbols.Name = "numericStartSymbols";
            this.numericStartSymbols.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // radioButtonStartsWith
            // 
            resources.ApplyResources(this.radioButtonStartsWith, "radioButtonStartsWith");
            this.radioButtonStartsWith.Name = "radioButtonStartsWith";
            this.radioButtonStartsWith.UseVisualStyleBackColor = true;
            this.radioButtonStartsWith.CheckedChanged += new System.EventHandler(this.radioButtonStartsWith_CheckedChanged);
            // 
            // radioButtonEquals
            // 
            resources.ApplyResources(this.radioButtonEquals, "radioButtonEquals");
            this.radioButtonEquals.Checked = true;
            this.radioButtonEquals.Name = "radioButtonEquals";
            this.radioButtonEquals.TabStop = true;
            this.radioButtonEquals.UseVisualStyleBackColor = true;
            this.radioButtonEquals.CheckedChanged += new System.EventHandler(this.radioButtonEquals_CheckedChanged);
            // 
            // radioButtonUserParameter
            // 
            resources.ApplyResources(this.radioButtonUserParameter, "radioButtonUserParameter");
            this.radioButtonUserParameter.Checked = true;
            this.radioButtonUserParameter.Name = "radioButtonUserParameter";
            this.radioButtonUserParameter.TabStop = true;
            this.radioButtonUserParameter.UseVisualStyleBackColor = true;
            this.radioButtonUserParameter.CheckedChanged += new System.EventHandler(this.radioButtonUserParameter_CheckedChanged);
            // 
            // radioButtonCheckHostMark
            // 
            resources.ApplyResources(this.radioButtonCheckHostMark, "radioButtonCheckHostMark");
            this.radioButtonCheckHostMark.Name = "radioButtonCheckHostMark";
            this.radioButtonCheckHostMark.TabStop = true;
            this.radioButtonCheckHostMark.UseVisualStyleBackColor = true;
            this.radioButtonCheckHostMark.CheckedChanged += new System.EventHandler(this.radioButtonCheckHostMark_CheckedChanged);
            // 
            // groupBox2
            // 
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Controls.Add(this.radioButtonUserParameter);
            this.groupBox2.Controls.Add(this.groupBox1);
            this.groupBox2.Controls.Add(this.radioButtonResetColors);
            this.groupBox2.Controls.Add(this.radioButtonCheckHostMark);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // radioButtonResetColors
            // 
            resources.ApplyResources(this.radioButtonResetColors, "radioButtonResetColors");
            this.radioButtonResetColors.Name = "radioButtonResetColors";
            this.radioButtonResetColors.TabStop = true;
            this.radioButtonResetColors.UseVisualStyleBackColor = true;
            this.radioButtonResetColors.CheckedChanged += new System.EventHandler(this.radioButtonCheckHostMark_CheckedChanged);
            // 
            // groupBox3
            // 
            resources.ApplyResources(this.groupBox3, "groupBox3");
            this.groupBox3.Controls.Add(this.checkBoxColorFill);
            this.groupBox3.Controls.Add(this.checkBoxColorLines);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.TabStop = false;
            // 
            // checkBoxColorFill
            // 
            resources.ApplyResources(this.checkBoxColorFill, "checkBoxColorFill");
            this.checkBoxColorFill.Checked = true;
            this.checkBoxColorFill.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxColorFill.Name = "checkBoxColorFill";
            this.checkBoxColorFill.UseVisualStyleBackColor = true;
            // 
            // checkBoxColorLines
            // 
            resources.ApplyResources(this.checkBoxColorLines, "checkBoxColorLines");
            this.checkBoxColorLines.Checked = true;
            this.checkBoxColorLines.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxColorLines.Name = "checkBoxColorLines";
            this.checkBoxColorLines.UseVisualStyleBackColor = true;
            // 
            // FormSelectParameterForFilters
            // 
            this.AcceptButton = this.buttonNext;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonNext);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FormSelectParameterForFilters";
            this.Load += new System.EventHandler(this.FormSelectParameterForFilters_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericStartSymbols)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ComboBox comboBoxParameters;
        private System.Windows.Forms.Button buttonNext;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label labelSymbols;
        private System.Windows.Forms.NumericUpDown numericStartSymbols;
        private System.Windows.Forms.RadioButton radioButtonStartsWith;
        private System.Windows.Forms.RadioButton radioButtonEquals;
        private System.Windows.Forms.RadioButton radioButtonUserParameter;
        private System.Windows.Forms.RadioButton radioButtonCheckHostMark;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox checkBoxColorFill;
        private System.Windows.Forms.CheckBox checkBoxColorLines;
        private System.Windows.Forms.RadioButton radioButtonResetColors;
    }
}