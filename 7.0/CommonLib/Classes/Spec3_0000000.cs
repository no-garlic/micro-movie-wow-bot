using System;
using System.Windows.Input;

namespace CommonLib
{
    public class Spec3_0000000 : Spec3OptionBase
    {
        public Spec3_0000000(MyClassSpec owner) : base(owner, null)
        {
        }
        
        public override GameKey OnATKKey(bool singleKey, bool multiKey, bool areaKey)
        {
            areaKey |= multiKey;

            if (Me.POWER.PrimaryToMax <= 5)
            {
                return Owner.IF;
            }

            if (Me.TH.IsReady && Them.INFO.Exists)
            {
                if (areaKey)
                {
                    return null;
                }
                return Owner.TH;
            }

            if (singleKey)
            {
                if (Me.MG.IsReady)
                {
                    return Owner.MG;
                }

                if (Me.MF.IsReady &&                                                                                    // in range
                   (Them.MF.IsNotActive || Them.MF.Timer < 1.5f || (Me.GG.IsActive && Me.POWER.PrimaryToMax >= 20)) &&  // missing debuff or gg proc
                   (Me.INFO.IsNotAggressive || Them.INFO.IsAggressive))                                                 // them in combat or me out of combat
                {
                    return Owner.MF;
                }
            }

            if (areaKey)
            {
                return Owner.SW;
            }
            
            return null; // SW
        }


    }
}
