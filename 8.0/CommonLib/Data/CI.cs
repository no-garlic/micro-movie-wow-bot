using System;

namespace CommonLib
{
    public class CI : Data
    {
        public int ClassID { get; private set; }
        public int SpecID  { get; private set; }

        public event EventHandler<EventArgs> OnChanged;

        public CI() : base(21)
        {
            ClassID = 0;
            SpecID  = 0;
        }

        public override void Update()
        {
            int classID = Conversion.ToEnumID(this, 15, ColorChannel.Red);
            int specID  = Conversion.ToInt2(this, 32, ColorChannel.Green, ColorChannel.Blue);

            if (classID != ClassID || specID != SpecID)
            {
                ClassID = classID;
                SpecID = specID;
                OnChanged?.Invoke(this, new EventArgs());
            }
        }
        
        public override void LogData()
        {
            //App.Log.Write(Log.LogDetail.High, "Class", ClassID.ToString());
            //App.Log.Write(Log.LogDetail.High, "Spec",  SpecID.ToString());
        }     

    }
}
