using System;

namespace CommonLib
{
    public class LIFE : Data
    {
        public int   Units      { get; private set; }
        public bool  IsMe       { get; private set; }
        public int   Maximum    { get; private set; }
        public int   Current    { get; private set; }
        public float Percentage { get; private set; }
        public bool  NearFull   { get { return Percentage >= 0.95f; } }
        
        public LIFE(bool isMe) : base(isMe ? 25 : 30)
        {
            Units = 1000;
            IsMe  = isMe;
        }
        
        public override void Update()
        {
            int   unit    = Conversion.ToInt(this, ColorChannel.Red);
            int   power   = Conversion.ToInt(this, ColorChannel.Green);
            float percent = Conversion.ToFloat(this, ColorChannel.Blue);

            power = Conversion.Clamp(power, 0, 7);
            double multiple = Math.Pow(10.0, (double) power);

            Maximum    = unit * (int) multiple;
            Percentage = percent;
            Current    = (int) (((float) Maximum) * Percentage);
        }

        public override void LogData()
        {
            string prefix = IsMe ? "" : "Target ";
            
            float fHealth = 1000.0f * Percentage;
            int iHealth = (int) fHealth;
            fHealth = iHealth;
            fHealth *= 0.1f;

            App.Log.Write(Log.LogDetail.High, prefix + "Health Max", Maximum.ToString());
            App.Log.Write(Log.LogDetail.High, prefix + "Health Current", Current.ToString());
            App.Log.Write(Log.LogDetail.High, prefix + "Health Percent", fHealth.ToString());
        }


    }    
}
