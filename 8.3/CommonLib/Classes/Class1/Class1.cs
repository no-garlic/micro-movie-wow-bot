using System;
using System.Diagnostics;
using System.Windows.Input;
using System.Collections.Generic;

namespace CommonLib
{
    public class Class1Data : ClassData
    {
        public override void Init()
        {
        }
    };

    public class Class1Spec : ClassSpec<Class1Data>
    {
    }

    public class Class1 : Class<Class1Data>
    {
        protected override int OnCreate()
        {
            AddClassSpec(71, new Class1Spec());
            AddClassSpec(72, new Class1Spec());
            AddClassSpec(73, new Class1Spec());
            return 1;
        }
    }
}
