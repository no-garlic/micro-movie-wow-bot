using System;
using CommonLib;

namespace Compiler
{
    public class LuaAura : LuaDataBase<AU>
    {
        public LuaAura(AU data) : base(data)
        {
        }

        public override string GetLuaFileName()
        {
            return "Aura.lua";
        }
        
        protected override string GetLuaName()
        {
            string name = Constants.Names[m_Data.NameIndex];
            name = Data.IsMe ? "player" + name : "target" + name;

            string luaName = "";

            if (name.Length > 0)
                luaName += Char.ToLower(name[0]);

            if (name.Length > 1)
                luaName += name.Substring(1);

            return luaName;
        }

        public override string ResolveLuaTokens(string text)
        {
            string name = Constants.Names[Data.NameIndex];
            string unit = Data.IsMe ? "\"player\"" : "\"target\"";
            string buff = Data.IsHelpful ? "Buff" : "Debuff";

            text = text.Replace("{aura}", "\"" + name + "\"");
            text = text.Replace("{unit}", unit);
            text = text.Replace("{type}", buff);

            return base.ResolveLuaTokens(text);
        }

    }    
}
