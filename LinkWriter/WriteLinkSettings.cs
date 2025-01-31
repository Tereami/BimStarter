using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System.Collections.Generic;
using System.Linq;

namespace LinkWriter
{
    public class WriteLinkSettings
    {
        public List<MyParameterValue> SheetParams;
        public List<MyParameterValue> ProjectParams;
        public List<MyParameterValue> TitleblockParams;
        public List<MyParameterValue> TypeParams;

        public WriteLinkSettings()
        {

        }

        public WriteLinkSettings(
            List<MyParameterValue> sheetParameters,
            List<MyParameterValue> titleblockParams,
            List<MyParameterValue> typeParameters,
            List<MyParameterValue> projectParameters)
        {
            SheetParams = sheetParameters;
            ProjectParams = projectParameters;
            TitleblockParams = titleblockParams;
            TypeParams = typeParameters;
        }

        public static WriteLinkSettings LoadAllValues(ExternalCommandData commandData, out string message)
        {
            Document mainDoc = commandData.Application.ActiveUIDocument.Document;
            FamilyInstance titleBlock = getTitleblockIsSelected(commandData.Application.ActiveUIDocument);
            if (titleBlock == null)
            {
                message = "Please select a titleblock to copy properties";
                return null;
            }

            ViewSheet openedSheet = mainDoc.ActiveView as ViewSheet;
            if (openedSheet == null)
            {
                message = "Please open a sheet to copy parameters";
                return null;
            }
            List<MyParameterValue> sheetParameters = GetParameterValues(openedSheet);

            List<MyParameterValue> titleblockParams = GetParameterValues(titleBlock);

            ElementType titleblockType = mainDoc.GetElement(titleBlock.GetTypeId()) as ElementType;
            List<MyParameterValue> typeParameters = GetParameterValues(titleblockType);

            ProjectInfo pi = mainDoc.ProjectInformation;
            List<MyParameterValue> projectParameters = GetParameterValues(pi);

            WriteLinkSettings wls = new WriteLinkSettings(sheetParameters, projectParameters, titleblockParams, typeParameters);

            message = string.Empty;
            return wls;
        }

        private static FamilyInstance getTitleblockIsSelected(UIDocument uidoc)
        {
            Selection sel = uidoc.Selection;
            if (sel == null) return null;
            if (sel.GetElementIds().Count != 1) return null;
            List<ElementId> selIds = sel.GetElementIds().ToList();
            Element selElem = uidoc.Document.GetElement(selIds[0]);
            if (!(selElem is FamilyInstance)) return null;

            bool isCategoryTitleblock = selElem.Category.Id.IntegerValue == (int)BuiltInCategory.OST_TitleBlocks;
            if (!isCategoryTitleblock) return null;

            FamilyInstance fi = selElem as FamilyInstance;
            return fi;
        }

        private static List<MyParameterValue> GetParameterValues(Element elem)
        {
            List<MyParameterValue> values = new List<MyParameterValue>();
            foreach (Parameter p in elem.Parameters)
            {
                string paramName = p.Definition.Name;
                MyParameterValue mpv = new MyParameterValue(p);
                if (mpv.IsNull || !mpv.IsValid) continue;
                values.Add(mpv);
            }
            return values.OrderBy(i => i.sourceParameter.Definition.Name).ToList();
        }

    }
}
