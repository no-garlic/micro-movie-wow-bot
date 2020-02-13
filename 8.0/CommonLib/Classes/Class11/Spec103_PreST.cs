using System;
using System.Windows.Input;

namespace CommonLib
{
    public partial class Spec103_PreST : Class11Spec
    {
        private int  Combo         => Me.POWER.Secondary;
        private int  ComboToMax    => Me.POWER.SecondaryToMax;
        private bool ComboAtMax    => Me.POWER.SecondaryAtMax;

        private int  Energy        => Me.POWER.Primary;
        private int  EnergyToMax   => Me.POWER.PrimaryToMax;
        private bool EnergyAtMax   => Me.POWER.PrimaryAtMax;      

        private bool UsingLI       => Me.TA.Tier1 == 3;
        private bool UsingSR       => Me.TA.Tier6 == 3;
        private bool UsingBS       => Me.TA.Tier6 == 2;
        private bool UsingST       => Me.TA.Tier6 == 1;
        private bool UsingBT       => Me.TA.Tier7 == 2;
        private bool UsingFF       => Me.TA.Tier7 == 3 && ((FightingABoss && Me.INFO.IsInRaid) || Me.INFO.IsNotInRaid);
        private bool UsingWC       => Me.TA.Tier2 == 3;
        private bool UsingMB       => Me.TA.Tier4 == 1;
        private bool UsingBE       => FightingABoss;
        private bool UsingSM       => TargetingABoss && Me.INFO.IsHuman && Me.INFO.PartySize > 1;
        private bool UsingTH       => Config.UseTHwithST && (Flag.Bit10 == 0);

        private bool EnableShortSR => UsingSR && true;
        private bool EnablePooling => UsingSR && true;

        private bool PoolEarly     => EnablePooling && false; // Legendary Gloves
        private bool PoolLate      => EnablePooling && true;

        private bool FocusMode     => Focus.IsReady && !Config.DisableFocusMode;

        private bool ShouldUseTF   => Them.INFO.IsEnemy && Them.INFO.IsLiving && Me.TF.IsReady && ((Me.CC.IsNotActive && Me.POWER.PrimaryToMax >= 60) || Me.POWER.PrimaryToMax >= 80);

        // -----------------------------------------------------------------------------------------------

        protected override bool GetIsReady()
        {
            if (Config.IgnoreReadyCheck)
                return true;

            if (Me.INFO.IsNotC)
                return false;

            if (Them.INFO.Exists == false || Them.INFO.IsEnemy == false || Them.INFO.IsNotLiving == true)
                return false;

            if (!(Me.INFO.IsHiding || Me.INFO.IsAggressive) || Me.INFO.IsNotInControl || Me.INFO.IsDriving || Me.INFO.IsNotLiving)
                return false;

            return base.GetIsReady();
        }

        private GameKey OnATKKey(bool aoeKey)
        {
            if (IsNotReady)
            {
                // Heal with the PS buff if not not ready
                if (Me.RE.IsReady && Me.PS.IsActive && Me.INFO.IsNotHiding)
                {
                    return RE;
                }

                // Pull with LI if alone and not stealthed
                if (UsingLI && Me.INFO.IsNotHiding && Me.INFO.IsSolo && Them.INFO.IsLiving && Them.INFO.IsEnemy)
                {
                    if (Me.MF.IsInRange)
                    {
                        return CheckTF(MF, MF_TF);
                    }
                }

                // Otherwise if not ready, do nothing
                return null;
            }
            else
            {
                // Pull with LI if the target is far away and I am alone
                if (UsingLI && Me.MF.IsInRange && Me.SH.IsOutOfRange && Me.INFO.IsSolo)
                {
                    return CheckTF(MF, MF_TF);
                }
            }

            // RK from stealth
            if (Me.INFO.IsHiding)
            {
                return CheckTF(RK, RK_TF);
            }
            
            // Use FB at any number of cp during execute phase to ensure rip does not fall off
            if (Me.FB.IsInRange && Them.RI.IsActive && Them.RI.Timer <= 3.0f && Them.HEALTH.Percentage < 0.25f && Combo > 0)
            {
                return CheckTF(FB, FB_TF);
            }

            // Dont waste a PS buff or use it to heal
            if ((Me.RE.IsReady && Me.PS.IsActive && Me.PS.Timer < 1.5f) || Me.HEALTH.Percentage < 0.25f)
            {
                return RE;
            }

            // Use TF/Berserk in bossmode
            if (ShouldUseTF && UsingBE && Me.BE.IsReady)
            {
                return TF_BE;
            }

            // Use SR before 5CP in openner
            if (EnableShortSR && Me.SR.IsNotActive && Them.RI.IsNotActive && Them.RK.IsActive && (Them.MF.IsActive || !UsingLI))
            {
                return CheckTF(SR, SR_TF);
            }

            // Use a finisher at max cp
            if (ComboAtMax)
            {
                return GetFinisher(aoeKey);
            }

            // Use a generator to gain cp
            return GetGenerator(aoeKey);
        }

        private GameKey GetGenerator(bool aoeKey)
        {
            // Refresh RK if there is still a BT available or if it is within the pandemic window
            if ((CombatTime > 0.5f && Them.RK.IsNotActive) || (CombatTime > 5.0f && Me.BT.IsActive && Them.RK.Timer <= 10.0f))
            {
                // Use the SM version in bossmode
                if (CombatTime > 5.0f && UsingSM && (Me.CC.IsActive || Energy >= 35) && Me.BT.IsActive && Me.BE.Timer < 165.0f)
                {
                    return RK_SM;
                }
                return CheckTF(RK, RK_TF);
            }

            // Area damage builders
            if (aoeKey)
            {
                return GetAOEGenerator();
            }

            // Use FF with 0..1 cp's
            if (UsingFF && ComboToMax >= 3 && Me.FF.Timer < GCD && (Me.SR.IsActive || !UsingSR))
            {
                return FF;
            }

            // Use MF if we have the LI talent
            if (UsingLI && (Them.MF.IsNotActive || Them.MF.Timer < 4.6f) && Me.MF.IsInRange && (Me.INFO.IsSolo || Them.INFO.IsAggressive || Me.SH.IsInRange))
            {
                return CheckTF(MF, MF_TF);
            }

            // Use MF at range to build CP
            if (UsingLI && ComboToMax > 0 && Me.MF.IsInRange && Me.SH.IsOutOfRange && Me.INFO.IsAggressive && Them.INFO.IsAggressive)
            {
                return CheckTF(MF, MF_TF);
            }
            
            // Use MF to pull and tag
            if (UsingLI && Me.MF.IsInRange && Me.SH.IsOutOfRange && Me.INFO.IsSolo)
            {
                return CheckTF(MF, MF_TF);
            }

            // Use a generator ability on the focus target
            GameKey key = null;
            if ((key = GetFocusTargetGenerator(aoeKey)) != null)
            {
                return key;
            }

            // Pool at 3-4cp with legendary gloves
            if (CheckPooling())
            {
                return NONE;
            }

            // Hold a BT in the openner for ST to use on RI
            if (FightingABoss && UsingST && Me.BT.IsActive && Them.RI.IsNotActive && Them.RK.IsActive && CombatTime < 10.0f)
            {
                return CheckTF(MF, MF_TF);
            }

            // Maintain TH on target
            if (UsingTH && (Them.TH.IsNotActive || Them.TH.Timer < 4.0f))
            {
                return CheckTF(TH, TH_TF);
            }

            // Shred
            return CheckTF(null, SH_TF);
        }

        private GameKey GetAOEGenerator()
        {
            // Keep TH on the target for aoeKey
            if (Them.TH.IsNotActive)
            {
                return CheckTF(TH, TH_TF);
            }

            // Dont get energy capped waiting for BS cooldown
            if (EnergyToMax <= 1 && Me.SW.IsNotReady)
            {
                return NONE;
            }

            // Use SW as the filler for aoeKey
            return CheckTF(SWBS, SW_TF);
        }

        private GameKey GetFocusTargetGenerator(bool aoeKey)
        {
            // Only if we have a focus target
            if (FocusMode)
            {
                // Check if the focus target has rake
                if (Focus.NeedsRK)
                {
                    if (ShouldUseTF)
                        return TF;
                    return RK_F;
                }

                // Check if the focus target has moonfire
                if (Focus.NeedsMF && UsingLI)
                {
                    if (ShouldUseTF)
                        return TF;
                    return MF_F;
                }
            }

            // Dont do anything to the focus target
            return null;
        }

        private GameKey GetFinisher(bool aoeKey)
        {
            // Cast regrowth if at max cp to apply BT if using BT
            if (UsingBT && Me.RE.IsReady && Me.PS.IsActive && Me.BT.IsNotActive && Me.INFO.IsNotHiding)
            {
                return RE;
            }

            // Apply savage roar if it is not applied
            if (UsingSR && Me.SR.IsNotActive)
            {
                return CheckTF(SR, SR_TF);
            }

            // Apply FB if the target is low enough health that FB would do more damage than rip
            if (!UsingSR && !EnablePooling && TimeToDie > 0.0f && TimeToDie < 7.0f)
            {
                return CheckTF(FB, FB_TF);
            }

            // If the remaining aura timer is lower than this value then it should be reapplied
            const float ReapplyRipTimer = 9.0f;
            const float ReapplySRTimer  = 10.5f;

            // Reapply rip if it's remaining duration is low and the target health is > 25%
            if (Them.RI.IsNotActive || (Them.RI.Timer < ReapplyRipTimer && Them.HEALTH.Percentage > 0.25f))
            {
                // stop pooling if we are not pooling, about to be energy capped, need to use berserk, tf is about to drop, or we need to reapply rip or rake
                if (!EnablePooling || EnergyToMax < 10 || Me.BEA.Timer > GCD || Me.TF.Timer < 3.0f || Me.CC.Timer > GCD || Them.RI.IsNotActive || Them.RK.Timer < 1.5f)
                {
                    if (UsingST)
                    {
                        if (Them.RI.IsActive && Them.RI.Timer > 1.0f)
                            return CheckTF(FB, FB_TF);

                        if (UsingBT && Me.INFO.IsNotSolo && (Me.BT.IsNotActive || Me.BT.Timer < 0.5f))
                            return CheckTF(FB, FB_TF);

                        if (Me.TF.IsReady)
                            return RI_TF;
                    }

                    return CheckTF(RI, RI_TF);
                }
            }

            // Reapply savage roar if it's remaining duration is low
            if (UsingSR && Me.SR.Timer <= ReapplySRTimer)
            {
                if (!EnablePooling || EnergyToMax < 10 || Me.BEA.Timer > GCD || Me.TF.Timer < 3.0f || Me.CC.Timer > GCD || Them.RI.IsNotActive || Them.RK.Timer < 1.5f)
                {
                    // stop pooling if we are not pooling, about to be energy capped, need to use berserk, tf is about to drop, or we need to reapply rip or rake
                    return CheckTF(SR, SR_TF);
                }
            }

            // Use FB if we dont need to reapply rip or SR
            if (!EnablePooling || EnergyToMax < 10 || Me.BEA.Timer > GCD || Me.TF.Timer < 3.0f)
            {
                return CheckTF(FB, FB_TF);
            }
            
            // Keep pooling
            return NONE;
        }
                
        private GameKey OnINTKey()
        {
            // Use wild charge if we have the talent and it is ready and in range
            if (Me.WC.IsReady && Me.WC.IsInRange && UsingWC)
                return null;

            // Use skull bash if it is ready and in range
            if (Me.SB.IsReady && Me.SB.IsInRange)
                return SB;

            // Use mighty bash if we have the talent and it is ready and in range
            if (Me.MB.IsReady && Me.MB.IsInRange && UsingMB)
                return MB;

            // Use war stomp if it is ready and if we are not human
            if (Me.WS.IsReady && Me.WS.IsInRange && !Me.INFO.IsHuman)
                return WS;

            // Use maim as a fallback in solo
            if (Me.INFO.IsSolo && Combo <=3 && Me.MA.IsReady && Me.MA.IsInRange)
                return MA;

            return null;
        }

    }
}
