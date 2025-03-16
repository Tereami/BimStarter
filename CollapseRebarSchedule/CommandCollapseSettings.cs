using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Diagnostics;

namespace SchedulesTools
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.ReadOnly)]
    public class CommandCollapseSettings : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            string commandName = nameof(CommandCollapseSettings);
            Trace.Listeners.Clear();
            Trace.Listeners.Add(new Tools.Logger.Logger(commandName));
            Debug.WriteLine($"{commandName} start");

            Tools.SettingsSaver.Saver<CollapseScheduleSettings> saver = new Tools.SettingsSaver.Saver<CollapseScheduleSettings>();
            CollapseScheduleSettings sets = saver.Activate("CollapseRebarSchedule");
            if (sets == null)
            {
                string msg = "Failed to read config xml file";
                Trace.WriteLine(msg);
                throw new System.Exception(msg);
            }
            FormCollapseSettings form = new FormCollapseSettings(sets);
            if (form.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                return Result.Cancelled;
            sets = form.NewSets;
            Debug.WriteLine($"Settings: {sets.ToString()}");

            saver.Save(sets);

            return Result.Succeeded;
        }
    }
}
