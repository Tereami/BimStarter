#region License
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
using System.Diagnostics;
#endregion

[assembly: System.Reflection.AssemblyVersion("1.0.*")]

namespace RevitAreaReinforcement
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    [Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.NoCommandData)]

    public class App : IExternalApplication
    {
        public static string assemblyPath = "";
        public static string assemblyFolder = "";
        public static string localFolder = "";

        public Result OnStartup(UIControlledApplication application)
        {
            string assemblyPath = typeof(App).Assembly.Location;

            string tabName = "TEST";
            try { application.CreateRibbonTab(tabName); }
            catch { }

            string projectName = nameof(RevitAreaReinforcement);
            RibbonPanel panel = application.CreateRibbonPanel(tabName, $"{projectName} panel");
            string commandName = nameof(CommandCreateAreaRebar);
            _ = panel.AddItem(new PushButtonData(
                commandName,
                commandName,
                assemblyPath,
                $"{projectName}.{commandName}")
                ) as PushButton;

            commandName = nameof(CommandCreateFloorRebar);
            _ = panel.AddItem(new PushButtonData(
                commandName,
                commandName,
                assemblyPath,
                $"{projectName}.{commandName}")
                ) as PushButton;

            commandName = nameof(CommandRestoreRebarArea);
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

        public static void ActivateConfigFolder()
        {
            Trace.WriteLine("Activate config folder");
            if (string.IsNullOrEmpty(App.assemblyFolder))
            {
                App.assemblyPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
                App.assemblyFolder = System.IO.Path.GetDirectoryName(App.assemblyPath);
            }

            string appdataPath =
                System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData);
            string rbspath = System.IO.Path.Combine(appdataPath, "bim-starter");
            if (!System.IO.Directory.Exists(rbspath))
                System.IO.Directory.CreateDirectory(rbspath);
            string solutionName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
            localFolder =
                System.IO.Path.Combine(rbspath, solutionName);
            if (!System.IO.Directory.Exists(localFolder))
                System.IO.Directory.CreateDirectory(localFolder);
            Trace.WriteLine("Activate folder " + localFolder);
        }
    }
}