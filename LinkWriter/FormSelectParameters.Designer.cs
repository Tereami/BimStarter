namespace LinkWriter
{
    partial class FormSelectParameters
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSelectParameters));
            this.checkedListBoxSheet = new System.Windows.Forms.CheckedListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.checkedListBoxTitleblock = new System.Windows.Forms.CheckedListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.checkedListBoxTitleblockType = new System.Windows.Forms.CheckedListBox();
            this.checkedListBoxProjectInfo = new System.Windows.Forms.CheckedListBox();
            this.label4 = new System.Windows.Forms.Label();
            this.buttonNext = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonReset = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // checkedListBoxSheet
            // 
            resources.ApplyResources(this.checkedListBoxSheet, "checkedListBoxSheet");
            this.checkedListBoxSheet.CheckOnClick = true;
            this.checkedListBoxSheet.FormattingEnabled = true;
            this.checkedListBoxSheet.Name = "checkedListBoxSheet";
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
            // checkedListBoxTitleblock
            // 
            resources.ApplyResources(this.checkedListBoxTitleblock, "checkedListBoxTitleblock");
            this.checkedListBoxTitleblock.CheckOnClick = true;
            this.checkedListBoxTitleblock.FormattingEnabled = true;
            this.checkedListBoxTitleblock.Name = "checkedListBoxTitleblock";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // checkedListBoxTitleblockType
            // 
            resources.ApplyResources(this.checkedListBoxTitleblockType, "checkedListBoxTitleblockType");
            this.checkedListBoxTitleblockType.CheckOnClick = true;
            this.checkedListBoxTitleblockType.FormattingEnabled = true;
            this.checkedListBoxTitleblockType.Name = "checkedListBoxTitleblockType";
            // 
            // checkedListBoxProjectInfo
            // 
            resources.ApplyResources(this.checkedListBoxProjectInfo, "checkedListBoxProjectInfo");
            this.checkedListBoxProjectInfo.CheckOnClick = true;
            this.checkedListBoxProjectInfo.FormattingEnabled = true;
            this.checkedListBoxProjectInfo.Name = "checkedListBoxProjectInfo";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // buttonNext
            // 
            resources.ApplyResources(this.buttonNext, "buttonNext");
            this.buttonNext.Name = "buttonNext";
            this.buttonNext.UseVisualStyleBackColor = true;
            this.buttonNext.Click += new System.EventHandler(this.buttonNext_Click);
            // 
            // buttonCancel
            // 
            resources.ApplyResources(this.buttonCancel, "buttonCancel");
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonReset
            // 
            resources.ApplyResources(this.buttonReset, "buttonReset");
            this.buttonReset.Name = "buttonReset";
            this.buttonReset.UseVisualStyleBackColor = true;
            this.buttonReset.Click += new System.EventHandler(this.buttonReset_Click);
            // 
            // FormSelectParameters
            // 
            this.AcceptButton = this.buttonNext;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.Controls.Add(this.buttonReset);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonNext);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.checkedListBoxProjectInfo);
            this.Controls.Add(this.checkedListBoxTitleblockType);
            this.Controls.Add(this.checkedListBoxTitleblock);
            this.Controls.Add(this.checkedListBoxSheet);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "FormSelectParameters";
            this.ShowIcon = false;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckedListBox checkedListBoxSheet;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckedListBox checkedListBoxTitleblock;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckedListBox checkedListBoxTitleblockType;
        private System.Windows.Forms.CheckedListBox checkedListBoxProjectInfo;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button buttonNext;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonReset;
    }
}