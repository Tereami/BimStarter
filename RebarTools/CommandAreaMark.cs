#region License
//Данный код опубликован под лицензией Creative Commons Attribution-NonCommercial-ShareAlike.
//Разрешено использовать, распространять, изменять и брать данный код за основу для производных в некоммерческих целях,
//при условии указания авторства и если производные лицензируются на тех же условиях.
//Код поставляется "как есть". Автор не несет ответственности за возможные последствия использования.
//Зуев Александр, 2019, все права защищены.
//This code is listed under the Creative Commons Attribution-NonCommercial-ShareAlike license.
//You may use, redistribute, remix, tweak, and build upon this work non-commercially, 
//as long as you credit the author by linking back and license your new creations under the same terms.
//This code is provided 'as is'. Author disclaims any implied warranty. 
//Zuev Aleksandr, 2019, all rigths reserved.
#endregion
#region Usings
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
#endregion

namespace RebarTools
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    class CommandAreaMark : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Trace.Listeners.Clear();
            Trace.Listeners.Add(new Tools.Logger.Logger("AreaRebarMark"));
            Trace.WriteLine("Start area rebar mark");
            double offset = 0; //мм

            //собираю все элементы армирования по площади и траектории
            Document doc = commandData.Application.ActiveUIDocument.Document;
            FilteredElementCollector areas = new FilteredElementCollector(doc)
                .OfClass(typeof(Autodesk.Revit.DB.Structure.AreaReinforcement))
                .WhereElementIsNotElementType();
            FilteredElementCollector paths = new FilteredElementCollector(doc)
                .OfClass(typeof(Autodesk.Revit.DB.Structure.PathReinforcement))
                .WhereElementIsNotElementType();
            List<Element> col = areas.Concat(paths).ToList();
            int count = 0;
            using (Transaction t = new Transaction(doc))
            {
                t.Start("Area rebar mark");
                //и Path, и Area попытаюсь обрабатываю в одном цикле, чтобы компактнее было
                foreach (Element rebarAreaPath in col)
                {
                    //благодаря hashset можно собрать только уникальный марки стержней
                    HashSet<string> marks = new HashSet<string>();

                    List<ElementId> rebarIds = null;
                    List<ElementId> curveIds = null;

                    double maxZ = 0;
                    double minZ = 0;

                    if (rebarAreaPath is AreaReinforcement)
                    {
                        AreaReinforcement ar = rebarAreaPath as AreaReinforcement;
                        rebarIds = ar.GetRebarInSystemIds().ToList();
                        curveIds = ar.GetBoundaryCurveIds().ToList();

                        //определяю нижнюю и верзнюю точку зоны
                        List<double> zs = new List<double>();
                        foreach (ElementId curveId in curveIds)
                        {
                            AreaReinforcementCurve curve =
                                doc.GetElement(curveId) as AreaReinforcementCurve;

                            XYZ start = curve.Curve.GetEndPoint(0);
                            double z1 = start.Z;
                            zs.Add(z1);

                            XYZ end = curve.Curve.GetEndPoint(1);
                            double z2 = end.Z;
                            zs.Add(z2);
                        }

                        maxZ = zs.Max();
                        maxZ += offset / 304.8;
                        minZ = zs.Min();
                        minZ += offset / 304.8;
                    }

                    if (rebarAreaPath is PathReinforcement)
                    {
                        PathReinforcement pr = rebarAreaPath as PathReinforcement;
                        rebarIds = pr.GetRebarInSystemIds().ToList();

                        maxZ = pr.get_BoundingBox(doc.ActiveView).Max.Z;
                        minZ = pr.get_BoundingBox(doc.ActiveView).Min.Z;
                    }

                    //получаю общие параметры, в которые буду записывать отметку верха и низа, они должны быть заранее добавлены
                    Parameter topelev = rebarAreaPath.LookupParameter("АрмПлощ.ОтмВерха");
                    Parameter botelev = rebarAreaPath.LookupParameter("АрмПлощ.ОтмНиза");

                    if (topelev == null)
                    {
                        topelev = rebarAreaPath.LookupParameter("Рзм.ОтмВерха");
                    }
                    if (botelev == null)
                    {
                        botelev = rebarAreaPath.LookupParameter("Рзм.ОтмНиза");
                    }

                    if (topelev != null && botelev != null)
                    {
                        topelev.Set(maxZ);
                        botelev.Set(minZ);
                    }

                    //еще хочу записать в зону длину самого длинного арматурного стержня в ней
                    double length = 0;

                    //обрабатываю арматурные стержни в составе зоны 
                    foreach (ElementId rebarId in rebarIds)
                    {
                        Element rebar = doc.GetElement(rebarId);
                        string rebarMark = rebar.get_Parameter(BuiltInParameter.ALL_MODEL_MARK).AsString();
                        marks.Add(rebarMark);
                        double tempLength = rebar.get_Parameter(BuiltInParameter.REBAR_ELEM_LENGTH).AsDouble();
                        if (tempLength > length)
                            length = tempLength;
                    }

                    //сортирую марки и записываю через запятую
                    List<string> marksList = marks.ToList();
                    marksList.Sort();
                    string rebarSystemMark = string.Join(", ", marksList);

                    rebarAreaPath.get_Parameter(BuiltInParameter.ALL_MODEL_MARK).Set(rebarSystemMark);

                    //записываю длину арматуры а зону
                    Parameter lengthParam = rebarAreaPath.LookupParameter("Рзм.Длина");
                    if (lengthParam != null)
                    {
                        lengthParam.Set(length);
                    }

                    count++;
                }
                t.Commit();
            }

            Tools.Forms.BalloonTip.Show("Success!", "Zones processed: " + count.ToString());
            Trace.WriteLine("Success, elements: " + count.ToString());

            return Result.Succeeded;
        }
    }
}