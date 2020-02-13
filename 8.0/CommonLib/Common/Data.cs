using System;
using System.IO;
using Microsoft.Win32;
using System.Diagnostics;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CommonLib
{
    public abstract class Data
    {
        public int NameIndex  { get; private set; }
        public int Index      { get; private set; }

        public Data(int nameIndex)
        {
            NameIndex = nameIndex;
            Index = App.Class.DataCount;

            App.Class.Add(this);

            int x = (Config.FrameSize / 2) + (Config.FrameSize * Index);
            int y = (Config.FrameSize / 2);
            App.Capture.RegisterVariable(NameIndex.ToString(), x, y);
        }

        public abstract void Update();

        public Color GetColor()
        {
            Color color = App.Capture.GetColor(NameIndex.ToString());
            return color;
        }

        public virtual void LogData()
        {
        }


    }
}
