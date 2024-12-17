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

using System;
using System.Collections.Generic;
using Autodesk.Revit.DB;


namespace RibbonBimStarter
{
    public class FamilyFileInfo
    {
        public int Id { get; set; }

        public string Guid { get; set; }

        public string Title { get; set; }

        public string FolderTitle { get; set; }

        public string ImagePath { get; set; }
        public string ImageBigPath { get; set; }

        public string Description { get; set; }

        public string ShortDescription { get; set; }

        public string HostType { get; set; }
        public string HostShortName { get; set; }

        public string CategoryName { get; set; }
        public string CategoryShortName { get; set; }

        public string CategoryTitleAndName { get; set; }

        public string DateAdd { get; set; }

        public string RevitVersion { get; set; }

        public string FamilyName { get; set; }

        public static int GetHostTypeId(Family fam)
        {
            FamilyPlacementType placeType = fam.FamilyPlacementType;
            switch (placeType)
            {
                case FamilyPlacementType.OneLevelBased:
                    return 1;
                case FamilyPlacementType.TwoLevelsBased:
                    return 4;
                case FamilyPlacementType.CurveBased:
                    return 5;
                case FamilyPlacementType.CurveDrivenStructural:
                    return 6;
                case FamilyPlacementType.ViewBased:
                    return 11;
                case FamilyPlacementType.CurveBasedDetail:
                    return 12;
                case FamilyPlacementType.Adaptive:
                    return 13;
            }

            foreach (Parameter p in fam.GetOrderedParameters())
            {
                InternalDefinition intdef = p.Definition as InternalDefinition;
                if (intdef == null) continue;
                if (intdef.BuiltInParameter == BuiltInParameter.FAMILY_HOSTING_BEHAVIOR)
                {
                    int hostTypeId = p.AsInteger();
                    switch (hostTypeId)
                    {
                        case 1: //стена
                            return 7;
                        case 2: //перекрытие
                            return 8;
                        case 3: //потолок
                            return 10;
                        case 4: //крыша
                            return 9;
                        case 5: //грань
                            return 3;
                    }
                }

                if (intdef.BuiltInParameter == BuiltInParameter.FAMILY_WORK_PLANE_BASED)
                {
                    if (p.AsInteger() == 1)
                    {
                        return 2;
                    }
                    else
                    {
                        return 1;
                    }
                }
            }
            return 14;
        }
    }
}
