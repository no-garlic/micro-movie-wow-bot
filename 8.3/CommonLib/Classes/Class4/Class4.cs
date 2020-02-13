using System;
using System.Diagnostics;
using System.Windows.Input;
using System.Collections.Generic;

namespace CommonLib
{
    public class Class4Data : ClassData
    {
        public override void Init()
        {
        }
    };

    public class Class4Spec : ClassSpec<Class4Data>
    {
    }

    public class Class4 : Class<Class4Data>
    {
        protected override int OnCreate()
        {
            AddClassSpec(259, new Class4Spec());
            AddClassSpec(260, new Class4Spec());
            AddClassSpec(261, new Class4Spec());
            return 4;
        }
    }
}
