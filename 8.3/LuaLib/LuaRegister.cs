﻿using System.Runtime.InteropServices;

namespace LuaLib
{
    /// <summary>
    /// LuaRegister store the name and the delegate to register a native function
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct LuaRegister
    {
        /// <summary>
        /// Function name
        /// </summary>
        public string name;
        /// <summary>
        /// Function delegate
        /// </summary>
        [MarshalAs(UnmanagedType.FunctionPtr)]
        public LuaFunction function;
    }
}
