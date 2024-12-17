namespace RebarTools
{
    partial class FormDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormDialog));
            this.buttonOk = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.checkBoxBody = new System.Windows.Forms.CheckBox();
            this.checkBoxOverlay = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioButtonAsBodyOff = new System.Windows.Forms.RadioButton();
            this.radioButtonAsBodyOn = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.radioButtonOverlayOff = new System.Windows.Forms.RadioButton();
            this.radioButtonOverlayOn = new System.Windows.Forms.RadioButton();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
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
            // checkBoxBody
            // 
            resources.ApplyResources(this.checkBoxBody, "checkBoxBody");
            this.checkBoxBody.Checked = true;
            this.checkBoxBody.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxBody.Name = "checkBoxBody";
            this.checkBoxBody.UseVisualStyleBackColor = true;
            this.checkBoxBody.CheckedChanged += new System.EventHandler(this.checkBoxBody_CheckedChanged);
            // 
            // checkBoxOverlay
            // 
            resources.ApplyResources(this.checkBoxOverlay, "checkBoxOverlay");
            this.checkBoxOverlay.Name = "checkBoxOverlay";
            this.checkBoxOverlay.UseVisualStyleBackColor = true;
            this.checkBoxOverlay.CheckedChanged += new System.EventHandler(this.checkBoxOverlay_CheckedChanged);
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Controls.Add(this.radioButtonAsBodyOff);
            this.groupBox1.Controls.Add(this.radioButtonAsBodyOn);
            this.groupBox1.Controls.Add(this.checkBoxBody);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // radioButtonAsBodyOff
            // 
            resources.ApplyResources(this.radioButtonAsBodyOff, "radioButtonAsBodyOff");
            this.radioButtonAsBodyOff.Name = "radioButtonAsBodyOff";
            this.radioButtonAsBodyOff.UseVisualStyleBackColor = true;
            // 
            // radioButtonAsBodyOn
            // 
            resources.ApplyResources(this.radioButtonAsBodyOn, "radioButtonAsBodyOn");
            this.radioButtonAsBodyOn.Checked = true;
            this.radioButtonAsBodyOn.Name = "radioButtonAsBodyOn";
            this.radioButtonAsBodyOn.TabStop = true;
            this.radioButtonAsBodyOn.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Controls.Add(this.radioButtonOverlayOff);
            this.groupBox2.Controls.Add(this.radioButtonOverlayOn);
            this.groupBox2.Controls.Add(this.checkBoxOverlay);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // radioButtonOverlayOff
            // 
            resources.ApplyResources(this.radioButtonOverlayOff, "radioButtonOverlayOff");
            this.radioButtonOverlayOff.Checked = true;
            this.radioButtonOverlayOff.Name = "radioButtonOverlayOff";
            this.radioButtonOverlayOff.TabStop = true;
            this.radioButtonOverlayOff.UseVisualStyleBackColor = true;
            // 
            // radioButtonOverlayOn
            // 
            resources.ApplyResources(this.radioButtonOverlayOn, "radioButtonOverlayOn");
            this.radioButtonOverlayOn.Name = "radioButtonOverlayOn";
            this.radioButtonOverlayOn.UseVisualStyleBackColor = true;
            // 
            // FormDialog
            // 
            this.AcceptButton = this.buttonOk;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOk);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FormDialog";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.CheckBox checkBoxBody;
        private System.Windows.Forms.CheckBox checkBoxOverlay;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioButtonAsBodyOff;
        private System.Windows.Forms.RadioButton radioButtonAsBodyOn;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton radioButtonOverlayOff;
        private System.Windows.Forms.RadioButton radioButtonOverlayOn;

    }
}