namespace RebarParametrisation
{
    partial class FormSelectParams
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
            this.chkboxHostid = new System.Windows.Forms.CheckBox();
            this.chkboxUniqId = new System.Windows.Forms.CheckBox();
            this.chkboxHostMark = new System.Windows.Forms.CheckBox();
            this.buttonOk = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.chkboxUseThickness = new System.Windows.Forms.CheckBox();
            this.chkboxRebarWeight = new System.Windows.Forms.CheckBox();
            this.chkboxRebarLength = new System.Windows.Forms.CheckBox();
            this.chkboxRebarDiameter = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cmbboxConcreteClass = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.radioAllKrLinks = new System.Windows.Forms.RadioButton();
            this.radioOnlyLibs = new System.Windows.Forms.RadioButton();
            this.radioNoLinks = new System.Windows.Forms.RadioButton();
            this.textBoxHostId = new System.Windows.Forms.TextBox();
            this.textBoxHostUniqId = new System.Windows.Forms.TextBox();
            this.textBoxWeight = new System.Windows.Forms.TextBox();
            this.textBoxLength = new System.Windows.Forms.TextBox();
            this.textBoxDiameter = new System.Windows.Forms.TextBox();
            this.textBoxHostMark = new System.Windows.Forms.TextBox();
            this.textBoxHostThickness = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // chkboxHostid
            // 
            this.chkboxHostid.AutoSize = true;
            this.chkboxHostid.Checked = true;
            this.chkboxHostid.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkboxHostid.Location = new System.Drawing.Point(6, 22);
            this.chkboxHostid.Margin = new System.Windows.Forms.Padding(3, 6, 3, 1);
            this.chkboxHostid.Name = "chkboxHostid";
            this.chkboxHostid.Size = new System.Drawing.Size(104, 17);
            this.chkboxHostid.TabIndex = 1;
            this.chkboxHostid.Text = "ID конструкции";
            this.chkboxHostid.UseVisualStyleBackColor = true;
            this.chkboxHostid.CheckedChanged += new System.EventHandler(this.chkboxHostid_CheckedChanged);
            // 
            // chkboxUniqId
            // 
            this.chkboxUniqId.AutoSize = true;
            this.chkboxUniqId.Checked = true;
            this.chkboxUniqId.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkboxUniqId.Location = new System.Drawing.Point(6, 72);
            this.chkboxUniqId.Margin = new System.Windows.Forms.Padding(3, 6, 3, 1);
            this.chkboxUniqId.Name = "chkboxUniqId";
            this.chkboxUniqId.Size = new System.Drawing.Size(136, 17);
            this.chkboxUniqId.TabIndex = 1;
            this.chkboxUniqId.Text = "UniqueId конструкции";
            this.chkboxUniqId.UseVisualStyleBackColor = true;
            this.chkboxUniqId.CheckedChanged += new System.EventHandler(this.chkboxUniqId_CheckedChanged);
            // 
            // chkboxHostMark
            // 
            this.chkboxHostMark.AutoSize = true;
            this.chkboxHostMark.Checked = true;
            this.chkboxHostMark.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkboxHostMark.Location = new System.Drawing.Point(5, 272);
            this.chkboxHostMark.Margin = new System.Windows.Forms.Padding(3, 6, 3, 1);
            this.chkboxHostMark.Name = "chkboxHostMark";
            this.chkboxHostMark.Size = new System.Drawing.Size(99, 17);
            this.chkboxHostMark.TabIndex = 1;
            this.chkboxHostMark.Text = "Метка основы";
            this.chkboxHostMark.UseVisualStyleBackColor = true;
            this.chkboxHostMark.CheckedChanged += new System.EventHandler(this.chkboxHostMark_CheckedChanged);
            // 
            // buttonOk
            // 
            this.buttonOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOk.Location = new System.Drawing.Point(102, 586);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(75, 23);
            this.buttonOk.TabIndex = 2;
            this.buttonOk.Text = "OK";
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(183, 586);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 2;
            this.buttonCancel.Text = "Отмена";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // chkboxUseThickness
            // 
            this.chkboxUseThickness.AutoSize = true;
            this.chkboxUseThickness.Checked = true;
            this.chkboxUseThickness.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkboxUseThickness.Location = new System.Drawing.Point(5, 322);
            this.chkboxUseThickness.Margin = new System.Windows.Forms.Padding(3, 6, 3, 1);
            this.chkboxUseThickness.Name = "chkboxUseThickness";
            this.chkboxUseThickness.Size = new System.Drawing.Size(113, 17);
            this.chkboxUseThickness.TabIndex = 1;
            this.chkboxUseThickness.Text = "Толщина основы";
            this.chkboxUseThickness.UseVisualStyleBackColor = true;
            this.chkboxUseThickness.CheckedChanged += new System.EventHandler(this.chkboxUseThickness_CheckedChanged);
            // 
            // chkboxRebarWeight
            // 
            this.chkboxRebarWeight.AutoSize = true;
            this.chkboxRebarWeight.Checked = true;
            this.chkboxRebarWeight.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkboxRebarWeight.Location = new System.Drawing.Point(6, 122);
            this.chkboxRebarWeight.Margin = new System.Windows.Forms.Padding(3, 6, 3, 1);
            this.chkboxRebarWeight.Name = "chkboxRebarWeight";
            this.chkboxRebarWeight.Size = new System.Drawing.Size(105, 17);
            this.chkboxRebarWeight.TabIndex = 1;
            this.chkboxRebarWeight.Text = "Масса стержня";
            this.chkboxRebarWeight.UseVisualStyleBackColor = true;
            this.chkboxRebarWeight.CheckedChanged += new System.EventHandler(this.chkboxRebarWeight_CheckedChanged);
            // 
            // chkboxRebarLength
            // 
            this.chkboxRebarLength.AutoSize = true;
            this.chkboxRebarLength.Checked = true;
            this.chkboxRebarLength.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkboxRebarLength.Location = new System.Drawing.Point(6, 172);
            this.chkboxRebarLength.Margin = new System.Windows.Forms.Padding(3, 6, 3, 1);
            this.chkboxRebarLength.Name = "chkboxRebarLength";
            this.chkboxRebarLength.Size = new System.Drawing.Size(105, 17);
            this.chkboxRebarLength.TabIndex = 1;
            this.chkboxRebarLength.Text = "Длина стержня";
            this.chkboxRebarLength.UseVisualStyleBackColor = true;
            this.chkboxRebarLength.CheckedChanged += new System.EventHandler(this.chkboxRebarLength_CheckedChanged);
            // 
            // chkboxRebarDiameter
            // 
            this.chkboxRebarDiameter.AutoSize = true;
            this.chkboxRebarDiameter.Checked = true;
            this.chkboxRebarDiameter.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkboxRebarDiameter.Location = new System.Drawing.Point(5, 222);
            this.chkboxRebarDiameter.Margin = new System.Windows.Forms.Padding(3, 6, 3, 1);
            this.chkboxRebarDiameter.Name = "chkboxRebarDiameter";
            this.chkboxRebarDiameter.Size = new System.Drawing.Size(118, 17);
            this.chkboxRebarDiameter.TabIndex = 1;
            this.chkboxRebarDiameter.Text = "Диаметр стержня";
            this.chkboxRebarDiameter.UseVisualStyleBackColor = true;
            this.chkboxRebarDiameter.CheckedChanged += new System.EventHandler(this.chkboxRebarDiameter_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textBoxHostThickness);
            this.groupBox1.Controls.Add(this.textBoxHostMark);
            this.groupBox1.Controls.Add(this.textBoxDiameter);
            this.groupBox1.Controls.Add(this.textBoxLength);
            this.groupBox1.Controls.Add(this.textBoxWeight);
            this.groupBox1.Controls.Add(this.textBoxHostUniqId);
            this.groupBox1.Controls.Add(this.textBoxHostId);
            this.groupBox1.Controls.Add(this.chkboxHostid);
            this.groupBox1.Controls.Add(this.chkboxUniqId);
            this.groupBox1.Controls.Add(this.chkboxRebarWeight);
            this.groupBox1.Controls.Add(this.chkboxUseThickness);
            this.groupBox1.Controls.Add(this.chkboxRebarLength);
            this.groupBox1.Controls.Add(this.chkboxHostMark);
            this.groupBox1.Controls.Add(this.chkboxRebarDiameter);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(248, 375);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Параметры для заполнения:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cmbboxConcreteClass);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Location = new System.Drawing.Point(12, 393);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(248, 84);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Бетон";
            // 
            // cmbboxConcreteClass
            // 
            this.cmbboxConcreteClass.FormattingEnabled = true;
            this.cmbboxConcreteClass.Items.AddRange(new object[] {
            "10",
            "15",
            "20",
            "25",
            "30",
            "35",
            "40"});
            this.cmbboxConcreteClass.Location = new System.Drawing.Point(6, 52);
            this.cmbboxConcreteClass.Name = "cmbboxConcreteClass";
            this.cmbboxConcreteClass.Size = new System.Drawing.Size(236, 21);
            this.cmbboxConcreteClass.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(6, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(182, 33);
            this.label1.TabIndex = 5;
            this.label1.Text = "Если не заполнен параметр Арм.КлассБетона, использовать:";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.radioAllKrLinks);
            this.groupBox3.Controls.Add(this.radioOnlyLibs);
            this.groupBox3.Controls.Add(this.radioNoLinks);
            this.groupBox3.Location = new System.Drawing.Point(12, 483);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(248, 90);
            this.groupBox3.TabIndex = 5;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Связи";
            // 
            // radioAllKrLinks
            // 
            this.radioAllKrLinks.AutoSize = true;
            this.radioAllKrLinks.Location = new System.Drawing.Point(10, 65);
            this.radioAllKrLinks.Name = "radioAllKrLinks";
            this.radioAllKrLinks.Size = new System.Drawing.Size(94, 17);
            this.radioAllKrLinks.TabIndex = 6;
            this.radioAllKrLinks.TabStop = true;
            this.radioAllKrLinks.Text = "Все связи КР";
            this.radioAllKrLinks.UseVisualStyleBackColor = true;
            // 
            // radioOnlyLibs
            // 
            this.radioOnlyLibs.AutoSize = true;
            this.radioOnlyLibs.Location = new System.Drawing.Point(10, 42);
            this.radioOnlyLibs.Name = "radioOnlyLibs";
            this.radioOnlyLibs.Size = new System.Drawing.Size(108, 17);
            this.radioOnlyLibs.TabIndex = 6;
            this.radioOnlyLibs.TabStop = true;
            this.radioOnlyLibs.Text = "Только связи lib";
            this.radioOnlyLibs.UseVisualStyleBackColor = true;
            // 
            // radioNoLinks
            // 
            this.radioNoLinks.AutoSize = true;
            this.radioNoLinks.Location = new System.Drawing.Point(10, 19);
            this.radioNoLinks.Name = "radioNoLinks";
            this.radioNoLinks.Size = new System.Drawing.Size(147, 17);
            this.radioNoLinks.TabIndex = 6;
            this.radioNoLinks.TabStop = true;
            this.radioNoLinks.Text = "Не обрабатывать связи";
            this.radioNoLinks.UseVisualStyleBackColor = true;
            // 
            // textBoxHostId
            // 
            this.textBoxHostId.Location = new System.Drawing.Point(5, 43);
            this.textBoxHostId.Name = "textBoxHostId";
            this.textBoxHostId.Size = new System.Drawing.Size(236, 20);
            this.textBoxHostId.TabIndex = 2;
            this.textBoxHostId.Text = "INGD_HostId";
            // 
            // textBoxHostUniqId
            // 
            this.textBoxHostUniqId.Location = new System.Drawing.Point(5, 93);
            this.textBoxHostUniqId.Name = "textBoxHostUniqId";
            this.textBoxHostUniqId.Size = new System.Drawing.Size(236, 20);
            this.textBoxHostUniqId.TabIndex = 2;
            this.textBoxHostUniqId.Text = "INGD_UniqueHostId";
            // 
            // textBoxWeight
            // 
            this.textBoxWeight.Location = new System.Drawing.Point(5, 143);
            this.textBoxWeight.Name = "textBoxWeight";
            this.textBoxWeight.Size = new System.Drawing.Size(236, 20);
            this.textBoxWeight.TabIndex = 2;
            this.textBoxWeight.Text = "INGD_Weight";
            // 
            // textBoxLength
            // 
            this.textBoxLength.Location = new System.Drawing.Point(5, 193);
            this.textBoxLength.Name = "textBoxLength";
            this.textBoxLength.Size = new System.Drawing.Size(236, 20);
            this.textBoxLength.TabIndex = 2;
            this.textBoxLength.Text = "INGD_Length";
            // 
            // textBoxDiameter
            // 
            this.textBoxDiameter.Location = new System.Drawing.Point(5, 243);
            this.textBoxDiameter.Name = "textBoxDiameter";
            this.textBoxDiameter.Size = new System.Drawing.Size(236, 20);
            this.textBoxDiameter.TabIndex = 2;
            this.textBoxDiameter.Text = "INGD_Diameter";
            // 
            // textBoxHostMark
            // 
            this.textBoxHostMark.Location = new System.Drawing.Point(5, 293);
            this.textBoxHostMark.Name = "textBoxHostMark";
            this.textBoxHostMark.Size = new System.Drawing.Size(236, 20);
            this.textBoxHostMark.TabIndex = 2;
            this.textBoxHostMark.Text = "INGD_Diameter";
            // 
            // textBoxHostThickness
            // 
            this.textBoxHostThickness.Location = new System.Drawing.Point(5, 343);
            this.textBoxHostThickness.Name = "textBoxHostThickness";
            this.textBoxHostThickness.Size = new System.Drawing.Size(236, 20);
            this.textBoxHostThickness.TabIndex = 2;
            this.textBoxHostThickness.Text = "INGD_HostThickness";
            // 
            // FormSelectParams
            // 
            this.AcceptButton = this.buttonOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(270, 621);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOk);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FormSelectParams";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Настройки";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.CheckBox chkboxHostid;
        private System.Windows.Forms.CheckBox chkboxUniqId;
        private System.Windows.Forms.CheckBox chkboxHostMark;
        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.CheckBox chkboxUseThickness;
        private System.Windows.Forms.CheckBox chkboxRebarWeight;
        private System.Windows.Forms.CheckBox chkboxRebarLength;
        private System.Windows.Forms.CheckBox chkboxRebarDiameter;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbboxConcreteClass;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RadioButton radioAllKrLinks;
        private System.Windows.Forms.RadioButton radioOnlyLibs;
        private System.Windows.Forms.RadioButton radioNoLinks;
        private System.Windows.Forms.TextBox textBoxHostThickness;
        private System.Windows.Forms.TextBox textBoxHostMark;
        private System.Windows.Forms.TextBox textBoxDiameter;
        private System.Windows.Forms.TextBox textBoxLength;
        private System.Windows.Forms.TextBox textBoxWeight;
        private System.Windows.Forms.TextBox textBoxHostUniqId;
        private System.Windows.Forms.TextBox textBoxHostId;
    }
}