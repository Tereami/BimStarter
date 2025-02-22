using System.Collections.Generic;
using System.Windows.Forms;

namespace LinkWriter
{
    public partial class FormSelectParameterValues : Form
    {
        public Dictionary<string, List<(string, string)>> ValuesSheets { get; set; }
        public Dictionary<string, List<(string, string)>> ValuesTitleblocks { get; set; }
        public Dictionary<string, List<(string, string)>> ValuesTitleblockType { get; set; }

        public List<(string, string)> ValuesProjectInfo { get; set; }

        List<string> LinkNames { get; set; }

        public FormSelectParameterValues(List<string> linkNames, WriteLinkSettings sets)
        {
            InitializeComponent();

            LinkNames = linkNames;

            BuildDatagridByLinks(dataGridViewSheet, sets.SheetParams);
            BuildDatagridByLinks(dataGridViewTitleblock, sets.TitleblockParams);
            BuildDatagridByLinks(dataGridViewTitleblockType, sets.TypeParams);
            BuildDatagrid(dataGridViewProjectInfo, sets.ProjectParams);
        }

        public void BuildDatagridByLinks(DataGridView dgv, List<MyParameterValue> values)
        {
            string prefix = dgv.Name;
            dgv.Columns.Add(new DataGridViewCheckBoxColumn() { Name = $"{prefix}_Enable", HeaderText = "On", FillWeight = 15 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn() { Name = $"{prefix}_ParamName", HeaderText = "Parameter", FillWeight = 40 });

            for (int i = 0; i < LinkNames.Count; i++)
            {
                dgv.Columns.Add(new DataGridViewTextBoxColumn() { Name = $"{prefix}_Link{i}", HeaderText = LinkNames[i], AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            }

            dgv.Rows.Clear();

            foreach (MyParameterValue param in values)
            {
                object[] row = new object[LinkNames.Count + 2];
                row[0] = param.IsEnabled;
                row[1] = param.ParameterName;
                for (int i = 0; i < LinkNames.Count; i++)
                {
                    row[i + 2] = param.GetValueAsString();
                }
                dgv.Rows.Add(row);
            }
        }

        public void BuildDatagrid(DataGridView dgv, List<MyParameterValue> values)
        {
            foreach (MyParameterValue param in values)
            {
                dgv.Rows.Add(param.IsEnabled, param.ParameterName, param.GetValueAsString());
            }
        }

        private void buttonCancel_Click(object sender, System.EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void buttonNext_Click(object sender, System.EventArgs e)
        {
            ValuesSheets = GetDataGridValusByLinks(dataGridViewSheet);
            ValuesTitleblocks = GetDataGridValusByLinks(dataGridViewTitleblock);
            ValuesTitleblockType = GetDataGridValusByLinks(dataGridViewTitleblockType);
            ValuesProjectInfo = GetDataGridValues(dataGridViewProjectInfo);

            this.DialogResult = DialogResult.OK;
            this.Close();
        }


        private Dictionary<string, List<(string, string)>> GetDataGridValusByLinks(DataGridView dgv)
        {
            Dictionary<string, List<(string, string)>> Values = new Dictionary<string, List<(string, string)>>();

            foreach (DataGridViewRow row in dgv.Rows)
            {
                if (row.IsNewRow) continue;
                bool enabled = (bool)row.Cells[0].Value;
                if (!enabled) continue;

                string paramName = (string)row.Cells[1].Value;

                for (int i = 2; i < row.Cells.Count; i++)
                {
                    string value = (string)row.Cells[i].Value;
                    string linkName = LinkNames[i - 2];

                    if (Values.ContainsKey(linkName))
                    {
                        Values[linkName].Add((paramName, value));
                    }
                    else
                    {
                        Values.Add(linkName, new List<(string, string)> { (paramName, value) });
                    }
                }
            }

            return Values;
        }

        private List<(string, string)> GetDataGridValues(DataGridView dgv)
        {
            List<(string, string)> values = new List<(string, string)>();

            foreach (DataGridViewRow row in dgv.Rows)
            {
                if (row.IsNewRow) continue;
                bool enabled = (bool)row.Cells[0].Value;
                if (!enabled) continue;

                string paramName = (string)row.Cells[1].Value;

                string paramValue = (string)row.Cells[2].Value;

                values.Add((paramName, paramValue));
            }

            return values;
        }
    }
}
