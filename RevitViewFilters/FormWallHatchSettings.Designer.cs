namespace RevitViewFilters
{
    partial class FormWallHatchSettings
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
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.checkBoxUseBaseLevel = new System.Windows.Forms.CheckBox();
            this.checkBoxUseHeight = new System.Windows.Forms.CheckBox();
            this.checkBoxUseType = new System.Windows.Forms.CheckBox();
            this.checkBoxUseThickness = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxHatchPrefix = new System.Windows.Forms.TextBox();
            this.textBoxImagePrefix = new System.Windows.Forms.TextBox();
            this.buttonOk = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(352, 60);
            this.label1.TabIndex = 0;
            this.label1.Text = "This program will do the following: sort walls by selected parameters; create a f" +
    "ilter with hatching for each group; and assign the corresponding image for outpu" +
    "t to the wall schedule.";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.checkBoxUseThickness);
            this.groupBox1.Controls.Add(this.checkBoxUseType);
            this.groupBox1.Controls.Add(this.checkBoxUseHeight);
            this.groupBox1.Controls.Add(this.checkBoxUseBaseLevel);
            this.groupBox1.Location = new System.Drawing.Point(12, 72);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(352, 77);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Parameters for grouping";
            // 
            // checkBoxUseBaseLevel
            // 
            this.checkBoxUseBaseLevel.AutoSize = true;
            this.checkBoxUseBaseLevel.Location = new System.Drawing.Point(6, 19);
            this.checkBoxUseBaseLevel.Name = "checkBoxUseBaseLevel";
            this.checkBoxUseBaseLevel.Size = new System.Drawing.Size(75, 17);
            this.checkBoxUseBaseLevel.TabIndex = 0;
            this.checkBoxUseBaseLevel.Text = "Base level";
            this.checkBoxUseBaseLevel.UseVisualStyleBackColor = true;
            // 
            // checkBoxUseHeight
            // 
            this.checkBoxUseHeight.AutoSize = true;
            this.checkBoxUseHeight.Location = new System.Drawing.Point(6, 42);
            this.checkBoxUseHeight.Name = "checkBoxUseHeight";
            this.checkBoxUseHeight.Size = new System.Drawing.Size(57, 17);
            this.checkBoxUseHeight.TabIndex = 0;
            this.checkBoxUseHeight.Text = "Height";
            this.checkBoxUseHeight.UseVisualStyleBackColor = true;
            // 
            // checkBoxUseType
            // 
            this.checkBoxUseType.AutoSize = true;
            this.checkBoxUseType.Location = new System.Drawing.Point(192, 19);
            this.checkBoxUseType.Name = "checkBoxUseType";
            this.checkBoxUseType.Size = new System.Drawing.Size(50, 17);
            this.checkBoxUseType.TabIndex = 0;
            this.checkBoxUseType.Text = "Type";
            this.checkBoxUseType.UseVisualStyleBackColor = true;
            // 
            // checkBoxUseThickness
            // 
            this.checkBoxUseThickness.AutoSize = true;
            this.checkBoxUseThickness.Location = new System.Drawing.Point(192, 42);
            this.checkBoxUseThickness.Name = "checkBoxUseThickness";
            this.checkBoxUseThickness.Size = new System.Drawing.Size(75, 17);
            this.checkBoxUseThickness.TabIndex = 0;
            this.checkBoxUseThickness.Text = "Thickness";
            this.checkBoxUseThickness.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.textBoxImagePrefix);
            this.groupBox2.Controls.Add(this.textBoxHatchPrefix);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Location = new System.Drawing.Point(12, 155);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(352, 124);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Hatch";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(6, 19);
            this.label2.Margin = new System.Windows.Forms.Padding(3);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(340, 33);
            this.label2.TabIndex = 2;
            this.label2.Text = "Hatch patterns and corresponding PNG images must be prepared in advance in the pr" +
    "oject and must have the correct name.";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 64);
            this.label3.Margin = new System.Windows.Forms.Padding(3);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(132, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Hatch pattern name prefix:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 90);
            this.label4.Margin = new System.Windows.Forms.Padding(3);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(96, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Image name prefix:";
            // 
            // textBoxHatchPrefix
            // 
            this.textBoxHatchPrefix.Location = new System.Drawing.Point(182, 64);
            this.textBoxHatchPrefix.Name = "textBoxHatchPrefix";
            this.textBoxHatchPrefix.Size = new System.Drawing.Size(164, 20);
            this.textBoxHatchPrefix.TabIndex = 4;
            // 
            // textBoxImagePrefix
            // 
            this.textBoxImagePrefix.Location = new System.Drawing.Point(182, 90);
            this.textBoxImagePrefix.Name = "textBoxImagePrefix";
            this.textBoxImagePrefix.Size = new System.Drawing.Size(164, 20);
            this.textBoxImagePrefix.TabIndex = 4;
            // 
            // buttonOk
            // 
            this.buttonOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOk.Location = new System.Drawing.Point(204, 298);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(75, 23);
            this.buttonOk.TabIndex = 2;
            this.buttonOk.Text = "OK";
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(289, 298);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 2;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // FormWallHatchSettings
            // 
            this.AcceptButton = this.buttonOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(376, 333);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOk);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FormWallHatchSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Wall hatch settings";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox checkBoxUseBaseLevel;
        private System.Windows.Forms.CheckBox checkBoxUseThickness;
        private System.Windows.Forms.CheckBox checkBoxUseType;
        private System.Windows.Forms.CheckBox checkBoxUseHeight;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxImagePrefix;
        private System.Windows.Forms.TextBox textBoxHatchPrefix;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.Button buttonCancel;
    }
}