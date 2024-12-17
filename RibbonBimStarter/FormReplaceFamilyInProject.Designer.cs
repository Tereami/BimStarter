
namespace RibbonBimStarter
{
    partial class FormReplaceFamilyInProject
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
            this.labelFamilyName = new System.Windows.Forms.Label();
            this.labelOldVersion = new System.Windows.Forms.Label();
            this.labelNewVersion = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.richTextBoxChanges = new System.Windows.Forms.RichTextBox();
            this.buttonReplace = new System.Windows.Forms.Button();
            this.buttonUseOld = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // labelFamilyName
            // 
            this.labelFamilyName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelFamilyName.Location = new System.Drawing.Point(12, 9);
            this.labelFamilyName.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.labelFamilyName.Name = "labelFamilyName";
            this.labelFamilyName.Size = new System.Drawing.Size(313, 34);
            this.labelFamilyName.TabIndex = 10;
            this.labelFamilyName.Text = "Ранее загруженное в проект семейство \"Имя семейства длинное длинное\" устарело.";
            // 
            // labelOldVersion
            // 
            this.labelOldVersion.AutoSize = true;
            this.labelOldVersion.Location = new System.Drawing.Point(12, 45);
            this.labelOldVersion.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.labelOldVersion.Name = "labelOldVersion";
            this.labelOldVersion.Size = new System.Drawing.Size(120, 13);
            this.labelOldVersion.TabIndex = 11;
            this.labelOldVersion.Text = "Версия в проекте: №0";
            // 
            // labelNewVersion
            // 
            this.labelNewVersion.AutoSize = true;
            this.labelNewVersion.Location = new System.Drawing.Point(12, 62);
            this.labelNewVersion.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.labelNewVersion.Name = "labelNewVersion";
            this.labelNewVersion.Size = new System.Drawing.Size(124, 13);
            this.labelNewVersion.TabIndex = 12;
            this.labelNewVersion.Text = "Доступная версия: №1";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 79);
            this.label4.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(68, 13);
            this.label4.TabIndex = 13;
            this.label4.Text = "Изменения:";
            // 
            // richTextBoxChanges
            // 
            this.richTextBoxChanges.Location = new System.Drawing.Point(12, 97);
            this.richTextBoxChanges.Name = "richTextBoxChanges";
            this.richTextBoxChanges.ReadOnly = true;
            this.richTextBoxChanges.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedVertical;
            this.richTextBoxChanges.Size = new System.Drawing.Size(313, 61);
            this.richTextBoxChanges.TabIndex = 5;
            this.richTextBoxChanges.Text = "";
            // 
            // buttonReplace
            // 
            this.buttonReplace.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonReplace.Location = new System.Drawing.Point(12, 211);
            this.buttonReplace.Name = "buttonReplace";
            this.buttonReplace.Size = new System.Drawing.Size(313, 23);
            this.buttonReplace.TabIndex = 1;
            this.buttonReplace.Text = "Обновить семейство в проекте";
            this.buttonReplace.UseVisualStyleBackColor = true;
            this.buttonReplace.Click += new System.EventHandler(this.buttonReplace_Click);
            // 
            // buttonUseOld
            // 
            this.buttonUseOld.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonUseOld.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonUseOld.Location = new System.Drawing.Point(93, 240);
            this.buttonUseOld.Name = "buttonUseOld";
            this.buttonUseOld.Size = new System.Drawing.Size(232, 23);
            this.buttonUseOld.TabIndex = 2;
            this.buttonUseOld.Text = "Использовать имеющееся";
            this.buttonUseOld.UseVisualStyleBackColor = true;
            this.buttonUseOld.Click += new System.EventHandler(this.buttonUseOld_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(12, 240);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 14;
            this.buttonCancel.Text = "Отмена";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(12, 163);
            this.label5.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(313, 37);
            this.label5.TabIndex = 15;
            this.label5.Text = "Рекомендуем запустить Проверку семейств для пакетного обновления всех семейств.";
            // 
            // FormReplaceFamilyInProject
            // 
            this.AcceptButton = this.buttonReplace;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(337, 275);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonUseOld);
            this.Controls.Add(this.buttonReplace);
            this.Controls.Add(this.richTextBoxChanges);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.labelNewVersion);
            this.Controls.Add(this.labelOldVersion);
            this.Controls.Add(this.labelFamilyName);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FormReplaceFamilyInProject";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Загрузка семейства";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelFamilyName;
        private System.Windows.Forms.Label labelOldVersion;
        private System.Windows.Forms.Label labelNewVersion;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.RichTextBox richTextBoxChanges;
        private System.Windows.Forms.Button buttonReplace;
        private System.Windows.Forms.Button buttonUseOld;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Label label5;
    }
}