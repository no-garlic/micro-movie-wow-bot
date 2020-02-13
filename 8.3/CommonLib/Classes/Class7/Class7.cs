using System;
using System.Diagnostics;
using System.Windows.Input;
using System.Collections.Generic;

namespace CommonLib
{
    public class Class7Data : ClassData
    {
        public override void Init()
        {
        }
    };

    public class Class7Spec : ClassSpec<Class7Data>
    {
    }

    public class Class7 : Class<Class7Data>
    {
        protected override int OnCreate()
        {
            AddClassSpec(262, new Class7Spec());
            AddClassSpec(263, new Class7Spec());
            AddClassSpec(264, new Class7Spec());
            return 7;
        }
    }
}
