using System;

namespace CommonLib
{
    public class CRC : Data
    {
        public int  X       { get; private set; }
        public int  Y       { get; private set; }
        public int  Z       { get; private set; }        
        public bool IsValid { get; private set; }

        private int m_X;
        private int m_Y;
        private int m_Z;

        public CRC(int nameIndex, int x, int y, int z) : base(nameIndex)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public override void Update()
        {
            m_X = Conversion.ToInt(this, ColorChannel.Red);
            m_Y = Conversion.ToInt(this, ColorChannel.Green);
            m_Z = Conversion.ToInt(this, ColorChannel.Blue);

            IsValid = ((m_X == X) && (m_Y == Y) && (m_Z == Z));
        }
        
        public override void LogData()
        {
            string text = "";

            if (IsValid)
            {
                text = "VALID";
            }
            else
            {
                text = $"X:{X}={m_X} Y:{Y}={m_Y} Z:{Z}={m_Z}";
            }

            //App.Log.Write(Log.LogDetail.High, "CRC", text);
        } 
    }
}
