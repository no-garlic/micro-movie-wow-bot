using System;
using System.Windows.Forms;
using CommonLib;
using LuaLib;

namespace MovieMaker
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            App.CurrentMode = App.Mode.CombatMovie;
            App.RegisterClasses();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MovieMaker());
        }
    }
}
