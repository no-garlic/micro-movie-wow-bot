using System;
using System.Windows.Forms;
using CommonLib;
using LuaLib;

namespace OceanMaster
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            App.CurrentMode = App.Mode.OceanMaster;
            App.RegisterClasses();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new OceanMaster());
        }
    }
}
