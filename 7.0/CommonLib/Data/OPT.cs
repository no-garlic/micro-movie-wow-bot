using System;
using System.Drawing;

namespace CommonLib
{
    public class OPT : Data
    {
        public static int NotSelected   = 0;
        public static int First         = 1;
        public static int Second        = 2;
        public static int Third         = 3;

        public int MyLevel { get; private set; } 
        public int Tier1   { get { return m_Chosen[0]; } }
        public int Tier2   { get { return m_Chosen[1]; } }
        public int Tier3   { get { return m_Chosen[2]; } }
        public int Tier4   { get { return m_Chosen[3]; } }
        public int Tier5   { get { return m_Chosen[4]; } }
        public int Tier6   { get { return m_Chosen[5]; } }
        public int Tier7   { get { return m_Chosen[6]; } }

        private int[] m_Chosen;
        private Color m_Previous;

        public event EventHandler<EventArgs> OnChanged;

        public OPT() : base(20)
        {
            m_Chosen   = new int[7];
            m_Previous = Color.Black;
        }
        
        public int GetSelected(int tier)
        {
            return m_Chosen[tier - 1];
        }

        public override void Update()
        {
            Color c = GetColor();

            int r = c.R;
            int g = c.G;

            MyLevel = Conversion.ToInt(this, ColorChannel.Blue);

            m_Chosen[0] = (r >> 6) & 3;
            m_Chosen[1] = (r >> 4) & 3;
            m_Chosen[2] = (r >> 2) & 3;
            m_Chosen[3] = (r     ) & 3;
            m_Chosen[4] = (g >> 6) & 3;
            m_Chosen[5] = (g >> 4) & 3;
            m_Chosen[6] = (g >> 2) & 3;

            if (m_Previous != c)
            {
                m_Previous = c;
                OnChanged?.Invoke(this, new EventArgs());
            }
        }

        public override void LogData()
        {
            string talents = "";
            for (int i = 0; i < 7; ++i)
                talents += m_Chosen[i].ToString();

            App.Log.Write(Log.LogDetail.High, "Talents", talents);
        }

    }
}

