using System;

namespace CommonLib
{
    public class TINF : Data
    {
        public int  Level        { get; private set; }
        public int  TypeID       { get; private set; }
        public bool Exists       { get; private set; }
        public bool IsFriend     { get; private set; }
        public bool IsEnemy      { get; private set; }
        public bool IsLiving     { get; private set; }
        public bool IsAggressive { get; private set; }
        public bool IsNotLiving  { get { return !IsLiving; } }

        private System.Drawing.Color c;
        
        public TINF() : base(19)
        {
        }

        public override void Update()
        {
            c = GetColor();

            Level        = Conversion.ToInt(this, ColorChannel.Red);
            TypeID       = Conversion.ToInt(this, ColorChannel.Blue);

            bool[] green = Conversion.ToBoolArray(this, ColorChannel.Green, 5);
            IsFriend     = green[0];
            IsEnemy      = green[1];
            Exists       = green[2];
            IsLiving     = green[3];
            IsAggressive = green[4];
        }
        
        public override void LogData()
        {
            string targetType = TypeID.ToString();
            switch (TypeID)
            {
                case 1: targetType = "Normal"; break;
                case 2: targetType = "Elite"; break;
                case 3: targetType = "Minus"; break;
                case 4: targetType = "Rare"; break;
                case 5: targetType = "RareElite"; break;
                case 6: targetType = "WorldBoss"; break;
            }

            App.Log.Write(Log.LogDetail.High, "Tgt Level",     Level.ToString());
            App.Log.Write(Log.LogDetail.High, "Tgt Type",      targetType);
            App.Log.Write(Log.LogDetail.High, "Tgt Exists",    Exists);
            App.Log.Write(Log.LogDetail.High, "Tgt Is Friend", IsFriend);
            App.Log.Write(Log.LogDetail.High, "Tgt Is Enemy",  IsEnemy);
            App.Log.Write(Log.LogDetail.High, "Tgt Alive",     IsLiving);
            App.Log.Write(Log.LogDetail.High, "Tgt Red",       c.R.ToString());
            App.Log.Write(Log.LogDetail.High, "Tgt Green",     c.G.ToString());
            App.Log.Write(Log.LogDetail.High, "Tgt Blue",      c.B.ToString());
        }

    }
}
