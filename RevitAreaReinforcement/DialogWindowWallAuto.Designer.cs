namespace RevitAreaReinforcement
{
    partial class DialogWindowWallAuto
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DialogWindowWallAuto));
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.numericUpDownVertFreeLengthRound = new System.Windows.Forms.NumericUpDown();
            this.checkBox_VertRebarStretched = new System.Windows.Forms.CheckBox();
            this.checkBox_AsymmVertFreeLength = new System.Windows.Forms.CheckBox();
            this.radioButtonFreeLengthAuto = new System.Windows.Forms.RadioButton();
            this.label9 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.radioButtonFreeLengthManual = new System.Windows.Forms.RadioButton();
            this.label8 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.numericUpDownHorizOffset = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.checkBoxIncreasedIntervalTopOrBottom = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.textBoxRazdelHoris = new System.Windows.Forms.TextBox();
            this.textBoxRazdelVert = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOk = new System.Windows.Forms.Button();
            this.checkBoxSkipAlreadyReinforced = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownVertFreeLengthRound)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownHorizOffset)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Controls.Add(this.numericUpDownVertFreeLengthRound);
            this.groupBox1.Controls.Add(this.checkBox_VertRebarStretched);
            this.groupBox1.Controls.Add(this.checkBox_AsymmVertFreeLength);
            this.groupBox1.Controls.Add(this.radioButtonFreeLengthAuto);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.radioButtonFreeLengthManual);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // numericUpDownVertFreeLengthRound
            // 
            resources.ApplyResources(this.numericUpDownVertFreeLengthRound, "numericUpDownVertFreeLengthRound");
            this.numericUpDownVertFreeLengthRound.Name = "numericUpDownVertFreeLengthRound";
            this.numericUpDownVertFreeLengthRound.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            // 
            // checkBox_VertRebarStretched
            // 
            resources.ApplyResources(this.checkBox_VertRebarStretched, "checkBox_VertRebarStretched");
            this.checkBox_VertRebarStretched.Name = "checkBox_VertRebarStretched";
            this.checkBox_VertRebarStretched.UseVisualStyleBackColor = true;
            // 
            // checkBox_AsymmVertFreeLength
            // 
            resources.ApplyResources(this.checkBox_AsymmVertFreeLength, "checkBox_AsymmVertFreeLength");
            this.checkBox_AsymmVertFreeLength.Name = "checkBox_AsymmVertFreeLength";
            this.checkBox_AsymmVertFreeLength.UseVisualStyleBackColor = true;
            // 
            // radioButtonFreeLengthAuto
            // 
            resources.ApplyResources(this.radioButtonFreeLengthAuto, "radioButtonFreeLengthAuto");
            this.radioButtonFreeLengthAuto.Name = "radioButtonFreeLengthAuto";
            this.radioButtonFreeLengthAuto.UseVisualStyleBackColor = true;
            this.radioButtonFreeLengthAuto.CheckedChanged += new System.EventHandler(this.radioButtonFreeLengthAuto_CheckedChanged);
            // 
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // radioButtonFreeLengthManual
            // 
            resources.ApplyResources(this.radioButtonFreeLengthManual, "radioButtonFreeLengthManual");
            this.radioButtonFreeLengthManual.Checked = true;
            this.radioButtonFreeLengthManual.Name = "radioButtonFreeLengthManual";
            this.radioButtonFreeLengthManual.TabStop = true;
            this.radioButtonFreeLengthManual.UseVisualStyleBackColor = true;
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // groupBox2
            // 
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Controls.Add(this.numericUpDownHorizOffset);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.checkBoxIncreasedIntervalTopOrBottom);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // numericUpDownHorizOffset
            // 
            resources.ApplyResources(this.numericUpDownHorizOffset, "numericUpDownHorizOffset");
            this.numericUpDownHorizOffset.Name = "numericUpDownHorizOffset";
            this.numericUpDownHorizOffset.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // checkBoxIncreasedIntervalTopOrBottom
            // 
            resources.ApplyResources(this.checkBoxIncreasedIntervalTopOrBottom, "checkBoxIncreasedIntervalTopOrBottom");
            this.checkBoxIncreasedIntervalTopOrBottom.Checked = true;
            this.checkBoxIncreasedIntervalTopOrBottom.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxIncreasedIntervalTopOrBottom.Name = "checkBoxIncreasedIntervalTopOrBottom";
            this.checkBoxIncreasedIntervalTopOrBottom.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // groupBox3
            // 
            resources.ApplyResources(this.groupBox3, "groupBox3");
            this.groupBox3.Controls.Add(this.textBoxRazdelHoris);
            this.groupBox3.Controls.Add(this.textBoxRazdelVert);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.TabStop = false;
            // 
            // textBoxRazdelHoris
            // 
            resources.ApplyResources(this.textBoxRazdelHoris, "textBoxRazdelHoris");
            this.textBoxRazdelHoris.Name = "textBoxRazdelHoris";
            // 
            // textBoxRazdelVert
            // 
            resources.ApplyResources(this.textBoxRazdelVert, "textBoxRazdelVert");
            this.textBoxRazdelVert.Name = "textBoxRazdelVert";
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
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
            // checkBoxSkipAlreadyReinforced
            // 
            resources.ApplyResources(this.checkBoxSkipAlreadyReinforced, "checkBoxSkipAlreadyReinforced");
            this.checkBoxSkipAlreadyReinforced.Name = "checkBoxSkipAlreadyReinforced";
            this.checkBoxSkipAlreadyReinforced.UseVisualStyleBackColor = true;
            // 
            // DialogWindowWallAuto
            // 
            this.AcceptButton = this.buttonOk;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.Controls.Add(this.checkBoxSkipAlreadyReinforced);
            this.Controls.Add(this.buttonOk);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "DialogWindowWallAuto";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownVertFreeLengthRound)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownHorizOffset)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioButtonFreeLengthAuto;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RadioButton radioButtonFreeLengthManual;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.NumericUpDown numericUpDownHorizOffset;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox textBoxRazdelHoris;
        private System.Windows.Forms.TextBox textBoxRazdelVert;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox checkBoxIncreasedIntervalTopOrBottom;
        private System.Windows.Forms.CheckBox checkBox_AsymmVertFreeLength;
        private System.Windows.Forms.CheckBox checkBox_VertRebarStretched;
        private System.Windows.Forms.NumericUpDown numericUpDownVertFreeLengthRound;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.CheckBox checkBoxSkipAlreadyReinforced;
    }
}