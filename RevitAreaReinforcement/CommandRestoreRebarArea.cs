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
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Tools.Extensions.Shortcuts;
#endregion

namespace RevitAreaReinforcement
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    class CommandRestoreRebarArea : IExternalCommand
    {
        List<ShortcutItem> requiredShortcuts = new List<ShortcutItem>
        {
            new ShortcutItem("Выбрать по коду", "ID_SELECT_BY_ID", new[] {"VK", "мл" }, "Управление&gt;Сведения"),
            new ShortcutItem("Редактировать эскиз; Редактировать границу; Редактировать проекцию; Редактировать траекторию", "Dialog_Essentials_SketchEdit:Control_Essentials_EditSketch", new[] { "HH", "рр" }, "Контекстные вкладки&gt;Режим"),
            new ShortcutItem("Вставить:С выравниванием по тому же месту", "ID_EDIT_PASTE_ALIGNED_SAME_PLACE", new[] { "DV", "вм" }, "Изменить&gt;Буфер обмена"),
            new ShortcutItem("Выход из режима редактирования", "ID_FINISH_SKETCH", new[] { "DH", "вр" }, "Контекстные вкладки&gt;Режим"),
            new ShortcutItem("Скрыть элемент" ,"ID_TEMPHIDE_HIDE", new [] {"CC", "сс" }, "Панель управления видами"),
        };

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            int revitVersionNumber = int.Parse(commandData.Application.Application.VersionNumber);
            ShortcutManager shortcutManager = new ShortcutManager(revitVersionNumber);
            List<ShortcutItem> incorrectKeys = shortcutManager.GetIncorrectShortcuts(requiredShortcuts);

            if (incorrectKeys.Count > 0)
            {
                FormShortcuts form1 = new FormShortcuts();
                if (form1.ShowDialog() != DialogResult.OK)
                    return Result.Cancelled;

                bool checkAddShortcuts = shortcutManager.SetShortcuts(incorrectKeys);
                if (!checkAddShortcuts)
                {
                    message = "Failed to add required shortcuts";
                    return Result.Failed;
                }

                FormShortcuts2 form2 = new FormShortcuts2(shortcutManager.xmlPath, shortcutManager.xmlFileBackup);
                form2.ShowDialog();
            }


            string assemblyPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string folder = System.IO.Path.GetDirectoryName(assemblyPath);
            folder = System.IO.Path.Combine(folder, "RevitAreaReinforcement_data");

            string shortcutsXmlPath = System.IO.Path.Combine(folder, "KeyboardShortcuts.xml");
            string idsFilePath = System.IO.Path.Combine(folder, "ids.txt");
            string speedFilePath = System.IO.Path.Combine(folder, "speed.txt");

            string speedString = "5";
            if (System.IO.File.Exists(speedFilePath))
            {
                speedString = System.IO.File.ReadAllText(speedFilePath);
            }
            int speed = int.Parse(speedString);
            DialogWindowRestoreAreaRebar form = new DialogWindowRestoreAreaRebar(shortcutsXmlPath, speed);
            if (form.ShowDialog() != System.Windows.Forms.DialogResult.OK)
            {
                return Result.Cancelled;
            }
            speed = form.speed;

            Document doc = commandData.Application.ActiveUIDocument.Document;
            Selection sel = commandData.Application.ActiveUIDocument.Selection;
            List<ElementId> selids = sel.GetElementIds().ToList();

            List<AreaReinforcement> ars = new List<AreaReinforcement>();
            foreach (ElementId id in selids)
            {
                AreaReinforcement ar = doc.GetElement(id) as AreaReinforcement;
                if (ar != null) ars.Add(ar);
            }

            if (ars.Count == 0)
            {
                message += MyStrings.MessageSelectAreaRebar;
                return Result.Failed;
            }


            if (System.IO.File.Exists(idsFilePath))
            {
                System.IO.File.Delete(idsFilePath);
            }

            using (System.IO.StreamWriter idsFileWriter = System.IO.File.CreateText(idsFilePath))
            {
                foreach (AreaReinforcement ar in ars)
                {
                    string line = $"{ar.Id}:";
                    List<ElementId> curveIds = ar.GetBoundaryCurveIds().ToList();
                    foreach (ElementId id in curveIds)
                    {
                        line = line + id.ToString() + ",";
                    }
                    idsFileWriter.WriteLine(line);
                }
                idsFileWriter.Close();
            }

            using (System.IO.StreamWriter speedFileWriter = System.IO.File.CreateText(speedFilePath))
            {
                speedFileWriter.Write(speed.ToString());
                speedFileWriter.Close();
            }


            string scriptPath = System.IO.Path.Combine(folder, "RestoreAreaRebar.exe");

            //нужно сбросить выделение, если есть выбранные элементы
            commandData.Application.ActiveUIDocument.Selection.SetElementIds(new List<ElementId>());

            //запускаю супер-програмку
            System.Diagnostics.Process.Start(scriptPath);


            return Result.Succeeded;
        }
    }
}
