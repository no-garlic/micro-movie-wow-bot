using System;
using System.Diagnostics;
using System.Windows.Input;
using System.Collections.Generic;

namespace CommonLib
{
    public class Class12Data : ClassData
    {
        public override void Init()
        {
        }
    };

    public class Class12Spec : ClassSpec<Class12Data>
    {
    }

    public class Class12 : Class<Class12Data>
    {
        protected override int OnCreate()
        {
            AddClassSpec(577, new Class12Spec());
            AddClassSpec(581, new Class12Spec());
            return 12;
        }
    }
}
