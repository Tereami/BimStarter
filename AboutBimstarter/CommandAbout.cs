using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using System.IO;
using System.Diagnostics;
using System.Net;


namespace AboutBimstarter
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]

    class CommandAbout : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            int installedVersion = 0;
            string appdataFolder =
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string bimstarterFolder =
                Path.Combine(appdataFolder, "Autodesk", "Revit", "Addins", "20xx", "BimStarter");
            if (Directory.Exists(bimstarterFolder))
            {
                string[] files = Directory.GetFiles(bimstarterFolder);
                string[] versionFiles = files
                    .Where(i => char.IsDigit(i.Last()))
                    .ToArray();
                if (versionFiles.Length > 0)
                {
                    string filename = versionFiles[0];
                    string filename0 = filename.Substring(filename.Length - 3);
                    int.TryParse(filename0, out installedVersion);
                }
            }

            SslConfig sets = SslConfig.Load();

            AboutBimstarter.AboutForm form = new AboutBimstarter.AboutForm(installedVersion, sets);

            if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                sets = form.newSets;
                if (sets == null) throw new Exception("Invalid settings");


                if (sets.tlsVersion == TlsVersion.Default)
                {
                    Debug.WriteLine("Certificate is not changed, use default");
                }
                else if (sets.tlsVersion == TlsVersion.Tls1)
                {
                    Debug.WriteLine("Set tls1");
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
                }
                else if (sets.tlsVersion == TlsVersion.Tls11)
                {
                    Debug.WriteLine("Set tls11");
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11;
                }
                else if (sets.tlsVersion == TlsVersion.Tls12)
                {
                    Debug.WriteLine("Set tls12");
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                }

                Debug.WriteLine($"Current certificate: {ServicePointManager.SecurityProtocol}");

                sets.Save();
            }

            return Result.Succeeded;
        }
    }
}
