using System;
using System.Diagnostics;
using System.Windows.Input;
using System.Collections.Generic;

namespace CommonLib
{
    public class Class9Data : ClassData
    {
        public override void Init()
        {
        }
    };

    public class Class9Spec : ClassSpec<Class9Data>
    {
    }

    public class Class9 : Class<Class9Data>
    {
        protected override int OnCreate()
        {
            AddClassSpec(265, new Class9Spec());
            AddClassSpec(266, new Class9Spec());
            AddClassSpec(267, new Class9Spec());
            return 9;
        }
    }
}
