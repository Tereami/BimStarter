namespace RevitAreaReinforcement
{
    partial class DialogWindowFloor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DialogWindowFloor));
            this.comboBoxType = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.numInterval = new System.Windows.Forms.NumericUpDown();
            this.buttonOk = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.numCoverBottom = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.numCoverTop = new System.Windows.Forms.NumericUpDown();
            this.checkBoxUseDirection = new System.Windows.Forms.CheckBox();
            this.checkBoxTurnTopBars = new System.Windows.Forms.CheckBox();
            this.checkBoxTurnBottomBars = new System.Windows.Forms.CheckBox();
            this.checkBoxSkipAlreadyReinforced = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.numInterval)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCoverBottom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCoverTop)).BeginInit();
            this.SuspendLayout();
            // 
            // comboBoxType
            // 
            resources.ApplyResources(this.comboBoxType, "comboBoxType");
            this.comboBoxType.FormattingEnabled = true;
            this.comboBoxType.Name = "comboBoxType";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // numInterval
            // 
            resources.ApplyResources(this.numInterval, "numInterval");
            this.numInterval.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numInterval.Name = "numInterval";
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
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // numCoverBottom
            // 
            resources.ApplyResources(this.numCoverBottom, "numCoverBottom");
            this.numCoverBottom.Name = "numCoverBottom";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // numCoverTop
            // 
            resources.ApplyResources(this.numCoverTop, "numCoverTop");
            this.numCoverTop.Name = "numCoverTop";
            // 
            // checkBoxUseDirection
            // 
            resources.ApplyResources(this.checkBoxUseDirection, "checkBoxUseDirection");
            this.checkBoxUseDirection.Name = "checkBoxUseDirection";
            this.checkBoxUseDirection.UseVisualStyleBackColor = true;
            // 
            // checkBoxTurnTopBars
            // 
            resources.ApplyResources(this.checkBoxTurnTopBars, "checkBoxTurnTopBars");
            this.checkBoxTurnTopBars.Name = "checkBoxTurnTopBars";
            this.checkBoxTurnTopBars.UseVisualStyleBackColor = true;
            // 
            // checkBoxTurnBottomBars
            // 
            resources.ApplyResources(this.checkBoxTurnBottomBars, "checkBoxTurnBottomBars");
            this.checkBoxTurnBottomBars.Checked = true;
            this.checkBoxTurnBottomBars.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxTurnBottomBars.Name = "checkBoxTurnBottomBars";
            this.checkBoxTurnBottomBars.UseVisualStyleBackColor = true;
            // 
            // checkBoxSkipAlreadyReinforced
            // 
            resources.ApplyResources(this.checkBoxSkipAlreadyReinforced, "checkBoxSkipAlreadyReinforced");
            this.checkBoxSkipAlreadyReinforced.Checked = true;
            this.checkBoxSkipAlreadyReinforced.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxSkipAlreadyReinforced.Name = "checkBoxSkipAlreadyReinforced";
            this.checkBoxSkipAlreadyReinforced.UseVisualStyleBackColor = true;
            // 
            // DialogWindowFloor
            // 
            this.AcceptButton = this.buttonOk;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.Controls.Add(this.checkBoxSkipAlreadyReinforced);
            this.Controls.Add(this.checkBoxTurnBottomBars);
            this.Controls.Add(this.checkBoxTurnTopBars);
            this.Controls.Add(this.checkBoxUseDirection);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOk);
            this.Controls.Add(this.numCoverTop);
            this.Controls.Add(this.numCoverBottom);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.numInterval);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBoxType);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "DialogWindowFloor";
            this.Load += new System.EventHandler(this.DialogWindowFloor_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numInterval)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCoverBottom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCoverTop)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBoxType;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numInterval;
        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.NumericUpDown numCoverBottom;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown numCoverTop;
        private System.Windows.Forms.CheckBox checkBoxUseDirection;
        private System.Windows.Forms.CheckBox checkBoxTurnTopBars;
        private System.Windows.Forms.CheckBox checkBoxTurnBottomBars;
        private System.Windows.Forms.CheckBox checkBoxSkipAlreadyReinforced;
    }
}