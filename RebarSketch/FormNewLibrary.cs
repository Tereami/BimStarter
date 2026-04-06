using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RebarSketch
{
    public partial class FormNewLibrary : Form
    {
        public string SelectedPath;
        private string _sourceFolder;
        public FormNewLibrary(string sourceFolder)
        {
            InitializeComponent();
            _sourceFolder = sourceFolder;
        }

        private void buttonBrowse_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "Select the target folder";
            dialog.ShowNewFolderButton = true;

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                textBoxPath.Text = dialog.SelectedPath;
            }
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            
            if(!Directory.Exists(textBoxPath.Text))
            { 
                MessageBox.Show("Incorrect path!");
                return;
            }

            SelectedPath = textBoxPath.Text;

            try
            {
                string rebarSketchSourcePath = Path.Combine(_sourceFolder, "RebarSketch");
                string rebarSketchTargetPath = Path.Combine(SelectedPath, "RebarSketch");
                Debug.WriteLine($"Copy {rebarSketchSourcePath} to {rebarSketchTargetPath}");
                CopyDirectory(rebarSketchSourcePath, rebarSketchTargetPath);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        public static void CopyDirectory(string sourceDir, string destDir, bool recursive = true)
        {
            var dir = new DirectoryInfo(sourceDir);
            if (!dir.Exists) throw new DirectoryNotFoundException("Source not found");

            Directory.CreateDirectory(destDir);
            foreach (FileInfo file in dir.GetFiles())
            {
                file.CopyTo(Path.Combine(destDir, file.Name), true);
            }

            if (recursive)
            {
                foreach (DirectoryInfo subDir in dir.GetDirectories())
                {
                    CopyDirectory(subDir.FullName, Path.Combine(destDir, subDir.Name), true);
                }
            }
        }
    }
}
