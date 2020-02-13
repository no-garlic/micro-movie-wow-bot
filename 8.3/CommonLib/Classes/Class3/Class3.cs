using System;
using System.Diagnostics;
using System.Windows.Input;
using System.Collections.Generic;

namespace CommonLib
{
    public class Class3Data : ClassData
    {
        public override void Init()
        {
        }
    };

    public class Class3Spec : ClassSpec<Class3Data>
    {
    }

    public class Class3 : Class<Class3Data>
    {
        protected override int OnCreate()
        {
            AddClassSpec(253, new Class3Spec());
            AddClassSpec(254, new Class3Spec());
            AddClassSpec(255, new Class3Spec());
            return 3;
        }
    }
}
