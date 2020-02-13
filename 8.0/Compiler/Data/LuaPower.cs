using System;
using CommonLib;

namespace Compiler
{
    public class LuaPower : LuaDataBase<POW>
    {
        public LuaPower(POW data) : base(data)
        {
        }

        public override string GetLuaFileName()
        {
            return "Power.lua";
        }

        public override string ResolveLuaTokens(string text)
        {
            string unit = Data.IsMe ? "\"player\"" : "\"target\"";

            text = text.Replace("{unit}", unit);

            return base.ResolveLuaTokens(text);
        }


    }    
}
