using System;
using System.Diagnostics;
using System.Windows.Input;
using System.Collections.Generic;

namespace CommonLib
{
    public class Class8 : Class<Class8Data>
    {
        protected override int OnCreate()
        {
            AddClassSpec(62, new Spec62()); // arcane
            AddClassSpec(63, new Spec63()); // fire
            AddClassSpec(64, new Spec64()); // frost
            return 8;
        }

        public override void OnInit()
        {
            Class8Data data = GetData<Class8Data>();
        }
    }
}
