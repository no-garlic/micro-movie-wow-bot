using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonLib;

namespace SimC
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
            RawData rawdata = new RawData();
            rawdata.Load("C:\\Games\\Logs\\WOWCombatLog.csv");
            
            CombatLog log = new CombatLog();
            log.Import("C:\\Games\\Logs\\WOWCombatLog.txt");
            log.Export("C:\\Games\\Logs\\WOWCombatLog.csv");
            */

            CombatLog log = new CombatLog();
            log.Import("E:\\Code\\Logs\\WoWCombatLog.txt");
            log.Export();

            /*
            SimCraft simc = new SimCraft();
            simc.Import("E:\\Code\\Logs\\boots.html");
            simc.Export();
            */
        }
    }
}
