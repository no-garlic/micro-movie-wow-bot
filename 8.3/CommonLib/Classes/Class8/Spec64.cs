using System;
using System.Windows.Input;

namespace CommonLib
{
    public class Spec64 : Class8Spec
    {        
        public Spec64() : base()
        {
        }
        
        public override bool IsValidKeyToLog(Key key)
        {
            return false;
        }
       
        protected override bool GetIsReady()
        {
            return base.GetIsReady();
        }

        public override GameKey OnKeyPress(Key pressed)
        {
            if (IsNotReady)
                return null;

            return null;
        }
    }
}
