namespace LinkWriter
{
    partial class FormSelectParametersToCopy
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
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonNext = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageSheet = new System.Windows.Forms.TabPage();
            this.checkedListBoxSheet = new System.Windows.Forms.CheckedListBox();
            this.tabPageProject = new System.Windows.Forms.TabPage();
            this.checkedListBoxProject = new System.Windows.Forms.CheckedListBox();
            this.tabPageTitleblock = new System.Windows.Forms.TabPage();
            this.tabPageTitleblockType = new System.Windows.Forms.TabPage();
            this.checkedListBoxTitleblock = new System.Windows.Forms.CheckedListBox();
            this.checkedListBoxType = new System.Windows.Forms.CheckedListBox();
            this.tabControl1.SuspendLayout();
            this.tabPageSheet.SuspendLayout();
            this.tabPageProject.SuspendLayout();
            this.tabPageTitleblock.SuspendLayout();
            this.tabPageTitleblockType.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(12, 355);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 0;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonNext
            // 
            this.buttonNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonNext.Location = new System.Drawing.Point(267, 355);
            this.buttonNext.Name = "buttonNext";
            this.buttonNext.Size = new System.Drawing.Size(75, 23);
            this.buttonNext.TabIndex = 0;
            this.buttonNext.Text = "Next >>";
            this.buttonNext.UseVisualStyleBackColor = true;
            this.buttonNext.Click += new System.EventHandler(this.buttonNext_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPageSheet);
            this.tabControl1.Controls.Add(this.tabPageProject);
            this.tabControl1.Controls.Add(this.tabPageTitleblock);
            this.tabControl1.Controls.Add(this.tabPageTitleblockType);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(330, 337);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPageSheet
            // 
            this.tabPageSheet.Controls.Add(this.checkedListBoxSheet);
            this.tabPageSheet.Location = new System.Drawing.Point(4, 22);
            this.tabPageSheet.Name = "tabPageSheet";
            this.tabPageSheet.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageSheet.Size = new System.Drawing.Size(322, 311);
            this.tabPageSheet.TabIndex = 0;
            this.tabPageSheet.Text = "Sheet";
            this.tabPageSheet.UseVisualStyleBackColor = true;
            // 
            // checkedListBoxSheet
            // 
            this.checkedListBoxSheet.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.checkedListBoxSheet.FormattingEnabled = true;
            this.checkedListBoxSheet.Location = new System.Drawing.Point(6, 6);
            this.checkedListBoxSheet.Name = "checkedListBoxSheet";
            this.checkedListBoxSheet.Size = new System.Drawing.Size(310, 289);
            this.checkedListBoxSheet.TabIndex = 2;
            // 
            // tabPageProject
            // 
            this.tabPageProject.Controls.Add(this.checkedListBoxProject);
            this.tabPageProject.Location = new System.Drawing.Point(4, 22);
            this.tabPageProject.Name = "tabPageProject";
            this.tabPageProject.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageProject.Size = new System.Drawing.Size(322, 311);
            this.tabPageProject.TabIndex = 1;
            this.tabPageProject.Text = "Project info";
            this.tabPageProject.UseVisualStyleBackColor = true;
            // 
            // checkedListBoxProject
            // 
            this.checkedListBoxProject.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.checkedListBoxProject.FormattingEnabled = true;
            this.checkedListBoxProject.Location = new System.Drawing.Point(6, 6);
            this.checkedListBoxProject.Name = "checkedListBoxProject";
            this.checkedListBoxProject.Size = new System.Drawing.Size(310, 289);
            this.checkedListBoxProject.TabIndex = 0;
            // 
            // tabPageTitleblock
            // 
            this.tabPageTitleblock.Controls.Add(this.checkedListBoxTitleblock);
            this.tabPageTitleblock.Location = new System.Drawing.Point(4, 22);
            this.tabPageTitleblock.Name = "tabPageTitleblock";
            this.tabPageTitleblock.Size = new System.Drawing.Size(322, 311);
            this.tabPageTitleblock.TabIndex = 2;
            this.tabPageTitleblock.Text = "Titleblock";
            this.tabPageTitleblock.UseVisualStyleBackColor = true;
            // 
            // tabPageTitleblockType
            // 
            this.tabPageTitleblockType.Controls.Add(this.checkedListBoxType);
            this.tabPageTitleblockType.Location = new System.Drawing.Point(4, 22);
            this.tabPageTitleblockType.Name = "tabPageTitleblockType";
            this.tabPageTitleblockType.Size = new System.Drawing.Size(322, 311);
            this.tabPageTitleblockType.TabIndex = 3;
            this.tabPageTitleblockType.Text = "Titleblock type";
            this.tabPageTitleblockType.UseVisualStyleBackColor = true;
            // 
            // checkedListBoxTitleblock
            // 
            this.checkedListBoxTitleblock.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.checkedListBoxTitleblock.FormattingEnabled = true;
            this.checkedListBoxTitleblock.Location = new System.Drawing.Point(6, 6);
            this.checkedListBoxTitleblock.Name = "checkedListBoxTitleblock";
            this.checkedListBoxTitleblock.Size = new System.Drawing.Size(310, 289);
            this.checkedListBoxTitleblock.TabIndex = 0;
            // 
            // checkedListBoxType
            // 
            this.checkedListBoxType.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.checkedListBoxType.FormattingEnabled = true;
            this.checkedListBoxType.Location = new System.Drawing.Point(6, 6);
            this.checkedListBoxType.Name = "checkedListBoxType";
            this.checkedListBoxType.Size = new System.Drawing.Size(310, 289);
            this.checkedListBoxType.TabIndex = 0;
            // 
            // FormSelectParametersToCopy
            // 
            this.AcceptButton = this.buttonNext;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(354, 390);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.buttonNext);
            this.Controls.Add(this.buttonCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MaximumSize = new System.Drawing.Size(787, 970);
            this.MinimumSize = new System.Drawing.Size(346, 379);
            this.Name = "FormSelectParametersToCopy";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Choose parameters to copy";
            this.tabControl1.ResumeLayout(false);
            this.tabPageSheet.ResumeLayout(false);
            this.tabPageProject.ResumeLayout(false);
            this.tabPageTitleblock.ResumeLayout(false);
            this.tabPageTitleblockType.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonNext;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageSheet;
        private System.Windows.Forms.TabPage tabPageProject;
        private System.Windows.Forms.TabPage tabPageTitleblock;
        private System.Windows.Forms.TabPage tabPageTitleblockType;
        private System.Windows.Forms.CheckedListBox checkedListBoxSheet;
        private System.Windows.Forms.CheckedListBox checkedListBoxProject;
        private System.Windows.Forms.CheckedListBox checkedListBoxTitleblock;
        private System.Windows.Forms.CheckedListBox checkedListBoxType;
    }
}