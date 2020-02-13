using System;
using System.Windows.Input;

namespace CommonLib
{
    public partial class Spec103 : Class11Spec
    {
        public GameKey  NONE    = new GameKey(Key.None);         

        // --------------- Input Keys ---------------
        public GameKey  INT     = new GameKey(Key.D1);          // Interrupt
        public GameKey  ST      = new GameKey(Key.D2);          // Single Target

        // ----------- Default Action Bar -----------
        // Wild Charge                       (Key.D1);          // Wild Charge
        // Shred                             (Key.D2);          // Shred
        public GameKey  TH      = new GameKey(Key.D3);          // Thrash
        public GameKey  SWBS    = new GameKey(Key.D4);          // Swipe or Brutal Slash
        public GameKey  RK      = new GameKey(Key.D5);          // Rake

        public GameKey  RE      = new GameKey(Key.D6);          // Regrowth
        public GameKey  RE_CAT  = new GameKey(Key.D6);          // Regrowth (cat only)
        public GameKey  RI      = new GameKey(Key.D7);          // Rip
        public GameKey  FB      = new GameKey(Key.D8);          // Ferocious Bite
        public GameKey  TF      = new GameKey(Key.D9);          // Tiger's Fury
        //                                   (Key.D0);          // Prowl
        public GameKey  SB      = new GameKey(Key.OemMinus);    // Skull Bash
        public GameKey  MB      = new GameKey(Key.OemPlus);     // Mighty Bash

        // --------- Bottom Left Action Bar ---------
        public GameKey  MF      = new GameKey(Key.F1);          // Moonfire
        //                                   (Key.F2);          // Healthstone
        //                                   (Key.F3);          // Survival Instinct
        public GameKey  BE      = new GameKey(Key.F4);          // Berserk
        //                                   (Key.F5);          // Remove Corruption
        //                                   (Key.F6);          // Rebirth
        public GameKey  CF      = new GameKey(Key.F7);          // Cat Form

        // --------- Bottom Right Action Bar ---------
        public GameKey  WS      = new GameKey(Key.F8);          // War Stomp
        public GameKey  MA      = new GameKey(Key.F9);          // Maim (also key ~)
        //                                   (Key.F10);         // Hibernate
        //                                   (Key.F11);         // Entangling Roots
        
        public Spec103() : base()
        {
        }

        public override bool IsValidKeyToLog(Key key)
        {
            return key == INT.Key || key == ST.Key;
        }

        public override GameKey OnKeyPress(Key pressed)
        {
            if (ST.WasPressed(pressed))
            {
                return OnATKKey();
            }
            else if (INT.WasPressed(pressed))
            {
                return OnINTKey();
            }
            return null;
        }        

    }
}
