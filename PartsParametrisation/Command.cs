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
using System.Diagnostics;
using System.Linq;

#endregion


namespace PartsParametrisation
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class Command : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Tools.Logger.Logger logger = new Tools.Logger.Logger(nameof(PartsParametrisation));
            Trace.Listeners.Clear();
            Trace.Listeners.Add(logger);
            Trace.WriteLine($"Start {nameof(PartsParametrisation)}");

            Tools.SettingsSaver.Saver<PartSettings> saver = new Tools.SettingsSaver.Saver<PartSettings>();
            PartSettings sets = saver.Activate(nameof(PartsParametrisation));
            if (sets.Parameters == null || sets.Parameters.Count == 0)
                sets.GetDefault();

            FormParameters form = new FormParameters(sets);
            if (form.ShowDialog() != System.Windows.Forms.DialogResult.OK) return Result.Cancelled;

            sets = form.userSettings;

            Document doc = commandData.Application.ActiveUIDocument.Document;
            List<Part> parts = new FilteredElementCollector(doc)
                .WhereElementIsNotElementType()
                .OfClass(typeof(Part))
                .Cast<Part>()
                .Where(i => i.get_BoundingBox(commandData.View) != null)
                .ToList();


            int partsCount = parts.Count;
            if (partsCount == 0)
            {
                TaskDialog.Show("Error", "No Parts found in the model!");
                return Result.Cancelled;
            }

            int counter = 0;
            int errorsCount = 0;
            using (Transaction t = new Transaction(doc))
            {
                t.Start("Parts parametrisation");
                foreach (Part p in parts)
                {
                    ElementId hostId = p.GetSourceElementIds().First().HostElementId;
                    Element hostElem = GetHostElement(doc, p);

                    foreach (PartParameter pp in sets.Parameters)
                    {
                        Parameter sourceParam = Tools.Model.ParameterUtils.Getter.GetParameter(hostElem, pp.HostParameterName);
                        if (sourceParam == null) continue;
                        if (!sourceParam.HasValue) continue;

                        Parameter targetParam = p.LookupParameter(pp.PartParameterName);
                        if (targetParam == null) continue;
                        if (targetParam.IsReadOnly) continue;

                        bool result = Tools.Model.ParameterUtils.Writer.WriteValueFromParamToParam(sourceParam, targetParam);
                        if (result) counter++;
                        else errorsCount++;
                    }
                }
                t.Commit();
            }

            saver.Save(sets);

            Tools.Forms.BalloonTip.Show("Success!", $"Parts found: {partsCount}\nParameters: {counter}\nErrors: {errorsCount}");

            return Result.Succeeded;
        }

        private Element GetHostElement(Document doc, Part part)
        {
            ElementId hostId = part.GetSourceElementIds().First().HostElementId;
            Element elem = doc.GetElement(hostId);

            if (elem is Part)
            {
                Part p2 = elem as Part;
                elem = GetHostElement(doc, p2);
            }
            return elem;
        }
    }
}
