using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Tools.LinksManager
{
    public abstract class MyRevitDocument : IComparable<MyRevitDocument>, IEquatable<MyRevitDocument>
    {

        public Document Doc;
        public bool SaveAfterClosing = false;
        public List<MySheet> Sheets;
        public string Name;
        public List<FamilyInstance> AllTitleblocksInDocument;
        public bool NeedLoadSheets = false;
        public int SelectedSheetsCount
        {
            get
            {
                if (Sheets == null) return 0;
                return Sheets.Count(i => i.IsPrintable);
            }
        }

        public MyRevitDocument(Document doc, bool loadSheets, Selection selectedSheets = null)
        {
            Trace.WriteLine($"Create MyRevitDocument: {doc.Title}");
            Doc = doc;
            Name = MyRevitDocument.GetDocNameWithoutRvt(doc);

            NeedLoadSheets = loadSheets;
            if (loadSheets)
            {
                LoadSheets(selectedSheets);
            }
        }

        public override string ToString()
        {
            return Name;
        }

        public static string GetDocNameWithoutRvt(Document doc)
        {
            string title = doc.Title;
            if (title.EndsWith(".rvt")) title = title.Substring(0, title.Length - 4);
            return title;
        }


        internal void LoadSheets(Selection sel = null)
        {
            Sheets = new FilteredElementCollector(Doc)
                    .WhereElementIsNotElementType()
                    .OfClass(typeof(ViewSheet))
                    .Cast<ViewSheet>()
                    .Where(i => !i.IsPlaceholder)
                    .Select(i => new MySheet(i))
                    .ToList();
            Sheets.Sort();

            Trace.WriteLine($"Sheets found: {Sheets.Count}");

            CheckSelectedSheets(sel);
            LoadTitleblocks();
        }

        internal void CheckSelectedSheets(Selection sel = null)
        {
            if (sel == null) return;
            List<ElementId> selIds = sel.GetElementIds().ToList();
            if (selIds.Count == 0) return;

            foreach (MySheet sheet in Sheets)
            {
                if (selIds.Contains(sheet.sheet.Id))
                {
                    sheet.IsPrintable = true;
                }
            }

            return;
        }

        internal void LoadTitleblocks()
        {
            //список основных надписей нужен потому, что размеры листа хранятся в них
            //могут быть примечания, сделанные Основной надписью, надо их отфильровать, поэтому >0.6
            AllTitleblocksInDocument = new FilteredElementCollector(Doc)
                    .WhereElementIsNotElementType()
                    .OfCategory(BuiltInCategory.OST_TitleBlocks)
                    .Cast<FamilyInstance>()
                    .Where(t => t.get_Parameter(BuiltInParameter.SHEET_HEIGHT).AsDouble() > 0.6)
                    .ToList();
            Trace.WriteLine($"Titleblocks found in model: {AllTitleblocksInDocument.Count}");
        }

        public int CompareTo(MyRevitDocument other)
        {
            return this.Name.CompareTo(other.Name);
        }

        public bool Equals(MyRevitDocument other)
        {
            return this.Name.Equals(other.Name);
        }
    }
}
