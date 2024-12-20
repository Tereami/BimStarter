﻿#region License
/*Данный код опубликован под лицензией Creative Commons Attribution-ShareAlike.
Разрешено использовать, распространять, изменять и брать данный код за основу для производных в коммерческих и
некоммерческих целях, при условии указания авторства и если производные лицензируются на тех же условиях.
Код поставляется "как есть". Автор не несет ответственности за возможные последствия использования.
Зуев Александр, 2024, все права защищены.
This code is listed under the Creative Commons Attribution-ShareAlike license.
You may use, redistribute, remix, tweak, and build upon this work non-commercially and commercially,
as long as you credit the author by linking back and license your new creations under the same terms.
This code is provided 'as is'. Author disclaims any implied warranty.
Zuev Aleksandr, 2024, all rights reserved.*/
#endregion
#region Usings
using Autodesk.Revit.UI;
#endregion

[assembly: System.Reflection.AssemblyVersion("1.0.*")]

namespace RevitViewFilters
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    [Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.NoCommandData)]

    public class App : IExternalApplication
    {
        public static string assemblyPath = "";

        public Result OnStartup(UIControlledApplication application)
        {
            assemblyPath = typeof(App).Assembly.Location;

            string tabName = "TEST";
            try { application.CreateRibbonTab(tabName); }
            catch { }

            string projectName = nameof(RevitViewFilters);
            RibbonPanel panel = application.CreateRibbonPanel(tabName, $"{projectName} panel");
            string commandName = nameof(CommandBatchDelete);
            _ = panel.AddItem(new PushButtonData(
                commandName,
                commandName,
                assemblyPath,
                $"{projectName}.{commandName}")
                ) as PushButton;

            commandName = nameof(CommandCreate);
            _ = panel.AddItem(new PushButtonData(
                commandName,
                commandName,
                assemblyPath,
                $"{projectName}.{commandName}")
                ) as PushButton;

            commandName = nameof(CommandViewColoring);
            _ = panel.AddItem(new PushButtonData(
                commandName,
                commandName,
                assemblyPath,
                $"{projectName}.{commandName}")
                ) as PushButton;

            commandName = nameof(CommandWallHatch);
            _ = panel.AddItem(new PushButtonData(
                commandName,
                commandName,
                assemblyPath,
                $"{projectName}.{commandName}")
                ) as PushButton;

            return Result.Succeeded;
        }


        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }
    }
}