using System;
using System.Diagnostics;
using System.Windows.Input;
using System.Collections.Generic;

namespace CommonLib
{
    public class Class5Data : ClassData
    {
        public override void Init()
        {
        }
    };

    public class Class5Spec : ClassSpec<Class5Data>
    {
    }

    public class Class5 : Class<Class5Data>
    {
        protected override int OnCreate()
        {
            AddClassSpec(256, new Class5Spec());
            AddClassSpec(257, new Class5Spec());
            AddClassSpec(258, new Class5Spec());
            return 5;
        }
    }
}
