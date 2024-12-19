#region License
/*Данный код опубликован под лицензией Creative Commons Attribution-ShareAlike.
Разрешено использовать, распространять, изменять и брать данный код за основу для производных в коммерческих и
некоммерческих целях, при условии указания авторства и если производные лицензируются на тех же условиях.
Код поставляется "как есть". Автор не несет ответственности за возможные последствия использования.
Зуев Александр, 2020, все права защищены.
This code is listed under the Creative Commons Attribution-ShareAlike license.
You may use, redistribute, remix, tweak, and build upon this work non-commercially and commercially,
as long as you credit the author by linking back and license your new creations under the same terms.
This code is provided 'as is'. Author disclaims any implied warranty.
Zuev Aleksandr, 2020, all rigths reserved.*/
#endregion

using System.Collections.Generic;
using System.Windows.Forms;

namespace RevitGraphicsOverride
{
    public partial class Form2 : Form
    {
        public long idToSelect;
        public long idViewToOpen;

        public Form2(Dictionary<string, List<OverridenResult>> resultsOnSheets)
        {
            InitializeComponent();
            AddTree();
            InitializeData(resultsOnSheets);
            FillTree();
            this.treeListView.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.treeListView_CellDoubleClick); //.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
        }

        private void treeListView_CellDoubleClick(object sender, MouseEventArgs e)
        {
            BrightIdeasSoftware.OLVListItem curRow = this.treeListView.SelectedItem;
            var idElemCell = curRow.GetSubItem(1);
            idToSelect = long.Parse(idElemCell.Text);

            var idViewCell = curRow.GetSubItem(3);
            idViewToOpen = long.Parse(idViewCell.Text);

            this.DialogResult = DialogResult.Yes;
            this.Close();
        }




        // embedded class
        class Node
        {
            public string Name { get; private set; }
            public string Column1 { get; private set; }
            public string Column2 { get; private set; }
            public string Column3 { get; private set; }

            public List<Node> Children { get; private set; }
            public Node(string name, string col1, string col2, string col3)
            {
                this.Name = name;
                this.Column1 = col1;
                this.Column2 = col2;
                this.Column3 = col3;
                this.Children = new List<Node>();
            }
        }

        // private fields
        private List<Node> data;
        private BrightIdeasSoftware.TreeListView treeListView;

        private void AddTree()
        {
            treeListView = new BrightIdeasSoftware.TreeListView();
            treeListView.Dock = DockStyle.Fill;
            this.Controls.Add(treeListView);
        }

        private void FillTree()
        {
            // set the delegate that the tree uses to know if a node is expandable
            this.treeListView.CanExpandGetter = x => (x as Node).Children.Count > 0;
            // set the delegate that the tree uses to know the children of a node
            this.treeListView.ChildrenGetter = x => (x as Node).Children;

            // create the tree columns and set the delegates to print the desired object proerty
            var nameCol = new BrightIdeasSoftware.OLVColumn("Имя вида", "Name");
            nameCol.AspectGetter = x => (x as Node).Name;
            nameCol.Width = 300;

            var col1 = new BrightIdeasSoftware.OLVColumn("ID элемента", "Column1");
            col1.AspectGetter = x => (x as Node).Column1;

            var col2 = new BrightIdeasSoftware.OLVColumn("Переопределение", "Column2");
            col2.AspectGetter = x => (x as Node).Column2;
            col2.Width = 500;

            var col3 = new BrightIdeasSoftware.OLVColumn("ID вида", "Column3");
            col3.AspectGetter = x => (x as Node).Column3;

            // add the columns to the tree
            this.treeListView.Columns.Add(nameCol);
            this.treeListView.Columns.Add(col1);
            this.treeListView.Columns.Add(col2);
            this.treeListView.Columns.Add(col3);

            // set the tree roots
            this.treeListView.Roots = data;
            this.treeListView.ExpandAll();
        }

        private void InitializeData(Dictionary<string, List<OverridenResult>> results)
        {
            data = new List<Node>();
            foreach (var kvp in results)
            {
                Node parent = new Node(kvp.Key, "-", "-", "-");
                foreach(OverridenResult res in kvp.Value)
                {
                    string colElemName = res.Elem.Name;
                    string colElemId = res.Elem.Id.ToString();
                    string colDescription = res.description;
                    string colViewId = res.ParentView.Id.ToString();
                    parent.Children.Add(new Node(colElemName, colElemId, colDescription, colViewId));
                }
                
                data.Add(parent);
            }
        }

    }
}
