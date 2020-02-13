using System;

namespace CommonLib
{
    public enum TargetType
    {
        UnSpecified = 0,
        Normal      = 1,
        Elite       = 2,
        Minus       = 3,
        Rare        = 4,
        RareElite   = 5,
        WorldBoss   = 6,
        Trivial     = 7
    };

    public class TINF : Data
    {
        public int        Level        { get; private set; }
        public TargetType Type         { get; private set; }
        public bool       Exists       { get; private set; }
        public bool       IsFriend     { get; private set; }
        public bool       IsEnemy      { get; private set; }
        public bool       IsLiving     { get; private set; }
        public bool       IsAggressive { get; private set; }
        public bool       IsBoss       { get; private set; }
        public bool       IsDummy      { get; private set; }
        public bool       IsCasting    { get; private set; }

        public bool IsNotLiving  => !IsLiving;
        public bool IsNotCasting => !IsCasting;


        private System.Drawing.Color c;
        
        public TINF() : base(19)
        {
        }

        public override void Update()
        {
            c = GetColor();

            Level        = Conversion.ToInt(this, ColorChannel.Red);
            int typeID   = Conversion.ToInt(this, ColorChannel.Blue);
            Type         = (TargetType) typeID;

            bool[] green = Conversion.ToBoolArray(this, ColorChannel.Green, 8);
            IsFriend     = green[0];
            IsEnemy      = green[1];
            Exists       = green[2];
            IsLiving     = green[3];
            IsAggressive = green[4];
            IsBoss       = green[5];
            IsDummy      = green[6];
            IsCasting    = green[7];
        }
        
        public override void LogData()
        {
            App.Log.Write(Log.LogDetail.High, "Tgt Level",      Level.ToString());
            App.Log.Write(Log.LogDetail.High, "Tgt Type",       Type.ToString());
            App.Log.Write(Log.LogDetail.High, "Tgt Exists",     Exists);
            App.Log.Write(Log.LogDetail.High, "Tgt Is Friend",  IsFriend);
            App.Log.Write(Log.LogDetail.High, "Tgt Is Enemy",   IsEnemy);
            App.Log.Write(Log.LogDetail.High, "Tgt Alive",      IsLiving);
            App.Log.Write(Log.LogDetail.High, "Tgt Is Boss",    IsBoss);
            App.Log.Write(Log.LogDetail.High, "Tgt Is Dummy",   IsDummy);
            App.Log.Write(Log.LogDetail.High, "Tgt Is Casting", IsCasting);
            App.Log.Write(Log.LogDetail.High, "Tgt Red",        c.R.ToString());
            App.Log.Write(Log.LogDetail.High, "Tgt Green",      c.G.ToString());
            App.Log.Write(Log.LogDetail.High, "Tgt Blue",       c.B.ToString());
        }

    }
}
