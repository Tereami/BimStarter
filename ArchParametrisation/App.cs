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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.ApplicationServices;
#endregion

namespace ArchParametrisation
{
    public class App : IExternalApplication
    {
        public Result OnStartup(UIControlledApplication application)
        {

            string assemblyPath = typeof(App).Assembly.Location;

            string tabName = "BIM-STARTER";
            application.CreateRibbonTab(tabName);

            //Панель excel
            RibbonPanel panelTest = application.CreateRibbonPanel(tabName, "Arch");

            PushButton btnAdskConvert = panelTest.AddItem(new PushButtonData(
                "ArchParametrisation",
                "АР Параметризация",
                assemblyPath,
                "ArchParametrisation.CmdArchParametrisation")
                ) as PushButton;

            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }
    }
}
