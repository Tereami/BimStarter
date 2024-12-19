namespace RevitGraphicsOverride
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioButtonCurrentVIew = new System.Windows.Forms.RadioButton();
            this.radioButtonAllProject = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.checkBoxShowResults = new System.Windows.Forms.CheckBox();
            this.checkBoxSelectElements = new System.Windows.Forms.CheckBox();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOk = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Controls.Add(this.radioButtonCurrentVIew);
            this.groupBox1.Controls.Add(this.radioButtonAllProject);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // radioButtonCurrentVIew
            // 
            resources.ApplyResources(this.radioButtonCurrentVIew, "radioButtonCurrentVIew");
            this.radioButtonCurrentVIew.Checked = true;
            this.radioButtonCurrentVIew.Name = "radioButtonCurrentVIew";
            this.radioButtonCurrentVIew.TabStop = true;
            this.radioButtonCurrentVIew.UseVisualStyleBackColor = true;
            // 
            // radioButtonAllProject
            // 
            resources.ApplyResources(this.radioButtonAllProject, "radioButtonAllProject");
            this.radioButtonAllProject.Name = "radioButtonAllProject";
            this.radioButtonAllProject.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Controls.Add(this.checkBoxShowResults);
            this.groupBox2.Controls.Add(this.checkBoxSelectElements);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // checkBoxShowResults
            // 
            resources.ApplyResources(this.checkBoxShowResults, "checkBoxShowResults");
            this.checkBoxShowResults.Checked = true;
            this.checkBoxShowResults.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxShowResults.Name = "checkBoxShowResults";
            this.checkBoxShowResults.UseVisualStyleBackColor = true;
            // 
            // checkBoxSelectElements
            // 
            resources.ApplyResources(this.checkBoxSelectElements, "checkBoxSelectElements");
            this.checkBoxSelectElements.Name = "checkBoxSelectElements";
            this.checkBoxSelectElements.UseVisualStyleBackColor = true;
            // 
            // buttonCancel
            // 
            resources.ApplyResources(this.buttonCancel, "buttonCancel");
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
            // Form1
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.buttonOk);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Form1";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioButtonCurrentVIew;
        private System.Windows.Forms.RadioButton radioButtonAllProject;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox checkBoxShowResults;
        private System.Windows.Forms.CheckBox checkBoxSelectElements;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOk;
    }
}