using System;
using System.Windows.Forms;
using CommonLib;

namespace MovieMaker
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            App.RegisterClasses();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MovieMaker());
        }
    }
}
