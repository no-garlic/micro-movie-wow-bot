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
}
