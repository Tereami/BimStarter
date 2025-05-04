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
Zuev Aleksandr, 2024, all rigths reserved.*/
#endregion
#region Usings
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
#endregion


namespace Autonumber
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class Command : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            string assemblyPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string folder = System.IO.Path.GetDirectoryName(assemblyPath);
            string exefile = System.IO.Path.Combine(folder, "Autonumber_data", "Autonumber.exe");

            bool checkFile = System.IO.File.Exists(exefile);
            if (!checkFile)
            {
                message = "Executing file is not found: " + exefile;
                return Result.Failed;
            }

            try
            {
                System.Diagnostics.Process.Start(exefile);
            }
            catch
            {
                message = "Failed to execute: " + exefile;
                return Result.Failed;
            }



            return Result.Succeeded;
        }
    }
}
