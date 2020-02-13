using System;

namespace CommonLib
{
    public class PINF : Data
    {
        public bool IsHiding     { get; private set; }
        public bool IsAggressive { get; private set; }
        public bool IsInControl  { get; private set; }
        public bool IsDriving    { get; private set; }
        public bool IsLiving     { get; private set; }
        public bool IsArtifact   { get; private set; }
        public bool IsInRaid     { get; private set; }
        public bool IsInDungeon  { get; private set; }
        public bool IsSolo       { get; private set; }
        public int  PartySize    { get; private set; }

        public bool IsNotHiding     => !IsHiding;
        public bool IsNotAggressive => !IsAggressive;
        public bool IsNotLiving     => !IsLiving;
        public bool IsNotArtifact   => !IsArtifact;
        public bool IsNotInControl  => !IsInControl;
        public bool IsNotDriving    => !IsDriving;
        public bool IsNotInRaid     => !IsInRaid;
        public bool IsNotInDungeon  => !IsInDungeon;
        public bool IsNotSolo       => !IsSolo;

        public enum Form { H, B, C, A, T, M, R, F };
        public Form CurrentForm  { get; private set; }

        public bool IsH { get { return CurrentForm == Form.H; } }
        public bool IsB { get { return CurrentForm == Form.B; } }
        public bool IsC { get { return CurrentForm == Form.C; } }
        public bool IsA { get { return CurrentForm == Form.A; } }
        public bool IsT { get { return CurrentForm == Form.T; } }
        public bool IsM { get { return CurrentForm == Form.M; } }
        public bool IsR { get { return CurrentForm == Form.R; } }
        public bool IsF { get { return CurrentForm == Form.F; } }

        public bool IsNotH => !IsH;
        public bool IsNotB => !IsB;
        public bool IsNotC => !IsC;
        public bool IsNotA => !IsA;
        public bool IsNotT => !IsT;
        public bool IsNotM => !IsM;
        public bool IsNotR => !IsR;
        public bool IsNotF => !IsF;

        public PINF() : base(26)
        {
            IsAggressive = false;
        }

        public override void Update()
        {
            bool[] group1   = Conversion.ToBoolArray(this, ColorChannel.Red, 8);
            int partySize   = Conversion.ToInt(this, ColorChannel.Green);
            int currentForm = Conversion.ToInt(this, ColorChannel.Blue);

            IsHiding      = group1[0];
            bool incombat = group1[1];
            IsInControl   = group1[2];
            IsDriving     = group1[3];
            IsLiving      = group1[4];
            IsArtifact    = group1[5];
            IsInRaid      = group1[6];
            IsInDungeon   = group1[7];
            PartySize     = partySize;
            IsSolo        = partySize <= 1;

            if (incombat != IsAggressive)
            {
                if (incombat)
                    App.Class.GetActiveClassSpec()?.OnCombatStart();
                else
                    App.Class.GetActiveClassSpec()?.OnCombatStop();

                IsAggressive = incombat;
            }

            switch (currentForm)
            {
                case 5:  CurrentForm = Form.B; break;
                case 1:  CurrentForm = Form.C; break;
                case 4:  CurrentForm = Form.A; break;
                case 3:  CurrentForm = Form.T; break;
                case 31: CurrentForm = Form.M; break;
                case 2:  CurrentForm = Form.R; break;
                case 27: CurrentForm = Form.F; break;
                default: CurrentForm = Form.H; break;
            }
        }

        public override void LogData()
        {
            string currentForm = CurrentForm.ToString();
            switch (CurrentForm)
            {
                case Form.B: currentForm = "Bear"; break;
                case Form.C: currentForm = "Cat"; break;
                case Form.A: currentForm = "Aquatic"; break;
                case Form.T: currentForm = "Travel"; break;
                case Form.M: currentForm = "Moonkin"; break;
                case Form.R: currentForm = "Tree"; break;
                case Form.F: currentForm = "Flight"; break;
                case Form.H: currentForm = "Human"; break;
            };

            App.Log.Write(Log.LogDetail.High, "Stealth",    IsHiding);
            App.Log.Write(Log.LogDetail.High, "In Combat",  IsAggressive);
            App.Log.Write(Log.LogDetail.High, "In Control", IsInControl);
            App.Log.Write(Log.LogDetail.High, "Driving",    IsDriving);
            App.Log.Write(Log.LogDetail.High, "Alive",      IsLiving);
            App.Log.Write(Log.LogDetail.High, "Artifact",   IsArtifact);
            App.Log.Write(Log.LogDetail.High, "In Raid",    IsInRaid);
            App.Log.Write(Log.LogDetail.High, "In Dungeon", IsInDungeon);
            App.Log.Write(Log.LogDetail.High, "Solo",       IsSolo);
            App.Log.Write(Log.LogDetail.High, "Party Size", PartySize.ToString());
            App.Log.Write(Log.LogDetail.High, "Form",       currentForm);
        }


    }
}
