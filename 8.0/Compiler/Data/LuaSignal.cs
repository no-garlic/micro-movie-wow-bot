using System;
using CommonLib;

namespace Compiler
{
    public class LuaSignal : LuaDataBase<SIGNAL>
    {
        public LuaSignal(SIGNAL data) : base(data)
        {
        }
        
        public override string ResolveLuaTokens(string text)
        {            
            string addon = Constants.AddonName;

            text = text.Replace("{command}", addon.ToUpper());
            text = text.Replace("{slash}",   addon.ToLower());

            return base.ResolveLuaTokens(text);
        }

    }
}

