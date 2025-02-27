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
            dgv.Columns.Add(new DataGridViewTextBoxColumn() { Name = $"{prefix}_ParamName", HeaderText = "Parameter", FillWeight = 40, ReadOnly = true });

            for (int i = 0; i < LinkNames.Count; i++)
            {
                dgv.Columns.Add(new DataGridViewTextBoxColumn() { Name = $"{prefix}_Link{i}", HeaderText = LinkNames[i], AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            }

            dgv.Rows.Clear();

            foreach (MyParameterValue param in values)
            {
                object[] row = new object[LinkNames.Count + 1];
                row[0] = param.ParameterName;
                for (int i = 0; i < LinkNames.Count; i++)
                {
                    row[i + 1] = param.GetValueAsString();
                }
                dgv.Rows.Add(row);
            }
        }

        public void BuildDatagrid(DataGridView dgv, List<MyParameterValue> values)
        {
            foreach (MyParameterValue param in values)
            {
                dgv.Rows.Add(param.ParameterName, param.GetValueAsString());
            }
        }

        private void buttonCancel_Click(object sender, System.EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void buttonNext_Click(object sender, System.EventArgs e)
        {
            ValuesSheets = GetDataGridValuesByLinks(dataGridViewSheet);
            ValuesTitleblocks = GetDataGridValuesByLinks(dataGridViewTitleblock);
            ValuesTitleblockType = GetDataGridValuesByLinks(dataGridViewTitleblockType);
            ValuesProjectInfo = GetDataGridValues(dataGridViewProjectInfo);

            this.DialogResult = DialogResult.OK;
            this.Close();
        }


        private Dictionary<string, List<(string, string)>> GetDataGridValuesByLinks(DataGridView dgv)
        {
            Dictionary<string, List<(string, string)>> Values = new Dictionary<string, List<(string, string)>>();

            foreach (DataGridViewRow row in dgv.Rows)
            {
                if (row.IsNewRow) continue;

                string paramName = (string)row.Cells[0].Value;

                for (int i = 1; i < row.Cells.Count; i++)
                {
                    string value = (string)row.Cells[i].Value;
                    string linkName = LinkNames[i - 1];

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

                string paramName = (string)row.Cells[0].Value;

                string paramValue = (string)row.Cells[1].Value;

                values.Add((paramName, paramValue));
            }

            return values;
        }

        private void buttonReset_Click(object sender, System.EventArgs e)
        {
            List<DataGridView> dgvs = new List<DataGridView>
            {
                dataGridViewSheet,
                dataGridViewTitleblock,
                dataGridViewTitleblockType,
                dataGridViewProjectInfo,
                dataGridViewOther
            };

            foreach (DataGridView dgv in dgvs)
            {
                foreach (DataGridViewRow row in dgv.Rows)
                {
                    if (row.IsNewRow) continue;
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        if (cell is DataGridViewCheckBoxCell)
                            cell.Value = false;
                    }
                }
            }
        }
    }
}