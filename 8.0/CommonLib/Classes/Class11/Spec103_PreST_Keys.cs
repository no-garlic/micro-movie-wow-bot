using System;
using System.Windows.Input;

namespace CommonLib
{
    public partial class Spec103_PreST : Class11Spec
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
        // Healing Potion                    (Key.F2);          // Survival instincts
        public GameKey  TF_BE   = new GameKey(Key.F3);          // Tiger's Fury + Berserk
        // Survival Instinct                 (Key.F4);          // Survival Instinct
        //                                   (Key.F5);          //
        //                                   (Key.F6);          //

        // --------- Bottom Right Action Bar ---------
        public GameKey  SR      = new GameKey(Key.F7);          // Savage Roar
        public GameKey  MA      = new GameKey(Key.F8);          // Maim (also key ~)
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
        public GameKey  FF      = new GameKey(Key.NumPad4);     // Feral Frenzy
        public GameKey  SH_TF   = new GameKey(Key.NumPad5);     // Tiger's Fury + Shred
        public GameKey  RI_TF   = new GameKey(Key.NumPad6);     // Tiger's Fury + Rip
        public GameKey  FB_TF   = new GameKey(Key.NumPad7);     // Tiger's Fury + Ferocious Bite
        public GameKey  SW_TF   = new GameKey(Key.NumPad8);     // Tiger's Fury + Swipe or Brutal Slash
        public GameKey  TH_TF   = new GameKey(Key.NumPad9);     // Tiger's Fury + Thrash
        public GameKey  SR_TF   = new GameKey(Key.Add);         // Tiger's Fury + Savage Roar
        public GameKey  MF_TF   = new GameKey(Key.Subtract);    // Tiger's Fury + Moonfire

        public Spec103_PreST() : base()
        {
        }

        public override bool IsValidKeyToLog(Key key)
        {
            return key == INT.Key || key == ST.Key || key == AOE.Key;
        }

        public override GameKey OnKeyPress(Key pressed)
        {
            if (AOE.WasPressed(pressed))
            {
                return OnATKKey(true);
            }
            else if (ST.WasPressed(pressed))
            {
                return OnATKKey(false);
            }
            else if (INT.WasPressed(pressed))
            {
                return OnINTKey();
            }
            return null;
        }        

    }
}
