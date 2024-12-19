namespace RebarSketch
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.buttonOpenLibraryFolder = new System.Windows.Forms.Button();
            this.richTextBoxFamilies = new System.Windows.Forms.RichTextBox();
            this.buttonHelp = new System.Windows.Forms.Button();
            this.buttonSettings = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.listView1 = new System.Windows.Forms.ListView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.buttonNewForm = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.ParameterName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Value = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FontSize = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.X = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Y = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Angle = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Fit = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Round = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ShowVarLengInterval = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Prefix = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Suffix = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            resources.ApplyResources(this.pictureBox1, "pictureBox1");
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.TabStop = false;
            // 
            // btnSave
            // 
            resources.ApplyResources(this.btnSave, "btnSave");
            this.btnSave.Name = "btnSave";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // buttonOpenLibraryFolder
            // 
            resources.ApplyResources(this.buttonOpenLibraryFolder, "buttonOpenLibraryFolder");
            this.buttonOpenLibraryFolder.Name = "buttonOpenLibraryFolder";
            this.buttonOpenLibraryFolder.UseVisualStyleBackColor = true;
            this.buttonOpenLibraryFolder.Click += new System.EventHandler(this.buttonOpenLibraryFolder_Click);
            // 
            // richTextBoxFamilies
            // 
            resources.ApplyResources(this.richTextBoxFamilies, "richTextBoxFamilies");
            this.richTextBoxFamilies.DetectUrls = false;
            this.richTextBoxFamilies.Name = "richTextBoxFamilies";
            // 
            // buttonHelp
            // 
            resources.ApplyResources(this.buttonHelp, "buttonHelp");
            this.buttonHelp.Name = "buttonHelp";
            this.buttonHelp.UseVisualStyleBackColor = true;
            this.buttonHelp.Click += new System.EventHandler(this.button1_Click);
            // 
            // buttonSettings
            // 
            resources.ApplyResources(this.buttonSettings, "buttonSettings");
            this.buttonSettings.Name = "buttonSettings";
            this.buttonSettings.UseVisualStyleBackColor = true;
            this.buttonSettings.Click += new System.EventHandler(this.buttonSettings_Click);
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
            // buttonCancel
            // 
            resources.ApplyResources(this.buttonCancel, "buttonCancel");
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // listView1
            // 
            resources.ApplyResources(this.listView1, "listView1");
            this.listView1.HideSelection = false;
            this.listView1.LargeImageList = this.imageList1;
            this.listView1.MultiSelect = false;
            this.listView1.Name = "listView1";
            this.listView1.ShowGroups = false;
            this.listView1.ShowItemToolTips = true;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            resources.ApplyResources(this.imageList1, "imageList1");
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // buttonNewForm
            // 
            resources.ApplyResources(this.buttonNewForm, "buttonNewForm");
            this.buttonNewForm.Name = "buttonNewForm";
            this.buttonNewForm.UseVisualStyleBackColor = true;
            this.buttonNewForm.Click += new System.EventHandler(this.buttonNewForm_Click);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToResizeRows = false;
            resources.ApplyResources(this.dataGridView1, "dataGridView1");
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ParameterName,
            this.Value,
            this.FontSize,
            this.X,
            this.Y,
            this.Angle,
            this.Fit,
            this.Round,
            this.ShowVarLengInterval,
            this.Prefix,
            this.Suffix});
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellEndEdit);
            this.dataGridView1.UserDeletedRow += new System.Windows.Forms.DataGridViewRowEventHandler(this.dataGridView1_UserDeletedRow);
            // 
            // ParameterName
            // 
            this.ParameterName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ParameterName.FillWeight = 80.64876F;
            resources.ApplyResources(this.ParameterName, "ParameterName");
            this.ParameterName.Name = "ParameterName";
            // 
            // Value
            // 
            this.Value.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Value.FillWeight = 37.63608F;
            resources.ApplyResources(this.Value, "Value");
            this.Value.Name = "Value";
            // 
            // FontSize
            // 
            this.FontSize.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle1.Format = "N2";
            dataGridViewCellStyle1.NullValue = null;
            this.FontSize.DefaultCellStyle = dataGridViewCellStyle1;
            this.FontSize.FillWeight = 26.88291F;
            resources.ApplyResources(this.FontSize, "FontSize");
            this.FontSize.Name = "FontSize";
            // 
            // X
            // 
            this.X.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle2.Format = "N2";
            dataGridViewCellStyle2.NullValue = null;
            this.X.DefaultCellStyle = dataGridViewCellStyle2;
            this.X.FillWeight = 30.07484F;
            resources.ApplyResources(this.X, "X");
            this.X.Name = "X";
            // 
            // Y
            // 
            this.Y.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle3.Format = "N2";
            dataGridViewCellStyle3.NullValue = null;
            this.Y.DefaultCellStyle = dataGridViewCellStyle3;
            this.Y.FillWeight = 30.07484F;
            resources.ApplyResources(this.Y, "Y");
            this.Y.Name = "Y";
            // 
            // Angle
            // 
            this.Angle.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle4.Format = "N2";
            dataGridViewCellStyle4.NullValue = null;
            this.Angle.DefaultCellStyle = dataGridViewCellStyle4;
            this.Angle.FillWeight = 30.07484F;
            resources.ApplyResources(this.Angle, "Angle");
            this.Angle.Name = "Angle";
            // 
            // Fit
            // 
            this.Fit.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Fit.FillWeight = 25.84242F;
            resources.ApplyResources(this.Fit, "Fit");
            this.Fit.Name = "Fit";
            this.Fit.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Fit.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // Round
            // 
            this.Round.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle5.Format = "N2";
            dataGridViewCellStyle5.NullValue = null;
            this.Round.DefaultCellStyle = dataGridViewCellStyle5;
            this.Round.FillWeight = 25F;
            resources.ApplyResources(this.Round, "Round");
            this.Round.Name = "Round";
            // 
            // ShowVarLengInterval
            // 
            this.ShowVarLengInterval.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ShowVarLengInterval.FillWeight = 30.3371F;
            resources.ApplyResources(this.ShowVarLengInterval, "ShowVarLengInterval");
            this.ShowVarLengInterval.Name = "ShowVarLengInterval";
            // 
            // Prefix
            // 
            this.Prefix.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Prefix.FillWeight = 25F;
            resources.ApplyResources(this.Prefix, "Prefix");
            this.Prefix.Name = "Prefix";
            // 
            // Suffix
            // 
            this.Suffix.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Suffix.FillWeight = 25F;
            resources.ApplyResources(this.Suffix, "Suffix");
            this.Suffix.Name = "Suffix";
            // 
            // Form1
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.Controls.Add(this.buttonNewForm);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.richTextBoxFamilies);
            this.Controls.Add(this.buttonOpenLibraryFolder);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.buttonSettings);
            this.Controls.Add(this.buttonHelp);
            this.Name = "Form1";
            this.ShowIcon = false;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button buttonOpenLibraryFolder;
        private System.Windows.Forms.RichTextBox richTextBoxFamilies;
        private System.Windows.Forms.Button buttonHelp;
        private System.Windows.Forms.Button buttonSettings;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.Button buttonNewForm;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.DataGridViewTextBoxColumn ParameterName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Value;
        private System.Windows.Forms.DataGridViewTextBoxColumn FontSize;
        private System.Windows.Forms.DataGridViewTextBoxColumn X;
        private System.Windows.Forms.DataGridViewTextBoxColumn Y;
        private System.Windows.Forms.DataGridViewTextBoxColumn Angle;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Fit;
        private System.Windows.Forms.DataGridViewTextBoxColumn Round;
        private System.Windows.Forms.DataGridViewCheckBoxColumn ShowVarLengInterval;
        private System.Windows.Forms.DataGridViewTextBoxColumn Prefix;
        private System.Windows.Forms.DataGridViewTextBoxColumn Suffix;
    }

}