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

        public static WriteLinkSettings LoadAllValues(ExternalCommandData commandData, out string message, Save save)
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
            List<MyParameterValue> sheetParameters = GetParameterValues(openedSheet, save.SheetParameters);

            List<MyParameterValue> titleblockParams = GetParameterValues(titleBlock, save.TitleblockParameters);

            ElementType titleblockType = mainDoc.GetElement(titleBlock.GetTypeId()) as ElementType;
            List<MyParameterValue> typeParameters = GetParameterValues(titleblockType, save.TypeParameters);

            ProjectInfo pi = mainDoc.ProjectInformation;
            List<MyParameterValue> projectParameters = GetParameterValues(pi, save.ProjectParameters);

            WriteLinkSettings wls = new WriteLinkSettings(sheetParameters, titleblockParams, typeParameters, projectParameters);

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

#if R2017 || R2018 || R2019 || R2020 || R2021 || R2022 || R2023
            bool isCategoryTitleblock = selElem.Category.Id.IntegerValue == (int)BuiltInCategory.OST_TitleBlocks;
#else
            bool isCategoryTitleblock = selElem.Category.BuiltInCategory == BuiltInCategory.OST_TitleBlocks;
#endif
            if (!isCategoryTitleblock) return null;

            FamilyInstance fi = selElem as FamilyInstance;
            return fi;
        }

        private static List<MyParameterValue> GetParameterValues(Element elem, IEnumerable<string> enabledParams)
        {
            List<MyParameterValue> values = new List<MyParameterValue>();
            foreach (Parameter p in elem.Parameters)
            {
                string paramName = p.Definition.Name;
                MyParameterValue mpv = new MyParameterValue(p);
                if (mpv.IsNull || !mpv.IsValid) continue;

                if (enabledParams != null && enabledParams.Contains(paramName))
                {
                    mpv.IsEnabled = true;
                }

                values.Add(mpv);
            }
            return values.OrderBy(i => i.sourceParameter.Definition.Name).ToList();
        }
    }
}