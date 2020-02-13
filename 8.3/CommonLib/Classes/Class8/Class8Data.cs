using System;
using System.Diagnostics;
using System.Windows.Input;
using System.Collections.Generic;

namespace CommonLib
{
    public class Class8Data : ClassData
    {
        public class Them
        {
            public TINF INFO    = new TINF();
            public LIFE HEALTH  = new LIFE(false);
            public TTD  TTDIE   = new TTD();
        }

        public class Me
        {
            public OPT  TA      = new OPT();   
            public PINF INFO    = new PINF();
            public LIFE HEALTH  = new LIFE(true);
            public POW  POWER   = new POW(true);
        }

        public Me     m_Me;
        public Them   m_Them;
        public SIGNAL m_Flag;
        public FOCUS  m_Focus;
        
        public override void Init()
        {
            m_Me    = new Me();
            m_Them  = new Them();
            m_Flag  = new SIGNAL();
            m_Focus = new FOCUS();
        }
    };
}
