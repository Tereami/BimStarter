using Autodesk.Revit.UI;

namespace LinkWriter
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class App : IExternalApplication
    {
        public static string assemblyPath = "";
        public Result OnStartup(UIControlledApplication application)
        {
            string tabName = "BIM-STARTER TEST";
            try { application.CreateRibbonTab(tabName); } catch { }

            assemblyPath = System.Reflection.Assembly.GetExecutingAssembly().Location;

            RibbonPanel panel1 = application.CreateRibbonPanel(tabName, "LINK WRITE TEXT");


            PushButton btnWriteLink = panel1.AddItem(new PushButtonData(
                "WriteLink",
                "Write\nLink",
                assemblyPath,
                "BatchPrintYay.CommandWriteLinkTitleblock")
                ) as PushButton;


            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }
    }
}