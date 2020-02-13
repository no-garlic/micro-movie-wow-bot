using System;
using System.Diagnostics;
using System.Windows.Input;
using System.Collections.Generic;

namespace CommonLib
{
    public class Class11Spec : ClassSpec<Class11Data>
    {
        public Class11Data.Me   Me       { get { return GetData().m_Me; } }
        public Class11Data.Them Them     { get { return GetData().m_Them; } }
        public SIGNAL           Flag     { get { return GetData().m_Flag; } }
        public FOCUS            Focus    { get { return GetData().m_Focus; } }

        public Class11Spec() : base()
        {
        }

        protected override float GetTimeToDie()
        {
            if (Them.INFO.IsEnemy == false || Them.INFO.IsLiving == false || Them.INFO.Exists == false)
                return 0.0f;

            if (Them.TTDIE.IsValid)
                return Them.TTDIE.Timer;

            return 0;
        }

        protected override bool IsTargetABoss()
        {
            if (Config.DisableBossMode)
                return false;

            if (Flag.Bit11 == 1) // Force off
                return false;

            if (Flag.Bit12 == 1) // Force on
                return true;

            if (Config.ForceFightingABoss)
                return true;

            // Auto - determine from target
            return Them.INFO.Exists && Them.INFO.IsEnemy && Them.INFO.IsLiving && Them.INFO.IsBoss;
        }

        protected override bool GetIsPaused()
        {
            return Flag.IsPaused || App.PauseOverride;
        }
        
        protected override string GetTalentsAsString()
        {
            return Me.TA.ToString();
        }

    }


}
