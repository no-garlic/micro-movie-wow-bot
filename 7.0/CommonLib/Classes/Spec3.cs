using System;
using System.Windows.Input;

namespace CommonLib
{
    public class Spec3 : MyClassSpec
    {
        public GameKey INT = new GameKey(Key.D1);
        public GameKey ST  = new GameKey(Key.D2);
        public GameKey MT  = new GameKey(Key.D3);

        public GameKey SW  = new GameKey(Key.D2);
        public GameKey TH  = new GameKey(Key.D3);        
        public GameKey MF  = new GameKey(Key.F1);
        public GameKey MG  = new GameKey(Key.D8);
        public GameKey IF  = new GameKey(Key.D6);

        public GameKey SB  = new GameKey(Key.OemMinus);
        public GameKey MB  = new GameKey(Key.OemPlus);

        public Spec3() : base()
        {
            Register(new Spec3_0000000(this));
        }
        
        protected override bool GetIsReady()
        {
            if (Config.IgnoreReadyCheck)
            {
                return true;
            }

            if (Me.INFO.IsNotB)
            {
                return false;
            }

            if (Them.INFO.Exists == false || Them.INFO.IsEnemy == false || Them.INFO.IsNotLiving == true)
            {
                return false;
            }

            if (Me.INFO.IsNotInControl)
            {
                return false;
            }

            return base.GetIsReady();
        }

        public override GameKey OnKeyPress(Key pressed)
        {            
            if (IsNotReady)
                return null;

            if (MT.WasPressed(pressed))
            {
                return OnATKKey(false, true, false);
            }
            else if (ST.WasPressed(pressed))
            {
                return OnATKKey(true, false, false);
            }
            else if (INT.WasPressed(pressed))
            {
                return OnINTKey();
            }
            return null;
        }
    }

    public class Spec3OptionBase : MyClassSpecOption<Spec3>
    {
        public Spec3OptionBase(MyClassSpec owner, int[] options) : base(owner, options)
        {
        }

        public override GameKey OnINTKey()
        {
            if (Me.WC.IsReady && Me.TA.Tier2 == 3)
                return null;

            if (Me.SB.IsReady)
                return Owner.SB;

            if (Me.MB.IsReady && Me.TA.Tier4 == 1)
                return Owner.MB;

            return Owner.SB;
        }


    }


}
