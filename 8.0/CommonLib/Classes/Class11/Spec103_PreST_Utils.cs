using System;
using System.Windows.Input;

namespace CommonLib
{
    public partial class Spec103_PreST : Class11Spec
    {
        private GameKey CheckTF(GameKey without, GameKey with)
        {
            if (ShouldUseTF)
            {
                if (Them.RI.IsActive && Them.RI.Timer > 1.5f && Them.RK.IsActive && Them.RK.Timer > 1.5f)
                    return with;

                if (Them.RI.Timer <= 1.5f && with == RI_TF)
                    return with;

                if (Them.RK.Timer <= 1.5f && with == RK_TF)
                    return with;
            }

            return without;
        }

        private bool CheckPooling()
        {
            if (EnablePooling)
            {
                if ((ComboAtMax && PoolLate) || (ComboToMax > 0 && ComboToMax <= 2 && PoolEarly))
                {
                    if (((EnergyToMax > 15) || (Me.CC.IsActive && Energy < 40)) && Them.RK.Timer > GCD && (Them.MF.Timer > GCD || !UsingLI))
                    {
                        if (Me.SR.IsActive && Them.RI.IsActive)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
    }
}
