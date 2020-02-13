using System;

namespace CommonLib
{
    public class AB : Data
    {
        public float Timer      { get; private set; }
        public bool  IsReady    { get; private set; }
        public bool  IsNotReady   => !IsReady;
        public bool  IsInRange  { get; private set; }
        public bool  IsOutOfRange => !IsInRange;

        public AB(int nameIndex) : base(nameIndex)
        {
            IsReady = false;
            Timer  = 0.0f;
        }

        public override void Update()
        {
            const float LagTolerance = 0.3f;

            bool nocharges, outofrange;
            Conversion.ToBools(this, ColorChannel.Red, out nocharges, out outofrange);
            Timer = Conversion.ToFloat2(this, ColorChannel.Green, ColorChannel.Blue);
            
            bool charges = !nocharges;
            bool inrange = !outofrange;
            IsInRange = inrange;
            IsReady = charges && inrange && (Timer <= LagTolerance);
        }

        public override void LogData()
        {
            string[] Names = 
            {
                /*  0 */ "Rip",                  
                /*  1 */ "Rake",
                /*  2 */ "Thrash",
                /*  3 */ "Moonfire",
                /*  4 */ "Predatory Swiftness",
                /*  5 */ "Bloodtalons",
                /*  6 */ "Clearcasting",
                /*  7 */ "Mangle",
                /*  8 */ "Maul",
                /*  9 */ "Moonfire",
                /* 10 */ "Thrash",
                /* 11 */ "Swipe",
                /* 12 */ "Shred",
                /* 13 */ "Rip",
                /* 14 */ "Rake",
                /* 15 */ "Ferocious Bite",
                /* 16 */ "Healing Touch",
                /* 17 */ "Tiger's Fury",
                /* 18 */ "Ashamane's Frenzy",
                /* 19 */ "TargetInfo",
                /* 20 */ "Talent",
                /* 21 */ "ClassInfo",
                /* 22 */ "Power",
                /* 23 */ "MagicNumber_17_11_9_",
                /* 24 */ "MagicNumber_23_13_19_",
                /* 25 */ "PlayerHealth",
                /* 26 */ "PlayerInfo",
                /* 27 */ "Wild Charge",
                /* 28 */ "Skull Bash",
                /* 29 */ "Maim",
                /* 30 */ "TargetHealth",
                /* 31 */ "Mighty Bash",
                /* 32 */ "Savage Roar",
                /* 33 */ "Savage Roar",
                /* 34 */ "",
                /* 35 */ "",
                /* 36 */ "",
                /* 37 */ "",
                /* 38 */ "",
                /* 39 */ "",
                /* 40 */ "",
                /* 41 */ "",
                /* 42 */ "",
                /* 43 */ "",
                /* 44 */ "",
                /* 45 */ "",
                /* 46 */ "",
                /* 47 */ "",
                /* 48 */ "",
                /* 49 */ "",
            };

            float fTime = 10.0f * Timer;
            int iTime = (int) fTime;
            fTime = iTime;
            fTime *= 0.1f;

            //App.Log.Write(Log.LogDetail.High, Names[NameIndex] + " Ready", IsReady);
            //App.Log.Write(Log.LogDetail.High, Names[NameIndex] + " In Range", IsInRange);
            //App.Log.Write(Log.LogDetail.High, Names[NameIndex] + " Cooldown", fTime.ToString());
        }

    }    
}
