#region License
/*Данный код опубликован под лицензией Creative Commons Attribution-ShareAlike.
Разрешено использовать, распространять, изменять и брать данный код за основу для производных в коммерческих и
некоммерческих целях, при условии указания авторства и если производные лицензируются на тех же условиях.
Код поставляется "как есть". Автор не несет ответственности за возможные последствия использования.
Зуев Александр, 2020, все права защищены.
This code is listed under the Creative Commons Attribution-ShareAlike license.
You may use, redistribute, remix, tweak, and build upon this work non-commercially and commercially,
as long as you credit the author by linking back and license your new creations under the same terms.
This code is provided 'as is'. Author disclaims any implied warranty.
Zuev Aleksandr, 2020, all rigths reserved.*/
#endregion
using RevitWorksets.WorksetWorkers;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;

namespace RevitWorksets
{
    [Serializable]
    public class InfosStorage
    {
        public bool WorksetByCategoryEnabled = true;
        public BindingList<WorksetByCategory> worksetsByCategory = new BindingList<WorksetByCategory>();

        public bool WorksetByFamilyEnabled = true;
        public BindingList<WorksetByFamily> worksetsByFamily = new BindingList<WorksetByFamily>();

        public bool WorksetByTypeEnabled = false;
        public BindingList<WorksetByType> worksetsByType = new BindingList<WorksetByType>();

        public bool WorksetByParameterEnabled = false;
        public WorksetByParameter worksetByParameter =
            new WorksetByParameter { ParameterName = "WorksetNameForPlugin" };

        public bool WorksetByLinkEnabled = true;
        public WorksetByLink worksetByLink = new WorksetByLink();


        public bool WorksetByDwgEnabled = true;
        public WorksetByDwg worksetByDwg = new WorksetByDwg { WorksetName = "_DWG" };

        public bool NoEmptyWorksets = false;

        public InfosStorage()
        {

        }

        public static InfosStorage Load(out string xmlPath)
        {
            InfosStorage model;
            xmlPath = InfosStorage.GetLastUsedXmlFile();
            if (string.IsNullOrEmpty(xmlPath) || !File.Exists(xmlPath))
            {
                model = InfosStorage.GetDefault();
                xmlPath = null;
            }
            else
            {
                model = InfosStorage.LoadFromFile(xmlPath);
            }
            return model;
        }

        public static string GetLastUsedXmlFile()
        {
            //проверяю, запускался ли плагин ранее - наличие файла с путем к xml
            string storageFolder = InfosStorage.GetDefaultStorageFolder();
            string pathFileName = Path.Combine(storageFolder, "path.txt");

            if (File.Exists(pathFileName))
            {
                string xmlPath = File.ReadAllText(pathFileName);
                return xmlPath;
            }
            else
            {
                return null;
            }
        }

        public static void SaveLastUsedXmlPath(string xmlPath)
        {
            string pathFileName = GetPathFile();
            File.WriteAllText(pathFileName, xmlPath);
        }

        public static void ClearLastXmlPath()
        {
            string pathFileName = GetPathFile();
            if (File.Exists(pathFileName))
            {
                try
                {
                    File.Delete(pathFileName);
                    Debug.WriteLine("Deleted file: " + pathFileName);
                }
                catch
                {
                    Debug.WriteLine("Failed delete file: " + pathFileName);
                }
            }
            else
            {
                Debug.WriteLine("No file: " + pathFileName);
            }
        }

        private static string GetPathFile()
        {
            string storageFolder = InfosStorage.GetDefaultStorageFolder();
            string pathFileName = Path.Combine(storageFolder, "path.txt");
            return pathFileName;
        }

        public static InfosStorage LoadFromFile(string xmlPath)
        {
            Debug.WriteLine("Load settings from file: " + xmlPath);
            InfosStorage model = null;
            if (File.Exists(xmlPath))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(InfosStorage));
                using (StreamReader reader = new StreamReader(xmlPath))
                {
                    try
                    {
                        model = (InfosStorage)serializer.Deserialize(reader);
                        Debug.WriteLine("Settings deserialize success");
                    }
                    catch { }
                }
            }
            if (model == null)
            {
                Debug.WriteLine("Settings is null, create new one");
                model = GetDefault();
            }
            Debug.WriteLine("Settings is loaded succesfully");
            return model;
        }

        public static string GetDefaultStorageFolder()
        {
            Debug.WriteLine("Get default settings folder");
            string appdataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string rbspath = Path.Combine(appdataPath, "bim-starter");
            if (!Directory.Exists(rbspath))
            {
                Debug.WriteLine("Create directory " + rbspath);
                Directory.CreateDirectory(rbspath);
            }
            string solutionName = "RevitWorksets"; //System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
            string solutionFolder = Path.Combine(rbspath, solutionName);
            if (!Directory.Exists(solutionFolder))
            {
                Directory.CreateDirectory(solutionFolder);
                Debug.WriteLine("Create directory " + solutionFolder);
            }

            Debug.WriteLine("Folder: " + solutionFolder);
            return solutionFolder;
        }


        public void Save(string xmlPath)
        {
            Debug.WriteLine("Start save settings to file " + xmlPath);

            if (string.IsNullOrEmpty(xmlPath))
                throw new Exception("XmlPath is empty");

            if (File.Exists(xmlPath)) File.Delete(xmlPath);
            XmlSerializer serializer = new XmlSerializer(typeof(InfosStorage));
            using (FileStream writer = new FileStream(xmlPath, FileMode.OpenOrCreate))
            {
                serializer.Serialize(writer, this);
            }
            SaveLastUsedXmlPath(xmlPath);
            Debug.WriteLine("Save settings success");
        }

        public static InfosStorage GetDefault()
        {
            Debug.WriteLine("Get default settings");
            InfosStorage storage = new InfosStorage();

            storage.worksetsByCategory = WorksetByCategory.GetDefault();

            storage.worksetsByFamily = WorksetByFamily.GetDefault();

            storage.worksetsByType = WorksetByType.GetDefault();

            Debug.WriteLine("Default settings is created sccesfully");
            return storage;
        }
    }
}
