#region License
/*Данный код опубликован под лицензией Creative Commons Attribution-NonСommercial-ShareAlike.
Разрешено использовать, распространять, изменять и брать данный код за основу для производных 
в некоммерческих целях, при условии указания авторства и если производные лицензируются на тех же условиях.
Код поставляется "как есть". Автор не несет ответственности за возможные последствия использования.
Зуев Александр, 2021, все права защищены.
This code is listed under the Creative Commons Attribution-NonСommercial-ShareAlike license.
You may use, redistribute, remix, tweak, and build upon this work non-commercially,
as long as you credit the author by linking back and license your new creations under the same terms.
This code is provided 'as is'. Author disclaims any implied warranty.
Zuev Aleksandr, 2021, all rigths reserved.*/
#endregion
#region usings
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
#endregion

namespace RibbonBimStarter
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
