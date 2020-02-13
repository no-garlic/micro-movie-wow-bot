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
            App.RegisterClass<Class1>(1);   // Warrior
            App.RegisterClass<Class2>(2);   // Paladin
            App.RegisterClass<Class3>(3);   // Hunter
            App.RegisterClass<Class4>(4);   // Rogue
            App.RegisterClass<Class5>(5);   // Priest
            App.RegisterClass<Class6>(6);   // Death Knight
            App.RegisterClass<Class7>(7);   // Shaman
            App.RegisterClass<Class8>(8);   // Mage
            App.RegisterClass<Class9>(9);   // Warlock
            App.RegisterClass<Class10>(10); // Monk
            App.RegisterClass<Class11>(11); // Druid
            App.RegisterClass<Class12>(12); // Demon Hunter
        }
    }
}
