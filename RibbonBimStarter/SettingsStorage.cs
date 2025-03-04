#region License
/*Данный код опубликован под лицензией Creative Commons Attribution-NonСommercial-ShareAlike.
Разрешено использовать, распространять, изменять и брать данный код за основу для производных 
в некоммерческих целях, при условии указания авторства и если производные лицензируются на тех же условиях.
Код поставляется "как есть". Автор не несет ответственности за возможные последствия использования.
Зуев Александр, 2021, все права защищены.
This code is listed under the Creative Commons Attribution-NonСommercial-ShareAlike license.
You may use, redistribute, remix, tweak, and build upon this work non-commercially,
as long as you credit the author by linking back and license your new creations under the same terms.
This code is provided 'as is'. Author disclaims any implied warranty.
Zuev Aleksandr, 2021, all rigths reserved.*/
#endregion
#region usings
using System;
using System.Diagnostics;
using System.IO;
//using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows.Forms;
using System.Xml.Serialization;
#endregion

namespace RibbonBimStarter
{
    public enum ProxyType { No, Auto, Manual };
    [Serializable]
    public class SettingsStorage
    {
        public string Email;
        public string Password;
        public string Website = @"https://bim-starter.com/";
        public ProxyType Proxy = ProxyType.No;
        public string ProxyServer = "192.168.0.1";
        public int ProxyPort = 8080;
        public bool UseProxyPassword = false;
        public string ProxyUsername;
        public string ProxyPassword;
        public int refreshInterval = 10;

        public bool ShowStartupWindow = true;

        [NonSerialized]
        public static string settingsPath;

        public SettingsStorage()
        {
            //пустой конструктор для сериализатора
        }

        public static SettingsStorage LoadSettings()
        {
            SettingsStorage ss = null;
            settingsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "bim-starter", "settings.xml");
            if (!File.Exists(settingsPath))
            {
                MessageBox.Show("Не найден файл конфигурации: " + settingsPath);
            }
            else
            {
                XmlSerializer serializer =
                    new XmlSerializer(typeof(SettingsStorage));
                using (StreamReader reader = new StreamReader(settingsPath))
                {
                    ss = (SettingsStorage)serializer.Deserialize(reader);
                }
            }
            return ss;
        }

        public void Save(SettingsStorage newSettings)
        {
            Debug.WriteLine("Start save settings to file " + settingsPath);
            XmlSerializer serializer = new XmlSerializer(typeof(SettingsStorage));
            using (FileStream writer = new FileStream(settingsPath, FileMode.OpenOrCreate))
            {
                serializer.Serialize(writer, newSettings);
            }
            Debug.WriteLine("Save settings success");
        }

    }
}
