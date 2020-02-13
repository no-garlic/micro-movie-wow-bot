using System;
using System.Diagnostics;
using System.Windows.Input;
using System.Collections.Generic;

namespace CommonLib
{
    public class MyData : ClassData
    {
        public class Them
        {
            public TINF INFO;   
            public LIFE HEALTH; 
            public AU   RI;     
            public AU   RK;     
            public AU   TH;     
            public AU   MF;          

            public Them()
            {
                INFO   = new TINF();
                HEALTH = new LIFE(false);
                RI     = new AU(0, false, false);
                RK     = new AU(1, false, false);
                TH     = new AU(2, false, false);
                MF     = new AU(3, false, false);
            }
        }

        public class Me
        {
            public OPT  TA;     
            public PINF INFO;   
            public LIFE HEALTH;
            public POW  POWER;  
            public AB   MG;     
            public AB   MF;     
            public AB   TH;     
            public AB   SA;     
            public AB   SW;     
            public AB   SH;     
            public AB   RI;     
            public AB   RK;     
            public AB   FB;     
            public AB   RE;     
            public AB   TF;     
            public AB   AF;     
            public AB   WC;     
            public AB   SB;     
            public AB   MB;
            public AB   BE;
            public AU   SR;     
            public AU   PS;     
            public AU   BT;     
            public AU   CC;
            public AU   GG;
            public AU   BEA;
            public AU   TFA;
                                                                    
            public Me()
            {
                TA      = new OPT();
                INFO    = new PINF();
                HEALTH  = new LIFE(true);
                POWER   = new POW(true);
                MG      = new AB(7);
                MF      = new AB(9);
                TH      = new AB(10);
                SA      = new AB(33);
                SW      = new AB(11);
                SH      = new AB(12);
                RI      = new AB(13);
                RK      = new AB(14);
                FB      = new AB(15);
                RE      = new AB(37);
                TF      = new AB(17);
                AF      = new AB(18);
                WC      = new AB(27);  
                SB      = new AB(28);  
                MB      = new AB(31);  
                BE      = new AB(35);  
                SR      = new AU(32);
                PS      = new AU(4);
                BT      = new AU(5);
                CC      = new AU(6);
                GG      = new AU(8);
                BEA     = new AU(39);
                TFA     = new AU(38);
            }
        }

        public Me     m_Me;
        public Them   m_Them;
        public SIGNAL m_Flag;
        public FOCUS  m_Focus;
        
        public override void Init()
        {
            m_Me    = new Me();
            m_Them  = new Them();
            m_Flag  = new SIGNAL();
            m_Focus = new FOCUS();
        }
    };
    
    public class Main : Class<MyData>
    {
        protected override int OnCreate()
        {
            AddClassSpec(103, new Spec2()); // C
            AddClassSpec(104, new Spec3()); // B
            AddClassSpec(102, new Spec1()); // M
            AddClassSpec(105, new Spec4()); // R
            return 11;
        }

        public override void OnInit()
        {
            MyData data = GetData<MyData>();
        }

        protected override bool IsValidKeyToLog(Key key)
        {
            MyClassSpec classSpec = GetActiveClassSpec() as MyClassSpec;
            if (classSpec == null)
                return true;

            return classSpec.IsValidKeyToLog(key);            
        }

        public override void LogData(Log.LogMode mode, Key input, Key output)
        {
            base.LogData(mode, input, output);

            string ttl = "0";
            string boss = "FALSE";

            MyClassSpec classSpec = GetActiveClassSpec() as MyClassSpec;
            if (classSpec != null)
            {
                float fttl = classSpec.TTL * 10.0f;
                int ittl = (int) fttl;
                fttl = ittl;
                fttl *= 0.01f;
                ttl = fttl.ToString();

                if (classSpec.FightingABoss)
                    boss = "TRUE";
            }

            App.Log.Write(Log.LogDetail.High, "TTL", ttl);
            App.Log.Write(Log.LogDetail.High, "Boss Mode", boss);
        }
    }

    public class MyClassSpecOptionBase
    {
        protected MyClassSpec  m_Owner;
        public    MyData.Me    Me    => m_Owner.Me;
        public    MyData.Them  Them  => m_Owner.Them;
        public    SIGNAL       Flag  => m_Owner.Flag;
        public    FOCUS        Focus => m_Owner.Focus;

        public    int[] Options { get; private set; }
        public    float GCD     { get { return 1.5f; } }

        private   Dictionary<GameKey, TimeSpan> m_KeyCooldowns;
        private   Stopwatch m_Stopwatch;

        public MyClassSpecOptionBase(MyClassSpec owner, int[] options)
        {
            m_KeyCooldowns = new Dictionary<GameKey, TimeSpan>();
            m_Stopwatch = new Stopwatch();
            m_Stopwatch.Start();
            m_Owner = owner;
            Options = options;
        }

        public bool IsValidFor(OPT options)
        {
            if (Options == null || Options.Length == 0)
                return true;

            int count = Math.Max(Options.Length, 7);
            for (int i = 0; i < count; ++i)
            {
                int selected = options.GetSelected(i + 1);
                int filtered = Options[i];

                if (filtered == 0 || filtered == selected)
                    continue;

                return false;
            }

            return true;
        }

        public virtual GameKey OnATKKey(bool singleKey, bool multiKey, bool areaKey)
        {
            return null;
        }

        public virtual GameKey OnINTKey()
        {
            return null;
        }

        public override string ToString()
        {
            if (Options == null || Options.Length == 0)
                return "0000000";

            string s = "";
            foreach (int i in Options)
                s += i.ToString();
            return s;
        }

        public void AddCooldown(GameKey key, float duration)
        {
            int seconds = (int) duration;
            int milliseconds = (int) (1000.0f * (duration - (float) seconds));

            TimeSpan ts = new TimeSpan(0, 0, 0, seconds, milliseconds);
            ts.Add(m_Stopwatch.Elapsed);

            m_KeyCooldowns[key] = ts;
        }

        public bool IsOnCooldown(GameKey key)
        {
            TimeSpan ts = new TimeSpan();

            if (m_KeyCooldowns.TryGetValue(key, out ts))
            {
                if (ts < m_Stopwatch.Elapsed)
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsOffCooldown(GameKey key)
        {
            return !IsOnCooldown(key);
        }
    }
    
    public class MyClassSpecOption<T> : MyClassSpecOptionBase where T : MyClassSpec
    {
        public T Owner { get { return m_Owner as T; } }

        public MyClassSpecOption(MyClassSpec owner, int[] options) : base(owner, options)
        {
        }
    }

    public class MyClassSpec : ClassSpec<MyData>
    {
        public MyData.Me   Me    { get { return GetData().m_Me; } }
        public MyData.Them Them  { get { return GetData().m_Them; } }
        public SIGNAL      Flag  { get { return GetData().m_Flag; } }
        public FOCUS       Focus { get { return GetData().m_Focus; } }
        public  string     DebugString        { get; set; }
        private string     DebugStringPrinted { get; set; }

        private bool m_Boss;
        private Stopwatch m_LogTimer;

        private List<MyClassSpecOptionBase> m_Options;

        public MyClassSpec() : base()
        {
            m_Options = new List<MyClassSpecOptionBase>();
            m_Boss = false;
            m_LogTimer = new Stopwatch();
            m_LogTimer.Reset();
        }

        public void Register(MyClassSpecOptionBase option)
        {
            m_Options.Add(option);
        }

        public MyClassSpecOptionBase GetActiveOption()
        {
            foreach (MyClassSpecOptionBase specOption in m_Options)
            {
                if (specOption.IsValidFor(Me.TA))
                    return specOption;
            }
            return null;
        }

        protected override float GetTTL()
        {
            if (Them.INFO.IsEnemy == false || Them.INFO.IsLiving == false || Them.INFO.Exists == false)
                return 0.0f;

            float dps = Me.INFO.IsInDungeon ? 800 : Me.INFO.IsInRaid ? 7500 : Me.INFO.IsSolo ? 250 : (Me.INFO.PartySize * 180);
            float health = Them.HEALTH.Current;
            float ttl = health / dps;

            return ttl;
        }

        protected override bool IsFightingABoss()
        {
            bool boss = IsTargetABoss();

            if (Me.INFO.IsAggressive)
            {
                m_Boss = m_Boss || boss;
            }
            else
            {
                m_Boss = boss;
            }
            
            return m_Boss;            
        }

        protected override bool IsTargetABoss()
        {
            if (Config.ForceFightingABoss)
                return true;

            bool outdoorBoss = Me.INFO.IsNotInDungeon && Me.INFO.IsNotInRaid && Them.INFO.Level >= 110;
            bool dungeonBoss = Me.INFO.IsInDungeon && Them.INFO.Level >= 112;
            bool raidBoss    = Me.INFO.IsInRaid && Them.INFO.Level >= 113;
            bool bossType    = Them.INFO.TypeID != 1 && Them.INFO.TypeID != 3;
            bool highHealth  = Them.HEALTH.Current > (Me.HEALTH.Maximum * 100);
            bool boss        = Them.INFO.Exists && Them.INFO.IsEnemy && Them.INFO.IsLiving && (bossType || highHealth) && (outdoorBoss || dungeonBoss || raidBoss);

            return boss;            
        }

        public virtual bool IsValidKeyToLog(Key key)
        {
            return true;
        }

        public virtual GameKey OnATKKey(bool singleKey, bool multiKey, bool areaKey)
        {
            MyClassSpecOptionBase specOption = GetActiveOption();
            if (specOption != null)
            {
                UpdateLogStatus(true);
                return specOption.OnATKKey(singleKey, multiKey, areaKey);
            }

            return null;
        }

        public virtual GameKey OnINTKey()
        {
            MyClassSpecOptionBase specOption = GetActiveOption();
            if (specOption != null)
            {
                UpdateLogStatus(true);
                return specOption.OnINTKey();
            }

            return null;
        }

        public override void OnUpdate()
        {
            //DebugString = "FightingABoss = " + FightingABoss.ToString();
            //DebugString = Them.MF.Timer.ToString();
            //           DebugString = "Bit0: " + Flag.Bit0.ToString() + "    " + "Bit1: " + Flag.Bit1.ToString();

            //DebugString = $"{Focus.IsReady}, {Focus.NeedsRI}, {Focus.NeedsRK}, {Focus.NeedsMF}";

            //float t = GetCombatTime();
            //float p = 1.0f;
            //while (t >= 10.0f)
            //{
            //    t = t * 0.1f;
            //    p = p * 10.0f;
            //}

            //t = (float) ((int) t);
            //t *= p;

            //DebugString = $"Combat Time: {t}s";

            if (DebugString != DebugStringPrinted)
            {
                DebugStringPrinted = DebugString;
                Console.Out.WriteLine(DebugStringPrinted);
            }

            UpdateLogStatus(false);
            
            base.OnUpdate();
        }

        private void UpdateLogStatus(bool allowedToCreateNewLog)
        {
            if (IsReady && allowedToCreateNewLog)
            {
                App.Log.Start();
                m_LogTimer.Reset();
            }
            else if (Me.INFO.IsNotAggressive)
            {
                if (m_LogTimer.IsRunning == false)
                {
                    m_LogTimer.Reset();
                    m_LogTimer.Start();
                }

                if (m_LogTimer.ElapsedMilliseconds > 2000)
                {
                    App.Log.Stop();
                    m_LogTimer.Reset();
                }
            }
            else
            {
                m_LogTimer.Reset();
            }
        }
    }


}
