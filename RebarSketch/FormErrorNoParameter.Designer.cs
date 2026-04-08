namespace RebarSketch
{
    partial class FormErrorNoParameter
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormErrorNoParameter));
            this.label1 = new System.Windows.Forms.Label();
            this.labelParameters = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.labelFamilies = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.linkLabelHelp = new System.Windows.Forms.LinkLabel();
            this.buttonOk = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // labelParameters
            // 
            resources.ApplyResources(this.labelParameters, "labelParameters");
            this.labelParameters.Name = "labelParameters";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // labelFamilies
            // 
            resources.ApplyResources(this.labelFamilies, "labelFamilies");
            this.labelFamilies.Name = "labelFamilies";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // linkLabelHelp
            // 
            resources.ApplyResources(this.linkLabelHelp, "linkLabelHelp");
            this.linkLabelHelp.Name = "linkLabelHelp";
            this.linkLabelHelp.TabStop = true;
            this.linkLabelHelp.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelHelp_LinkClicked);
            // 
            // buttonOk
            // 
            resources.ApplyResources(this.buttonOk, "buttonOk");
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
            // 
            // FormErrorNoParameter
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.buttonOk);
            this.Controls.Add(this.linkLabelHelp);
            this.Controls.Add(this.labelFamilies);
            this.Controls.Add(this.labelParameters);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FormErrorNoParameter";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelParameters;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label labelFamilies;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.LinkLabel linkLabelHelp;
        private System.Windows.Forms.Button buttonOk;
    }
}