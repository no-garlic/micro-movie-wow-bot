using System;
using CommonLib;

namespace Compiler
{
    public class LuaMagicNumber : LuaDataBase<CRC>
    {
        public LuaMagicNumber(CRC data) : base(data)
        {
        }

        public override string GetLuaFileName()
        {
            return "MagicNumber.lua";
        }

        public override string ResolveLuaTokens(string text)
        {
            text = text.Replace("{x}", Data.X.ToString());
            text = text.Replace("{y}", Data.Y.ToString());
            text = text.Replace("{z}", Data.Z.ToString());

            return base.ResolveLuaTokens(text);
        }


    }
}
