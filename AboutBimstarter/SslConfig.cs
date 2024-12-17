using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AboutBimstarter
{
    public enum TlsVersion { Default, Tls1, Tls11, Tls12 };

    [Serializable]
    public class SslConfig
    {
        private static string configPath;


        public TlsVersion tlsVersion = TlsVersion.Default;

        public SslConfig()
        {
        }


        public static SslConfig Load()
        {
            string appdataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string rbspath = Path.Combine(appdataPath, "bim-starter");
            if (!Directory.Exists(rbspath)) Directory.CreateDirectory(rbspath);
            string solutionName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
            string localFolder = Path.Combine(rbspath, solutionName);
            if (!Directory.Exists(localFolder)) Directory.CreateDirectory(localFolder);
            configPath = Path.Combine(localFolder, "config.xml");

            XmlSerializer serializer = new XmlSerializer(typeof(SslConfig));
            SslConfig cfg = null;
            bool checkCfgFile = File.Exists(configPath);
            if (checkCfgFile)
            {
                using (StreamReader reader = new StreamReader(configPath))
                {
                    try
                    {
                        cfg = (SslConfig)serializer.Deserialize(reader);
                    }
                    catch
                    {
                        cfg = new SslConfig();
                    }
                    if (cfg == null)
                    {
                        throw new Exception("Failed to serialize: " + configPath);
                    }
                }
            }
            else
            {
                cfg = new SslConfig();
            }

            return cfg;
        }

        public void Save()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(SslConfig));
            if (File.Exists(configPath)) File.Delete(configPath);
            using (FileStream writer = new FileStream(configPath, FileMode.OpenOrCreate))
            {
                serializer.Serialize(writer, this);
            }
        }
    }
}
