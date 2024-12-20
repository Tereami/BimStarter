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
using Autodesk.Revit.DB.Structure;
using System.Collections.Generic;
#endregion

namespace Tools.LinksManager
{
    public static class LibFilesTools
    {
        public static Element GetConcreteElementIsHostForLibLinkFile(Document mainDoc, View mainDocView, List<Element> mainDocAllConcreteElems, RevitLinkInstance linkInstance)
        {
            Element hostElem = null;

            //получаю из связи все арматурные стержни
            Document linkDoc = linkInstance.GetLinkDocument();
            FilteredElementCollector linkRebars = new FilteredElementCollector(linkDoc)
                .WhereElementIsNotElementType()
                .OfCategory(BuiltInCategory.OST_Rebar);
            Transform linkTransform = linkInstance.GetTransform();

            //проверяю, с какими элементами в основном файле они пересекаются
            List<Element> resultMainHostElements = new List<Element>();

            foreach (Element curHost in mainDocAllConcreteElems)
            {
                foreach (Element rebar in linkRebars)
                {
                    bool checkIntersection = false;
                    if (rebar is Rebar)
                    {
                        Rebar bar = rebar as Rebar;
                        checkIntersection = Tools.Model.RebarTools.RebarHost.CheckIntersectionRebarAndElement(mainDoc, bar, curHost, mainDocView, linkTransform);
                    }
                    else
                    {
                        var ir = Tools.Geometry.Intersection.CheckElementsIntersection(mainDoc, curHost, rebar, linkTransform);
                        if (ir == Geometry.Intersection.MyIntersectionResult.Intersection)
                            checkIntersection = true;

                    }
                    if (checkIntersection)
                        resultMainHostElements.Add(curHost);
                }
            }

            //получаю самый нижний элемент
            if (resultMainHostElements.Count == 0) return null;

            hostElem = Tools.Geometry.Height.GetBottomElement(resultMainHostElements, mainDocView);

            return hostElem;
        }
    }
}
