using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Windows.Input;
using System.Text;

namespace CommonLib
{
    public abstract class ClassSpecBase
    {
        protected ClassData m_Data;

        private Stopwatch m_CombatTimer;
        private Stopwatch m_CombatCooldown;
        private Stopwatch m_LogTimer;
        private bool m_InCombat;
        private bool m_FightingABoss;

        public int     SpecID         { get; private set; }
        public float   GCD            { get { return 1.5f; } }

        public float   TimeToDie      { get { return GetTimeToDie(); } }
        public float   CombatTime     { get { return GetCombatTime(); } }
        public bool    InCombat       { get { return m_InCombat; } }
        public string  Talents        { get { return GetTalentsAsString(); } }
        public bool    TargetingABoss { get { return IsTargetABoss(); } }
        public bool    FightingABoss  { get { return IsFightingABoss(); } }
        public bool    IsReady        { get { return GetIsReady(); } }
        public bool    IsPaused       { get { return GetIsPaused(); } }

        public bool    NotTargetingABoss => !TargetingABoss;
        public bool    NotFightingABoss  => !FightingABoss;
        public bool    IsNotReady => !IsReady;
        public bool    IsNotPaused => !IsPaused;

        public  string DebugString        { get; set; }
        private string DebugStringPrinted { get; set; }

        public void Create(int specId, ClassData data)
        {
            SpecID = specId;
            m_Data = data;

            m_LogTimer = new Stopwatch();
            m_CombatTimer = new Stopwatch();
            m_CombatCooldown = new Stopwatch();
            m_InCombat = false;
            m_FightingABoss = false;

            OnCreate();
        }

        public void Init()
        {
            OnInit();
        }

        public void Reset()
        {
            OnReset();
        }

        public virtual void OnReset()
        {
        }
        
        public virtual void OnCreate()
        {
        }

        public virtual void OnInit()
        {
        }

        public virtual void OnKeyPress(KeyPressedArgs e)
        {
        }

        public virtual void OnTAChanged()
        {
        }

        public virtual void OnUpdate()
        {            
            UpdateLogStatus(false);

            UpdateCombatTimer();

            if (DebugString != DebugStringPrinted)
            {
                DebugStringPrinted = DebugString;
                Console.Out.WriteLine(DebugStringPrinted);
            }
        }

        protected virtual float GetTimeToDie()
        {
            return 0.0f;
        }

        protected virtual bool IsFightingABoss()
        {
            bool boss = IsTargetABoss();

            if (m_InCombat)
            {
                m_FightingABoss = m_FightingABoss || boss;
            }
            else
            {
                m_FightingABoss = boss;
            }
            
            return m_FightingABoss;
        }

        protected virtual bool IsTargetABoss()
        {
            return false;
        }

        protected virtual bool GetIsPaused()
        {
            return false;
        }

        protected virtual bool GetIsReady()
        {
            return true;
        }

        protected virtual string GetTalentsAsString()
        {
            return "";
        }
        
        public virtual bool IsValidKeyToLog(Key key)
        {
            return true;
        }
        
        public virtual void OnCombatStart()
        {
            m_InCombat = true;

            if (m_CombatTimer.IsRunning)
            {
                if (m_CombatCooldown.IsRunning)
                {
                    if (m_CombatCooldown.ElapsedMilliseconds < 1000)
                    {
                        m_CombatCooldown.Stop();
                        m_CombatCooldown.Reset();
                    }
                }
            }
            else
            {
                m_CombatTimer.Restart();
            }
        }
        
        public virtual void OnCombatStop()
        {
            m_InCombat = false;

            if (m_CombatTimer.IsRunning)
            {
                m_CombatCooldown.Restart();
            }
            else
            {
                m_CombatCooldown.Reset();
            }
        }

        private void UpdateCombatTimer()
        {
            if (m_CombatCooldown.IsRunning && m_CombatCooldown.ElapsedMilliseconds >= 1000)
            {
                m_CombatTimer.Reset();
                m_CombatCooldown.Reset();
            }
        }

        private float GetCombatTime()
        {
            if (m_CombatTimer.IsRunning)
            {
                float elapsed = m_CombatTimer.ElapsedMilliseconds;
                elapsed *= 0.001f;
                return elapsed;
            }
            return 0.0f;
        }
        
        private void UpdateLogStatus(bool allowedToCreateNewLog)
        {
            if (IsReady && allowedToCreateNewLog)
            {
                App.Log.Start();
                m_LogTimer.Reset();
            }
            else if (!m_InCombat)
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
    
    public abstract class ClassSpec<T> : ClassSpecBase where T : ClassData 
    {
        public virtual GameKey OnKeyPress(Key pressed) { return null; }
        
        public T GetData()
        {
            return m_Data as T;
        }
        
        public override void OnKeyPress(KeyPressedArgs e)
        {
            Key pressed = e.Key;

            if (Config.RemapCapslock && pressed == Key.CapsLock)
            {
                e.Key = Key.Tab;
                return;
            }

            if (IsNotPaused)
            {
                GameKey command = OnKeyPress(pressed);
            
                if (command != null)
                {
                    e.Key = command.Key;
                }
            }
        }
    }
}
