#region License
/*This code is listed under the Creative Commons Attribution-ShareAlike license.
You may use, redistribute, remix, tweak, and build upon this work non-commercially and commercially,
as long as you credit the author by linking back and license your new creations under the same terms.
This code is provided 'as is'. Author disclaims any implied warranty.
Zuev Aleksandr, 2024. All rights reserved.*/
#endregion
#region Usings
using System;
using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;
#endregion

[assembly: System.Reflection.AssemblyVersion("1.0.*")]
namespace Tools.SettingsSaver
{
    public class Saver<T> where T : class, new()
    {
        private string xmlPath;

        T Value;

        public T Activate(string ProjectName)
        {
            Debug.WriteLine("Start activate settings");
            string appdataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string rbspath = Path.Combine(appdataPath, "bim-starter");
            if (!Directory.Exists(rbspath))
            {
                Debug.WriteLine("Create directory " + rbspath);
                Directory.CreateDirectory(rbspath);
            }
            string solutionName = ProjectName;
            string solutionFolder = Path.Combine(rbspath, solutionName);
            if (!Directory.Exists(solutionFolder))
            {
                Directory.CreateDirectory(solutionFolder);
                Debug.WriteLine("Create directory " + solutionFolder);
            }
            xmlPath = Path.Combine(solutionFolder, "settings.xml");
            Value = null;

            if (File.Exists(xmlPath))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                using (StreamReader reader = new StreamReader(xmlPath))
                {
                    try
                    {
                        Value = (T)serializer.Deserialize(reader);
                        Debug.WriteLine("Settings deserialize success");
                    }
                    catch { }
                }
            }
            if (Value == null)
            {
                Value = new T();
                Debug.WriteLine("Settings is null, create new one");
            }

            Debug.WriteLine("Success");
            return Value;
        }

        public void Save(T newSettings)
        {
            Debug.WriteLine("Start save settings to file " + xmlPath);
            Reset();
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (FileStream writer = new FileStream(xmlPath, FileMode.OpenOrCreate))
            {
                serializer.Serialize(writer, newSettings);
            }
            Debug.WriteLine("Save settings success");
        }

        public void Reset()
        {
            if (!File.Exists(xmlPath)) return;

            try
            {
                File.Delete(xmlPath);
            }
            catch
            {
                throw new Exception("FAILED TO DELETE FILE " + xmlPath);
            }
        }

        public string GetXmlPath()
        {
            return xmlPath;
        }
    }
}
