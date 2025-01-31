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
#endregion

namespace SchedulesTools
{
    [Serializable]
    public class TableSettings
    {
        public bool useComplects = false;
        public string sheetComplectParamName = MyStrings.ParameterSheetSet;

        [NonSerialized]
        public bool getLinkFiles = false;

        public bool useStandardSheetNumber = true;
        public string altSheetNumberParam = MyStrings.ParameterSheetNumber;

        public double rowHeight = 0.027;
        public int maxCharsInOneLine = 85;
        public string newLineSymbol = "@";
        public double rowHeightCoeff = 1.5;

        public static string xmlPath = "";

        //public static TableSettings Activate(bool includeLinks)
        //{
        //    Trace.WriteLine("Start activate settings");
        //    string appdataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        //    string rbspath = Path.Combine(appdataPath, "bim-starter");
        //    if (!Directory.Exists(rbspath))
        //    {
        //        Trace.WriteLine("Create directory " + rbspath);
        //        Directory.CreateDirectory(rbspath);
        //    }
        //    string solutionName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
        //    string solutionFolder = Path.Combine(rbspath, solutionName);
        //    if (!Directory.Exists(solutionFolder))
        //    {
        //        Directory.CreateDirectory(solutionFolder);
        //        Trace.WriteLine("Create directory " + solutionFolder);
        //    }
        //    xmlPath = Path.Combine(solutionFolder, "settings.xml");
        //    TableSettings s = null;

        //    if (File.Exists(xmlPath))
        //    {
        //        XmlSerializer serializer = new XmlSerializer(typeof(TableSettings));
        //        using (StreamReader reader = new StreamReader(xmlPath))
        //        {
        //            try
        //            {
        //                s = (TableSettings)serializer.Deserialize(reader);
        //                Trace.WriteLine("Settings deserialize success");
        //            }
        //            catch { }
        //        }
        //    }
        //    if (s == null)
        //    {
        //        s = new TableSettings();
        //        Trace.WriteLine("Settings is null, create new one");
        //    }

        //    s.getLinkFiles = includeLinks;
        //    FormTableSettings form = new FormTableSettings(s);
        //    Trace.WriteLine("Show settings form");
        //    form.ShowDialog();
        //    if (form.DialogResult != System.Windows.Forms.DialogResult.OK)
        //    {
        //        Trace.WriteLine("Setting form cancelled");
        //        throw new OperationCanceledException();
        //    }
        //    s = form.sets;
        //    Trace.WriteLine("Settings success");
        //    return s;
        //}

        //public void Save()
        //{
        //    Trace.WriteLine("Start save settins to file " + xmlPath);
        //    if (File.Exists(xmlPath)) File.Delete(xmlPath);
        //    XmlSerializer serializer = new XmlSerializer(typeof(TableSettings));
        //    using (FileStream writer = new FileStream(xmlPath, FileMode.OpenOrCreate))
        //    {
        //        serializer.Serialize(writer, this);
        //    }
        //    Trace.WriteLine("Save settings success");
        //}
    }
}