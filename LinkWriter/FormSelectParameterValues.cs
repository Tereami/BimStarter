﻿using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using Tools.Model.CategoryTools;

namespace LinkWriter
{
    public partial class FormSelectParameterValues : System.Windows.Forms.Form
    {
        public Dictionary<string, List<(string, string)>> ValuesSheets { get; set; }
        public Dictionary<string, List<(string, string)>> ValuesTitleblocks { get; set; }
        public Dictionary<string, List<(string, string)>> ValuesTitleblockType { get; set; }

        public List<(string, string)> ValuesProjectInfo { get; set; }

        public Dictionary<string, List<(string, string, List<BuiltInCategory>)>> ValuesCustomParameters { get; set; }

        List<string> LinkNames { get; set; }

        Save savedData { get; set; }

        List<RevitCategory> AllCategories { get; set; }

        public FormSelectParameterValues(List<string> linkNames, WriteLinkSettings sets, List<RevitCategory> allCategories)
        {
            InitializeComponent();

            LinkNames = linkNames;
            savedData = sets.SavedData;

            AllCategories = allCategories;

            BuildDatagridByLinks(dataGridViewSheet, sets.SheetParams);
            BuildDatagridByLinks(dataGridViewTitleblock, sets.TitleblockParams);
            BuildDatagridByLinks(dataGridViewTitleblockType, sets.TypeParams);
            BuildDatagrid(dataGridViewProjectInfo, sets.ProjectParams);
            BuildDataGridWithCategories(dataGridViewOther, savedData.CustomParameters);
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

        public void BuildDataGridWithCategories(DataGridView dgv, BindingList<CustomParameter> values)
        {
            string prefix = dgv.Name;
            dgv.Columns.Add(new DataGridViewTextBoxColumn() { Name = $"{prefix}_ParamName", HeaderText = "Parameter", FillWeight = 40 });

            dgv.Columns.Add(new DataGridViewButtonColumn() { Name = $"{prefix}_Categories", HeaderText = "Categories", FillWeight = 40 });

            for (int i = 0; i < LinkNames.Count; i++)
            {
                dgv.Columns.Add(new DataGridViewTextBoxColumn() { Name = $"{prefix}_Link{i}", HeaderText = LinkNames[i], AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            }

            dgv.Rows.Clear();

            foreach (CustomParameter param in values)
            {
                object[] row = new object[LinkNames.Count + 2];
                row[0] = param.Name;
                row[1] = param.CategoriesText;
                for (int i = 0; i < LinkNames.Count; i++)
                {
                    row[i + 2] = param.Value;
                }
                dgv.Rows.Add(row);
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
            ValuesCustomParameters = GetDataGridValuesCustom(dataGridViewOther);

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

        public Dictionary<string, List<(string, string, List<BuiltInCategory>)>> GetDataGridValuesCustom(DataGridView dgv)
        {
            Dictionary<string, List<(string, string, List<BuiltInCategory>)>> values =
                new Dictionary<string, List<(string, string, List<BuiltInCategory>)>>();

            for (int r = 0; r < dgv.RowCount; r++)
            {
                DataGridViewRow row = dgv.Rows[r];
                if (row.IsNewRow) continue;
                CustomParameter cp = savedData.CustomParameters[r];

                string paramName = row.Cells[0].Value as string;
                List<BuiltInCategory> cats = cp.revitCategories;

                for (int i = 2; i < row.Cells.Count; i++)
                {
                    string value = (string)row.Cells[i].Value;
                    string linkName = LinkNames[i - 2];

                    if (values.ContainsKey(linkName))
                    {
                        values[linkName].Add((paramName, value, cats));
                    }
                    else
                    {
                        values.Add(linkName, new List<(string, string, List<BuiltInCategory>)> { (paramName, value, cats) });
                    }
                }
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

        private void dataGridViewOther_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dgv = sender as DataGridView ?? throw new Exception("DataGridView cast error");
            int rowNumber = -1;
            if (dgv.Columns[e.ColumnIndex] is DataGridViewButtonColumn)
            {
                rowNumber = e.RowIndex;
            }
            if (rowNumber == -1) return;

            if (rowNumber >= savedData.CustomParameters.Count) return;

            CustomParameter cp = savedData.CustomParameters[rowNumber];
            if (cp.revitCategories == null)
                cp.revitCategories = new List<Autodesk.Revit.DB.BuiltInCategory>();

            Tools.Forms.FormSelectCategories formSelect = new Tools.Forms.FormSelectCategories(cp.revitCategories, AllCategories);
            if (formSelect.ShowDialog() != DialogResult.OK)
                return;
            cp.revitCategories = formSelect.SelectedCategories;
            dgv.Rows[rowNumber].Cells[1].Value = cp.CategoriesText;
            this.Refresh();
        }


        private void buttonAddCustomParam_Click(object sender, EventArgs e)
        {
            CustomParameter cp = CustomParameter.GetDefault();
            savedData.CustomParameters.Add(cp);

            object[] row = new object[LinkNames.Count + 2];
            row[0] = cp.Name;
            row[1] = cp.CategoriesText;
            for (int i = 0; i < LinkNames.Count; i++)
            {
                row[i + 2] = cp.Value;
            }
            dataGridViewOther.Rows.Add(row);
        }
    }
}