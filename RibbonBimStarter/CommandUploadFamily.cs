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
using System.Linq;
using System.Collections.Generic;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Newtonsoft.Json;
#endregion

namespace RibbonBimStarter
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class CommandUploadFamily : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Trace.Listeners.Clear();
            Trace.Listeners.Add(new Logger("Uploadfamily"));
            string rfapath = string.Empty;
            using (System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog())
            {
                openFileDialog.Multiselect = false;
                openFileDialog.Filter = "Revit family file (*.rfa)|*.rfa";

                if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    rfapath = openFileDialog.FileName;
                else
                    return Result.Cancelled;
            }

            Document famDocForImage = commandData.Application.Application.OpenDocumentFile(rfapath);
            string famimage = FamilyImage.ImageCreate(famDocForImage);
            famDocForImage.Close(false);
            Document famdoc = commandData.Application.Application.OpenDocumentFile(rfapath);

            Family ownfam = famdoc.OwnerFamily;
            string categoryId = ownfam.FamilyCategoryId.GetValue().ToString();
            string hostId = FamilyFileInfo.GetHostTypeId(ownfam).ToString();

            WebConnection connect = new WebConnection(App.settings.Email, App.settings.Password, App.settings.Website);

            List<string> groups = new List<string>();
            ServerResponse responseGroups = connect.Request("loadgroups");
            if (responseGroups.Statuscode >= 400)
            {
                message = "Не удалось загрузить список групп семейств: " + responseGroups.Message;
                System.IO.File.Delete(famimage);
                famdoc.Close(false);
                return Result.Failed;
            }
            else
            {
                groups = JsonConvert.DeserializeObject<List<string>>(responseGroups.Message);
            }

            List<Family> nestedSharedFams = new FilteredElementCollector(famdoc)
                    .OfClass(typeof(Family))
                    .Cast<Family>()
                    .Where(f => !f.IsOwnerFamily)
                    .Where(f => f.get_Parameter(BuiltInParameter.FAMILY_SHARED).AsInteger() == 1)
                    .ToList();
            Dictionary<string, FamilySymbol> nestedGuids = new Dictionary<string, FamilySymbol>();
            List<string> nestedNoGuid = new List<string>();
            foreach (Family fam in nestedSharedFams)
            {
                if(fam.GetFamilySymbolIds().Count == 0)
                {
                    message = "Неиспользуемое вложенное общее семейство: : " + fam.Name;
                    System.IO.File.Delete(famimage);
                    famdoc.Close(false);
                    return Result.Failed;
                }
                FamilySymbol fsymb = famdoc.GetElement(fam.GetFamilySymbolIds().First()) as FamilySymbol;
                Parameter guidParam = fsymb.LookupParameter("RBS_GUID");
                if (guidParam == null || !guidParam.HasValue)
                {
                    nestedNoGuid.Add(fam.Name);
                }
                else
                {
                    string nestedFamGuid = guidParam.AsString();
                    nestedGuids.Add(nestedFamGuid, fsymb);
                }
            }
            if (nestedNoGuid.Count > 0)
            {
                message = "Обнаружены не подписанные вложенные общие семейства. " +
                    "Заранее загрузите их в библиотеку и обновите их в данном семействе. " +
                    "Имена семейств: " + Environment.NewLine + string.Join(Environment.NewLine, nestedNoGuid);
                famdoc.Close(false);
                System.IO.File.Delete(famimage);
                return Result.Failed;
            }
            List<string> obsoleteNestedFamNames = new List<string>();
            List<string> nestedGuidsWithVersion = new List<string>();
            if (nestedGuids.Count > 0)
            {
                nestedGuidsWithVersion = new List<string>();
                string nestedGuidsString = String.Join(",", nestedGuids.Keys);
                ServerResponse responseNested = connect.Request("familygetinfo", new Dictionary<string, string>() { ["guid"] = nestedGuidsString });
                if (responseNested.Statuscode >= 400)
                {
                    message = "Error check nested families " + responseNested.ToString();
                    famdoc.Close(false);
                    System.IO.File.Delete(famimage);
                    return Result.Failed;
                }

                Dictionary<string, FamilyCard> nestedInfos = null;

                try
                {
                    var nestedJsonObj = JsonConvert.DeserializeObject(responseNested.Message);
                    nestedInfos = JsonConvert.DeserializeObject<Dictionary<string, FamilyCard>>(responseNested.Message);
                }
                catch
                {
                    message = responseNested.Message;
                    famdoc.Close(false);
                    System.IO.File.Delete(famimage);
                    return Result.Failed;
                }

                foreach (var kvp in nestedInfos)
                {
                    string curNestedGuid = kvp.Key;
                    FamilyCard nestedInfo = kvp.Value;
                    FamilySymbol nestedFamSymb = nestedGuids[curNestedGuid];
                    string nestedFamilyName = nestedFamSymb.FamilyName;
                    Parameter nestedVersionParam = nestedFamSymb.LookupParameter("RBS_VERSION");
                    if (nestedVersionParam == null)
                    {
                        message = "Нет параметра RBS_VERSION в семействе " + nestedFamSymb.FamilyName;
                        famdoc.Close(false);
                        System.IO.File.Delete(famimage);
                        return Result.Failed;
                    }
                    int nestedVersion = nestedVersionParam.AsInteger();
                    int serverVersion = nestedInfo.GetLastActualVersionNumber();
                    if (nestedVersion < serverVersion)
                    {
                        obsoleteNestedFamNames.Add(nestedFamilyName);
                    }
                    if (nestedVersion > serverVersion)
                    {
                        message = "Некорректная версия вложенного семейства! Обновите из библиотеки: " + nestedFamilyName;
                        famdoc.Close(false);
                        System.IO.File.Delete(famimage);
                        return Result.Failed;
                    }

                    FamilyShortInfo fsi = nestedInfo.shortinfo;
                    nestedGuidsWithVersion.Add(curNestedGuid + "_" + nestedVersion.ToString());
                    string nestedNameFromServer = fsi.GetFamilyName();
                    if (nestedNameFromServer != nestedFamilyName)
                    {
                        using (Transaction renameNestedTransaction = new Transaction(famdoc))
                        {
                            renameNestedTransaction.Start("Rename");
                            nestedFamSymb.Family.Name = nestedNameFromServer;
                            Debug.WriteLine("Incorrect nested fam name, renamed from " + nestedFamilyName + " to " + nestedNameFromServer);
                            renameNestedTransaction.Commit();
                        }
                    }
                }
            }

            if (obsoleteNestedFamNames.Count > 0)
            {
                message = "Вложенные семейства в данном семействе устарели! Обновите из библиотеки: "
                    + String.Join(",", obsoleteNestedFamNames);
                famdoc.Close(false);
                System.IO.File.Delete(famimage);
                return Result.Failed;
            }

            UploadOption uploadOption = UploadOption.FirstLoad;
            string resultUrl = string.Empty;
            Dictionary<string, string> parentUrls = new Dictionary<string, string>();

            FamilyCard info = null;
            FamilyManager fman = famdoc.FamilyManager;
            FamilyParameter guidparam = fman.get_Parameter("RBS_GUID");
            if (guidparam != null) //в семействе есть прописанный guid - скорее всего, семейство уже ранее загружалось в библиотеку
            {
                string existedGuid = guidparam.Formula.Trim('"');
                //Guid.TryParse(existedGuid)
                if (existedGuid.Length != 36)
                {
                    message = "Incorrect GUID: " + existedGuid;
                    famdoc.Close(false);
                    System.IO.File.Delete(famimage);
                    return Result.Failed;
                }

                var requestDataForExistedGuid = new Dictionary<string, string>() { ["guid"] = existedGuid, ["getnesting"] = "true" };
                ServerResponse sr = connect.Request("familygetinfo", requestDataForExistedGuid);
                if (sr.Statuscode >= 400)
                {
                    message = sr.ToString();
                    famdoc.Close(false);
                    System.IO.File.Delete(famimage);
                    return Result.Failed;
                }

                Dictionary<string, FamilyCard> checkInfos = null;
                try
                {
                    checkInfos = JsonConvert.DeserializeObject<Dictionary<string, FamilyCard>>(sr.Message);
                }
                catch { }
                if (checkInfos == null || checkInfos.Count == 0)
                {
                    message = "Не удалось получить информацию о семействе";
                    famdoc.Close(false);
                    System.IO.File.Delete(famimage);
                    return Result.Failed;
                }
                info = checkInfos[existedGuid];

                if (info.exists == false)
                {
                    //guid прописан, но его нет в библиотеке
                    message = "Некорректный GUID. Удалите из семейства параметр RBS_GUID и загрузите семейство повторно";
                    famdoc.Close(false);
                    return Result.Failed;
                }

                if (int.Parse(App.revitVersion) > int.Parse(info.shortinfo.revitversion))
                {
                    message = "Версия Revit выше, чем у версии этого семейства в библиотеке. Удалите RBS параметры и загрузите семейство как новое";
                    famdoc.Close(false);
                    System.IO.File.Delete(famimage);
                    return Result.Failed;
                }

                //такой guid есть в библиотеке, надо проверить номер версии - должен быть такой же как в библиотеке
                FamilyParameter versionParam = fman.get_Parameter("RBS_VERSION");
                if (versionParam == null)
                {
                    message = "Нет параметра RBS_VERSION. Обновите семейство из библиотеки";
                    famdoc.Close(false);
                    return Result.Failed;
                }
                int curVersion = int.Parse(versionParam.Formula.Trim('"'));
                int lastActualServerVersion = info.GetLastActualVersionNumber();

                if (curVersion < lastActualServerVersion)
                {
                    message = "Семейство устарело! В библиотеке есть более свежая версия: " +
                         App.settings.Website + "family?guid=" + existedGuid;
                    famdoc.Close(false);
                    System.IO.File.Delete(famimage);
                    return Result.Failed;
                }
                if (curVersion > lastActualServerVersion)
                {
                    message = "Некорректная версия семейства! Скачайте актуальную версию из библиотеки";
                    famdoc.Close(false);
                    System.IO.File.Delete(famimage);
                    return Result.Failed;
                }

                FormUploadReplaceOrNewFamily formSelectUploadType = new FormUploadReplaceOrNewFamily(existedGuid);
                if (formSelectUploadType.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                {
                    famdoc.Close(false);
                    System.IO.File.Delete(famimage);
                    return Result.Cancelled;
                }

                if (formSelectUploadType.selectedOption == UploadOption.AddVersion)
                {
                    foreach (FamilyShortInfo parentCard in info.parent)
                    {
                        parentUrls.Add(parentCard.name, App.settings.Website + "family?guid=" + parentCard.guid);
                    }

                    uploadOption = UploadOption.AddVersion;
                    int nextVersionNumber = info.GetLastVersionNumber() + 1;
                    using (Transaction t = new Transaction(famdoc))
                    {
                        t.Start("Write version number");
                        fman.SetFormula(versionParam, nextVersionNumber.ToString());
                        t.Commit();
                    }
                    famdoc.Close(true);

                    ServerResponse responseNewVersion = connect.UploadFamilyVersion(
                        existedGuid,
                        nextVersionNumber,
                        formSelectUploadType.versionDescription,
                        rfapath,
                        nestedGuidsWithVersion);

                    if (responseNewVersion.Statuscode >= 400)
                    {
                        message = "Error " + sr.Statuscode + ". " + sr.Message;
                        System.IO.File.Delete(famimage);
                        return Result.Failed;

                    }
                    resultUrl = App.settings.Website + responseNewVersion.Message;

                }
                else
                {
                    uploadOption = UploadOption.LoadAsNew;
                }
            }


            if (uploadOption == UploadOption.FirstLoad || uploadOption == UploadOption.LoadAsNew)
            {
                string defaultFamName = info == null ? "" : info.shortinfo.name;
                string defaultDescription = info == null ? "" : info.shortinfo.description;
                FormAddFamily formAdd = new FormAddFamily(defaultFamName, defaultDescription, famimage, groups);
                if (formAdd.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                {
                    famdoc.Close(false);
                    System.IO.File.Delete(famimage);
                    return Result.Cancelled;
                }

                string newGuid = Guid.NewGuid().ToString();
                using (Transaction t = new Transaction(famdoc))
                {
                    t.Start("Add rbs parameters");

                    FamilyParameter guidParam = null;
                    FamilyParameter versionParam = null;
                    if (uploadOption == UploadOption.FirstLoad)
                    {

#if R2017 || R2018 || R2019 || R2020 || R2021
                        guidParam = fman.AddParameter("RBS_GUID", BuiltInParameterGroup.PG_IDENTITY_DATA, ParameterType.Text, false);
                        versionParam = fman.AddParameter("RBS_VERSION", BuiltInParameterGroup.PG_IDENTITY_DATA, ParameterType.Integer, false);
#else
                        guidParam = fman.AddParameter("RBS_GUID", GroupTypeId.IdentityData, SpecTypeId.String.Text, false);
                        versionParam = fman.AddParameter("RBS_VERSION", GroupTypeId.IdentityData, SpecTypeId.String.Text, false);
#endif
                    }
                    else
                    {
                        guidParam = fman.get_Parameter("RBS_GUID");
                        versionParam = fman.get_Parameter("RBS_VERSION");
                    }
                    fman.SetFormula(guidParam, "\"" + newGuid + "\"");
                    fman.SetFormula(versionParam, "1");

                    t.Commit();
                }

                famdoc.Close(true);

                string jpg300path = FamilyImage.FitImageInSquare(formAdd.Imagepath, 300, 90);
                string jpg140path = FamilyImage.FitImageInSquare(formAdd.Imagepath, 140, 90);

                ServerResponse sr = connect.UploadFamily(
                    formAdd.FamilyName,
                    newGuid,
                    formAdd.Group,
                    categoryId,
                    hostId,
                    formAdd.Description,
                    formAdd.Munufacturer,
                    formAdd.Document,
                    rfapath,
                    jpg300path,
                    jpg140path,
                    nestedGuidsWithVersion
                );

                System.IO.File.Delete(jpg300path);
                System.IO.File.Delete(jpg140path);

                if (sr.Statuscode >= 400)
                {
                    message = "Error " + sr.Statuscode + ". " + sr.Message;
                    System.IO.File.Delete(famimage);
                    return Result.Failed;
                }
                resultUrl = App.settings.Website + sr.Message.Trim('"').Trim('/');
            }

            System.IO.File.Delete(famimage);

            FormFamilyUploadResult formSuccess = new FormFamilyUploadResult(resultUrl, parentUrls);
            formSuccess.ShowDialog();
            return Result.Succeeded;
        }
    }

}
