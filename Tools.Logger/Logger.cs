using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace Tools.Logger
{
    public class Logger : TraceListener
    {
        public static string filePath = "";
        string title = "";

        public Logger(string parentTitle)
        {
            title = parentTitle;
            string appdataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string logsFolder = Path.Combine(appdataFolder, @"Autodesk\Revit\Addins\20xx\BimStarter\logs");
            if (!Directory.Exists(logsFolder))
            {
                Directory.CreateDirectory(logsFolder);
            }
            filePath = Path.Combine(logsFolder, title + "_log_" + DateTime.Now.ToString("yyyyMMdd HHmmss") + ".log");
        }

        public async override void Write(string message)
        {
            try
            {
                await FileWriteAsync(filePath, message);
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to write log: " + filePath + ". Message: " + ex.Message);
            }
        }

        public async override void WriteLine(string message)
        {
            try
            {
                string msg = DateTime.Now.ToString("yyyy MM dd_HH:mm:ss") + " : " + message;
                await FileWriteAsync(filePath, msg);
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to write log: " + filePath + ". Message: " + ex.Message);
            }
        }

        private async Task FileWriteAsync(string filePath, string messaage)
        {
            bool append = File.Exists(filePath);

            using (FileStream stream = new FileStream(filePath, append ? FileMode.Append : FileMode.Create, FileAccess.Write, FileShare.None, 4096, true))
            {
                using (StreamWriter sw = new StreamWriter(stream))
                {
                    await sw.WriteLineAsync(messaage);
                }
            }
        }
    }
}
