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
#region usings
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#endregion

namespace BatchPrintYay
{
    public enum OrientationType { Portrait, Landscape, Automatic };

    public static class SupportRegistry
    {
        

        public static void ActivateSettingsForPDFCreator(string outputFolder)
        {
            string outputFolderDoubleSlashes = outputFolder.Replace("\\", "\\\\");
            try
            {
                Microsoft.Win32.Registry.SetValue(
                    "HKEY_CURRENT_USER\\Software\\pdfforge\\PDFCreator\\Settings\\ConversionProfiles\\0\\AutoSave", "Enabled", "True");
                Microsoft.Win32.Registry.SetValue(
                    "HKEY_CURRENT_USER\\Software\\pdfforge\\PDFCreator\\Settings\\ConversionProfiles\\0", "FileNameTemplate", "<InputFilename>");
                Microsoft.Win32.Registry.SetValue(
                    "HKEY_CURRENT_USER\\Software\\pdfforge\\PDFCreator\\Settings\\ConversionProfiles\\0", "OpenViewer", "False");
                Microsoft.Win32.Registry.SetValue(
                    "HKEY_CURRENT_USER\\Software\\pdfforge\\PDFCreator\\Settings\\ConversionProfiles\\0", "OpenWithPdfArchitect", "False");
                Microsoft.Win32.Registry.SetValue(
                    "HKEY_CURRENT_USER\\Software\\pdfforge\\PDFCreator\\Settings\\ConversionProfiles\\0", "TargetDirectory", outputFolderDoubleSlashes);
            }
            catch
            {
                string msg = MyStrings.MessageUnableToMakePdfSettings;
                MessageBox msgbox = new MessageBox(msg);
                msgbox.ShowDialog();
            }

        }

        public static void SetOrientationForPdfCreator(OrientationType orientType)
        {
            string keyName = "HKEY_CURRENT_USER\\Software\\pdfforge\\PDFCreator\\Settings\\ConversionProfiles\\0\\PdfSettings";
            string keyTitle = "PageOrientation";
            string text = Enum.GetName(typeof(OrientationType), orientType);
            string check = "";
            try
            {
                check = Microsoft.Win32.Registry.GetValue(keyName, keyTitle, "default") as string;
                if (check == null || check == "default" || string.IsNullOrEmpty(check))
                {
                    return;
                }
                else
                {
                    if (check != text)
                    {
                        Microsoft.Win32.Registry.SetValue(keyName, keyTitle, text);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                string msg = MyStrings.MessageUnableToMakePdfSettingsFirstLaunch;
                msg += MyStrings.MessageRegistryKeyName + text + MyStrings.MessageRegistryKeyValue + check + ". ";
                msg = msg + MyStrings.MessageErrorText + ex.Message;
                MessageBox msgbox = new MessageBox(msg);
                msgbox.ShowDialog();
                throw new Exception(msg);
            }
        }

        public static void RestoreSettingsForPDFCreator()
        {
            try
            {
                Microsoft.Win32.Registry.SetValue(
                    "HKEY_CURRENT_USER\\Software\\pdfforge\\PDFCreator\\Settings\\ConversionProfiles\\0\\AutoSave", "Enabled", "False");
                Microsoft.Win32.Registry.SetValue(
                    "HKEY_CURRENT_USER\\Software\\pdfforge\\PDFCreator\\Settings\\ConversionProfiles\\0", "FileNameTemplate", "<Title>");
                Microsoft.Win32.Registry.SetValue(
                    "HKEY_CURRENT_USER\\Software\\pdfforge\\PDFCreator\\Settings\\ConversionProfiles\\0", "OpenViewer", "True");
                Microsoft.Win32.Registry.SetValue(
                    "HKEY_CURRENT_USER\\Software\\pdfforge\\PDFCreator\\Settings\\ConversionProfiles\\0", "OpenWithPdfArchitect", "False");
                Microsoft.Win32.Registry.SetValue(
                    "HKEY_CURRENT_USER\\Software\\pdfforge\\PDFCreator\\Settings\\ConversionProfiles\\0", "TargetDirectory", "");
            }
            catch
            {
                string msg = MyStrings.MessageUnableToRestorePdfSettings;
                MessageBox msgbox = new MessageBox(msg);
                msgbox.ShowDialog();
                throw new Exception(msg);
            }
        }
    }
}