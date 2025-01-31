using System.Collections.Generic;
using System.Windows.Forms;

namespace LinkWriter
{
    public partial class FormSelectParameterValues : Form
    {
        public Dictionary<string, List<(string, string)>> ValuesSheets { get; set; }
        public Dictionary<string, List<(string, string)>> ValuesTitleblocks { get; set; }
        public Dictionary<string, List<(string, string)>> ValuesTitleblockType { get; set; }

        List<(string, string)> ValuesProjectInfo { get; set; }

        List<string> LinkNames { get; set; }

        public FormSelectParameterValues(List<string> linkNames, WriteLinkSettings sets)
        {
            InitializeComponent();

            LinkNames = linkNames;

            dataGridViewSheet.Columns.Add(new DataGridViewCheckBoxColumn() { Name = "Enable", HeaderText = "Вкл", FillWeight = 15 });
            dataGridViewSheet.Columns.Add(new DataGridViewTextBoxColumn() { Name = "ParameterName", HeaderText = "Параметр", FillWeight = 40 });

            for (int i = 0; i < linkNames.Count; i++)
            {
                dataGridViewSheet.Columns.Add(new DataGridViewTextBoxColumn() { Name = $"Link{i}", HeaderText = linkNames[i], AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            }

            dataGridViewSheet.Rows.Clear();

            foreach (MyParameterValue param in sets.SheetParams)
            {
                object[] row = new object[linkNames.Count + 2];
                row[0] = false;
                row[1] = param.ParameterName;
                for (int i = 0; i < linkNames.Count; i++)
                {
                    row[i + 2] = param.GetValueAsString();
                }
                dataGridViewSheet.Rows.Add(row);
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

        private void WriteDataGridValues(List<MyParameterValue> values)
        {

        }

        private void WriteDataGridValuesByLinks(List<string> linkNames, List<MyParameterValue> values)
        {

        }

        private Dictionary<string, List<(string, string)>> GetDataGridValusByLinks(DataGridView dgv)
        {
            Dictionary<string, List<(string, string)>> Values = new Dictionary<string, List<(string, string)>>();

            foreach (DataGridViewRow row in dataGridViewSheet.Rows)
            {
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

            foreach (DataGridViewRow row in dataGridViewSheet.Rows)
            {
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
