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
#region Usings
using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;
#endregion

namespace RebarSketch
{
    public static class FileSupport
    {
        public static string[] ReadFileWithAnyDecoding(string filePath)
        {
            bool check = File.Exists(filePath);
            if (!check)
            {
                throw new Exception($"{MyStrings.ErrorFileNotFound}: {filePath}");
            }
            Encoding e = GetEncoding(filePath);
            string[] familiesNames = File.ReadAllLines(filePath, e);
            return familiesNames;
        }


        public static Encoding GetEncoding(string filename)
        {
            // Read the BOM
            var bom = new byte[4];
            using (var file = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                file.Read(bom, 0, 4);
                file.Close();
            }

            // Analyze the BOM

            if (bom[0] == 0xd0 && bom[1] == 0xbe && bom[2] == 0xd0 && bom[3] == 0xb1)
            {
                Trace.WriteLine("Encoding: UTF8");
                return Encoding.UTF8;
            }
            if (bom[0] == 0x32 && bom[1] == 0x36 && bom[2] == 0x31 && bom[3] == 0x5f)
            {
                Trace.WriteLine("Encoding: 1251");
                return Encoding.GetEncoding(1251);
            }
            Trace.WriteLine("Encoding: UTF8 as default");
            return Encoding.UTF8;
        }


        public static void CheckAndDeleteFolder(string path)
        {
            Trace.WriteLine("Try to delete folder: " + path);
            bool checkExists = System.IO.Directory.Exists(path);
            if (!checkExists) return;

            System.IO.DirectoryInfo di = new DirectoryInfo(path);

            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }
            foreach (DirectoryInfo dir in di.GetDirectories())
            {
                dir.Delete(true);
            }

            System.IO.Directory.Delete(path);
            Trace.WriteLine("Folder deleted: " + path);
        }


        public static System.Drawing.FontStyle GetFontStyle(string text)
        {
            if (text == "Bold")
            {
                Trace.WriteLine("Font style: bold");
                return System.Drawing.FontStyle.Bold;
            }
            if (text == "Italic")
            {
                Trace.WriteLine("Font style: italic");
                return System.Drawing.FontStyle.Italic;
            }
            Trace.WriteLine("Font style: regular");
            return System.Drawing.FontStyle.Regular;
        }

        //можно хранить информацию прямо в addin-файле и считывать его как xml, может еще где-то пригодится
        //public static string GetMainWeandrevitFolder(string addinPath)
        //{
        //    Trace.WriteLine("Font style italic");
        //    bool check = File.Exists(addinPath);
        //    string gatesPath = "";
        //    XmlDocument xDoc = new XmlDocument();
        //    xDoc.Load(addinPath);
        //    XmlElement xRoot = xDoc.DocumentElement;
        //    XmlNode addinNode = xRoot.SelectSingleNode("AddIn");
        //    XmlNode pathNode = addinNode.SelectSingleNode("GatesPath");
        //    gatesPath = pathNode.InnerText;

        //    return gatesPath;
        //}
    }
}
