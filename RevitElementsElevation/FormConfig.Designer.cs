namespace RevitElementsElevation
{
    partial class FormConfig
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormConfig));
            this.btnOk = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.txtBoxLevelElevation = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtBoxElevationFromLevel = new System.Windows.Forms.TextBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.checkBoxWallAndColumns = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxTopElevParamName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxBottomElevParamName = new System.Windows.Forms.TextBox();
            this.checkBoxElevIsCurrency = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            resources.ApplyResources(this.btnOk, "btnOk");
            this.btnOk.Name = "btnOk";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.ForeColor = System.Drawing.SystemColors.MenuText;
            this.label4.Name = "label4";
            // 
            // txtBoxLevelElevation
            // 
            resources.ApplyResources(this.txtBoxLevelElevation, "txtBoxLevelElevation");
            this.txtBoxLevelElevation.Name = "txtBoxLevelElevation";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.ForeColor = System.Drawing.SystemColors.MenuText;
            this.label5.Name = "label5";
            // 
            // txtBoxElevationFromLevel
            // 
            resources.ApplyResources(this.txtBoxElevationFromLevel, "txtBoxElevationFromLevel");
            this.txtBoxElevationFromLevel.Name = "txtBoxElevationFromLevel";
            // 
            // btnCancel
            // 
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // checkBoxWallAndColumns
            // 
            resources.ApplyResources(this.checkBoxWallAndColumns, "checkBoxWallAndColumns");
            this.checkBoxWallAndColumns.Name = "checkBoxWallAndColumns";
            this.checkBoxWallAndColumns.UseVisualStyleBackColor = true;
            this.checkBoxWallAndColumns.CheckedChanged += new System.EventHandler(this.checkBoxWallAndColumns_CheckedChanged);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // textBoxTopElevParamName
            // 
            resources.ApplyResources(this.textBoxTopElevParamName, "textBoxTopElevParamName");
            this.textBoxTopElevParamName.Name = "textBoxTopElevParamName";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // textBoxBottomElevParamName
            // 
            resources.ApplyResources(this.textBoxBottomElevParamName, "textBoxBottomElevParamName");
            this.textBoxBottomElevParamName.Name = "textBoxBottomElevParamName";
            // 
            // checkBoxElevIsCurrency
            // 
            resources.ApplyResources(this.checkBoxElevIsCurrency, "checkBoxElevIsCurrency");
            this.checkBoxElevIsCurrency.Name = "checkBoxElevIsCurrency";
            this.checkBoxElevIsCurrency.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // FormConfig
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label2);
            this.Controls.Add(this.checkBoxElevIsCurrency);
            this.Controls.Add(this.textBoxBottomElevParamName);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBoxTopElevParamName);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.checkBoxWallAndColumns);
            this.Controls.Add(this.txtBoxElevationFromLevel);
            this.Controls.Add(this.txtBoxLevelElevation);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FormConfig";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtBoxLevelElevation;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtBoxElevationFromLevel;
        private System.Windows.Forms.Button btnCancel;


        #endregion

        private System.Windows.Forms.CheckBox checkBoxWallAndColumns;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxTopElevParamName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxBottomElevParamName;
        private System.Windows.Forms.CheckBox checkBoxElevIsCurrency;
        private System.Windows.Forms.Label label2;
    }
}