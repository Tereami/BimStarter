namespace RevitAreaReinforcement
{
    partial class FormAddShortcutsSelect
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAddShortcutsSelect));
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonDefaultShortcuts = new System.Windows.Forms.Button();
            this.buttonCustomShortcuts = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.SuspendLayout();
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
            // buttonDefaultShortcuts
            // 
            resources.ApplyResources(this.buttonDefaultShortcuts, "buttonDefaultShortcuts");
            this.buttonDefaultShortcuts.Name = "buttonDefaultShortcuts";
            this.buttonDefaultShortcuts.UseVisualStyleBackColor = true;
            this.buttonDefaultShortcuts.Click += new System.EventHandler(this.buttonDefaultShortcuts_Click);
            // 
            // buttonCustomShortcuts
            // 
            resources.ApplyResources(this.buttonCustomShortcuts, "buttonCustomShortcuts");
            this.buttonCustomShortcuts.Name = "buttonCustomShortcuts";
            this.buttonCustomShortcuts.UseVisualStyleBackColor = true;
            this.buttonCustomShortcuts.Click += new System.EventHandler(this.buttonCustomShortcuts_Click);
            // 
            // label10
            // 
            resources.ApplyResources(this.label10, "label10");
            this.label10.Name = "label10";
            // 
            // FormAddShortcutsSelect
            // 
            this.AcceptButton = this.buttonDefaultShortcuts;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label10);
            this.Controls.Add(this.buttonCustomShortcuts);
            this.Controls.Add(this.buttonDefaultShortcuts);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FormAddShortcutsSelect";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonDefaultShortcuts;
        private System.Windows.Forms.Button buttonCustomShortcuts;
        private System.Windows.Forms.Label label10;
    }
}