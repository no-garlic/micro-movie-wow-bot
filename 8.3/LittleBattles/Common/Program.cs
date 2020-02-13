using System;
using System.Windows.Forms;
using CommonLib;
using LuaLib;

namespace LittleBattles
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            App.CurrentMode = App.Mode.LittleBattles;
            App.RegisterClasses();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new LittleBattles());
        }
    }
}
