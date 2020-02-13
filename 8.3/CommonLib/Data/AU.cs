using System;

namespace CommonLib
{
    public class AU : Data
    {
        public float Timer       { get; private set; }
        public int   Count       { get; private set; }
        public bool  IsActive    { get; private set; }
        public bool  IsNotActive { get { return !IsActive; } }

        public bool  IsMe        { get; private set; }
        public bool  IsHelpful   { get; private set; }

        public AU(int nameIndex, bool isMe = true, bool isHelpful = true) : base(nameIndex)
        {
            IsActive  = false;
            Timer     = 0.0f;
            Count     = 0;
            IsMe      = isMe;
            IsHelpful = isHelpful;
        }

        public override void Update()
        {
            const float LagTolerance = 0.5f;

            float remaining = Conversion.ToFloat2(this, ColorChannel.Red, ColorChannel.Green);
            int count = Conversion.ToInt(this, ColorChannel.Blue);
            
            IsActive = (remaining > LagTolerance);
            Timer = IsActive ? remaining : 0.0f;
            Count = IsActive ? count : 0;
        }

    }    
}
