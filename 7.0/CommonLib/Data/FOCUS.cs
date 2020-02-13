using System;

namespace CommonLib
{
    public class FOCUS : Data
    {
        public bool IsReady { get; private set; }
        public bool NeedsRI { get; private set; }
        public bool NeedsRK { get; private set; }
        public bool NeedsMF { get; private set; }

        private System.Drawing.Color c;
        
        public FOCUS() : base(36)
        {
        }

        public override void Update()
        {
            c = GetColor();

            bool isReady, needsRI, needsRK, needsMF;
            Conversion.ToBools(this, ColorChannel.Red, out isReady, out needsRI, out needsRK, out needsMF);

            IsReady = isReady;
            NeedsRI = needsRI;
            NeedsRK = needsRK;
            NeedsMF = needsMF;
        }
        
        public override void LogData()
        {
        }

    }
}
