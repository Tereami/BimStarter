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

using Autodesk.Revit.DB;
using System.Collections.Generic;
using System.Diagnostics;

namespace RevitGraphicsOverride
{
    public static class SupportGraphics
    {
        public static string HaveOverrides(OverrideGraphicSettings ogs)
        {
#if R2017 || R2018 || R2019 || R2020 || R2021 || R2022 || R2023
            ElementId invalidId = new ElementId(OverrideGraphicSettings.InvalidPenNumber);
#else
            ElementId invalidId = new ElementId((long)OverrideGraphicSettings.InvalidPenNumber);
#endif

            List<string> messagesList = new List<string>();

            if (ogs.Halftone == true)
                messagesList.Add(MyStrings.Halftones);

            if (ogs.ProjectionLineWeight != -1)
                messagesList.Add(MyStrings.ProjectionLineThickness);

            if (ogs.ProjectionLineColor.IsValid)
                messagesList.Add(MyStrings.ProjectionLineColor);

            if (ogs.ProjectionLinePatternId != invalidId)
                messagesList.Add(MyStrings.ProjectionLinePattern);

#if R2017 || R2018
            if (ogs.IsProjectionFillPatternVisible == false)
                messagesList.Add(MyStrings.SurfaceHatchDisable);

            if (ogs.ProjectionFillColor.IsValid)
                messagesList.Add(MyStrings.SurfaceHatchColor);

            if (ogs.ProjectionFillPatternId != invalidId)
                messagesList.Add(MyStrings.SurfaceHatchPattern);

            if (ogs.IsCutFillPatternVisible == false)
                messagesList.Add(MyStrings.SectionHatchDisable);

            if (ogs.CutFillColor.IsValid)
                messagesList.Add(MyStrings.SectionHatchColor);

            if (ogs.CutFillPatternId != invalidId)
                messagesList.Add(MyStrings.SectionHatchPattern);

#else
            if (ogs.IsSurfaceForegroundPatternVisible == false)
                messagesList.Add(MyStrings.SurfaceForegroundHatchDisable);
            if (ogs.IsSurfaceBackgroundPatternVisible == false)
                messagesList.Add(MyStrings.SurfaceBackgroundHatchDisable);

            if (ogs.SurfaceForegroundPatternColor.IsValid)
                messagesList.Add(MyStrings.SurfaceForegroundHatchColor);
            if (ogs.SurfaceBackgroundPatternColor.IsValid)
                messagesList.Add(MyStrings.SurfaceBackgroundHatchColor);

            if (ogs.SurfaceForegroundPatternId != invalidId)
                messagesList.Add(MyStrings.SurfaceForegroundHatchPattern);
            if (ogs.SurfaceBackgroundPatternId != invalidId)
                messagesList.Add(MyStrings.SurfaceBackgroundHatchPattern);

            if (ogs.IsCutForegroundPatternVisible == false)
                messagesList.Add(MyStrings.SectionForegroundHatchDisable);
            if (ogs.IsCutBackgroundPatternVisible == false)
                messagesList.Add(MyStrings.SectionBackgroundHatchDisable);

            if (ogs.CutForegroundPatternColor.IsValid)
                messagesList.Add(MyStrings.SectionForegroundHatchColor);
            if (ogs.CutBackgroundPatternColor.IsValid)
                messagesList.Add(MyStrings.SectionBackgroundHatchColor);

            if (ogs.CutForegroundPatternId != invalidId)
                messagesList.Add(MyStrings.SectionForegroundHatchPattern);
            if (ogs.CutBackgroundPatternId != invalidId)
                messagesList.Add(MyStrings.SectionBackgroundHatchPattern);
#endif

            if (ogs.Transparency != 0)
                messagesList.Add(MyStrings.Transparency);


            if (ogs.CutLineWeight != -1)
                messagesList.Add(MyStrings.SectionLineThickness);

            if (ogs.CutLineColor.IsValid)
                messagesList.Add(MyStrings.SectionLineColor);

            if (ogs.CutLinePatternId != invalidId)
                messagesList.Add(MyStrings.SectionLinePattern);

            string msg = string.Join(", ", messagesList);
            return msg;
        }


        public static List<OverridenResult> getOverridenElemsOnView(Document doc, View curView)
        {
            List<OverridenResult> overridenElems = new List<OverridenResult>();
            Trace.WriteLine($"Find overrides on view: {curView.Name}");

            FilteredElementCollector col = new FilteredElementCollector(doc, curView.Id);
            foreach (Element elem in col)
            {
                if (elem.Category == null) continue;
                OverrideGraphicSettings ogs = curView.GetElementOverrides(elem.Id);

                string msg = SupportGraphics.HaveOverrides(ogs);

                if (msg != "")
                {
                    OverridenResult ores = new OverridenResult(elem, curView, ogs, msg);
                    overridenElems.Add(ores);
                    Trace.WriteLine($"Overrides found for elem id: {elem.Id}");
                }
            }
            Trace.WriteLine($"Overriden elems found: {overridenElems.Count}");
            return overridenElems;
        }
    }
}
