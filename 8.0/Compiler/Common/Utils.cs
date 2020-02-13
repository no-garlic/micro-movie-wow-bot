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
        private static Dictionary<int, string> m_ClassAndSpecNames;

        public static bool GetWOWFolder(out string wowFolder, out string addonFolder)
        {
            bool beta = Config.GenerateForBeta;

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

        public static string GetClassOrSpecNameFromID(int classID)
        {
            if (m_ClassAndSpecNames == null)
            {
                m_ClassAndSpecNames = new Dictionary<int, string>();

                m_ClassAndSpecNames[6]   = "DeathKnight";
                m_ClassAndSpecNames[250] = "Blood";
                m_ClassAndSpecNames[251] = "Frost";
                m_ClassAndSpecNames[252] = "Unholy";

                m_ClassAndSpecNames[12]  = "DemonHunter";
                m_ClassAndSpecNames[577] = "Havoc";
                m_ClassAndSpecNames[581] = "Vengeance";

                m_ClassAndSpecNames[11]  = "Druid";
                m_ClassAndSpecNames[102] = "Balance";
                m_ClassAndSpecNames[103] = "Feral";
                m_ClassAndSpecNames[104] = "Guardian";
                m_ClassAndSpecNames[105] = "Restoration";

                m_ClassAndSpecNames[3]   = "Hunter";
                m_ClassAndSpecNames[253] = "Beast Mastery";
                m_ClassAndSpecNames[254] = "Marksmanship";
                m_ClassAndSpecNames[255] = "Survival";

                m_ClassAndSpecNames[8]   = "Mage";
                m_ClassAndSpecNames[62]  = "Arcane";
                m_ClassAndSpecNames[63]  = "Fire";
                m_ClassAndSpecNames[64]  = "Frost";

                m_ClassAndSpecNames[10]  = "Monk";
                m_ClassAndSpecNames[268] = "Brewmaster";
                m_ClassAndSpecNames[269] = "Windwalker";
                m_ClassAndSpecNames[270] = "Mistweaver";

                m_ClassAndSpecNames[2]   = "Paladin";
                m_ClassAndSpecNames[65]  = "Holy";
                m_ClassAndSpecNames[66]  = "Protection";
                m_ClassAndSpecNames[70]  = "Retribution";

                m_ClassAndSpecNames[5]   = "Priest";
                m_ClassAndSpecNames[256] = "Discipline";
                m_ClassAndSpecNames[257] = "Holy";
                m_ClassAndSpecNames[258] = "Shadow";

                m_ClassAndSpecNames[4]   = "Rogue";
                m_ClassAndSpecNames[259] = "Assassination";
                m_ClassAndSpecNames[260] = "Outlaw";
                m_ClassAndSpecNames[261] = "Subtlety";

                m_ClassAndSpecNames[7]   = "Shaman";
                m_ClassAndSpecNames[262] = "Elemental";
                m_ClassAndSpecNames[263] = "Enhancement";
                m_ClassAndSpecNames[264] = "Restoration";

                m_ClassAndSpecNames[9]   = "Warlock";
                m_ClassAndSpecNames[265] = "Affliction";
                m_ClassAndSpecNames[266] = "Demonology";
                m_ClassAndSpecNames[267] = "Destruction";

                m_ClassAndSpecNames[1]   = "Warrior";
                m_ClassAndSpecNames[71]  = "Arms";
                m_ClassAndSpecNames[72]  = "Fury";
                m_ClassAndSpecNames[73]  = "Protection";
            }

            string result = "";
            m_ClassAndSpecNames.TryGetValue(classID, out result);
            return result;
        }
    
        public static void CopyFiles(string sourceDirectory, string targetDirectory)
        {
            DirectoryInfo diSource = new DirectoryInfo(sourceDirectory);
            DirectoryInfo diTarget = new DirectoryInfo(targetDirectory);

            CopyAllFiles(diSource, diTarget);
        }

        private static void CopyAllFiles(DirectoryInfo source, DirectoryInfo target)
        {
            Directory.CreateDirectory(target.FullName);

            // Copy each file into the new directory.
            foreach (FileInfo fi in source.GetFiles())
            {
                //Console.WriteLine(@"Copying {0}\{1}", target.FullName, fi.Name);
                fi.CopyTo(Path.Combine(target.FullName, fi.Name), true);
            }

            // Copy each subdirectory using recursion.
            foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
            {
                DirectoryInfo nextTargetSubDir =
                    target.CreateSubdirectory(diSourceSubDir.Name);
                CopyAllFiles(diSourceSubDir, nextTargetSubDir);
            }
        }
    }
}