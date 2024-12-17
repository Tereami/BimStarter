//Данный код опубликован под лицензией Creative Commons Attribution-NonCommercial-ShareAlike.
//Разрешено редактировать, изменять и брать данный код за основу для производных в некоммерческих целях,
//при условии указания авторства и если производные лицензируются на тех же условиях.
//Программа поставляется "как есть". Автор не несет ответственности за возможные последствия её использования.
//Зуев Александр, 2019, все права защищены.
//This code is listed under the Creative Commons Attribution-NonCommercial-ShareAlike license.
//You may redistribute, remix, tweak, and build upon this work non-commercially, 
//as long as you credit the author by linking back and license your new creations under the same terms.
//This code is provided 'as is'. Author disclaims any implied warranty. 
//Zuev Aleksandr, 2019, all rigths reserved.


namespace GroupedAssembly
{
    partial class FormEnterName
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormEnterName));
            this.label1 = new System.Windows.Forms.Label();
            this.txtBoxName = new System.Windows.Forms.TextBox();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.checkBoxGroupElements = new System.Windows.Forms.CheckBox();
            this.checkBoxUntouchBeamsEnds = new System.Windows.Forms.CheckBox();
            this.checkBoxUntouchBeamsPlane = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // txtBoxName
            // 
            resources.ApplyResources(this.txtBoxName, "txtBoxName");
            this.txtBoxName.Name = "txtBoxName";
            // 
            // btnOk
            // 
            resources.ApplyResources(this.btnOk, "btnOk");
            this.btnOk.Name = "btnOk";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // checkBoxGroupElements
            // 
            resources.ApplyResources(this.checkBoxGroupElements, "checkBoxGroupElements");
            this.checkBoxGroupElements.Name = "checkBoxGroupElements";
            this.checkBoxGroupElements.UseVisualStyleBackColor = true;
            // 
            // checkBoxUntouchBeamsEnds
            // 
            resources.ApplyResources(this.checkBoxUntouchBeamsEnds, "checkBoxUntouchBeamsEnds");
            this.checkBoxUntouchBeamsEnds.Name = "checkBoxUntouchBeamsEnds";
            this.checkBoxUntouchBeamsEnds.UseVisualStyleBackColor = true;
            // 
            // checkBoxUntouchBeamsPlane
            // 
            resources.ApplyResources(this.checkBoxUntouchBeamsPlane, "checkBoxUntouchBeamsPlane");
            this.checkBoxUntouchBeamsPlane.Name = "checkBoxUntouchBeamsPlane";
            this.checkBoxUntouchBeamsPlane.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // FormEnterName
            // 
            this.AcceptButton = this.btnOk;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.Controls.Add(this.label2);
            this.Controls.Add(this.checkBoxUntouchBeamsPlane);
            this.Controls.Add(this.checkBoxUntouchBeamsEnds);
            this.Controls.Add(this.checkBoxGroupElements);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.txtBoxName);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FormEnterName";
            this.Load += new System.EventHandler(this.FormEnterName_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtBoxName;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.CheckBox checkBoxGroupElements;
        private System.Windows.Forms.CheckBox checkBoxUntouchBeamsEnds;
        private System.Windows.Forms.CheckBox checkBoxUntouchBeamsPlane;
        private System.Windows.Forms.Label label2;
    }
}