using System;
using System.Windows.Input;

// -------------------------------------------------------------------------------------------------------------------------
// TODO:
//
// EditBox:HasFocus() for chat box to disable input mapping
//
// Continue to refine the use of MF+SR when running around a boss room dumping rot for example
// Add UsingBT support for taking a different Tier7 talent
// Add Brutal Slash Support
// Add usage of FB during berserk
// Add support for Incarnation talent
// Add support for mass aoe spam button
// Fix interrupts in guardian spec
// Check moonfire button as a pull mechanic when solo with LI
// Map stampeeding roar to Y
// Fix weakaura for EN - remove icons I dont need, make sure correct sound plays when I have to move
// Change signals to use weakauras showing the cooldown and status, and have a button to toggle (on, off, auto), when auto is true show a highlight
// Try an APL based on simcraft
//
// ┌─────────────┬────────┬────────┬────────┬────────┐
// │             │  Rake  │  Rip   │ Thrash │Moonfire│
// ├─────────────┼────────┴────────┴────────┴────────┤
// │Savage Roar  │               +25%                │
// ├─────────────┼───────────────────────────────────┤
// │Tiger's Fury │               +15%                │
// ├─────────────┼──────────────────────────┬────────┤
// │Bloodtalons  │           +50%           │  N/A   │
// ├─────────────┼────────┬─────────────────┴────────┤
// │Improved Rake│  +100% │           N/A            │
// └─────────────┴────────┴──────────────────────────┘
//
// Fix opener: pre HT - prowl - pot - rake - li - sr - zerk - tf - af - shred to 5 - rip
// Stop SM+Rake in opener
// Dont overwrite rakes when berserk is active
// AOE Key (4) needs testing (not working when not ready, swap swipe into that slot?)
//

namespace CommonLib
{
    public class Spec2 : MyClassSpec
    {
        public GameKey  NONE    = new GameKey(Key.None);         

        // ----------- Default Action Bar -----------
        public GameKey  INT     = new GameKey(Key.D1);
        public GameKey  ST      = new GameKey(Key.D2);
        public GameKey  MT      = new GameKey(Key.D3);
        public GameKey  AOE     = new GameKey(Key.D4);                                
        public GameKey  FB      = new GameKey(Key.D5);
        public GameKey  RE      = new GameKey(Key.Multiply);
        public GameKey  SR      = new GameKey(Key.D7);
        public GameKey  MF      = new GameKey(Key.D8);
        public GameKey  AF      = new GameKey(Key.D9);
        // Stealth                           (Key.D0);
        public GameKey  SB      = new GameKey(Key.OemMinus);
        public GameKey  MB      = new GameKey(Key.OemPlus);

        // --------- Bottom Left Action Bar ---------
        // Survival Instinct                 (Key.F1);
        // Healing Potion                    (Key.F2);
        //                                   (Key.F3);
        public GameKey  TF_BE   = new GameKey(Key.F4);
        public GameKey  TF      = new GameKey(Key.F5);
        //                                   (Key.F6);
        public GameKey  SW      = new GameKey(Key.F7);
        public GameKey  TH      = new GameKey(Key.F8);
        //                                   (Key.F9);
        //                                   (Key.F10);
        public GameKey  RK_F    = new GameKey(Key.F11);
        public GameKey  MF_F    = new GameKey(Key.F12);
            
        // -------------- Right Action Bar ---------------
        //              Maim                 (Key.~);
        public GameKey  RK      = new GameKey(Key.NumPad0);
        public GameKey  RK_TF   = new GameKey(Key.NumPad1);
        public GameKey  RK_SM   = new GameKey(Key.NumPad2);
        public GameKey  AF_TFB  = new GameKey(Key.NumPad3);
        public GameKey  RI      = new GameKey(Key.NumPad4);
        public GameKey  SH_TF   = new GameKey(Key.NumPad5);
        public GameKey  RI_TF   = new GameKey(Key.NumPad6);              
        public GameKey  FB_TF   = new GameKey(Key.NumPad7);              
        public GameKey  SW_TF   = new GameKey(Key.NumPad8);
        public GameKey  TH_TF   = new GameKey(Key.NumPad9);
        public GameKey  SR_TF   = new GameKey(Key.Add);
        public GameKey  MF_TF   = new GameKey(Key.Subtract);
        //                                   ();
        //                                   (Key.Decimal);
        
        public Spec2() : base()
        {
            Register(new Spec2_0000000(this));
        }

        public override bool IsValidKeyToLog(Key key)
        {
            return key == INT.Key || key == ST.Key || key == MT.Key || key == AOE.Key;
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
                bool aoeWasPressed = AOE.WasPressed(pressed);

                if ((ST.WasPressed(pressed) || MT.WasPressed(pressed) || aoeWasPressed) && Me.RE.IsReady && Me.PS.IsActive && Me.INFO.IsNotHiding)
                {
                    return RE;
                }

                if (aoeWasPressed && Me.INFO.IsSolo && Me.INFO.IsNotDriving && Me.INFO.IsC && Me.INFO.IsInControl)
                {
                    return OnATKKey(false, false, true);
                }
                else
                {
                    return null;
                }
            }

            if (MT.WasPressed(pressed))
            {
                return OnATKKey(false, true, false);
            }
            else if (ST.WasPressed(pressed))
            {
                return OnATKKey(true, false, false);
            }
            else if (AOE.WasPressed(pressed))
            {
                return OnATKKey(false, false, true);
            }
            else if (INT.WasPressed(pressed))
            {
                return OnINTKey();
            }

            return null;
        }
        

    }

    public class Spec2OptionBase : MyClassSpecOption<Spec2>
    {
        public class RefreshWindow
        {
            private MyClassSpec Owner;

            public float RI { get { return Owner.Me.TA.Tier6 == 2 ? 4.8f : 7.2f; } }
            public float RK { get { return Owner.Me.TA.Tier6 == 2 ? 3.0f : 4.5f; } }
            public float TH { get { return Owner.Me.TA.Tier6 == 2 ? 3.0f : 4.5f; } }
            public float MF { get { return 4.2f; } }
            public float SR { get { return 7.2f; } } // actually depends on CP so should be 7.2 * (cp / maxcp)

            public RefreshWindow(MyClassSpec owner)
            {
                Owner = owner;
            }
        }

        public class EnergyCost
        {
            private MyClassSpec Owner;
            
            public int MF { get { return Owner.Me.CC.IsActive ? 0 : 30; } }
            //public int RI { get { return Owner.Me.CC.IsActive ? 0 : 30; } }
            //public int RK { get { return Owner.Me.CC.IsActive ? 0 : 40; } }
            //public int SH { get { return Owner.Me.CC.IsActive ? 0 : 35; } }
            //public int FB { get { return Owner.Me.CC.IsActive ? 0 : 40; } }
            //public int SW { get { return Owner.Me.CC.IsActive ? 0 : 40; } }
            //public int TH { get { return Owner.Me.CC.IsActive ? 0 : 50; } }

            public EnergyCost(MyClassSpec owner)
            {
                Owner = owner;
            }
        }

        public RefreshWindow Refresh;
        public EnergyCost Cost;

        public Spec2OptionBase(MyClassSpec owner, int[] options) : base(owner, options)
        {
            Refresh = new RefreshWindow(owner);
            Cost    = new EnergyCost(owner);
        }

        public override GameKey OnINTKey()
        {
            if (Me.WC.IsReady && Me.TA.Tier2 == 3)
                return null;

            if (Me.SB.IsReady)
                return Owner.SB;

            if (Me.MB.IsReady && Me.TA.Tier4 == 1)
                return Owner.MB;

            return null;
        }
    }

    

}
