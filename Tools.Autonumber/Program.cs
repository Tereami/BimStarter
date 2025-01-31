using System;
using System.Runtime.Versioning;
using System.Windows.Forms;


namespace Tools.Autonumber
{
    [SupportedOSPlatform("windows")]
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new App());
        }
    }
}