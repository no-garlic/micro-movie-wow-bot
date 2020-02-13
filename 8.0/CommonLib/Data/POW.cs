using System;

namespace CommonLib
{
    public class POW : Data
    {
        public bool  IsMe           { get; private set; }

        public int   Primary        { get; private set; }
        public int   Secondary      { get; private set; }
        public int   PrimaryMax     { get; private set; }
        public int   SecondaryMax   { get; private set; }

        public int   PrimaryToMax   { get { return PrimaryMax - Primary; } }
        public int   SecondaryToMax { get { return SecondaryMax - Secondary; } }
        public bool  PrimaryAtMax   { get { return Primary >= PrimaryMax; } }
        public bool  SecondaryAtMax { get { return Secondary >= SecondaryMax; } }
        public bool  PrimaryNotAtMax   => !PrimaryAtMax;
        public bool  SecondaryNotAtMax => !SecondaryAtMax;
        
        public POW(bool isMe) : base(22)
        {
            IsMe = isMe;
        }

        public override void Update()
        {
            int primary         = Conversion.ToInt(this, ColorChannel.Red);
            int primaryMax      = Conversion.ToInt(this, ColorChannel.Green);
            int secondaryPacked = Conversion.ToInt(this, ColorChannel.Blue);
            int secondary       = secondaryPacked % 10;
            int secondaryMax    = (secondaryPacked - secondary) / 10;
            
            Primary      = primary;
            PrimaryMax   = primaryMax;
            Secondary    = secondary;
            SecondaryMax = secondaryMax;
        }
        
        public override void LogData()
        {
            App.Log.Write(Log.LogDetail.High, "Combo",      Secondary.ToString());
            App.Log.Write(Log.LogDetail.High, "Combo Max",  SecondaryMax.ToString());
            App.Log.Write(Log.LogDetail.High, "Energy ",    Primary.ToString());
            App.Log.Write(Log.LogDetail.High, "Energy Max", PrimaryMax.ToString());
        }

    }    
}
