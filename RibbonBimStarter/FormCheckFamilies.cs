using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Autodesk.Revit.DB;

namespace RibbonBimStarter
{
    public partial class FormCheckFamilies : System.Windows.Forms.Form
    {
        private Dictionary<string, List<Autodesk.Revit.DB.Family>> _duplicates;
        private List<Family> _invalidFamilies;

        public FormCheckFamilies(Dictionary<string, List<Family>> Duplicates, List<string[]> obsoleteFams, List<Family> InvalidFamilies, List<string[]> names)
        {
            InitializeComponent();
            

            _duplicates = Duplicates;
            _invalidFamilies = InvalidFamilies;

            foreach (var kvp in _duplicates)
            {
                int newNodeId = treeViewDuplicates.Nodes.Add(new TreeNode(kvp.Key));

                foreach (var fam in kvp.Value)
                {
                    treeViewDuplicates.Nodes[newNodeId].Nodes.Add(new TreeNode(fam.Name));
                }
            }

            foreach(string[] info in obsoleteFams)
            {
                ListViewItem item = new ListViewItem(info);
                listViewObsoleteFams.Items.Add(item);
            }
            listViewObsoleteFams.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            listViewObsoleteFams.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);

            foreach (Family fam in _invalidFamilies)
            {
                string[] values = new[] { fam.Id.GetValue().ToString(), fam.Name };
                ListViewItem item = new ListViewItem(values);
                listViewInvalidFamilies.Items.Add(item);
            }
            listViewInvalidFamilies.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            listViewInvalidFamilies.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);

            foreach(string[] info in names)
            {
                ListViewItem item = new ListViewItem(info);
                listViewIncorrectNames.Items.Add(item);
            }
            listViewIncorrectNames.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            listViewIncorrectNames.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);


        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonRefreshAll_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Yes;
            this.Close();
        }
    }
}
