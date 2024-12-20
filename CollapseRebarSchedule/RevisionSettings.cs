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
using System.Collections.Generic;
#endregion

namespace SchedulesTools
{
    public static class Settings
    {
        public static bool UseCloudsCount = true;
        public static bool UseRevisionsOnThisSheet = true;
        public static bool UseGroupingRevisions = true;

        public static RevisionParameter revisionParam = new RevisionParameter(RevisionParameterType.IssuedTo);
        public static List<RevisionParameter> revParams = RevisionParameter.GetAllRevisionParams();

        public static string SheetNumberParameter = "Номер листа";
        public static List<string> SheetNumberParameters = new List<string>()
        {
            "Номер листа",
            "Ш_Номер листа",
            "О_НомерЛиста"
        };

        public static string SheetNoteParam = "Ш.ПримечаниеЛиста";
        public static string SheetsForLastRevision = "Ш.ЛистыТекущегоИзменения";

        public static bool UseStandartRevisionDescription = true;
        public static bool UseParameterForRevisionDescription = false;
        public static string SheetRevisionDescriptionParameter = "Ш.ОписаниеИзменения";
    }
}
