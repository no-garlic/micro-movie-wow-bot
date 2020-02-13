﻿using System;
using System.IO;
using Microsoft.Win32;
using System.Diagnostics;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CommonLib
{
    public class ScreenCorner
    {
        public static ScreenCorner TopLeft     = new ScreenCorner("TOPLEFT",      1, false, false, false);
        public static ScreenCorner TopRight    = new ScreenCorner("TOPRIGHT",    -1, true,  true,  false);
        public static ScreenCorner BottomLeft  = new ScreenCorner("BOTTOMLEFT",   1, false, false, true);
        public static ScreenCorner BottomRight = new ScreenCorner("BOTTOMRIGHT", -1, true,  true,  true);

        public  string ScriptToken { get; private set; }
        public  int    Scalar      { get; private set; }
        public  bool   Flip        { get; private set; }
        private bool   X           { get; set; }
        private bool   Y           { get; set; }

        private ScreenCorner(string scriptToken, int scalar, bool flip, bool x, bool y)
        {
            ScriptToken = scriptToken;
            Scalar = scalar;
            Flip = flip;
            X = x;
            Y = y;
        }

        public Rectangle GetCaptureRectangle(Size captureSize)
        {
            Point location = new Point();

            int scale = 1;

            if (SystemInformation.ComputerName.StartsWith("Mac"))
            {
                scale = 2;
            }

            int screenWidth  = Screen.PrimaryScreen.Bounds.Width  * scale;
            int screenHeight = Screen.PrimaryScreen.Bounds.Height * scale;

            if (X) { location.X = screenWidth  - captureSize.Width;  }
            if (Y) { location.Y = screenHeight - captureSize.Height; }
            
            return new Rectangle(location, captureSize);
        }

    }
}
