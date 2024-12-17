using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Interop;
using System.Xml.Serialization;

namespace RibbonBimStarter
{
    public enum TlsVersion { Default, Tls1, Tls11, Tls12 };
    [Serializable]
    public class SslConfig
    {
        public TlsVersion tlsVersion = TlsVersion.Default;

        public SslConfig()
        {
        }

        public static void Load()
        {
            TlsVersion curTls = TlsVersion.Default;
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "bim-starter", "AboutBimstarter", "config.xml");
            if (!File.Exists(path))
            {
                string msg = "Не найден файл конфигурации: " + path;
                //System.Windows.Forms.MessageBox.Show(msg);
                Debug.WriteLine(msg);
            }
            else
            {
                SslConfig ss = null;
                XmlSerializer serializer =
                    new XmlSerializer(typeof(SslConfig));
                using (StreamReader reader = new StreamReader(path))
                {
                    ss = (SslConfig)serializer.Deserialize(reader);
                }
                curTls = ss.tlsVersion;
                string name = "Выбран сертификат: " + Enum.GetName(typeof(TlsVersion), curTls);
                //System.Windows.Forms.MessageBox.Show(name);
                Debug.WriteLine(name);
            }

            if (curTls == TlsVersion.Default)
            {
                Debug.WriteLine("Certificate is not changed, use default");
            }
            else if (curTls == TlsVersion.Tls1)
            {
                Debug.WriteLine("Set tls1");
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
            }
            else if (curTls == TlsVersion.Tls11)
            {
                Debug.WriteLine("Set tls11");
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11;
            }
            else if (curTls == TlsVersion.Tls12)
            {
                Debug.WriteLine("Set tls12");
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            }
        }
    }
}
