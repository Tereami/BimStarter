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
#region usings
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
#endregion

namespace RebarSketch
{
    [Serializable]
    public class XmlSketchItem
    {
        [System.Xml.Serialization.XmlIgnore]
        public bool IsSubtype = false;
        [System.Xml.Serialization.XmlIgnore]
        public int SubtypeNumber = 0;
        [System.Xml.Serialization.XmlIgnore]
        public bool IsXmlSource = false;
        [System.Xml.Serialization.XmlIgnore]
        public string formName;
        [System.Xml.Serialization.XmlIgnore]
        public string templateImagePath;
        [System.Xml.Serialization.XmlIgnore]
        public string folder;

        public List<string> families { get; set; }
        public List<ScetchParameter> parameters { get; set; }



        public XmlSketchItem()
        {
            //пустой конструктор для сериализатора
        }

        public static XmlSketchItem Load(string folder)
        {
            XmlSketchItem xsi = null;

            string xmlConfigFilePath = System.IO.Path.Combine(folder, "config.xml");
            string parametersTxtPath = Path.Combine(folder, "parameters.txt");
            if (System.IO.File.Exists(xmlConfigFilePath))

                xsi = XmlSketchItem.LoadFromXml(xmlConfigFilePath);
            else if (System.IO.File.Exists(parametersTxtPath))
                xsi = XmlSketchItem.LoadFromTxt(folder);
            else
                throw new Exception("Incorrect template " + folder.Replace("\\", "\\ "));

            xsi.folder = folder;
            xsi.formName = folder.Split('\\').Last();
            xsi.templateImagePath = Path.Combine(folder, "scetch.png");

            if(!System.IO.File.Exists(xsi.templateImagePath))
                throw new Exception("Image not found " + xsi.templateImagePath.Replace("\\", "\\ "));

            return xsi;
        }

        private static XmlSketchItem LoadFromXml(string xmlPath)
        {
            Trace.WriteLine("Read Xml sketch file: " + xmlPath);
            XmlSketchItem xsi;
            XmlSerializer serializer = new XmlSerializer(typeof(XmlSketchItem));

            using (StreamReader reader = new StreamReader(xmlPath))
            {
                xsi = (XmlSketchItem)serializer.Deserialize(reader);
                if (xsi == null)
                {
                    System.Windows.Forms.MessageBox.Show(MyStrings.ErrorFailedToLoadSettings);
                    Trace.WriteLine("Unable to get setiings, set default");
                    xsi = new XmlSketchItem();
                }
            }
            xsi.IsXmlSource = true;
            return xsi;
        }

        private static XmlSketchItem LoadFromTxt(string folder)
        {
            XmlSketchItem xsi = new XmlSketchItem();
            xsi.IsXmlSource = false;

            string parametersTxtPath = Path.Combine(folder, "parameters.txt");

            string[] paramsArray = FileSupport.ReadFileWithAnyDecoding(parametersTxtPath);

            xsi.parameters = new List<ScetchParameter>();
            for (int i = 0; i < paramsArray.Length; i++)
            {
                string p = paramsArray[i];
                if (p.StartsWith("#")) continue;
                if (p.Length < 1) continue;
                string[] paramInfo = p.Split(',');
                if (paramInfo.Length < 4)
                {
                    throw new Exception("Incorrect syntax in file " + parametersTxtPath + ", line " + i);
                }
                ScetchParameter sparam = new ScetchParameter();
                sparam.Name = paramInfo[0];
                sparam.PositionX = float.Parse(paramInfo[1]);
                sparam.PositionY = float.Parse(paramInfo[2]);
                sparam.Rotation = float.Parse(paramInfo[3]);

                if (paramInfo.Length > 4)
                {
                    if (paramInfo[4] == "1")
                    {
                        sparam.IsNarrow = true;
                    }
                }
                xsi.parameters.Add(sparam);
            }

            xsi.families = new List<string>();
            string familiesTxtPath = System.IO.Path.Combine(folder, "families.txt");
            if (!System.IO.File.Exists(familiesTxtPath))
                throw new Exception("File not found " + familiesTxtPath);

            string[] familiesArray = FileSupport.ReadFileWithAnyDecoding(familiesTxtPath);
            xsi.families = familiesArray.ToList();

            return xsi;
        }

        public void Save()
        {
            string xmlPath = Path.Combine(folder, "config.xml");
            Trace.WriteLine("Save sketch config to file: " + xmlPath);
            if (File.Exists(xmlPath))
            {
                try
                {
                    File.Delete(xmlPath);
                }
                catch
                {
                    string msg = $"{MyStrings.ErrorNoPermission} {xmlPath}";
                    System.Windows.Forms.MessageBox.Show(msg);
                    throw new Exception(msg);
                }
            }

            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(XmlSketchItem));
                using (FileStream writer = new FileStream(xmlPath, FileMode.OpenOrCreate))
                {
                    serializer.Serialize(writer, this);
                    Trace.WriteLine("Save settings success");
                }
            }
            catch
            {
                Trace.WriteLine("Save settings failed");
                throw new Exception("Error save file " + xmlPath);
            }

            string txtParametersFile = Path.Combine(folder, "parameters.txt");
            if (File.Exists(txtParametersFile))
            {
                File.Move(txtParametersFile, txtParametersFile + "old");
                Trace.WriteLine("File deleted: " + txtParametersFile);
            }

            string txtFamiliesFile = Path.Combine(folder, "families.txt");
            if (File.Exists(txtFamiliesFile))
            {
                File.Move(txtFamiliesFile, txtFamiliesFile + "old");
                Trace.WriteLine("File deleted: " + txtFamiliesFile);
            }
        }
    }
}
