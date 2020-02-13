using System;
using System.Drawing;
using System.Collections.Generic;
using Microsoft.Win32;
using System.IO;
using CommonLib;

namespace Compiler
{
    public abstract class Utils
    {
        public static bool GetWOWFolder(bool beta, out string wowFolder, out string addonFolder)
        {           
            wowFolder   = "";
            addonFolder = "";
             
            string key = @"Software\Wow6432Node\Blizzard Entertainment\World of Warcraft";

            if (beta)
                key += @"\Beta";

            RegistryKey rk = Registry.LocalMachine.OpenSubKey(key);

            string folder = "";

            if (rk == null)
                return false;
            
            folder = (string) rk.GetValue("InstallPath");
            wowFolder   = folder;
            addonFolder = folder + "Interface\\Addons\\" + Constants.AddonName;

            return true;
        }
    }


}
