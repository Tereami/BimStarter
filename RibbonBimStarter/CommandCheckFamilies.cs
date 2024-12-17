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

using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace RibbonBimStarter
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class CommandCheckFamilies : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Trace.Listeners.Clear();
            Trace.Listeners.Add(new Logger("CheckFamilies"));


            Document doc = commandData.Application.ActiveUIDocument.Document;
            List<Family> families = new FilteredElementCollector(doc)
                .OfClass(typeof(FamilyInstance))
                .Cast<FamilyInstance>()
                .Select(i => i.Symbol.Family)
                .GroupBy(i => i.Id.GetValue())
                .Select(grp => grp.FirstOrDefault())
                .OrderBy(i => i.Id.GetValue())
                .ToList();

            Dictionary<string, List<Family>> guidsAndFamilies = new Dictionary<string, List<Family>>();
            List<Family> invalidSymbols = new List<Family>();
            List<Family> noGuids = new List<Family>();


            foreach (Family fam in families)
            {
                IEnumerable<ElementId> symbIds = fam.GetFamilySymbolIds();
                if (symbIds.Count() == 0)
                {
                    invalidSymbols.Add(fam);
                    continue;
                }
                FamilySymbol symb = doc.GetElement(symbIds.First()) as FamilySymbol;
                Parameter guidParam = symb.LookupParameter("RBS_GUID");
                Parameter versionParam = symb.LookupParameter("RBS_VERSION");
                if (guidParam == null || versionParam == null)
                {
                    noGuids.Add(fam);
                    continue;
                }
                string guid = guidParam.AsString();
                if (guidsAndFamilies.ContainsKey(guid))
                {
                    guidsAndFamilies[guid].Add(fam);
                }
                else
                {
                    guidsAndFamilies.Add(guid, new List<Family> { fam });
                }
            }

            if(guidsAndFamilies.Count == 0)
            {
                TaskDialog.Show("Ошибка", "В проекте нет семейств из библиотеки Bim-Starter");
                return Result.Failed;
            }

            Dictionary<string, List<Family>> duplicates = guidsAndFamilies
                .Where(i => i.Value.Count > 1)
                .ToDictionary(i => i.Key, i => i.Value);

            string guids = string.Join(",", guidsAndFamilies.Keys);

            WebConnection connect = new WebConnection(App.settings.Email, App.settings.Password, App.settings.Website);
            ServerResponse famInfoResponse = connect.Request("familygetinfo", new Dictionary<string, string>() { ["guid"] = guids });
            if (famInfoResponse.Statuscode >= 400)
            {
                message = famInfoResponse.Message;
                return Result.Failed;
            }

            Dictionary<string, FamilyCard> checkInfos = Newtonsoft.Json.JsonConvert
                .DeserializeObject<Dictionary<string, FamilyCard>>(famInfoResponse.Message);

            List<string[]> incorrectNames = new List<string[]>();
            List<string[]> obsoleteVersions = new List<string[]>();

            foreach (var kvp in checkInfos)
            {
                Family fam = guidsAndFamilies[kvp.Key][0];
                FamilyCard info = kvp.Value;
                if (info.exists == false)
                {
                    noGuids.Add(fam);
                    continue;
                }
                string famnameInProject = fam.Name;
                string famNameByServer = info.shortinfo.GetFamilyName();
                if (famnameInProject != famNameByServer)
                {
                    incorrectNames.Add(new[] { fam.Id.GetValue().ToString(), famnameInProject, famNameByServer, kvp.Key });
                }
                IEnumerable<ElementId> symbIds = fam.GetFamilySymbolIds();
                if (symbIds.Count() == 0)
                {
                    invalidSymbols.Add(fam);
                    continue;
                }
                FamilySymbol symb = doc.GetElement(fam.GetFamilySymbolIds().First()) as FamilySymbol;
                int versionInProject = symb.LookupParameter("RBS_VERSION").AsInteger();
                FamilyVersion lastVersion = info.GetLastActualVersion();
                int versionInLibrary = lastVersion.version;
                if (versionInLibrary > versionInProject)
                {
                    string[] obsoleteInfo = new[]
                    {
                        fam.Id.GetValue().ToString(),
                        fam.Name,
                        versionInProject.ToString(),
                        versionInLibrary.ToString(),
                        lastVersion.changes,
                        kvp.Key
                    };
                    obsoleteVersions.Add(obsoleteInfo);
                }
            }

            FormCheckFamilies form = new FormCheckFamilies(duplicates, obsoleteVersions, noGuids, incorrectNames);
            var formResult = form.ShowDialog();
            if (formResult == System.Windows.Forms.DialogResult.Yes)
            {
                int loadfamcount = 0;
                int errorcount = 0;
                string errorFamsNames = "";
                foreach (var kvp in obsoleteVersions)
                {
                    string famguid = kvp[5];
                    string famname = kvp[1];
                    Family loadedFam = EventLoadFamily.LoadFamily(connect, doc, famguid, famname);
                    if (loadedFam == null)
                    {
                        errorcount++;
                    }
                    else
                    {
                        loadfamcount++;
                        errorFamsNames += famname + " ";
                    }

                }
                string msg = "Успешно обновлено семейств: " + loadfamcount.ToString();
                if (errorcount > 0)
                {
                    msg += ", не удалось обновить: " + errorFamsNames;
                }

                TaskDialog.Show("Обновление семейств", msg);
            }

            return Result.Succeeded;
        }
    }
}
