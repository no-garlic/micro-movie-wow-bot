using System;
using System.Diagnostics;
using System.Windows.Input;
using System.Collections.Generic;

namespace CommonLib
{
    public class Class8Spec : ClassSpec<Class8Data>
    {
        public Class8Data.Me   Me       { get { return GetData().m_Me; } }
        public Class8Data.Them Them     { get { return GetData().m_Them; } }
        public SIGNAL          Flag     { get { return GetData().m_Flag; } }
        public FOCUS           Focus    { get { return GetData().m_Focus; } }

        public Class8Spec() : base()
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
            if (Config.ForceFightingABoss)
                return true;

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

        public override void OnUpdate()
        {
            base.OnUpdate();
        }
    }


}
