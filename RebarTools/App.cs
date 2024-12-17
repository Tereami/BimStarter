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
using Autodesk.Revit.UI;
#endregion
namespace RebarTools
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class App : IExternalApplication
    {
        public static string assemblyPath = "";
        public Result OnStartup(UIControlledApplication application)
        {
            assemblyPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string tabName = "BIM-STARTER TEST";
            try { application.CreateRibbonTab(tabName); } catch { }

            RibbonPanel panel1 = application.CreateRibbonPanel(tabName, "Армирование");
            _ = panel1.AddItem(new PushButtonData(
                nameof(CommandAreaMark),
                "Марка по площади",
                assemblyPath,
                $"{nameof(RebarTools)}.{nameof(CommandAreaMark)}")
                ) as PushButton;

            _ = panel1.AddItem(new PushButtonData(
                nameof(CommandExplode),
                "Взорвать",
                assemblyPath,
                $"{nameof(RebarTools)}.{nameof(CommandExplode)}")
                ) as PushButton;

            _ = panel1.AddItem(new PushButtonData(
                nameof(CommandRebarPresentation),
                "Скрыть",
                assemblyPath,
                $"{nameof(RebarTools)}.{nameof(CommandRebarPresentation)}")
                ) as PushButton;

            _ = panel1.AddItem(new PushButtonData(
                nameof(CommandRebarVisibility),
                "Как тело",
                assemblyPath,
                $"{nameof(RebarTools)}.{nameof(CommandRebarVisibility)}")
                ) as PushButton;

            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }


    }
}
