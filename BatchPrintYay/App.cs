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
#region usings
using Autodesk.Revit.UI;
#endregion

namespace BatchPrintYay
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]

    public class App : IExternalApplication
    {
        public static string assemblyPath = "";
        //public static Dictionary<Document, List<ViewSheet>> allSheets
        //    = new Dictionary<Document, List<ViewSheet>>();


        public Result OnStartup(UIControlledApplication application)
        {
            string tabName = "BIM-STARTER TEST";
            try { application.CreateRibbonTab(tabName); } catch { }

            assemblyPath = System.Reflection.Assembly.GetExecutingAssembly().Location;

            RibbonPanel panel1 = application.CreateRibbonPanel(tabName, "Print test");

            PushButton btnCreate = panel1.AddItem(new PushButtonData(
                "BatchPrint",
                "Batch print",
                assemblyPath,
                "BatchPrintYay.CommandBatchPrint")
                ) as PushButton;

            PushButton btnRefresh = panel1.AddItem(new PushButtonData(
                "RefreshSchedules",
                "Refresh\nschedules",
                assemblyPath,
                "BatchPrintYay.CommandRefreshSchedules")
                ) as PushButton;



            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }
    }
}
