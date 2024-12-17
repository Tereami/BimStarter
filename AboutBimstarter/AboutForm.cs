using System;
using System.Net;
using System.Windows.Forms;

namespace AboutBimstarter
{
    public partial class AboutForm : Form
    {
        public SslConfig newSets;


        public AboutForm(int version, SslConfig sets)
        {
            InitializeComponent();

            if (version > 0)
                labelVersion.Text = version.ToString();
            else
                labelVersion.Text = "NULL";
            newSets = sets;
            switch (sets.tlsVersion)
            {
                case TlsVersion.Default:
                    radioButtonDefault.Checked = true;
                    break;
                case TlsVersion.Tls1:
                    radioButtonTLS1.Checked = true;
                    break;
                case TlsVersion.Tls11:
                    radioButtonTLS11.Checked = true;
                    break;
                case TlsVersion.Tls12:
                    radioButtonTLS12.Checked = true;
                    break;
            }

            string currentCert = ServicePointManager.SecurityProtocol.ToString();
            labelCurrentCertificate.Text = currentCert;
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process procMail = new System.Diagnostics.Process();
            procMail.StartInfo.FileName = "mailto:info@bim-starter.com?subject=Вопрос_по_Bim-Starter";
            procMail.Start();
        }


        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://bim-starter.com");
        }


        private void buttonOk_Click(object sender, EventArgs e)
        {
            if (radioButtonTLS1.Checked == true)
                newSets.tlsVersion = TlsVersion.Tls1;
            else if (radioButtonTLS11.Checked == true)
                newSets.tlsVersion = TlsVersion.Tls11;
            else if (radioButtonTLS12.Checked == true)
                newSets.tlsVersion = TlsVersion.Tls12;
            else
            {
                if (newSets.tlsVersion != TlsVersion.Default)
                {
                    MessageBox.Show("Перезапустите Ревит для применения этой настройки");
                }
                newSets.tlsVersion = TlsVersion.Default;
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
