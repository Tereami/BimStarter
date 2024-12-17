using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Tools.LinksManager
{
    public class MyRevitMainDocument : MyRevitDocument
    {
        public MyRevitMainDocument(Document doc, bool loadSheets, Selection selectedSheets = null) : base(doc, loadSheets, selectedSheets)
        {
        }

        public List<MyRevitLinkDocument> GetLinkDocuments()
        {
            List<MyRevitLinkDocument> linkDocuments = new List<MyRevitLinkDocument>();

            List<RevitLinkInstance> linkInstances = new FilteredElementCollector(Doc)
                .WhereElementIsNotElementType()
                .OfClass(typeof(RevitLinkInstance))
                .Cast<RevitLinkInstance>()
                .ToList();

            List<RevitLinkType> linkTypes = new FilteredElementCollector(Doc)
                .WhereElementIsElementType()
                .OfClass(typeof(RevitLinkType))
                .Cast<RevitLinkType>()
                .Where(i => !i.IsNestedLink && !i.LocallyUnloaded)
                .ToList();

            foreach (RevitLinkType linkType in linkTypes)
            {
                Trace.WriteLine($"Try to get document from link {linkType.Name}");
                RevitLinkInstance linkInstance = linkInstances.FirstOrDefault(i => i.GetTypeId() == linkType.Id);
                if (linkInstance == null) continue;
                Document linkDoc = linkInstance.GetLinkDocument();
                if (linkDoc == null) continue;

                MyRevitLinkDocument myLinkDoc = new MyRevitLinkDocument(linkDoc, true);
                myLinkDoc.LinkInstance = linkInstance;
                myLinkDoc.LinkType = linkType;
                linkDocuments.Add(myLinkDoc);
            }
            return linkDocuments;
        }
    }
}
