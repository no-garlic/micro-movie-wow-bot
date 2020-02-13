using System;
using CommonLib;

namespace Compiler
{
    public class LuaPet : LuaDataBase<PET>
    {
        public LuaPet(PET data) : base(data)
        {
        }

        public override string GetLuaFileName()
        {
            return "Pet.lua";
        }

        public override string ResolveLuaTokens(string text)
        {
            return base.ResolveLuaTokens(text);
        }
        

    }    
}
