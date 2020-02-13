using System;
using System.Windows.Input;

namespace CommonLib
{
    public partial class Spec103 : Class11Spec
    {
        private int  Combo         => Me.POWER.Secondary;
        private int  ComboToMax    => Me.POWER.SecondaryToMax;
        private bool ComboAtMax    => Me.POWER.SecondaryAtMax;

        private int  Energy        => Me.POWER.Primary;
        private int  EnergyToMax   => Me.POWER.PrimaryToMax;
        private bool EnergyAtMax   => Me.POWER.PrimaryAtMax;      

        private bool UsingWC       => Me.TA.Tier2 == 3;
        private bool UsingMB       => Me.TA.Tier4 == 1;
        private bool UsingLI       => Me.TA.Tier1 == 3;
        private bool UsingBE       => FightingABoss;
        private bool UsingTH       => Config.UseTHwithST && (Flag.Bit10 == 0);

        private bool UsingHardCastRE    => (TargetingABoss && FightingABoss && Flag.Bit13 == 0) || (Flag.Bit14 == 1 && Flag.Bit13 == 0);
        private bool UsingLIOpener      => (TargetingABoss && FightingABoss && Flag.Bit15 == 1) || (Flag.Bit16 == 1 && Flag.Bit15 == 0);

        //0, 0 off
        //1, 0 auto
        //0, 1 on



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

        private GameKey OnATKKey()
        {
            if (IsNotReady)
            {
                // Enter Cat Form
                if (Me.INFO.IsNotC)
                {
                    return CF;
                }

                // Heal with the PS buff if not not ready
                if (Me.RE.IsReady && Me.PS.IsActive && Me.INFO.IsNotHiding)
                {
                    Console.Out.WriteLine("RE_CAT");
                    return RE_CAT;
                }

                // Pull with LI if alone and not stealthed
                if (UsingLI && Me.INFO.IsNotHiding && Me.INFO.IsSolo && Them.INFO.IsLiving && Them.INFO.IsEnemy)
                {
                    if (Me.MF.IsInRange)
                    {
                        return MF;
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
                    return MF;
                }
            }

            // RK from stealth
            if (Me.INFO.IsHiding)
            {
                return RK;
            }

            // Use BE
            if (UsingBE && Me.BE.IsReady && Energy >= 30 && (Me.TF.IsReady || Me.TFA.Timer > 5.0f))
            {
                return BE;
            }

            // Use TF
            if (Them.INFO.IsEnemy && Them.INFO.IsLiving && Me.TF.IsReady && EnergyToMax >= 60)
            {
                return TF;
            }

            // Use FB
            if (Me.FB.IsInRange && Them.RI.IsActive && Them.RI.Timer <= 3.0f && Combo > 0)
            {
                return FB;
            }
            
            if (ComboAtMax && Me.PS.IsActive && Me.BT.IsNotActive && Them.RI.IsActive && Them.RI.Timer < 8.0f)
            {
                Console.Out.WriteLine("RE_CAT");
                return RE_CAT;
            }

            if (ComboAtMax)
            {
                return GetFinisher();
            }

            return GetGenerator();
        }

        private GameKey GetGenerator()
        {
            // Refresh RK if there is still a BT available or if it is within the pandemic window
            if ((CombatTime > 0.5f && Them.RK.IsNotActive) || (CombatTime > 5.0f && Me.BT.IsActive && Combo == 0))
            {
                return RK;
            }

            // Use MF if we have the LI talent
            if (UsingLI && (Them.MF.IsNotActive || Them.MF.Timer < 4.6f) && Me.MF.IsInRange && (Me.INFO.IsSolo || Them.INFO.IsAggressive || Me.SH.IsInRange))
            {
                return MF;
            }

            // Use MF at range to build CP
            if (UsingLI && ComboToMax > 0 && Me.MF.IsInRange && Me.SH.IsOutOfRange && Me.INFO.IsAggressive && Them.INFO.IsAggressive)
            {
                return MF;
            }
            
            // Use MF to pull and tag
            if (UsingLI && Me.MF.IsInRange && Me.SH.IsOutOfRange && Me.INFO.IsSolo)
            {
                return MF;
            }

            // Hold a BT in the opener for ST to use on RI
            if (UsingLIOpener && Me.BT.IsActive && Them.RI.IsNotActive && Them.RK.IsActive && CombatTime < 10.0f)
            {
                return MF;
            }

            // Maintain TH on target
            if (UsingTH && (Them.TH.IsNotActive || Them.TH.Timer < 4.0f))
            {
                return TH;
            }

            // Shred
            return null;
        }

        private GameKey GetFinisher()
        {
            // Cast regrowth if at max cp to apply BT if using BT
            if (Me.RE.IsReady && Me.PS.IsActive && Me.BT.IsNotActive && Me.INFO.IsNotHiding)
            {
                Console.Out.WriteLine("RE_CAT");
                return RE_CAT;
            }

            // Apply FB if the target is low enough health that FB would do more damage than rip
            if (TimeToDie > 0.0f && TimeToDie < 7.0f)
            {
                return FB;
            }

            if (Them.RI.IsNotActive)
            {
                // Hard cast RE
                if (Me.BT.IsNotActive && UsingHardCastRE)
                {
                    Console.Out.WriteLine("RE");
                    return RE;
                }

                return RI;
            }
            
            // Use FB if we dont need to reapply rip or SR
            return FB;
        }

        private GameKey OnINTKey()
        {
            // Use wild charge if we have the talent and it is ready and in range
            if (Me.WC.IsReady && Me.WC.IsInRange && UsingWC)
                return null;

            // Use skull bash if it is ready and in range
            if (Them.INFO.IsCasting && Me.SB.IsReady && Me.SB.IsInRange)
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
