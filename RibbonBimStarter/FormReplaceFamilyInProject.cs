using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RibbonBimStarter
{
    public partial class FormReplaceFamilyInProject : Form
    {
        public FormReplaceFamilyInProject(string famname, int versionInProject, List<FamilyVersion> versions)
        {
            InitializeComponent();

            labelFamilyName.Text = $"Ранее загруженное в проект семейство {famname} устарело.";
            labelOldVersion.Text = $"Версия в проекте: {versionInProject}";
            int maxVersion = versions.Max(i => i.version);
            labelNewVersion.Text = $"Доступная версия: {maxVersion}";

            foreach(FamilyVersion fv in versions)
            {
                richTextBoxChanges.Text += fv.version.ToString() + ": " + fv.changes + ";" + System.Environment.NewLine;
            }
        }

        private void buttonReplace_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Yes;
            this.Close();
        }

        private void buttonUseOld_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.No;
            this.Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
