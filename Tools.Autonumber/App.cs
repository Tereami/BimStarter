using System;
using System.Windows.Forms;
using Tools.KeyboardSimulation;

[assembly: System.Reflection.AssemblyVersion("1.0.*")]
namespace Tools.Autonumber
{
    public class App : ApplicationContext
    {
        public App()
        {
            FormAutonumber form = new FormAutonumber();
            if (form.ShowDialog() != DialogResult.OK)
            {
                Application.Exit();
                Environment.Exit(0);
                return;
            }

            Keyboard k = new Keyboard();

            double interval1 = 5000 / Math.Pow(2, form.Speed);
            int interval = (int)interval1;
            //MessageBox.Show($"{interval1} - {interval}");

            System.Threading.Thread.Sleep(2000);

            for (int i = form.StartNumber; i <= form.EndNumber; i++)
            {
                string text = i.ToString();

                k.Send(text, interval);

                if (form.SuppressTooltips)
                {
                    k.Send(Keyboard.ScanCodeShort.ESCAPE);
                    System.Threading.Thread.Sleep(interval);
                }

                k.Send(Keyboard.ScanCodeShort.RETURN);
                System.Threading.Thread.Sleep(interval);
            }


            Application.Exit();
            Environment.Exit(0);
        }
    }
}
