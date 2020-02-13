using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonLib;

namespace CommonLib
{
    public static partial class App
    {
        public static void RegisterClasses()
        {
            App.RegisterClass<Class8 >(8);  // mage
            App.RegisterClass<Class11>(11); // druid
        }
    }
}
