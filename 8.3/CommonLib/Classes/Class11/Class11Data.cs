using System;
using System.Diagnostics;
using System.Windows.Input;
using System.Collections.Generic;

namespace CommonLib
{
    public class Class11Data : ClassData
    {
        public class Them
        {
            public TINF INFO    = new TINF();
            public LIFE HEALTH  = new LIFE(false);
            public TTD  TTDIE   = new TTD();
            public AU   RI      = new AU(0, false, false);
            public AU   RK      = new AU(1, false, false);
            public AU   TH      = new AU(2, false, false);
            public AU   MF      = new AU(3, false, false);    
        }

        public class Me
        {
            public OPT  TA      = new OPT();   
            public PINF INFO    = new PINF();
            public LIFE HEALTH  = new LIFE(true);
            public POW  POWER   = new POW(true);
            public AB   MG      = new AB(7);
            public AB   MF      = new AB(9);
            public AB   TH      = new AB(10);
            public AB   SA      = new AB(33);
            public AB   SW      = new AB(11);
            public AB   SH      = new AB(12);
            public AB   RI      = new AB(13);
            public AB   RK      = new AB(14);
            public AB   FB      = new AB(15);
            public AB   RE      = new AB(37);
            public AB   TF      = new AB(17);
            public AB   WC      = new AB(27);  
            public AB   SB      = new AB(28);  
            public AB   MA      = new AB(29);
            public AB   MB      = new AB(31);  
            public AB   BE      = new AB(35);
            public AB   FF      = new AB(47);
            public AB   WS      = new AB(44);
            public AB   FR      = new AB(18);
            public AB   PU      = new AB(43);
            public AB   IR      = new AB(45);
            public AB   ML      = new AB(46);
            public AU   IF      = new AU(41);
            public AU   FRE     = new AU(42);
            public AU   SR      = new AU(32);
            public AU   PS      = new AU(4);
            public AU   BT      = new AU(5);
            public AU   CC      = new AU(6);
            public AU   GG      = new AU(8);
            public AU   BEA     = new AU(39);
            public AU   TFA     = new AU(38);
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
