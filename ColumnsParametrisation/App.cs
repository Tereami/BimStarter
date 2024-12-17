#region License
/*Данный код опубликован под лицензией Creative Commons Attribution-ShareAlike.
Разрешено использовать, распространять, изменять и брать данный код за основу для производных в коммерческих и
некоммерческих целях, при условии указания авторства и если производные лицензируются на тех же условиях.
Код поставляется "как есть". Автор не несет ответственности за возможные последствия использования.
Зуев Александр, 2021, все права защищены.
This code is listed under the Creative Commons Attribution-ShareAlike license.
You may use, redistribute, remix, tweak, and build upon this work non-commercially and commercially,
as long as you credit the author by linking back and license your new creations under the same terms.
This code is provided 'as is'. Author disclaims any implied warranty.
Zuev Aleksandr, 2021, all rigths reserved.*/
#endregion
using Autodesk.Revit.UI;
using System.Diagnostics;

namespace ColumnsParametrisation
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class App : IExternalApplication
    {
        public static string assemblyPath = "";
        public static string assemblyFolder = "";
        public static string localFolder = "";

        public Result OnStartup(UIControlledApplication application)
        {
            assemblyPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            assemblyFolder = System.IO.Path.GetDirectoryName(assemblyPath);

            string tabName = "BIM-STARTER TEST";
            try { application.CreateRibbonTab(tabName); } catch { }
            RibbonPanel panel1 = application.CreateRibbonPanel(tabName, "Стены");
            PushButton btn = panel1.AddItem(new PushButtonData(
                "ColumnParametrisation",
                "Марки пилонов и стен",
                assemblyPath,
                "ColumnsParametrisation.Command")
                ) as PushButton;

            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }
    }
}
