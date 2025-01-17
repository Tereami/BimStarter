namespace Tools.Autonumber
{
    partial class FormAutonumber
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAutonumber));
            buttonCancel = new System.Windows.Forms.Button();
            buttonOk = new System.Windows.Forms.Button();
            label1 = new System.Windows.Forms.Label();
            numericStart = new System.Windows.Forms.NumericUpDown();
            label2 = new System.Windows.Forms.Label();
            numericEnd = new System.Windows.Forms.NumericUpDown();
            checkBoxSuppressTooltips = new System.Windows.Forms.CheckBox();
            trackBarSpeed = new System.Windows.Forms.TrackBar();
            label5 = new System.Windows.Forms.Label();
            label6 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)numericStart).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericEnd).BeginInit();
            ((System.ComponentModel.ISupportInitialize)trackBarSpeed).BeginInit();
            SuspendLayout();
            // 
            // buttonCancel
            // 
            resources.ApplyResources(buttonCancel, "buttonCancel");
            buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            buttonCancel.Name = "buttonCancel";
            buttonCancel.UseVisualStyleBackColor = true;
            buttonCancel.Click += buttonCancel_Click;
            // 
            // buttonOk
            // 
            resources.ApplyResources(buttonOk, "buttonOk");
            buttonOk.Name = "buttonOk";
            buttonOk.UseVisualStyleBackColor = true;
            buttonOk.Click += buttonOk_Click;
            // 
            // label1
            // 
            resources.ApplyResources(label1, "label1");
            label1.Name = "label1";
            // 
            // numericStart
            // 
            resources.ApplyResources(numericStart, "numericStart");
            numericStart.Name = "numericStart";
            numericStart.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // label2
            // 
            resources.ApplyResources(label2, "label2");
            label2.Name = "label2";
            // 
            // numericEnd
            // 
            resources.ApplyResources(numericEnd, "numericEnd");
            numericEnd.Name = "numericEnd";
            numericEnd.Value = new decimal(new int[] { 10, 0, 0, 0 });
            // 
            // checkBoxSuppressTooltips
            // 
            resources.ApplyResources(checkBoxSuppressTooltips, "checkBoxSuppressTooltips");
            checkBoxSuppressTooltips.Name = "checkBoxSuppressTooltips";
            checkBoxSuppressTooltips.UseVisualStyleBackColor = true;
            // 
            // trackBarSpeed
            // 
            resources.ApplyResources(trackBarSpeed, "trackBarSpeed");
            trackBarSpeed.Name = "trackBarSpeed";
            trackBarSpeed.Value = 8;
            // 
            // label5
            // 
            resources.ApplyResources(label5, "label5");
            label5.Name = "label5";
            // 
            // label6
            // 
            resources.ApplyResources(label6, "label6");
            label6.Name = "label6";
            // 
            // FormAutonumber
            // 
            AcceptButton = buttonOk;
            resources.ApplyResources(this, "$this");
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            CancelButton = buttonCancel;
            Controls.Add(label6);
            Controls.Add(trackBarSpeed);
            Controls.Add(checkBoxSuppressTooltips);
            Controls.Add(numericEnd);
            Controls.Add(numericStart);
            Controls.Add(label5);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(buttonOk);
            Controls.Add(buttonCancel);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            Name = "FormAutonumber";
            ShowIcon = false;
            ShowInTaskbar = false;
            ((System.ComponentModel.ISupportInitialize)numericStart).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericEnd).EndInit();
            ((System.ComponentModel.ISupportInitialize)trackBarSpeed).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numericStart;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numericEnd;
        private System.Windows.Forms.CheckBox checkBoxSuppressTooltips;
        private System.Windows.Forms.TrackBar trackBarSpeed;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
    }
}