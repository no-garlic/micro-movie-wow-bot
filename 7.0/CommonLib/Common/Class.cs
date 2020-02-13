using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Windows.Input;
using System.Text;

namespace CommonLib
{
    public abstract class ClassData
    {
        public List<Data> DataList = new List<Data>();
        public bool m_FirstUpdate = true;

        public abstract void Init();

        public virtual void Update()
        {
            foreach (Data data in DataList)
            {
                data.Update();
            }

            if (m_FirstUpdate)
            {
                m_FirstUpdate = false;
                App.Class.LogData(Log.LogMode.Header, Key.None, Key.None);
                App.Log.EndLine();
            }
        }

        public void LogData(Log.LogMode mode, Key input, Key output)
        {
            if (m_FirstUpdate)
                return;

            if (App.Log.Enabled)
            {
                App.Log.Mode = mode;

                string timeStamp = DateTime.Now.ToString("M/d HH:mm:ss.fff");

                App.Log.Write(Log.LogDetail.High, "Time", timeStamp);
                App.Log.Write(Log.LogDetail.High, "Key In", input);
                App.Log.Write(Log.LogDetail.High, "Key Out", output);
                App.Log.Write(Log.LogDetail.High, "Valid Data", App.Class.IsOK);
                App.Log.Write(Log.LogDetail.High, "Ready", App.Class.IsReady);

                foreach (Data data in DataList)
                {
                    data.LogData();
                }
            }
        }
    }

    public abstract class ClassSpecBase
    {
        protected ClassData m_Data;

        private Stopwatch m_CombatTimer;
        private Stopwatch m_CombatCooldown;

        public int   SpecID         { get; private set; }
        public float TTL            { get { return GetTTL(); } }
        public bool  TargetingBoss  { get { return IsTargetABoss(); } }
        public bool  FightingABoss  { get { return IsFightingABoss(); } }
        public bool  IsReady        { get { return GetIsReady(); } }
        public bool  IsNotReady => !IsReady;

        public void Create(int specId, ClassData data)
        {
            SpecID = specId;
            m_Data = data;

            m_CombatTimer = new Stopwatch();
            m_CombatCooldown = new Stopwatch();

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
            UpdateCombatTimer();
        }

        protected virtual float GetTTL()
        {
            return 0.0f;
        }

        protected virtual bool IsFightingABoss()
        {
            return false;
        }

        protected virtual bool IsTargetABoss()
        {
            return false;
        }

        protected virtual bool GetIsReady()
        {
            return true;
        }

        public void OnCombatStart()
        {
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
        
        public void OnCombatStop()
        {
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

        public float GetCombatTime()
        {
            if (m_CombatTimer.IsRunning)
            {
                float elapsed = m_CombatTimer.ElapsedMilliseconds;
                elapsed *= 0.001f;
                return elapsed;
            }
            return 0.0f;
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

            GameKey command = OnKeyPress(pressed);
            if (command != null)
            {
                e.Key = command.Key;
            }
        }
    }

    public abstract class ClassBase
    {
        private CRC m_MagicNumberA;
        private CI m_ClassInfo;
        private CRC m_MagicNumberB;
        private ClassData m_Data;
        private Dictionary<int, ClassSpecBase> m_ClassSpecs;
        ClassSpecBase m_ActiveClassSpec;

        public int DataCount        { get { return m_Data.DataList.Count; } }
        public List<Data> DataList  { get { return m_Data.DataList; } }

        public int ClassID { get; private set; }
        
        protected abstract ClassData CreateClassData();
        protected abstract int OnCreate();

        public bool IsOK { get; private set; }
        public bool IsNotOK => !IsOK;
        public bool IsReady { get { return GetIsReady(); } }
        public bool IsNotReady => !IsReady;

        public T GetData<T>() where T : ClassData
        {
            return m_Data as T;
        }

        public void AddClassSpec(int specId, ClassSpecBase spec)
        {
            m_ClassSpecs.Add(specId, spec);
            spec.Create(specId, m_Data);
        }

        public ClassSpecBase GetActiveClassSpec()
        {
            IsOK = false;
            
            if (!Config.IgnoreDataChecks && (m_MagicNumberA.IsValid == false || m_MagicNumberB.IsValid == false))
                return null;

            if (!Config.IgnoreDataChecks && ClassID != m_ClassInfo.ClassID)
                return null;

            foreach (KeyValuePair<int, ClassSpecBase> kvp in m_ClassSpecs)
            {
                if (kvp.Key != m_ClassInfo.SpecID && !Config.IgnoreDataChecks)
                    continue;

                IsOK = true;
                return kvp.Value;
            }

            return null;
        }

        public ClassBase()
        {
            m_ClassSpecs = new Dictionary<int, ClassSpecBase>();
        }

        public void Create()
        {
            m_Data = CreateClassData();

            m_MagicNumberA = new CRC(23, 17, 11, 9);

            m_ClassInfo = new CI();
            m_ClassInfo.OnChanged += OnClassInfoChanged;

            ClassID = OnCreate();

            IsOK = true;
        }

        public void Init()
        {              
            m_Data.Init();
            OnInit();

            m_MagicNumberB = new CRC(24, 23, 13, 19);
        }

        public virtual void OnInit()
        {
        }
        
        public virtual void Update()
        {
            m_Data.Update();
            GetActiveClassSpec()?.OnUpdate();
        }
        
        public virtual void OnKeyPress(KeyPressedArgs e)
        {
            Key input = e.Key;
            GetActiveClassSpec()?.OnKeyPress(e);
            Key output = e.Key;

            if (IsValidKeyToLog(input))
            {
                LogData(Log.LogMode.Value, input, output);
                App.Log.EndLine();
            }
        }

        public virtual void LogData(Log.LogMode mode, Key input, Key output)
        {
            m_Data.LogData(mode, input, output);
        }

        protected abstract bool IsValidKeyToLog(Key key);

        private bool GetIsReady()
        {
            ClassSpecBase classSpec = GetActiveClassSpec();
            if (classSpec == null)
                return false;

            return classSpec.IsReady;
        }
        
        public void Add(Data data)
        {
            m_Data.DataList.Add(data);
        }

        private void OnClassInfoChanged(object sender, EventArgs e)
        {
            ClassSpecBase activeClassSpec = GetActiveClassSpec();

            if (activeClassSpec != m_ActiveClassSpec)
            {
                if (m_ActiveClassSpec != null)
                {
                    m_ActiveClassSpec.Reset();
                }

                m_ActiveClassSpec = activeClassSpec;

                if (m_ActiveClassSpec != null)
                {
                    m_ActiveClassSpec.Init();
                }
            }
        }
    }
        
    public abstract class Class<T> : ClassBase where T : new()
    {
        public Class() : base()
        {
        }

        protected override ClassData CreateClassData()
        {
            return new T() as ClassData;
        }
    }

    


}
