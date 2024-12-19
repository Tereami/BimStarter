
namespace RebarSketch
{
    partial class FormGlobalSettings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormGlobalSettings));
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxTempPath = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBoxFontName = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.numericFontSize = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.numericRound = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.comboBoxFontStyle = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.textBoxImageParamName = new System.Windows.Forms.TextBox();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOk = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.numericLinesSpacing = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.numericFontSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericRound)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericLinesSpacing)).BeginInit();
            this.SuspendLayout();
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // textBoxTempPath
            // 
            resources.ApplyResources(this.textBoxTempPath, "textBoxTempPath");
            this.textBoxTempPath.Name = "textBoxTempPath";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // comboBoxFontName
            // 
            resources.ApplyResources(this.comboBoxFontName, "comboBoxFontName");
            this.comboBoxFontName.FormattingEnabled = true;
            this.comboBoxFontName.Name = "comboBoxFontName";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // numericFontSize
            // 
            resources.ApplyResources(this.numericFontSize, "numericFontSize");
            this.numericFontSize.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericFontSize.Name = "numericFontSize";
            this.numericFontSize.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // numericRound
            // 
            resources.ApplyResources(this.numericRound, "numericRound");
            this.numericRound.Name = "numericRound";
            this.numericRound.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // comboBoxFontStyle
            // 
            resources.ApplyResources(this.comboBoxFontStyle, "comboBoxFontStyle");
            this.comboBoxFontStyle.FormattingEnabled = true;
            this.comboBoxFontStyle.Name = "comboBoxFontStyle";
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // textBoxImageParamName
            // 
            resources.ApplyResources(this.textBoxImageParamName, "textBoxImageParamName");
            this.textBoxImageParamName.Name = "textBoxImageParamName";
            // 
            // buttonCancel
            // 
            resources.ApplyResources(this.buttonCancel, "buttonCancel");
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonOk
            // 
            resources.ApplyResources(this.buttonOk, "buttonOk");
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // numericLinesSpacing
            // 
            resources.ApplyResources(this.numericLinesSpacing, "numericLinesSpacing");
            this.numericLinesSpacing.DecimalPlaces = 1;
            this.numericLinesSpacing.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numericLinesSpacing.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numericLinesSpacing.Name = "numericLinesSpacing";
            this.numericLinesSpacing.Value = new decimal(new int[] {
            18,
            0,
            0,
            65536});
            // 
            // FormGlobalSettings
            // 
            this.AcceptButton = this.buttonOk;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.Controls.Add(this.buttonOk);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.numericRound);
            this.Controls.Add(this.numericLinesSpacing);
            this.Controls.Add(this.numericFontSize);
            this.Controls.Add(this.comboBoxFontStyle);
            this.Controls.Add(this.comboBoxFontName);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.textBoxImageParamName);
            this.Controls.Add(this.textBoxTempPath);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FormGlobalSettings";
            ((System.ComponentModel.ISupportInitialize)(this.numericFontSize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericRound)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericLinesSpacing)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxTempPath;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboBoxFontName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown numericFontSize;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown numericRound;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox comboBoxFontStyle;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBoxImageParamName;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.NumericUpDown numericLinesSpacing;
    }
}