using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonLib;

namespace SimC
{
    public class Data
    {
        public List<Data.Record> Records { get; private set; }

        public Data()
        {
            Records = new List<Record>();
        }

        public enum Ability
        {
            None,
            Shred,
            Rake,
            Rip,
            AshamanesFrenzy,
            FerociousBite,
            HealingTouch,
            SavageRoar,
            Moonfire,
            TigersFury,
            Berserk,
            Swipe,
            Thrash,
            Shadowmeld
        };

        [Flags]
        public enum Buff
        {
            None                = 0,
            SavageRoar          = 1,
            Bloodtalons         = 2,
            PredatorySwiftness  = 4,
            TigersFury          = 8,
            Berserk             = 16,
            ClearCasting        = 32,
        };

        public class Record
        {
            public DateTime DateTime    { get; set; }
            public float    Elapsed     { get; set; }
            public float    Delta       { get; set; }
            public Ability  Cast        { get; set; }
            public Buff     Buffs       { get; set; }
            public int      Energy      { get; set; }
            public int      MaxEnergy   { get; set; }
            public int      Combo       { get; set; }
            public int      MaxCombo    { get; set; }

            public void Print(TextWriter writer = null)
            {
                if (writer == null)
                    writer = Console.Out;

                string buffs = Buffs.ToString();
                buffs = buffs.Replace(", ", ";");

                writer.WriteLine(
                    DateTime.ToString("M/d HH:mm:ss.fff") + "," + 
                    Elapsed.ToString("0.000") + "," + 
                    Delta.ToString("0.000") + "," + 
                    Cast.ToString() + "," + 
                    Energy.ToString() + "," + 
                    MaxEnergy.ToString() + "," + 
                    Combo.ToString() + "," + 
                    MaxCombo.ToString() + "," + 
                    buffs);
            }
        }

        


    }
}
