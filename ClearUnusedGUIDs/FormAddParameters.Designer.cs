namespace ClearUnusedGUIDs
{
    partial class FormAddParameters
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAddParameters));
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioButtonByInstance = new System.Windows.Forms.RadioButton();
            this.radioButtonByType = new System.Windows.Forms.RadioButton();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBoxGrouping = new System.Windows.Forms.ComboBox();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOk = new System.Windows.Forms.Button();
            this.listBoxParams = new System.Windows.Forms.ListBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Controls.Add(this.radioButtonByInstance);
            this.groupBox1.Controls.Add(this.radioButtonByType);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.comboBoxGrouping);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // radioButtonByInstance
            // 
            resources.ApplyResources(this.radioButtonByInstance, "radioButtonByInstance");
            this.radioButtonByInstance.Name = "radioButtonByInstance";
            this.radioButtonByInstance.UseVisualStyleBackColor = true;
            // 
            // radioButtonByType
            // 
            resources.ApplyResources(this.radioButtonByType, "radioButtonByType");
            this.radioButtonByType.Checked = true;
            this.radioButtonByType.Name = "radioButtonByType";
            this.radioButtonByType.TabStop = true;
            this.radioButtonByType.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // comboBoxGrouping
            // 
            resources.ApplyResources(this.comboBoxGrouping, "comboBoxGrouping");
            this.comboBoxGrouping.FormattingEnabled = true;
            this.comboBoxGrouping.Name = "comboBoxGrouping";
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
            // listBoxParams
            // 
            resources.ApplyResources(this.listBoxParams, "listBoxParams");
            this.listBoxParams.FormattingEnabled = true;
            this.listBoxParams.Name = "listBoxParams";
            this.listBoxParams.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            // 
            // FormAddParameters
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.listBoxParams);
            this.Controls.Add(this.buttonOk);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FormAddParameters";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioButtonByInstance;
        private System.Windows.Forms.RadioButton radioButtonByType;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboBoxGrouping;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.ListBox listBoxParams;
    }
}