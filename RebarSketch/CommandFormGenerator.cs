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
using System.Collections.Generic;
using System.Linq;

#endregion


namespace RebarSketch
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.ReadOnly)]
    public class CommandFormGenerator : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            App.ActivatePaths();

            Document doc = commandData.Application.ActiveUIDocument.Document;

            ScetchLibrary lib = new ScetchLibrary();
            lib.Activate(App.libraryPath);
            List<XmlSketchItem> oldFormatTemplates = lib.templates.Where(i => i.IsXmlSource == false).ToList();
            if (oldFormatTemplates.Count > 0)
            {
                TaskDialog.Show(MyStrings.Info, MyStrings.MessageLibraryWillBeUpdated);
                foreach (XmlSketchItem xsi in lib.templates)
                {
                    xsi.Save();
                }
            }

            string librarypath = App.libraryPath;
            Form1 form = new Form1(librarypath);

            form.ShowDialog();

            return Result.Succeeded;
        }
    }
}
