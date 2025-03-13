namespace RibbonBimStarter
{
    partial class FormVersionInfo
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormVersionInfo));
            label1 = new System.Windows.Forms.Label();
            labelVersion = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            labelNews = new System.Windows.Forms.Label();
            buttonOk = new System.Windows.Forms.Button();
            linkLabelBimstarter = new System.Windows.Forms.LinkLabel();
            SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(label1, "label1");
            label1.Name = "label1";
            // 
            // labelVersion
            // 
            resources.ApplyResources(labelVersion, "labelVersion");
            labelVersion.Name = "labelVersion";
            // 
            // label3
            // 
            resources.ApplyResources(label3, "label3");
            label3.Name = "label3";
            // 
            // labelNews
            // 
            resources.ApplyResources(labelNews, "labelNews");
            labelNews.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            labelNews.Name = "labelNews";
            // 
            // buttonOk
            // 
            resources.ApplyResources(buttonOk, "buttonOk");
            buttonOk.Name = "buttonOk";
            buttonOk.UseVisualStyleBackColor = true;
            buttonOk.Click += buttonOk_Click;
            // 
            // linkLabelBimstarter
            // 
            resources.ApplyResources(linkLabelBimstarter, "linkLabelBimstarter");
            linkLabelBimstarter.Name = "linkLabelBimstarter";
            linkLabelBimstarter.TabStop = true;
            linkLabelBimstarter.LinkClicked += linkLabelBimstarter_LinkClicked;
            // 
            // FormVersionInfo
            // 
            AcceptButton = buttonOk;
            resources.ApplyResources(this, "$this");
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(linkLabelBimstarter);
            Controls.Add(buttonOk);
            Controls.Add(labelNews);
            Controls.Add(label3);
            Controls.Add(labelVersion);
            Controls.Add(label1);
            Name = "FormVersionInfo";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelVersion;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label labelNews;
        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.LinkLabel linkLabelBimstarter;
    }
}