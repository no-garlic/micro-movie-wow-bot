using System;
using System.Windows.Input;

namespace CommonLib
{
    public class Spec2_0000000 : Spec2OptionBase
    {
        private float TTL               = 0.0f;
        private float CombatTime        = 0.0f;

        private int  Combo              = 0;
        private int  ComboToMax         = 0;
        private bool ComboAtMax         = false;

        private int  Energy             = 0;
        private int  EnergyToMax        = 0;
        private bool EnergyAtMax        = false;

        private bool FightingABoss      = false;
        private bool NotFightingABoss   = true;
        private bool TargetingABoss     = false;
        private bool NotTargetingABoss  = true;

        private bool UsingLI            = false;
        private bool UsingSR            = false;

        private bool EnablePooling      = false;
        private bool EnableShortSR      = false;

        private bool FocusMode          = false;

        private bool UsingAF            = false;
        private bool UsingBE            = false;
        private bool UsingSM            = false;

        private bool ShouldUseTF        = false;

        public Spec2_0000000(MyClassSpec owner) : base(owner, new int[] { 0, 0, 0, 0, 0, 0, 0 })
        {
        }

        public virtual GameKey CheckTF(GameKey without, GameKey with)
        {
            if (ShouldUseTF)
            {
                if (Them.RI.IsActive && Them.RI.Timer > 1.5f)
                    return with;
                if (Them.RK.IsActive && Them.RK.Timer > 1.5f)
                    return with;
            }

            return without;
        }

        public bool CheckPooling()
        {
            if (EnablePooling)
            {
                bool poolEarly = Config.HaveLegendaryGloves;
                bool poolLate  = true;

                if ((ComboAtMax && poolLate) || (ComboToMax > 0 && ComboToMax <= 2 && poolEarly))
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

        private GameKey GetFocusTargetGenerator()
        {
            if (FocusMode)
            {
                if (Owner.Focus.NeedsRK)
                {
                    if (ShouldUseTF)
                        return Owner.TF;
                    return Owner.RK_F;
                }

                if (Owner.Focus.NeedsMF && UsingLI)
                {
                    if (ShouldUseTF)
                        return Owner.TF;
                    return Owner.MF_F;
                }
            }
            return null;
        }
        
        public void UpdateState()
        {
            TTL = Owner.TTL;
            CombatTime = Owner.GetCombatTime();

            Combo         = Me.POWER.Secondary;
            ComboToMax    = Me.POWER.SecondaryToMax;
            ComboAtMax    = Me.POWER.SecondaryAtMax;

            Energy        = Me.POWER.Primary;
            EnergyToMax   = Me.POWER.PrimaryToMax;
            EnergyAtMax   = Me.POWER.PrimaryAtMax;

            FightingABoss     = Owner.FightingABoss;
            NotFightingABoss  = !FightingABoss;
            TargetingABoss    = Owner.TargetingBoss;
            NotTargetingABoss = !TargetingABoss;

            UsingLI = Me.TA.Tier1 == 3;
            UsingSR = Me.TA.Tier5 == 3;

            EnablePooling = UsingSR;
            EnableShortSR = UsingSR;

            FocusMode = Owner.Focus.IsReady && !Config.DisableFocusMode;

            UsingAF = (Owner.Flag.Bit1 == 0 && Me.INFO.IsArtifact && ((FightingABoss && Me.INFO.IsInRaid) || Me.INFO.IsNotInRaid));
            UsingBE = (Owner.Flag.Bit2 == 0 && FightingABoss);
            UsingSM = (Owner.Flag.Bit3 == 0 && TargetingABoss && Me.INFO.PartySize > 1);

            ShouldUseTF = false;
            if (Them.INFO.IsEnemy && Them.INFO.IsLiving && Me.TF.IsReady)
            {
                if ((Me.CC.IsNotActive && Me.POWER.PrimaryToMax >= 60) || Me.POWER.PrimaryToMax >= 80)
                {
                    if (Me.AF.IsReady || Me.AF.Timer > 5.0f)
                    {
                        ShouldUseTF = true;
                    }
                }
            }

        }

        public override GameKey OnATKKey(bool singleKey, bool multiKey, bool areaKey)
        {
            UpdateState();
            
            // Area key overrides single key
            if (areaKey)
            {
                GameKey key = OnAreaKey();
                if (key != null)
                    return key;
            }

            // RK from stealth
            if (Me.INFO.IsHiding)
            {
                return CheckTF(Owner.RK, Owner.RK_TF);
            }
            
            // Use FB at any number of cp during execute phase to ensure rip does not fall off
            if (Me.FB.IsInRange && Them.RI.IsActive && Them.RI.Timer <= 3.0f && Them.HEALTH.Percentage < 0.25f && Combo > 0)
            {
                return CheckTF(Owner.FB, Owner.FB_TF);
            }

            // Use AF
            if (UsingAF && Combo <= 2 && Me.AF.Timer < GCD && (Me.PS.IsActive || Me.BT.IsActive) && (Me.SR.IsActive || !UsingSR))
            {
                // Make sure we have a BT buff if possible before we AF
                if (Me.RE.IsReady && Me.PS.IsActive && Me.BT.IsNotActive && Me.INFO.IsNotHiding)
                {
                    return Owner.RE;
                }
                
                // Use the (AF + TF + Berserk) version in boss mode
                if (UsingBE)
                {
                    return Owner.AF_TFB;
                }

                return Owner.AF;
            }

            // Dont waste a PS buff or use it to heal
            if (Me.RE.IsReady && Me.PS.IsActive && Me.PS.Timer < 1.5f)
            {
                return Owner.RE;
            }

            // Use TF/Berserk as a seperate ability in bossmode, run out of keys for another macro version for each ability
            if (ShouldUseTF && UsingBE && Me.BE.IsReady)
            {
                return Owner.TF_BE;
            }

            // Use SR before 5CP in openner
            if (EnableShortSR && Me.SR.IsNotActive && Them.RI.IsNotActive && Them.RK.IsActive && (Them.MF.IsActive || !UsingLI))
            {
                return CheckTF(Owner.SR, Owner.SR_TF);
            }

            // Use a Finisher
            if (ComboAtMax)
            {
                if (Config.UseOldFinishingMove)
                {
                    return GetOldFinishingMove();
                }

                return GetFinishingMove();
            }

            // Refresh RK if there is still a BT available or if it is within the pandemic window
            if ((CombatTime > 0.5f && Them.RK.IsNotActive) || (CombatTime > 5.0f && Me.BT.IsActive && Them.RK.Timer <= 10.0f))
            {
                // Use the SM version in bossmode
                if (CombatTime > 5.0f && UsingSM && (Me.CC.IsActive || Energy >= 35) && Me.BT.IsActive && Me.BE.Timer < 165.0f)
                {
                    return Owner.RK_SM;
                }

                return CheckTF(Owner.RK, Owner.RK_TF);
            }

            // Use MF if we have the LI talent
            if (UsingLI && (Them.MF.IsNotActive || Them.MF.Timer < 4.6f) && Me.MF.IsInRange && (Me.INFO.IsSolo || Them.INFO.IsAggressive || Me.SH.IsInRange))
            {
                return CheckTF(Owner.MF, Owner.MF_TF);
            }

            // Area damage builders
            if (multiKey)
            {
                // Keep TH on the target for multiKey
                if (Them.TH.IsNotActive)
                {
                    return CheckTF(Owner.TH, Owner.TH_TF);
                }

                // Use SW as the filler for multiKey
                return CheckTF(Owner.SW, Owner.SW_TF);
            }

            // Use MF at range to build CP
            if (UsingLI && ComboToMax > 0 && Me.MF.IsInRange && Me.SH.IsOutOfRange && Me.INFO.IsAggressive && Them.INFO.IsAggressive)
            {
                return CheckTF(Owner.MF, Owner.MF_TF);
            }

            // Use MF to pull and tag
            if (UsingLI && Me.MF.IsInRange && Me.SH.IsOutOfRange && Me.INFO.IsSolo)
            {
                return CheckTF(Owner.MF, Owner.MF_TF);
            }

            // Use a generator ability on the focus target
            GameKey focusTargetGenerator = GetFocusTargetGenerator();
            if (focusTargetGenerator != null)
            {
                return focusTargetGenerator;
            }

            // Pool at 3-4cp with legendary gloves
            if (CheckPooling())
            {
                Console.Out.WriteLine("Pooling (3-4)");
                return Owner.NONE;
            }

            return CheckTF(null, Owner.SH_TF);
        }
        
        private GameKey GetFinishingMove()
        {
            if (Me.RE.IsReady && Me.PS.IsActive && Me.BT.IsNotActive && Me.INFO.IsNotHiding)
            {
                return Owner.RE;
            }

            if (UsingSR && Me.SR.IsNotActive)
            {
                return CheckTF(Owner.SR, Owner.SR_TF);
            }

            const float RIP_TIMER = 9.0f; // APL = 8.0f

            if (Them.RI.IsNotActive || (Them.RI.Timer < RIP_TIMER && Them.HEALTH.Percentage > 0.25f))
            {
                if (!EnablePooling || EnergyToMax < 10 || Me.BEA.Timer > GCD || Me.TF.Timer < 3.0f || Me.CC.Timer > GCD || Them.RI.IsNotActive || Them.RK.Timer < 1.5f)
                {
                    return CheckTF(Owner.RI, Owner.RI_TF);
                }
            }

            if (UsingSR && Me.SR.Timer <= 10.5f) // 7.2?
            {
                if (!EnablePooling || EnergyToMax < 10 || Me.BEA.Timer > GCD || Me.TF.Timer < 3.0f || Me.CC.Timer > GCD || Them.RI.IsNotActive || Them.RK.Timer < 1.5f)
                {
                    return CheckTF(Owner.SR, Owner.SR_TF);
                }
            }

            if (!EnablePooling || EnergyToMax < 10 || Me.BEA.Timer > GCD || Me.TF.Timer < 3.0f)
            {
                return CheckTF(Owner.FB, Owner.FB_TF);
            }
            
            Console.Out.WriteLine("Pooling (5)");
            return Owner.NONE;
        }
        
        private GameKey GetOldFinishingMove()
        {
            // Proc BT
            if (Me.RE.IsReady && Me.PS.IsActive && Me.BT.IsNotActive && Me.INFO.IsNotHiding)
            {
                return Owner.RE;
            }

            // Pool before using a finisher
            if (CheckPooling())
            {
                Console.Out.WriteLine("Pooling (5)");
                return Owner.NONE;
            }

            // SR if it is not up and if the target has RI
            if (UsingSR && (Me.SR.IsNotActive || (Them.RI.IsActive && Them.RI.Timer >= Me.SR.Timer)))
            {
                return CheckTF(Owner.SR, Owner.SR_TF);
            }
                 
            // Use FB if the target is less than 25% health and has RI
            if (Them.RI.IsActive && Me.FB.IsInRange && (Them.HEALTH.Percentage < 0.25f || (Them.RI.Timer > 10.0f && !UsingSR)))
            {
                return CheckTF(Owner.FB, Owner.FB_TF);
            }

            // Use SR if RI is out of range and we are almost energy capped
            if (UsingSR && Me.RI.IsOutOfRange && (EnergyToMax <= 10 || Me.SR.IsNotActive || Me.SR.Timer < GCD))
                return Owner.SR;

            // Use RI
            return CheckTF(Owner.RI, Owner.RI_TF);
        }
        
        private GameKey OnAreaKey()
        {
            bool UseTF = Me.TF.IsReady && ((Me.CC.IsNotActive && Me.POWER.PrimaryToMax >= 60) || Me.POWER.PrimaryToMax >= 80);

            // Keep SR up
            if (ComboAtMax && UsingSR && Me.SR.IsNotActive)
            {
                return UseTF ? Owner.SR_TF : Owner.SR;
            }

            // Dont waste PS
            if (Me.RE.IsReady && Me.PS.IsActive && (Me.HEALTH.Percentage <= 0.5f || Me.PS.Timer < 1.5f))
            {
                return Owner.RE;
            }

            // Keep TH on the target for areaKey
            if (Them.TH.IsNotActive)
            {
                return UseTF ? Owner.TH_TF : Owner.TH;
            }

            // Use SW as the filler for areaKey
            return UseTF ? Owner.SW_TF : Owner.SW;
        }


    }
}

