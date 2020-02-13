using System;
using System.Windows.Input;

namespace CommonLib
{
    public class Spec104 : Class11Spec
    {
        public GameKey  NONE    = new GameKey(Key.None);         

        // ----------- Input Keys -----------
        public GameKey  INT     = new GameKey(Key.D1);          // Interrupt
        public GameKey  DPS     = new GameKey(Key.D2);          // DPS
        public GameKey  PROT    = new GameKey(Key.D3);          // Protection

        // ----------- Default Action Bar -----------
        // Wild Charge                       (Key.D1);          // Wild Charge
        // Swipe                             (Key.D2);          // Swipe
        // Thrash                            (Key.D3);          // Thrash
        // Growl                             (Key.D4);          // Growl
        public GameKey  FR      = new GameKey(Key.D5);          // Frenzied Regeneration
        // Regrowth                          (Key.D6);          // Regrowth
        public GameKey  IF      = new GameKey(Key.D7);          // Ironfur
        public GameKey  ML      = new GameKey(Key.D8);          // Maul
        public GameKey  MG      = new GameKey(Key.D9);          // Mangle
        // Prowl                             (Key.D0);          // Prowl
        public GameKey  SB      = new GameKey(Key.OemMinus);    // Skull Bash
        public GameKey  MB      = new GameKey(Key.OemPlus);     // Mighty Bash

        // --------- Bottom Left Action Bar ---------
        public GameKey  MF      = new GameKey(Key.F1);          // Moonfire
        public GameKey  WS      = new GameKey(Key.F8);          // War Stomp
        public GameKey  IR      = new GameKey(Key.F9);          // Incapacitating Roar (also key ~)

        // --------- Right Action Bar ---------
        public GameKey  PU      = new GameKey(Key.Multiply);    // Pulverize
        public GameKey  SW      = new GameKey(Key.Add);         // Swipe
        public GameKey  TH      = new GameKey(Key.Subtract);    // Thrash

        // ----------------------------------------------------

        public Spec104() : base()
        {
        }
        
        public override bool IsValidKeyToLog(Key key)
        {
            return key == INT.Key || key == DPS.Key || key == PROT.Key;
        }
       
        protected override bool GetIsReady()
        {
            if (Config.IgnoreReadyCheck)
                return true;

            if (IsPaused || Me.INFO.IsNotB || Me.INFO.IsNotInControl || Them.INFO.Exists == false || Them.INFO.IsEnemy == false || Them.INFO.IsNotLiving == true)
                return false;

            return base.GetIsReady();
        }

        public override GameKey OnKeyPress(Key pressed)
        {
            if (IsNotReady)
                return null;

            if (INT.WasPressed(pressed))
                return OnINTKey();
            else if (DPS.WasPressed(pressed))
                return OnATKKey(true, false);
            else if (PROT.WasPressed(pressed))
                return OnATKKey(false, true);

            return null;
        }
    
        private GameKey OnATKKey(bool dpsKey, bool protKey)
        {
            if (ShouldHeal())
                return FR;

            GameKey rageDumpKey = GetRageDump(dpsKey, protKey);
            if (rageDumpKey != null)
                return rageDumpKey;
            
            if (ShouldThrash()) 
                return TH;

            if (ShouldPulverize())
                return PU;

            if (ShouldMoonfire())
                return MF;

            if (ShouldMangle())
                return MG;

            if (protKey)
                return SW;

            return null; // SW
        }

        protected GameKey OnINTKey()
        {
            if (Me.WC.IsReady && Me.WC.IsInRange && Me.TA.Tier2 == 3)
                return null;

            if (Them.INFO.IsCasting && Me.SB.IsReady)
                return SB;

            if (Me.MB.IsReady && Me.MB.IsInRange && Me.TA.Tier4 == 1)
                return MB;

            if (Me.IR.IsReady && Me.IR.IsInRange)
                return IR;

            if (Me.WS.IsReady && Me.WS.IsInRange)
                return WS;

            return SB;
        }

        // ----------------------------------------------------

        private bool ShouldHeal()
        {
            if (Me.POWER.Primary >= 10 && Me.HEALTH.Percentage < 0.65f)
            {
                if (Me.FR.IsReady && (Me.FRE.IsNotActive || Me.FRE.Timer < 0.2f))
                {
                    return true;
                }
            }
            return false;
        }
        
        private bool ShouldMoonfire()
        {
            if (Them.INFO.Exists && Me.MF.IsReady &&                                                                // target in range
               (Them.MF.IsNotActive || Them.MF.Timer < 1.5f || (Me.GG.IsActive && Me.POWER.PrimaryToMax >= 20)) &&  // missing debuff or gg proc
               (Me.INFO.IsNotAggressive || Them.INFO.IsAggressive || Them.INFO.IsDummy))                            // them in combat or me out of combat
            {
                return true;
            }
            return false;
        }
        
        private bool ShouldPulverize()
        {
            if (Me.TA.Tier7 != 3)
                return false;

            if (Them.INFO.Exists && Them.TH.IsActive && Them.TH.Count == 3 && Me.PU.IsReady && Me.PU.IsInRange)
                return true;

            return false;
        }

        private bool ShouldThrash()
        {
            if (Me.TH.IsReady && Them.INFO.Exists)
                return true;

            return false;
        }

        private bool ShouldMangle()
        {
            if (Me.MG.IsReady && Them.INFO.Exists)
            {
                return true;
            }
            return false;
        }

        private GameKey GetRageDump(bool dpsKey, bool protKey)
        {
            if (Me.INFO.IsInDungeon || (Me.INFO.IsInRaid && Me.INFO.PartySize > 2))
            {
                dpsKey  = protKey;
                protKey = !dpsKey;
            }

            if (dpsKey && Them.INFO.Exists && Me.POWER.Primary >= 60 && Me.ML.IsInRange)
                return ML;

            if (protKey)
            {
                if (Me.POWER.Primary > 45 && Me.IF.IsNotActive)
                    return IF;
                if (Me.POWER.PrimaryToMax <= 10)
                    return IF;
            }

            return null;
        }
        

    }
}
