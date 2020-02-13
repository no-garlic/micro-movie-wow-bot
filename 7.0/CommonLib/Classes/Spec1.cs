using System;
using System.Windows.Input;

namespace CommonLib
{
    public class Spec1 : MyClassSpec
    {
        public override GameKey OnKeyPress(Key pressed)
        {
            return base.OnKeyPress(pressed);
        }


    }
}
