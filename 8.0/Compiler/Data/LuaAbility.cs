using System;
using CommonLib;

namespace Compiler
{
    public class LuaAbility : LuaDataBase<AB>
    {
        public LuaAbility(AB data) : base(data)
        {
        }

        public override string GetLuaFileName()
        {
            return "Ability.lua";
        }

        public override string ResolveLuaTokens(string text)
        {
            string name = Constants.Names[Data.NameIndex];
            text = text.Replace("{spell}", @"""" + name + @"""");
            return base.ResolveLuaTokens(text);
        }
        

    }    
}
