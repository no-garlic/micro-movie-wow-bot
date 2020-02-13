using System;
using System.Diagnostics;

namespace CommonLib
{
    public class TTD : Data
    {
        public float Timer       { get; private set; }
        public bool  IsValid     { get; private set; }
        public bool  IsNotValid  { get { return !IsValid; } }

        private float[] m_Samples;
        private int m_Index;
        private Stopwatch m_Timer;
        private System.Drawing.Color c;
        
        public TTD() : base(40)
        {
            m_Timer = new Stopwatch();
            m_Samples = new float[4];
            m_Index = 0;
        }

        public override void Update()
        {
            c = GetColor();

            Timer = Conversion.ToTime(this);

            if (c.R == 255 && c.G == 255 && c.B == 255)
            {
                Timer = 0;
                IsValid = false;
                m_Timer.Stop();
                m_Timer.Reset();
                return;
            }

            if (Timer > 0 && !m_Timer.IsRunning)
            {
                m_Timer.Start();
            }
            else if (Timer == 0 && m_Timer.IsRunning)
            {
                m_Timer.Stop();
                m_Timer.Reset();
            }

            if (m_Timer.IsRunning && m_Timer.ElapsedMilliseconds < 1000)
            {
                Timer = 0;
            }

            if (Timer == 0)
            {
                ClearSamples();
            }
            else
            {
                AddSample(Timer);
                Timer = GetSample();
            }

            IsValid = (Timer > 0);
        }

        private void ClearSamples()
        {
            m_Samples[0] = 0.0f;
            m_Samples[1] = 0.0f;
            m_Samples[2] = 0.0f;
            m_Samples[3] = 0.0f;
            m_Index = 0;
        }

        private void AddSample(float value)
        {
            m_Samples[m_Index] = value;
            m_Index++;
            if (m_Index >= 4)
                m_Index = 0;
        }

        private float GetSample()
        {
            float value = 0.0f;
            float count = 0;

            for (int i = 0; i < 4; i++)
            {
                float sample = m_Samples[i];
                if (sample > 0.0f)
                {
                    value += sample;
                    count++;
                }
            }

            if (count == 0)
                return 0.0f;

            value = value / (float) count;
            return value;
        }

        public override void LogData()
        {
        }
    }
}
