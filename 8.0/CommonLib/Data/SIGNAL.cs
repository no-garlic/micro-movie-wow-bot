using System;

namespace CommonLib
{
    public class SIGNAL : Data
    {
        public const int PauseBit = 24;

        public int  Bits     { get; private set; }
        public int  Bit1     { get { return TestBit(1);  } }    // TH
        public int  Bit2     { get { return TestBit(2);  } }
        public int  Bit3     { get { return TestBit(3);  } }
        public int  Bit4     { get { return TestBit(4);  } }
        public int  Bit5     { get { return TestBit(5);  } }
        public int  Bit6     { get { return TestBit(6);  } }
        public int  Bit7     { get { return TestBit(7);  } }
        public int  Bit8     { get { return TestBit(8);  } }
        public int  Bit9     { get { return TestBit(9);  } }
        public int  Bit10    { get { return TestBit(10); } }
        public int  Bit11    { get { return TestBit(11); } }
        public int  Bit12    { get { return TestBit(12); } }
        public int  Bit13    { get { return TestBit(13); } }
        public int  Bit14    { get { return TestBit(14); } }
        public int  Bit15    { get { return TestBit(15); } }
        public int  Bit16    { get { return TestBit(16); } }
        public int  Bit17    { get { return TestBit(17); } }
        public int  Bit18    { get { return TestBit(18); } }
        public int  Bit19    { get { return TestBit(19); } }
        public int  Bit20    { get { return TestBit(20); } }
        public int  Bit21    { get { return TestBit(21); } }
        public int  Bit22    { get { return TestBit(22); } }
        public int  Bit23    { get { return TestBit(23); } }
        public bool IsPaused { get { return TestBit(PauseBit) != 0; } }
    
        public SIGNAL() : base(34)
        {
            Bits = 0;
        }

        public override void Update()
        {
            int word1 = Conversion.ToInt(this, ColorChannel.Red);
            int word2 = Conversion.ToInt(this, ColorChannel.Green);
            int word3 = Conversion.ToInt(this, ColorChannel.Blue);

            Bits = word1 + (word2 << 8) + (word3 << 16);
        }

        private int TestBit(int bit)
        {
            int mask = (1 << (bit - 1));
            return ((mask & Bits) == mask) ? 1 : 0;
        }
        
        public override void LogData()
        {
            string bitString = Convert.ToString(Bits, 2);
            App.Log.Write(Log.LogDetail.High, "Signal", bitString);
        }

    }
}
