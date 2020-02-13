using System;
using System.Diagnostics;
using System.Windows.Input;
using System.Collections.Generic;

namespace CommonLib
{
    public class Class6Data : ClassData
    {
        public override void Init()
        {
        }
    };

    public class Class6Spec : ClassSpec<Class6Data>
    {
    }

    public class Class6 : Class<Class6Data>
    {
        protected override int OnCreate()
        {
            AddClassSpec(250, new Class6Spec());
            AddClassSpec(251, new Class6Spec());
            AddClassSpec(252, new Class6Spec());
            return 6;
        }
    }
}
