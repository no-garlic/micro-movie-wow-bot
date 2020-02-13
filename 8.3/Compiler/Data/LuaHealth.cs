using System;
using CommonLib;

namespace Compiler
{
    public class LuaHealth : LuaDataBase<LIFE>
    {
        public LuaHealth(LIFE data) : base(data)
        {
        }

        public override string GetLuaFileName()
        {
            return "Health.lua";
        }

        public override string ResolveLuaTokens(string text)
        {
            string unit = Data.IsMe ? "\"player\"" : "\"target\"";

            text = text.Replace("{unit}", unit);
            text = text.Replace("{scale}", Data.Units.ToString());

            return base.ResolveLuaTokens(text);
        }


    }    
}
