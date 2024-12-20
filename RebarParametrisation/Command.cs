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
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Collections.Generic;
using System.Linq;
#endregion


namespace RebarParametrisation
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class Command : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Document mainDoc = commandData.Application.ActiveUIDocument.Document;
            Autodesk.Revit.ApplicationServices.Application revitApp = commandData.Application.Application;

            Tools.SettingsSaver.Saver<RebarParametrisationSettings> saver = new Tools.SettingsSaver.Saver<RebarParametrisationSettings>();
            RebarParametrisationSettings sets = saver.Activate(nameof(RebarParametrisation));

            //открываю окно выбора параметров, которые буду заполняться
            FormSelectParams formSelectParams = new FormSelectParams(sets);
            if (formSelectParams.ShowDialog() != System.Windows.Forms.DialogResult.OK) return Result.Cancelled;

            sets = formSelectParams.userSettings;

            RebarDocumentWorker mainWorker = new RebarDocumentWorker();
            List<Element> mainDocConcreteElements = new List<Element>();



            string mainWorkerMessage = mainWorker.Start(mainDoc, revitApp, Transform.Identity, out mainDocConcreteElements, sets);
            if (!string.IsNullOrEmpty(mainWorkerMessage))
            {
                message = $"{mainWorkerMessage}. Документ {mainDoc.Title}";
                return Result.Failed;
            }

            if (sets.LinkFilesSetting == ProcessedLinkFiles.NoLinks)
                return Result.Succeeded;

            List<RevitLinkInstance> linksAll = new FilteredElementCollector(mainDoc)
                .OfClass(typeof(RevitLinkInstance))
                .Cast<RevitLinkInstance>()
                .ToList();

            List<RevitLinkInstance> linksLib = linksAll
                .Where(i => i.Name.Contains(".lib"))
                .ToList();

            // имя ссылки lib и список конструкций, которые она пересекает
            Dictionary<string, List<Element>> hostElemsForLibLinks = new Dictionary<string, List<Element>>();

            foreach (RevitLinkInstance rli in linksLib)
            {
                string linkInstanceTitle = Tools.LinksManager.Extensions.GetDocumentTitleFromLinkInstance(rli);
                Autodesk.Revit.DB.View defaultView = Tools.Model.ViewUtils.GetDefaultView(mainDoc);
                Element hostElem = Tools.LinksManager.LibFilesTools.GetConcreteElementIsHostForLibLinkFile(mainDoc, defaultView, mainDocConcreteElements, rli);
                if (hostElem == null) continue;
                if (hostElemsForLibLinks.ContainsKey(linkInstanceTitle))
                {
                    hostElemsForLibLinks[linkInstanceTitle].Add(hostElem);
                }
                else
                {
                    hostElemsForLibLinks.Add(linkInstanceTitle, new List<Element> { hostElem });
                }
            }
            List<RevitLinkInstance> linksWithoutDuplicates = Tools.LinksManager.Extensions.DeleteDuplicates(linksAll);

            foreach (RevitLinkInstance rli in linksWithoutDuplicates)
            {
                RevitLinkType rlt = mainDoc.GetElement(rli.GetTypeId()) as RevitLinkType;
                Document linkDoc = rli.GetLinkDocument();
                if (linkDoc == null) continue;

                string linkDocTitle = Tools.LinksManager.Extensions.GetDocumentTitleFromLinkInstance(rli);
                if (!linkDocTitle.Contains("-КР-")) continue;
                if (!linkDocTitle.Contains("lib") && sets.LinkFilesSetting == ProcessedLinkFiles.OnlyLibs) continue;



                ModelPath mPath = linkDoc.GetWorksharingCentralModelPath();

                rlt.Unload(new SaveCoordinates());

                OpenOptions oo = new OpenOptions();

                linkDoc = revitApp.OpenDocumentFile(mPath, oo);


                RebarDocumentWorker linkWorker = new RebarDocumentWorker();
                if (linkDocTitle.Contains("lib"))
                {
                    if (hostElemsForLibLinks.ContainsKey(linkDocTitle))
                    {
                        List<Element> mainElemsForLib = hostElemsForLibLinks[linkDocTitle];
                        if (mainElemsForLib.Count > 0)
                        {
                            linkWorker.MainElementsForLibFile = mainElemsForLib;
                        }
                    }
                }
                Transform linkTransform = rli.GetTransform();
                List<Element> linkConcreteElements = new List<Element>();
                string linkWorkerMessage = linkWorker.Start(linkDoc, revitApp, linkTransform, out linkConcreteElements, sets);
                if (!string.IsNullOrEmpty(linkWorkerMessage))
                {
                    message = linkWorkerMessage + ". Связь " + linkDoc.Title;
                    return Result.Failed;
                }

                TransactWithCentralOptions transOpt = new TransactWithCentralOptions();
                SynchronizeWithCentralOptions syncOpt = new SynchronizeWithCentralOptions();

                RelinquishOptions relOpt = new RelinquishOptions(true);
                syncOpt.SetRelinquishOptions(relOpt);

                linkDoc.SynchronizeWithCentral(transOpt, syncOpt);

                linkDoc.Close();
#if R2017 
                RevitLinkLoadResult rllr = rlt.Reload();
#else
                LinkLoadResult llr = rlt.Reload();
#endif
            }

            saver.Save(sets);
            return Result.Succeeded;
        }
    }

    public class SaveCoordinates : ISaveSharedCoordinatesCallback
    {
        public SaveModifiedLinksOptions GetSaveModifiedLinksOption(RevitLinkType link)
        {
            return SaveModifiedLinksOptions.DoNotSaveLinks;
        }
    }
}