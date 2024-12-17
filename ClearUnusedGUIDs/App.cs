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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.UI;
using System.Windows.Media.Imaging;
#endregion

namespace ClearUnusedGUIDs
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class App : IExternalApplication
    {
        public static string assemblyPath = "";
        public Result OnStartup(UIControlledApplication application)
        {
            assemblyPath = System.Reflection.Assembly.GetExecutingAssembly().Location;

            string tabName = "Weandrevit";
            try { application.CreateRibbonTab(tabName); } catch { }

            RibbonPanel panel1 = application.CreateRibbonPanel(tabName, "Параметры в семействе");

            PushButton btnAdd = panel1.AddItem(new PushButtonData(
                "AddSharedParams",
                "Добавить",
                assemblyPath,
                "ClearUnusedGUIDs.CommandAddParameters")
                ) as PushButton;
            btnAdd.ToolTip = "Пакетное добавление общих параметров в семейство.";
            btnAdd.AvailabilityClassName = "ClearUnusedGUIDs.AvailableButtonInFamily";


            PushButton btnAddByAnalog = panel1.AddItem(new PushButtonData(
                "AddParamsByAnalog",
                "По аналогу",
                assemblyPath,
                "ClearUnusedGUIDs.CommandAddParamsByAnalog")
                ) as PushButton;
            btnAddByAnalog.ToolTip = "Добавление общих параметров, используя другое семейство как образец.";
            btnAddByAnalog.AvailabilityClassName = "ClearUnusedGUIDs.AvailableButtonInFamily";



            PushButton btnClear = panel1.AddItem(new PushButtonData(
                "ClearFamilyGuids",
                "Очистить GUID",
                assemblyPath,
                "ClearUnusedGUIDs.CommandClear")
                ) as PushButton;
            btnClear.ToolTip = "Очистка семейства от неиспользуемых определений общих параметров. Доступно только в редакторе семейств.";



            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }
    }
}
