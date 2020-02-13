using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Windows.Input;
using System.Text;

namespace CommonLib
{
    public abstract class ClassBase
    {
        public int          ClassID     { get; private set; }
        public int          DataCount   { get { return m_Data.DataList.Count; } }
        public List<Data>   DataList    { get { return m_Data.DataList; } }
        public bool         IsOK        { get; private set; }
        public bool         IsReady     { get { return GetIsReady(); } }
        public bool         IsPaused    { get { return GetIsPaused(); } }

        public bool IsNotOK => !IsOK;
        public bool IsNotReady => !IsReady;
        public bool IsNotPaused => !IsPaused;

        private Dictionary<int, ClassSpecBase> m_ClassSpecs;
        private CRC             m_MagicNumberA;
        private CI              m_ClassInfo;
        private CRC             m_MagicNumberB;
        private ClassData       m_Data;
        private ClassSpecBase   m_ActiveClassSpec;

        protected abstract ClassData CreateClassData();
        protected abstract int OnCreate();

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

        public int ShouldChangeClass()
        {
            if (m_MagicNumberB.IsValid)
            {
                if (m_ClassInfo.ClassID > 0)
                {
                    if (m_ClassInfo.ClassID != ClassID)
                    {
                        return m_ClassInfo.ClassID;
                    }
                }
            }
            return 0;
        }

        public ClassBase()
        {
            m_ClassSpecs = new Dictionary<int, ClassSpecBase>();
        }

        public void Create()
        {
            m_Data = CreateClassData();

            m_MagicNumberA = new CRC(23, 17, 11, 9);

            ClassID = OnCreate();

            IsOK = true;
        }

        public void Init()
        {              
            m_Data.Init();

            m_ClassInfo = new CI();
            m_ClassInfo.OnChanged += OnClassInfoChanged;

            m_MagicNumberB = new CRC(24, 23, 13, 19);

            OnInit();
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

            string ttd = "0";
            string boss = "FALSE";

            ClassSpecBase classSpec = GetActiveClassSpec() as ClassSpecBase;
            if (classSpec != null)
            {
                float fttd = classSpec.TimeToDie * 10.0f;
                int ittd = (int) fttd;
                fttd = ittd;
                fttd *= 0.01f;
                ttd = fttd.ToString();

                if (classSpec.FightingABoss)
                    boss = "TRUE";
            }

            App.Log.Write(Log.LogDetail.High, "TTD", ttd);
            App.Log.Write(Log.LogDetail.High, "Boss Mode", boss);
        }

        public virtual bool IsValidKeyToLog(Key key)
        {
            ClassSpecBase classSpec = GetActiveClassSpec() as ClassSpecBase;
            if (classSpec == null)
                return true;

            return classSpec.IsValidKeyToLog(key);
        }

        private bool GetIsPaused()
        {
            ClassSpecBase classSpec = GetActiveClassSpec();
            if (classSpec == null)
                return false;

            return classSpec.IsPaused;
        }

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
