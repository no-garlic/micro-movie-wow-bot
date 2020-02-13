using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CommonLib;

namespace Compiler
{
    class Program
    {
        static void Main(string[] args)
        {
            App.Create();
            App.Init();

            Addon addon = new Addon();

            addon.RegisterLuaType<AB,     LuaAbility>();
            addon.RegisterLuaType<AU,     LuaAura>();
            addon.RegisterLuaType<CI,     LuaClassInfo>();
            addon.RegisterLuaType<LIFE,   LuaHealth>();
            addon.RegisterLuaType<CRC,    LuaMagicNumber>();
            addon.RegisterLuaType<PINF,   LuaPlayerInfo>();
            addon.RegisterLuaType<POW,    LuaPower>();
            addon.RegisterLuaType<OPT,    LuaTalent>();
            addon.RegisterLuaType<TINF,   LuaTargetInfo>();
            addon.RegisterLuaType<SIGNAL, LuaSignal>();
            addon.RegisterLuaType<FOCUS,  LuaFocusTarget>();

            //addon.Generate(true);            
            addon.Generate(false);            
        }
    }
}
