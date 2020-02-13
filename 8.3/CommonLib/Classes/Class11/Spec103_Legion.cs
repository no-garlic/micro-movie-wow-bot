using System;
using System.Windows.Input;

namespace CommonLib
{
    public class Spec103_Legion : Class11Spec
    {
        public GameKey  NONE    = new GameKey(Key.None);         

        // --------------- Input Keys ---------------
        public GameKey  INT     = new GameKey(Key.D1);          // Interrupt
        public GameKey  ST      = new GameKey(Key.D2);          // Single Target
        public GameKey  AOE     = new GameKey(Key.D3);          // Multi Target

        // ----------- Default Action Bar -----------
        // Wild Charge                       (Key.D1);          // Wild Charge
        // Shred                             (Key.D2);          // Shred
        // Rake                              (Key.D3);          // Rake
        public GameKey  TH      = new GameKey(Key.D4);          // Thrash
        public GameKey  SWBS    = new GameKey(Key.D5);          // Swipe or Brutal Slash
        // Regrowth                          (Key.D6);          // Regrowth
        public GameKey  RI      = new GameKey(Key.D7);          // Rip
        public GameKey  FB      = new GameKey(Key.D8);          // Ferocious Bite
        public GameKey  TF      = new GameKey(Key.D9);          // Tiger's Fury
        // Prowl                             (Key.D0);          // Prowl
        public GameKey  SB      = new GameKey(Key.OemMinus);    // Skull Bash
        public GameKey  MB      = new GameKey(Key.OemPlus);     // Mighty Bash

        // --------- Bottom Left Action Bar ---------
        public GameKey  MF      = new GameKey(Key.F1);          // Moonfire
        // Healing Potion                    (Key.F2);          // Healing Potion
        //                                   (Key.F3);          //
        // Survival Instinct                 (Key.F4);          // Survival Instinct
        public GameKey  TF_BE   = new GameKey(Key.F5);          // Tiger's Fury + Berserk
        //                                   (Key.F6);          //

        // --------- Bottom Right Action Bar ---------
        public GameKey  SR      = new GameKey(Key.F7);          // Savage Roar
        //                                   (Key.F8); (Key.~); // Maim
        //                                   (Key.F9);          // Hibernate
        //                                   (Key.F10);         // Entangling Roots
        //                                   (Key.Y);           // Stampeding Roar
        //                                   (Key.T);           // Dash

        // ----------- Right Action Bar 1 ------------
        public GameKey  WS      = new GameKey(Key.Multiply);    // War Stomp
        public GameKey  RK_F    = new GameKey(Key.F11);         // (Focus Target) Rake
        public GameKey  MF_F    = new GameKey(Key.F12);         // (Focus Target) Moonfire
            
        // ----------- Right Action Bar 2 ------------
        public GameKey  RK      = new GameKey(Key.NumPad0);     // Rake
        public GameKey  RK_TF   = new GameKey(Key.NumPad1);     // Tiger's Fury + Rake
        public GameKey  RK_SM   = new GameKey(Key.NumPad2);     // Shadowmeld + Rake
        public GameKey  RE      = new GameKey(Key.NumPad3);     // Regrowth (Macro - Dont Change Stance)
        //public GameKey        = new GameKey(Key.NumPad4);     // 
        public GameKey  SH_TF   = new GameKey(Key.NumPad5);     // Tiger's Fury + Shred
        public GameKey  RI_TF   = new GameKey(Key.NumPad6);     // Tiger's Fury + Rip
        public GameKey  FB_TF   = new GameKey(Key.NumPad7);     // Tiger's Fury + Ferocious Bite
        public GameKey  SW_TF   = new GameKey(Key.NumPad8);     // Tiger's Fury + Swipe or Brutal Slash
        public GameKey  TH_TF   = new GameKey(Key.NumPad9);     // Tiger's Fury + Thrash
        public GameKey  SR_TF   = new GameKey(Key.Add);         // Tiger's Fury + Savage Roar
        public GameKey  MF_TF   = new GameKey(Key.Subtract);    // Tiger's Fury + Moonfire
        
        // ----------------------------------------------------

        private int  Combo         = 0;
        private int  ComboToMax    = 0;
        private bool ComboAtMax    = false;

        private int  Energy        = 0;
        private int  EnergyToMax   = 0;
        private bool EnergyAtMax   = false;

        private bool UsingLI       = false;
        private bool UsingSR       = false;
        private bool UsingBS       = false;

        private bool EnablePooling = false;
        private bool EnableShortSR = false;

        private bool FocusMode     = false;

        private bool UsingBE       = false;
        private bool UsingSM       = false;

        private bool ShouldUseTF   = false;

        private bool HaveLegendaryGloves = false;

        // ----------------------------------------------------

        public Spec103_Legion() : base()
        {
        }

        public override bool IsValidKeyToLog(Key key)
        {
            return key == INT.Key || key == ST.Key || key == AOE.Key;
        }
        
        protected override bool GetIsReady()
        {
            if (Config.IgnoreReadyCheck)
            {
                return true;
            }

            if (Me.INFO.IsNotC)
            {
                return false;
            }

            if (Them.INFO.Exists == false || Them.INFO.IsEnemy == false || Them.INFO.IsNotLiving == true)
            {
                return false;
            }

            if (!(Me.INFO.IsHiding || Me.INFO.IsAggressive) || Me.INFO.IsNotInControl || Me.INFO.IsDriving || Me.INFO.IsNotLiving)
            {
                return false;
            }

            return base.GetIsReady();
        }

        public override GameKey OnKeyPress(Key pressed)
        {
            if (IsNotReady)
            {
                if ((ST.WasPressed(pressed) || AOE.WasPressed(pressed)) && Me.RE.IsReady && Me.PS.IsActive && Me.INFO.IsNotHiding)
                {
                    return RE;
                }

                return null;
            }

            if (AOE.WasPressed(pressed))
            {
                return OnATKKey(false, true);
            }
            else if (ST.WasPressed(pressed))
            {
                return OnATKKey(true, false);
            }
            else if (INT.WasPressed(pressed))
            {
                return OnINTKey();
            }

            return null;
        }        
        
        private GameKey CheckTF(GameKey without, GameKey with)
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

        private bool CheckPooling()
        {
            if (EnablePooling)
            {
                bool poolEarly = HaveLegendaryGloves;
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
                if (Focus.NeedsRK)
                {
                    if (ShouldUseTF)
                        return TF;
                    return RK_F;
                }

                if (Focus.NeedsMF && UsingLI)
                {
                    if (ShouldUseTF)
                        return TF;
                    return MF_F;
                }
            }
            return null;
        }
        
        private void UpdateState()
        {
            Combo         = Me.POWER.Secondary;
            ComboToMax    = Me.POWER.SecondaryToMax;
            ComboAtMax    = Me.POWER.SecondaryAtMax;

            Energy        = Me.POWER.Primary;
            EnergyToMax   = Me.POWER.PrimaryToMax;
            EnergyAtMax   = Me.POWER.PrimaryAtMax;

            UsingLI = Me.TA.Tier1 == 3;
            UsingSR = Me.TA.Tier6 == 3;
            UsingBS = Me.TA.Tier6 == 2;

            EnablePooling = UsingSR;
            EnableShortSR = UsingSR;

            FocusMode = Focus.IsReady && !Config.DisableFocusMode;

            UsingBE = (Flag.Bit2 == 0 && FightingABoss);
            UsingSM = (Flag.Bit3 == 0 && TargetingABoss && Me.INFO.PartySize > 1);

            ShouldUseTF = false;
            if (Them.INFO.IsEnemy && Them.INFO.IsLiving && Me.TF.IsReady)
            {
                if ((Me.CC.IsNotActive && Me.POWER.PrimaryToMax >= 60) || Me.POWER.PrimaryToMax >= 80)
                {
                    ShouldUseTF = true;
                }
            }
        }

        private GameKey OnATKKey(bool singleKey, bool aoeKey)
        {
            UpdateState();
            
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
            if (Me.RE.IsReady && Me.PS.IsActive && Me.PS.Timer < 1.5f)
            {
                return RE;
            }

            // Use TF/Berserk as a seperate ability in bossmode, run out of keys for another macro version for each ability
            if (ShouldUseTF && UsingBE && Me.BE.IsReady)
            {
                return TF_BE;
            }

            // Use SR before 5CP in openner
            if (EnableShortSR && Me.SR.IsNotActive && Them.RI.IsNotActive && Them.RK.IsActive && (Them.MF.IsActive || !UsingLI))
            {
                return CheckTF(SR, SR_TF);
            }

            // Use a Finisher
            if (ComboAtMax)
            {
                return GetFinishingMove();
            }

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

            // Use MF if we have the LI talent
            if (UsingLI && (Them.MF.IsNotActive || Them.MF.Timer < 4.6f) && Me.MF.IsInRange && (Me.INFO.IsSolo || Them.INFO.IsAggressive || Me.SH.IsInRange))
            {
                return CheckTF(MF, MF_TF);
            }

            // Area damage builders
            if (aoeKey)
            {
                // Keep TH on the target for aoeKey
                if (Them.TH.IsNotActive)
                {
                    return CheckTF(TH, TH_TF);
                }

                // Use SW as the filler for aoeKey
                return CheckTF(SWBS, SW_TF);
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
            GameKey focusTargetGenerator = GetFocusTargetGenerator();
            if (focusTargetGenerator != null)
            {
                return focusTargetGenerator;
            }

            // Pool at 3-4cp with legendary gloves
            if (CheckPooling())
            {
                //Console.Out.WriteLine("Pooling (3-4)");
                return NONE;
            }

            return CheckTF(null, SH_TF);
        }
        
        private GameKey GetFinishingMove()
        {
            if (Me.RE.IsReady && Me.PS.IsActive && Me.BT.IsNotActive && Me.INFO.IsNotHiding)
            {
                return RE;
            }

            if (UsingSR && Me.SR.IsNotActive)
            {
                return CheckTF(SR, SR_TF);
            }

            const float RIP_TIMER = 9.0f; // APL = 8.0f

            if (Them.RI.IsNotActive || (Them.RI.Timer < RIP_TIMER && Them.HEALTH.Percentage > 0.25f))
            {
                if (!EnablePooling || EnergyToMax < 10 || Me.BEA.Timer > GCD || Me.TF.Timer < 3.0f || Me.CC.Timer > GCD || Them.RI.IsNotActive || Them.RK.Timer < 1.5f)
                {
                    if (TimeToDie > 6.0f)
                    {
                        return CheckTF(RI, RI_TF);
                    }
                }
            }

            if (UsingSR && Me.SR.Timer <= 10.5f) // 7.2?
            {
                if (!EnablePooling || EnergyToMax < 10 || Me.BEA.Timer > GCD || Me.TF.Timer < 3.0f || Me.CC.Timer > GCD || Them.RI.IsNotActive || Them.RK.Timer < 1.5f)
                {
                    if (TimeToDie > 6.0f)
                    {
                        return CheckTF(SR, SR_TF);
                    }
                }
            }

            if (!EnablePooling || EnergyToMax < 10 || Me.BEA.Timer > GCD || Me.TF.Timer < 3.0f)
            {
                return CheckTF(FB, FB_TF);
            }
            
            //Console.Out.WriteLine("Pooling (5)");
            return NONE;
        }
                
        private GameKey OnINTKey()
        {
            if (Me.WC.IsReady && Me.TA.Tier2 == 3)
                return null;

            if (Me.SB.IsReady)
                return SB;

            if (Me.MB.IsReady && Me.TA.Tier4 == 1)
                return MB;

            if (Me.WS.IsReady && Me.WS.IsInRange)
                return WS;

            return null;
        }

    }
}
