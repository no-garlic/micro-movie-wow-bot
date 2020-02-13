using System;
using System.Diagnostics;
using System.Windows.Forms;
using CommonLib;

namespace OceanMaster
{
    public class MouseRecorder
    {
        public static int RECORD_DELAY = 1000;

        private MouseGesture m_MouseGesture;
        private Stopwatch    m_Stopwatch;
        private bool         m_TestRotate = false;

        public MouseRecorder()
        {
            m_MouseGesture = new MouseGesture();
            m_Stopwatch = new Stopwatch();
        }

        public void Update(MouseArgs args)
        {
            m_Stopwatch.Restart();
            m_MouseGesture.Add(args);
        }

        public bool Tick()
        {            
            if (!m_TestRotate && m_Stopwatch.ElapsedMilliseconds >= RECORD_DELAY)
            {
                m_MouseGesture.Transform(new System.Drawing.Point(100, 400), new System.Drawing.Point(700, 400));
                m_TestRotate = true;
                return true;
            }

            if (m_Stopwatch.ElapsedMilliseconds >= RECORD_DELAY * 2)
            {
                m_Stopwatch.Stop();
                m_TestRotate = false;

                if (m_MouseGesture.Count > 0)
                {
                    m_MouseGesture.Save();
                    m_MouseGesture.Send(null);
                    m_MouseGesture.Clear();                    
                    return true;
                }

            }
            return false;
        }

        public void Paint(PaintEventArgs e)
        {
            m_MouseGesture.Draw(e);
        }


    }
}
