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
            this.checkedListBoxSheet.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.checkedListBoxSheet.CheckOnClick = true;
            this.checkedListBoxSheet.FormattingEnabled = true;
            this.checkedListBoxSheet.Location = new System.Drawing.Point(12, 27);
            this.checkedListBoxSheet.Name = "checkedListBoxSheet";
            this.checkedListBoxSheet.Size = new System.Drawing.Size(203, 424);
            this.checkedListBoxSheet.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Sheet:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(218, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Titleblock:";
            // 
            // checkedListBoxTitleblock
            // 
            this.checkedListBoxTitleblock.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.checkedListBoxTitleblock.CheckOnClick = true;
            this.checkedListBoxTitleblock.FormattingEnabled = true;
            this.checkedListBoxTitleblock.Location = new System.Drawing.Point(221, 27);
            this.checkedListBoxTitleblock.Name = "checkedListBoxTitleblock";
            this.checkedListBoxTitleblock.Size = new System.Drawing.Size(203, 424);
            this.checkedListBoxTitleblock.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(427, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(79, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Titleblock type:";
            // 
            // checkedListBoxTitleblockType
            // 
            this.checkedListBoxTitleblockType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.checkedListBoxTitleblockType.CheckOnClick = true;
            this.checkedListBoxTitleblockType.FormattingEnabled = true;
            this.checkedListBoxTitleblockType.Location = new System.Drawing.Point(430, 27);
            this.checkedListBoxTitleblockType.Name = "checkedListBoxTitleblockType";
            this.checkedListBoxTitleblockType.Size = new System.Drawing.Size(203, 424);
            this.checkedListBoxTitleblockType.TabIndex = 0;
            // 
            // checkedListBoxProjectInfo
            // 
            this.checkedListBoxProjectInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.checkedListBoxProjectInfo.CheckOnClick = true;
            this.checkedListBoxProjectInfo.FormattingEnabled = true;
            this.checkedListBoxProjectInfo.Location = new System.Drawing.Point(639, 27);
            this.checkedListBoxProjectInfo.Name = "checkedListBoxProjectInfo";
            this.checkedListBoxProjectInfo.Size = new System.Drawing.Size(203, 424);
            this.checkedListBoxProjectInfo.TabIndex = 0;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(636, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(97, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "Project information:";
            // 
            // buttonNext
            // 
            this.buttonNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonNext.Location = new System.Drawing.Point(679, 461);
            this.buttonNext.Name = "buttonNext";
            this.buttonNext.Size = new System.Drawing.Size(163, 23);
            this.buttonNext.TabIndex = 2;
            this.buttonNext.Text = "Next >>";
            this.buttonNext.UseVisualStyleBackColor = true;
            this.buttonNext.Click += new System.EventHandler(this.buttonNext_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(12, 461);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 3;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonReset
            // 
            this.buttonReset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonReset.Location = new System.Drawing.Point(93, 461);
            this.buttonReset.Name = "buttonReset";
            this.buttonReset.Size = new System.Drawing.Size(75, 23);
            this.buttonReset.TabIndex = 4;
            this.buttonReset.Text = "Reset";
            this.buttonReset.UseVisualStyleBackColor = true;
            this.buttonReset.Click += new System.EventHandler(this.buttonReset_Click);
            // 
            // FormSelectParameters
            // 
            this.AcceptButton = this.buttonNext;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(851, 496);
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
            this.MaximumSize = new System.Drawing.Size(867, 1000);
            this.MinimumSize = new System.Drawing.Size(867, 500);
            this.Name = "FormSelectParameters";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Choose parameters";
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