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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.IO;
#endregion

namespace RibbonBimStarter
{
    public class EventLoadFamily : IExternalEventHandler
    {
        public static string familyguid;
        public static string familyname;

        public void Execute(UIApplication app)
        {
            Trace.Listeners.Clear();
            Trace.Listeners.Add(new Logger("Downloadfamily"));
            Document doc = app.ActiveUIDocument.Document;
            

            WebConnection connect = new WebConnection(App.settings.Email, App.settings.Password, App.settings.Website);
            ServerResponse famInfoResponse = connect.Request("familygetinfo", new Dictionary<string, string>() { ["guid"] = familyguid });
            if (famInfoResponse.Statuscode >= 400)
            {
                TaskDialog.Show("Ошибка", famInfoResponse.Message);
                return;
            }

            Dictionary<string, FamilyCard> checkInfos = null;
            try
            {
                checkInfos = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, FamilyCard>>(famInfoResponse.Message);
            }
            catch { }
            if (checkInfos == null || checkInfos.Count == 0)
            {
                TaskDialog.Show("Ошибка", "Не удалось получить информацию о семействе");
                return;
            }
            FamilyCard info = checkInfos[familyguid];
            List<FamilyVersion> versions = info.versions
                .Where(v => v.status == "ok").ToList();

            Family fam = GetFamilyNyName(familyname, doc);
            FamilySymbol famSymb = null;
            bool loadFamilyFromServer = false;

            if (fam != null)
            {
                famSymb = GetFamilySymbol(fam);
                if (famSymb == null) return;
                
                int existedFamVersionNum = -1;
                Parameter versionParam = famSymb.LookupParameter("RBS_VERSION");
                if (versionParam != null)
                    existedFamVersionNum = versionParam.AsInteger();

                List<FamilyVersion> newerVersions = versions.Where(v => v.version > existedFamVersionNum).ToList();
                if(newerVersions.Count > 0)
                {
                    FormReplaceFamilyInProject formReplace = new FormReplaceFamilyInProject(familyname, existedFamVersionNum, newerVersions);
                    formReplace.ShowDialog();

                    if (formReplace.DialogResult == System.Windows.Forms.DialogResult.Yes)
                        loadFamilyFromServer = true;
                    else if (formReplace.DialogResult == System.Windows.Forms.DialogResult.No)
                        loadFamilyFromServer = false;
                    else
                    {
                        Debug.WriteLine("Cancelled by user");
                        return;
                    }
                }
            }

            if(fam == null || loadFamilyFromServer == true)
            {
                fam = EventLoadFamily.LoadFamily(connect, doc, familyguid, familyname);
                if (fam == null) return;
            }
           
            famSymb = GetFamilySymbol(fam);
            if (!famSymb.IsActive)
            {
                using (Transaction t2 = new Transaction(doc))
                {
                    t2.Start("Activate symbol");
                    famSymb.Activate();
                    Debug.WriteLine("Family symbol has activated");
                    t2.Commit();
                }
            }

            app.ActiveUIDocument.PostRequestForElementTypePlacement(famSymb);
            //Debug.WriteLine("Family has loaded succesfully");
        }

        public static Family LoadFamily(WebConnection connect, Document doc, string famGuid, string famName)
        {
            Family fam = null;
            Debug.WriteLine("Start download family: " + famGuid);
            ServerResponse sr = connect.DownloadFamily(famGuid, famName);
            if (sr.Statuscode >= 400)
            {
                TaskDialog.Show("Error " + sr.Statuscode.ToString(), sr.Message);
                return null;
            }
            string fampath = sr.Message;

            using (Transaction t = new Transaction(doc))
            {
                t.Start("Load family " + famName);
                bool loadSuccess = false;
                try
                {
                    loadSuccess = doc.LoadFamily(fampath, new FamilyLoadOptions(), out fam);
                }
                catch
                {
                    Debug.WriteLine("Unable to load family: " + fampath);
                    return null;
                }
                if (!loadSuccess)
                {
                    TaskDialog.Show("Ошибка", "Не удалось загрузить " + fampath);
                    return null; ;
                }
                //string familyname = Path.GetFileNameWithoutExtension(fampath);

                /*fam = GetFamilyNyName(familyname, doc);
                if (fam == null)
                {
                    TaskDialog.Show("Ошибка", "Не удалось найти загруженное семейство " + familyname);
                    Debug.WriteLine("Loaded family isnt found: " + familyname);
                    return;
                }*/
                t.Commit();
            }
            return fam;
        }

        public string GetName()
        {
            return "Bim-Starter_LoadFamily_Event";
        }

        private FamilySymbol GetFamilySymbol(Family fam)
        {
            IEnumerable<ElementId> symbIds = fam.GetFamilySymbolIds();
            if (symbIds.Count() == 0)
            {
                TaskDialog.Show("Ошибка", "Семейство не имеет типоразмеров: " + fam.Name);
                return null;
            }
            FamilySymbol symb = fam.Document.GetElement(symbIds.First()) as FamilySymbol;
            return symb;
        }

        private Family GetFamilyNyName(string familyname, Document doc)
        {
            IEnumerable<Family> fams = new FilteredElementCollector(doc)
                    .WhereElementIsNotElementType()
                    .OfClass(typeof(Family))
                    .Where(i => i.Name.Equals(familyname))
                    .Cast<Family>();
            if (fams.Count() == 0)
                return null;
            else
                return fams.First();
        }
    }
}
