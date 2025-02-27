using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace LinkWriter
{
    public partial class FormSelectParameters : Form
    {
        public WriteLinkSettings Settings { get; set; }
        public FormSelectParameters(WriteLinkSettings sets)
        {
            InitializeComponent();

            Settings = sets;

            WriteList(checkedListBoxSheet, sets.SheetParams);
            WriteList(checkedListBoxTitleblock, sets.TitleblockParams);
            WriteList(checkedListBoxTitleblockType, sets.TypeParams);
            WriteList(checkedListBoxProjectInfo, sets.ProjectParams);
        }



        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void buttonNext_Click(object sender, EventArgs e)
        {
            Settings.SheetParams = ReadList(checkedListBoxSheet);
            Settings.TitleblockParams = ReadList(checkedListBoxTitleblock);
            Settings.TypeParams = ReadList(checkedListBoxTitleblockType);
            Settings.ProjectParams = ReadList(checkedListBoxProjectInfo);


            if (Settings.ParametersCount == 0)
            {
                MessageBox.Show("Please choose parameters");
                return;
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void WriteList(CheckedListBox list, List<MyParameterValue> values)
        {
            for (int i = 0; i < values.Count; i++)
            {
                MyParameterValue val = values[i];
                int addedRowIndex = list.Items.Add(val);
                if (val.IsEnabled)
                {
                    list.SetItemChecked(i, true);
                }
            }
        }

        private List<MyParameterValue> ReadList(CheckedListBox list)
        {
            List<MyParameterValue> checkedparams = new List<MyParameterValue>();
            foreach (MyParameterValue row in list.CheckedItems)
            {
                checkedparams.Add(row);
            }
            return checkedparams;
        }

        private void buttonReset_Click(object sender, EventArgs e)
        {
            foreach (Control c in this.Controls)
            {
                if (c is CheckedListBox)
                {
                    CheckedListBox clb = (CheckedListBox)c;
                    for (int i = 0; i < clb.Items.Count; i++)
                    {
                        clb.SetItemChecked(i, false);
                    }
                }
            }
        }
    }
}
