using System;
using System.Diagnostics;
using System.Windows.Input;
using System.Collections.Generic;

namespace CommonLib
{
    public class Class10Data : ClassData
    {
        public override void Init()
        {
        }
    };

    public class Class10Spec : ClassSpec<Class10Data>
    {
    }

    public class Class10 : Class<Class10Data>
    {
        protected override int OnCreate()
        {
            AddClassSpec(268, new Class10Spec());
            AddClassSpec(269, new Class10Spec());
            AddClassSpec(270, new Class10Spec());
            return 10;
        }
    }
}
