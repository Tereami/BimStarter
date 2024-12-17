
namespace RibbonBimStarter
{
    partial class FormCheckFamilies
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabDuplicates = new System.Windows.Forms.TabPage();
            this.treeViewDuplicates = new System.Windows.Forms.TreeView();
            this.tabVersions = new System.Windows.Forms.TabPage();
            this.buttonRefreshAll = new System.Windows.Forms.Button();
            this.listViewObsoleteFams = new System.Windows.Forms.ListView();
            this.columnId = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnFamName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnProjVersion = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnServerVersion = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnChanges = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnOldVersGuid = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tabInvalid = new System.Windows.Forms.TabPage();
            this.listViewInvalidFamilies = new System.Windows.Forms.ListView();
            this.columnFamId = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tabIncorrectNames = new System.Windows.Forms.TabPage();
            this.listViewIncorrectNames = new System.Windows.Forms.ListView();
            this.columnFamilyId = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnProjectName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnServerName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnGuid = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.buttonOk = new System.Windows.Forms.Button();
            this.buttonExport = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabDuplicates.SuspendLayout();
            this.tabVersions.SuspendLayout();
            this.tabInvalid.SuspendLayout();
            this.tabIncorrectNames.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabVersions);
            this.tabControl1.Controls.Add(this.tabDuplicates);
            this.tabControl1.Controls.Add(this.tabInvalid);
            this.tabControl1.Controls.Add(this.tabIncorrectNames);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(639, 483);
            this.tabControl1.TabIndex = 0;
            // 
            // tabDuplicates
            // 
            this.tabDuplicates.Controls.Add(this.treeViewDuplicates);
            this.tabDuplicates.Location = new System.Drawing.Point(4, 22);
            this.tabDuplicates.Name = "tabDuplicates";
            this.tabDuplicates.Padding = new System.Windows.Forms.Padding(3);
            this.tabDuplicates.Size = new System.Drawing.Size(631, 457);
            this.tabDuplicates.TabIndex = 0;
            this.tabDuplicates.Text = "Дубли";
            this.tabDuplicates.UseVisualStyleBackColor = true;
            // 
            // treeViewDuplicates
            // 
            this.treeViewDuplicates.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.treeViewDuplicates.Location = new System.Drawing.Point(6, 6);
            this.treeViewDuplicates.Name = "treeViewDuplicates";
            this.treeViewDuplicates.Size = new System.Drawing.Size(619, 445);
            this.treeViewDuplicates.TabIndex = 0;
            // 
            // tabVersions
            // 
            this.tabVersions.Controls.Add(this.buttonRefreshAll);
            this.tabVersions.Controls.Add(this.listViewObsoleteFams);
            this.tabVersions.Location = new System.Drawing.Point(4, 22);
            this.tabVersions.Name = "tabVersions";
            this.tabVersions.Padding = new System.Windows.Forms.Padding(3);
            this.tabVersions.Size = new System.Drawing.Size(631, 457);
            this.tabVersions.TabIndex = 1;
            this.tabVersions.Text = "Устаревшие";
            this.tabVersions.UseVisualStyleBackColor = true;
            // 
            // buttonRefreshAll
            // 
            this.buttonRefreshAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonRefreshAll.Location = new System.Drawing.Point(6, 428);
            this.buttonRefreshAll.Name = "buttonRefreshAll";
            this.buttonRefreshAll.Size = new System.Drawing.Size(217, 23);
            this.buttonRefreshAll.TabIndex = 3;
            this.buttonRefreshAll.Text = "Обновить все семейства";
            this.buttonRefreshAll.UseVisualStyleBackColor = true;
            this.buttonRefreshAll.Click += new System.EventHandler(this.buttonRefreshAll_Click);
            // 
            // listViewObsoleteFams
            // 
            this.listViewObsoleteFams.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewObsoleteFams.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnId,
            this.columnFamName,
            this.columnProjVersion,
            this.columnServerVersion,
            this.columnChanges,
            this.columnOldVersGuid});
            this.listViewObsoleteFams.HideSelection = false;
            this.listViewObsoleteFams.Location = new System.Drawing.Point(6, 6);
            this.listViewObsoleteFams.Name = "listViewObsoleteFams";
            this.listViewObsoleteFams.Size = new System.Drawing.Size(619, 416);
            this.listViewObsoleteFams.TabIndex = 0;
            this.listViewObsoleteFams.UseCompatibleStateImageBehavior = false;
            this.listViewObsoleteFams.View = System.Windows.Forms.View.Details;
            // 
            // columnId
            // 
            this.columnId.Text = "ID";
            // 
            // columnFamName
            // 
            this.columnFamName.Text = "Имя семейства";
            // 
            // columnProjVersion
            // 
            this.columnProjVersion.Text = "Версия в проекте";
            // 
            // columnServerVersion
            // 
            this.columnServerVersion.Text = "Доступная версия";
            // 
            // columnChanges
            // 
            this.columnChanges.Text = "Изменения";
            // 
            // columnOldVersGuid
            // 
            this.columnOldVersGuid.Text = "Guid";
            // 
            // tabInvalid
            // 
            this.tabInvalid.Controls.Add(this.listViewInvalidFamilies);
            this.tabInvalid.Location = new System.Drawing.Point(4, 22);
            this.tabInvalid.Name = "tabInvalid";
            this.tabInvalid.Size = new System.Drawing.Size(631, 457);
            this.tabInvalid.TabIndex = 2;
            this.tabInvalid.Text = "Сторонние";
            this.tabInvalid.UseVisualStyleBackColor = true;
            // 
            // listViewInvalidFamilies
            // 
            this.listViewInvalidFamilies.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewInvalidFamilies.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnFamId,
            this.columnName});
            this.listViewInvalidFamilies.HideSelection = false;
            this.listViewInvalidFamilies.Location = new System.Drawing.Point(3, 3);
            this.listViewInvalidFamilies.Name = "listViewInvalidFamilies";
            this.listViewInvalidFamilies.Size = new System.Drawing.Size(625, 451);
            this.listViewInvalidFamilies.TabIndex = 0;
            this.listViewInvalidFamilies.UseCompatibleStateImageBehavior = false;
            this.listViewInvalidFamilies.View = System.Windows.Forms.View.Details;
            // 
            // columnFamId
            // 
            this.columnFamId.Text = "ID";
            this.columnFamId.Width = 70;
            // 
            // columnName
            // 
            this.columnName.Text = "Имя семейства";
            this.columnName.Width = 474;
            // 
            // tabIncorrectNames
            // 
            this.tabIncorrectNames.Controls.Add(this.listViewIncorrectNames);
            this.tabIncorrectNames.Location = new System.Drawing.Point(4, 22);
            this.tabIncorrectNames.Name = "tabIncorrectNames";
            this.tabIncorrectNames.Size = new System.Drawing.Size(631, 457);
            this.tabIncorrectNames.TabIndex = 3;
            this.tabIncorrectNames.Text = "Некорректные имена";
            this.tabIncorrectNames.UseVisualStyleBackColor = true;
            // 
            // listViewIncorrectNames
            // 
            this.listViewIncorrectNames.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewIncorrectNames.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnFamilyId,
            this.columnProjectName,
            this.columnServerName,
            this.columnGuid});
            this.listViewIncorrectNames.HideSelection = false;
            this.listViewIncorrectNames.Location = new System.Drawing.Point(3, 3);
            this.listViewIncorrectNames.Name = "listViewIncorrectNames";
            this.listViewIncorrectNames.Size = new System.Drawing.Size(625, 451);
            this.listViewIncorrectNames.TabIndex = 0;
            this.listViewIncorrectNames.UseCompatibleStateImageBehavior = false;
            this.listViewIncorrectNames.View = System.Windows.Forms.View.Details;
            // 
            // columnFamilyId
            // 
            this.columnFamilyId.Text = "ID";
            // 
            // columnProjectName
            // 
            this.columnProjectName.Text = "Имя в проекте";
            // 
            // columnServerName
            // 
            this.columnServerName.Text = "Имя в базе";
            // 
            // columnGuid
            // 
            this.columnGuid.Text = "GUID";
            // 
            // buttonOk
            // 
            this.buttonOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOk.Location = new System.Drawing.Point(547, 501);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(100, 23);
            this.buttonOk.TabIndex = 1;
            this.buttonOk.Text = "ОК";
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
            // 
            // buttonExport
            // 
            this.buttonExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonExport.Location = new System.Drawing.Point(12, 501);
            this.buttonExport.Name = "buttonExport";
            this.buttonExport.Size = new System.Drawing.Size(100, 23);
            this.buttonExport.TabIndex = 2;
            this.buttonExport.Text = "Экспорт";
            this.buttonExport.UseVisualStyleBackColor = true;
            this.buttonExport.Visible = false;
            // 
            // FormCheckFamilies
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(663, 536);
            this.Controls.Add(this.buttonExport);
            this.Controls.Add(this.buttonOk);
            this.Controls.Add(this.tabControl1);
            this.MinimumSize = new System.Drawing.Size(500, 400);
            this.Name = "FormCheckFamilies";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Проверка семейств";
            this.tabControl1.ResumeLayout(false);
            this.tabDuplicates.ResumeLayout(false);
            this.tabVersions.ResumeLayout(false);
            this.tabInvalid.ResumeLayout(false);
            this.tabIncorrectNames.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabVersions;
        private System.Windows.Forms.TabPage tabInvalid;
        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.Button buttonExport;
        private System.Windows.Forms.TabPage tabIncorrectNames;
        private System.Windows.Forms.TabPage tabDuplicates;
        private System.Windows.Forms.TreeView treeViewDuplicates;
        private System.Windows.Forms.Button buttonRefreshAll;
        private System.Windows.Forms.ListView listViewObsoleteFams;
        private System.Windows.Forms.ColumnHeader columnId;
        private System.Windows.Forms.ColumnHeader columnFamName;
        private System.Windows.Forms.ColumnHeader columnProjVersion;
        private System.Windows.Forms.ColumnHeader columnServerVersion;
        private System.Windows.Forms.ColumnHeader columnChanges;
        private System.Windows.Forms.ListView listViewInvalidFamilies;
        private System.Windows.Forms.ColumnHeader columnFamId;
        private System.Windows.Forms.ColumnHeader columnName;
        private System.Windows.Forms.ListView listViewIncorrectNames;
        private System.Windows.Forms.ColumnHeader columnFamilyId;
        private System.Windows.Forms.ColumnHeader columnProjectName;
        private System.Windows.Forms.ColumnHeader columnServerName;
        private System.Windows.Forms.ColumnHeader columnGuid;
        private System.Windows.Forms.ColumnHeader columnOldVersGuid;
    }
}