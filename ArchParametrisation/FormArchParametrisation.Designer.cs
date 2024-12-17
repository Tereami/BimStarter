
namespace ArchParametrisation
{
    partial class FormArchParametrisation
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormArchParametrisation));
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOk = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtbxMirroredParameter = new System.Windows.Forms.TextBox();
            this.txtbxMirroredText = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.chkbxActivateMirrored = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.chkBoxCalculateOneRoomOpenings = new System.Windows.Forms.CheckBox();
            this.txtbxOpeningsAreaParam = new System.Windows.Forms.TextBox();
            this.txtbxOpeningHeightParam = new System.Windows.Forms.TextBox();
            this.txtbxOpeningWidthParam = new System.Windows.Forms.TextBox();
            this.chkbxActivateOpeningsArea = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.checkBoxUseRoomNames = new System.Windows.Forms.CheckBox();
            this.chkbxFloorsIncludeInFinishing = new System.Windows.Forms.CheckBox();
            this.chkboxRoomFinishingSequenceNumbers = new System.Windows.Forms.CheckBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtbxWallNumbers = new System.Windows.Forms.TextBox();
            this.txtbxFloorNumbers = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.txtbxFinishingRoomNumberParam = new System.Windows.Forms.TextBox();
            this.label19 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.txtbxFlatNumber = new System.Windows.Forms.TextBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.RommName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RoomCoeff = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RoomIsLiving = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.chkbxActivateFlatography = new System.Windows.Forms.CheckBox();
            this.label18 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.txtbxIsLivingParam = new System.Windows.Forms.TextBox();
            this.txtbxAreaCoeff = new System.Windows.Forms.TextBox();
            this.txtbxRoomCount = new System.Windows.Forms.TextBox();
            this.txtbxFlatLivingArea = new System.Windows.Forms.TextBox();
            this.txtbxFlatSumArea = new System.Windows.Forms.TextBox();
            this.txtbxFlatArea = new System.Windows.Forms.TextBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.chxbxRoomNumberToFinishing = new System.Windows.Forms.CheckBox();
            this.label20 = new System.Windows.Forms.Label();
            this.buttonReset = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonCancel
            // 
            resources.ApplyResources(this.buttonCancel, "buttonCancel");
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // buttonOk
            // 
            resources.ApplyResources(this.buttonOk, "buttonOk");
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtbxMirroredParameter);
            this.groupBox1.Controls.Add(this.txtbxMirroredText);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.chkbxActivateMirrored);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // txtbxMirroredParameter
            // 
            resources.ApplyResources(this.txtbxMirroredParameter, "txtbxMirroredParameter");
            this.txtbxMirroredParameter.Name = "txtbxMirroredParameter";
            // 
            // txtbxMirroredText
            // 
            resources.ApplyResources(this.txtbxMirroredText, "txtbxMirroredText");
            this.txtbxMirroredText.Name = "txtbxMirroredText";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
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
            // chkbxActivateMirrored
            // 
            resources.ApplyResources(this.chkbxActivateMirrored, "chkbxActivateMirrored");
            this.chkbxActivateMirrored.Checked = true;
            this.chkbxActivateMirrored.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkbxActivateMirrored.Name = "chkbxActivateMirrored";
            this.chkbxActivateMirrored.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.chkBoxCalculateOneRoomOpenings);
            this.groupBox2.Controls.Add(this.txtbxOpeningsAreaParam);
            this.groupBox2.Controls.Add(this.txtbxOpeningHeightParam);
            this.groupBox2.Controls.Add(this.txtbxOpeningWidthParam);
            this.groupBox2.Controls.Add(this.chkbxActivateOpeningsArea);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label5);
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // chkBoxCalculateOneRoomOpenings
            // 
            resources.ApplyResources(this.chkBoxCalculateOneRoomOpenings, "chkBoxCalculateOneRoomOpenings");
            this.chkBoxCalculateOneRoomOpenings.Name = "chkBoxCalculateOneRoomOpenings";
            this.chkBoxCalculateOneRoomOpenings.UseVisualStyleBackColor = true;
            // 
            // txtbxOpeningsAreaParam
            // 
            resources.ApplyResources(this.txtbxOpeningsAreaParam, "txtbxOpeningsAreaParam");
            this.txtbxOpeningsAreaParam.Name = "txtbxOpeningsAreaParam";
            // 
            // txtbxOpeningHeightParam
            // 
            resources.ApplyResources(this.txtbxOpeningHeightParam, "txtbxOpeningHeightParam");
            this.txtbxOpeningHeightParam.Name = "txtbxOpeningHeightParam";
            // 
            // txtbxOpeningWidthParam
            // 
            resources.ApplyResources(this.txtbxOpeningWidthParam, "txtbxOpeningWidthParam");
            this.txtbxOpeningWidthParam.Name = "txtbxOpeningWidthParam";
            // 
            // chkbxActivateOpeningsArea
            // 
            resources.ApplyResources(this.chkbxActivateOpeningsArea, "chkbxActivateOpeningsArea");
            this.chkbxActivateOpeningsArea.Checked = true;
            this.chkbxActivateOpeningsArea.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkbxActivateOpeningsArea.Name = "chkbxActivateOpeningsArea";
            this.chkbxActivateOpeningsArea.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.checkBoxUseRoomNames);
            this.groupBox3.Controls.Add(this.chkbxFloorsIncludeInFinishing);
            this.groupBox3.Controls.Add(this.chkboxRoomFinishingSequenceNumbers);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.txtbxWallNumbers);
            this.groupBox3.Controls.Add(this.txtbxFloorNumbers);
            this.groupBox3.Controls.Add(this.label11);
            this.groupBox3.Controls.Add(this.label10);
            resources.ApplyResources(this.groupBox3, "groupBox3");
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.TabStop = false;
            // 
            // checkBoxUseRoomNames
            // 
            resources.ApplyResources(this.checkBoxUseRoomNames, "checkBoxUseRoomNames");
            this.checkBoxUseRoomNames.Name = "checkBoxUseRoomNames";
            this.checkBoxUseRoomNames.UseVisualStyleBackColor = true;
            // 
            // chkbxFloorsIncludeInFinishing
            // 
            resources.ApplyResources(this.chkbxFloorsIncludeInFinishing, "chkbxFloorsIncludeInFinishing");
            this.chkbxFloorsIncludeInFinishing.Name = "chkbxFloorsIncludeInFinishing";
            this.chkbxFloorsIncludeInFinishing.UseVisualStyleBackColor = true;
            // 
            // chkboxRoomFinishingSequenceNumbers
            // 
            resources.ApplyResources(this.chkboxRoomFinishingSequenceNumbers, "chkboxRoomFinishingSequenceNumbers");
            this.chkboxRoomFinishingSequenceNumbers.Checked = true;
            this.chkboxRoomFinishingSequenceNumbers.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkboxRoomFinishingSequenceNumbers.Name = "chkboxRoomFinishingSequenceNumbers";
            this.chkboxRoomFinishingSequenceNumbers.UseVisualStyleBackColor = true;
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // txtbxWallNumbers
            // 
            resources.ApplyResources(this.txtbxWallNumbers, "txtbxWallNumbers");
            this.txtbxWallNumbers.Name = "txtbxWallNumbers";
            // 
            // txtbxFloorNumbers
            // 
            resources.ApplyResources(this.txtbxFloorNumbers, "txtbxFloorNumbers");
            this.txtbxFloorNumbers.Name = "txtbxFloorNumbers";
            // 
            // label11
            // 
            resources.ApplyResources(this.label11, "label11");
            this.label11.Name = "label11";
            // 
            // label10
            // 
            resources.ApplyResources(this.label10, "label10");
            this.label10.Name = "label10";
            // 
            // txtbxFinishingRoomNumberParam
            // 
            resources.ApplyResources(this.txtbxFinishingRoomNumberParam, "txtbxFinishingRoomNumberParam");
            this.txtbxFinishingRoomNumberParam.Name = "txtbxFinishingRoomNumberParam";
            // 
            // label19
            // 
            resources.ApplyResources(this.label19, "label19");
            this.label19.Name = "label19";
            // 
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
            // 
            // txtbxFlatNumber
            // 
            resources.ApplyResources(this.txtbxFlatNumber, "txtbxFlatNumber");
            this.txtbxFlatNumber.Name = "txtbxFlatNumber";
            // 
            // groupBox4
            // 
            resources.ApplyResources(this.groupBox4, "groupBox4");
            this.groupBox4.Controls.Add(this.dataGridView1);
            this.groupBox4.Controls.Add(this.chkbxActivateFlatography);
            this.groupBox4.Controls.Add(this.label18);
            this.groupBox4.Controls.Add(this.label12);
            this.groupBox4.Controls.Add(this.label17);
            this.groupBox4.Controls.Add(this.label16);
            this.groupBox4.Controls.Add(this.label15);
            this.groupBox4.Controls.Add(this.label14);
            this.groupBox4.Controls.Add(this.label13);
            this.groupBox4.Controls.Add(this.label9);
            this.groupBox4.Controls.Add(this.txtbxIsLivingParam);
            this.groupBox4.Controls.Add(this.txtbxAreaCoeff);
            this.groupBox4.Controls.Add(this.txtbxRoomCount);
            this.groupBox4.Controls.Add(this.txtbxFlatLivingArea);
            this.groupBox4.Controls.Add(this.txtbxFlatSumArea);
            this.groupBox4.Controls.Add(this.txtbxFlatArea);
            this.groupBox4.Controls.Add(this.txtbxFlatNumber);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.TabStop = false;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            resources.ApplyResources(this.dataGridView1, "dataGridView1");
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.RommName,
            this.RoomCoeff,
            this.RoomIsLiving});
            this.dataGridView1.Name = "dataGridView1";
            // 
            // RommName
            // 
            this.RommName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            resources.ApplyResources(this.RommName, "RommName");
            this.RommName.Name = "RommName";
            this.RommName.ReadOnly = true;
            // 
            // RoomCoeff
            // 
            this.RoomCoeff.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.RoomCoeff.FillWeight = 50F;
            resources.ApplyResources(this.RoomCoeff, "RoomCoeff");
            this.RoomCoeff.Name = "RoomCoeff";
            // 
            // RoomIsLiving
            // 
            this.RoomIsLiving.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.RoomIsLiving.FillWeight = 30F;
            resources.ApplyResources(this.RoomIsLiving, "RoomIsLiving");
            this.RoomIsLiving.Name = "RoomIsLiving";
            // 
            // chkbxActivateFlatography
            // 
            resources.ApplyResources(this.chkbxActivateFlatography, "chkbxActivateFlatography");
            this.chkbxActivateFlatography.Checked = true;
            this.chkbxActivateFlatography.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkbxActivateFlatography.Name = "chkbxActivateFlatography";
            this.chkbxActivateFlatography.UseVisualStyleBackColor = true;
            // 
            // label18
            // 
            resources.ApplyResources(this.label18, "label18");
            this.label18.Name = "label18";
            // 
            // label12
            // 
            resources.ApplyResources(this.label12, "label12");
            this.label12.Name = "label12";
            // 
            // label17
            // 
            resources.ApplyResources(this.label17, "label17");
            this.label17.Name = "label17";
            // 
            // label16
            // 
            resources.ApplyResources(this.label16, "label16");
            this.label16.Name = "label16";
            // 
            // label15
            // 
            resources.ApplyResources(this.label15, "label15");
            this.label15.Name = "label15";
            // 
            // label14
            // 
            resources.ApplyResources(this.label14, "label14");
            this.label14.Name = "label14";
            // 
            // label13
            // 
            resources.ApplyResources(this.label13, "label13");
            this.label13.Name = "label13";
            // 
            // txtbxIsLivingParam
            // 
            resources.ApplyResources(this.txtbxIsLivingParam, "txtbxIsLivingParam");
            this.txtbxIsLivingParam.Name = "txtbxIsLivingParam";
            // 
            // txtbxAreaCoeff
            // 
            resources.ApplyResources(this.txtbxAreaCoeff, "txtbxAreaCoeff");
            this.txtbxAreaCoeff.Name = "txtbxAreaCoeff";
            // 
            // txtbxRoomCount
            // 
            resources.ApplyResources(this.txtbxRoomCount, "txtbxRoomCount");
            this.txtbxRoomCount.Name = "txtbxRoomCount";
            // 
            // txtbxFlatLivingArea
            // 
            resources.ApplyResources(this.txtbxFlatLivingArea, "txtbxFlatLivingArea");
            this.txtbxFlatLivingArea.Name = "txtbxFlatLivingArea";
            // 
            // txtbxFlatSumArea
            // 
            resources.ApplyResources(this.txtbxFlatSumArea, "txtbxFlatSumArea");
            this.txtbxFlatSumArea.Name = "txtbxFlatSumArea";
            // 
            // txtbxFlatArea
            // 
            resources.ApplyResources(this.txtbxFlatArea, "txtbxFlatArea");
            this.txtbxFlatArea.Name = "txtbxFlatArea";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.chxbxRoomNumberToFinishing);
            this.groupBox5.Controls.Add(this.label20);
            this.groupBox5.Controls.Add(this.label19);
            this.groupBox5.Controls.Add(this.txtbxFinishingRoomNumberParam);
            resources.ApplyResources(this.groupBox5, "groupBox5");
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.TabStop = false;
            // 
            // chxbxRoomNumberToFinishing
            // 
            resources.ApplyResources(this.chxbxRoomNumberToFinishing, "chxbxRoomNumberToFinishing");
            this.chxbxRoomNumberToFinishing.Checked = true;
            this.chxbxRoomNumberToFinishing.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chxbxRoomNumberToFinishing.Name = "chxbxRoomNumberToFinishing";
            this.chxbxRoomNumberToFinishing.UseVisualStyleBackColor = true;
            // 
            // label20
            // 
            resources.ApplyResources(this.label20, "label20");
            this.label20.Name = "label20";
            // 
            // buttonReset
            // 
            resources.ApplyResources(this.buttonReset, "buttonReset");
            this.buttonReset.Name = "buttonReset";
            this.buttonReset.UseVisualStyleBackColor = true;
            this.buttonReset.Click += new System.EventHandler(this.buttonReset_Click);
            // 
            // FormArchParametrisation
            // 
            this.AcceptButton = this.buttonOk;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.Controls.Add(this.buttonReset);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.buttonOk);
            this.Controls.Add(this.buttonCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FormArchParametrisation";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtbxMirroredParameter;
        private System.Windows.Forms.TextBox txtbxMirroredText;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkbxActivateMirrored;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox txtbxOpeningsAreaParam;
        private System.Windows.Forms.TextBox txtbxOpeningHeightParam;
        private System.Windows.Forms.TextBox txtbxOpeningWidthParam;
        private System.Windows.Forms.CheckBox chkbxActivateOpeningsArea;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.CheckBox chkboxRoomFinishingSequenceNumbers;
        private System.Windows.Forms.TextBox txtbxWallNumbers;
        private System.Windows.Forms.TextBox txtbxFloorNumbers;
        private System.Windows.Forms.TextBox txtbxFlatNumber;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.CheckBox chkbxActivateFlatography;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox txtbxAreaCoeff;
        private System.Windows.Forms.TextBox txtbxRoomCount;
        private System.Windows.Forms.TextBox txtbxFlatLivingArea;
        private System.Windows.Forms.TextBox txtbxFlatSumArea;
        private System.Windows.Forms.TextBox txtbxFlatArea;
        private System.Windows.Forms.TextBox txtbxFinishingRoomNumberParam;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.CheckBox chxbxRoomNumberToFinishing;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.CheckBox chkbxFloorsIncludeInFinishing;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txtbxIsLivingParam;
        private System.Windows.Forms.CheckBox checkBoxUseRoomNames;
        private System.Windows.Forms.CheckBox chkBoxCalculateOneRoomOpenings;
        private System.Windows.Forms.DataGridViewTextBoxColumn RommName;
        private System.Windows.Forms.DataGridViewTextBoxColumn RoomCoeff;
        private System.Windows.Forms.DataGridViewCheckBoxColumn RoomIsLiving;
        private System.Windows.Forms.Button buttonReset;
    }
}