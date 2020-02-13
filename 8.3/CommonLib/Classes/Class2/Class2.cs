using System;
using System.Diagnostics;
using System.Windows.Input;
using System.Collections.Generic;

namespace CommonLib
{
    public class Class2Data : ClassData
    {
        public override void Init()
        {
        }
    };

    public class Class2Spec : ClassSpec<Class2Data>
    {
    }

    public class Class2 : Class<Class2Data>
    {
        protected override int OnCreate()
        {
            AddClassSpec(65, new Class10Spec());
            AddClassSpec(66, new Class10Spec());
            AddClassSpec(70, new Class10Spec());
            return 2;
        }
    }
}
