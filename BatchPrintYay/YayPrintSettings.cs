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
using Autodesk.Revit.DB;
using System.Collections.Generic;
using System.Xml.Serialization;
#endregion

namespace BatchPrintYay
{
    /// <summary>
    /// Вспомогательный класс, более удобно хранящий информацию о параметрах печати листа
    /// </summary>
    public class YayPrintSettings
    {
        public string printerName = new System.Drawing.Printing.PrinterSettings().PrinterName;
        public string outputFolder = @"C:\PDF_Print";
        public string nameConstructor = MyStrings.DefaultPDFfilename;
        public HiddenLineViewsType hiddenLineProcessing = HiddenLineViewsType.VectorProcessing;
        public ColorType colorsType = ColorType.Monochrome;
        public RasterQualityType rasterQuality = RasterQualityType.High;
        public string alwaysColorParamName = MyStrings.DefaultAlwaysColoredParamname;

        public bool mergePdfs = false;
        public bool printToPaper = false;
        public bool useOrientation = false;
        public bool refreshSchedules = true;

        public bool exportToDwg = false;
        public string dwgNameConstructor = MyStrings.DefaultDwgName;
        public string selectedDwgExportProfileName = "Monochrome";
        [XmlIgnore]
        public List<ExportDWGSettings> dwgProfiles = new List<ExportDWGSettings>();


        public static List<PdfColor> GetStandardExcludeColors()
        {
            List<PdfColor> list = new List<PdfColor>
                {
                    new PdfColor(System.Drawing.Color.FromArgb(0,0,255)),
                    new PdfColor(System.Drawing.Color.FromArgb(192,192,192)),
                    new PdfColor(System.Drawing.Color.FromArgb(242,242,242))
                };
            return list;
        }

        public List<PdfColor> excludeColors;


        ///// <summary>
        ///// Получение параметров печати
        ///// </summary>
        //public static YayPrintSettings GetSavedPrintSettings()
        //{
        //    string xmlpath = ActivateFolder();

        //    YayPrintSettings ps;
        //    XmlSerializer serializer = new XmlSerializer(typeof(YayPrintSettings));

        //    if (File.Exists(xmlpath))
        //    {
        //        using (StreamReader reader = new StreamReader(xmlpath))
        //        {
        //            ps = (YayPrintSettings)serializer.Deserialize(reader);
        //            if (ps == null)
        //            {
        //                TaskDialog.Show("Warning", MyStrings.MessageUnableToGetSavedPrintSettings);
        //                Trace.WriteLine("Unable to get print setiings, set to default");
        //                ps = new YayPrintSettings();
        //            }
        //        }
        //    }
        //    else
        //    {
        //        ps = new YayPrintSettings();
        //    }

        //    if(ps.excludeColors == null || ps.excludeColors.Count == 0)
        //    {
        //        ps.excludeColors = YayPrintSettings.GetStandardExcludeColors();
        //    }

        //    return ps;
        //}

        //public static bool SaveSettings(YayPrintSettings yps)
        //{
        //    string xmlpath = ActivateFolder();
        //    XmlSerializer serializer = new XmlSerializer(typeof(YayPrintSettings));
        //    if (File.Exists(xmlpath)) File.Delete(xmlpath);
        //    using (FileStream writer = new FileStream(xmlpath, FileMode.OpenOrCreate))
        //    {
        //        serializer.Serialize(writer, yps);
        //    }
        //    return true;
        //}

        //private static string ActivateFolder()
        //{
        //    string appdataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        //    string bspath = Path.Combine(appdataPath, "bim-starter");
        //    if (!Directory.Exists(bspath)) Directory.CreateDirectory(bspath);
        //    string localFolder = Path.Combine(bspath, "BatchPrintYay");
        //    if (!Directory.Exists(localFolder)) Directory.CreateDirectory(localFolder);
        //    string xmlpath = Path.Combine(localFolder, "settings.xml");
        //    return xmlpath;
        //}

        /// <summary>
        /// Беспараметрический конструктор для сериализатора
        /// </summary>
        public YayPrintSettings()
        {

        }
    }
}
