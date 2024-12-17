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
    public partial class FormFamilyUploadResult : Form
    {
        

        public FormFamilyUploadResult(string mainUrl, Dictionary<string,string> parentUrls = null)
        {
            InitializeComponent();
            linkLabelMainFam.Tag = mainUrl;

            int rowHeight = 20;
            if(parentUrls != null && parentUrls.Count > 0)
            {
                this.Height = 230 + rowHeight * parentUrls.Count;
                label2.Visible = true;
                label3.Visible = true;
                int i = 1;
                int curPosition = 122;

                foreach(var kvp in parentUrls)
                {
                    LinkLabel parentLinkLabel = new LinkLabel();
                    parentLinkLabel.Location = new System.Drawing.Point(12, curPosition);
                    parentLinkLabel.Name = "linkLabelParentFam" + i.ToString();
                    parentLinkLabel.Size = new System.Drawing.Size(146, 13);
                    parentLinkLabel.Tag = kvp.Value;
                    parentLinkLabel.Text = kvp.Key;
                    parentLinkLabel.LinkClicked += new LinkLabelLinkClickedEventHandler(this.linkLabelMainFam_LinkClicked);

                    this.Controls.Add(parentLinkLabel);
                    i++;
                    curPosition += rowHeight;
                }
            }
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void linkLabelMainFam_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            LinkLabel lab = sender as LinkLabel;
            string url = (string)lab.Tag;
            System.Diagnostics.Process.Start(url);
        }
    }
}
