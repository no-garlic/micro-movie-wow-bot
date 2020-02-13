using System;
using System.Diagnostics;
using System.Windows.Input;
using System.Collections.Generic;

namespace CommonLib
{
    public class Class11 : Class<Class11Data>
    {
        protected override int OnCreate()
        {
            AddClassSpec(102, new Spec102()); // bal
            AddClassSpec(103, new Spec103()); // fer
            AddClassSpec(104, new Spec104()); // gua
            AddClassSpec(105, new Spec105()); // res
            return 11;
        }

        public override void OnInit()
        {
            Class11Data data = GetData<Class11Data>();
        }
    }
}
