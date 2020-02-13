using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Windows.Forms;
using Microsoft.Win32;

namespace CommonLib
{
    public class Capture
    {
        class Variable
        {
            public string m_Name;
            public Point  m_Point;
            public int    m_RawValue;
            public Color  m_Color;
        };

        public Rectangle CaptureRectangle { get; private set; }
        public Bitmap    CaptureBitmap    { get; private set; }

        Dictionary<string, Variable> m_Variables;

        public Capture()
        {
            m_Variables = new Dictionary<string, Variable>();
        }
        
        public void RegisterVariable(string name, Point point)
        {
            RegisterVariable(name, point.X, point.Y);
        }
        
        public void RegisterVariable(string name, int x, int y)
        {
            Variable v = new Variable();
            v.m_Name = name;
            v.m_Point.X = x;
            v.m_Point.Y = y;
            v.m_RawValue = 0;
            v.m_Color = Color.Black;
            m_Variables.Add(v.m_Name, v);
            UpdateCaptureRectangle();
        }

        private void UpdateCaptureRectangle()
        {
            Size size = new Size(Config.FrameSize * m_Variables.Count, Config.FrameSize);
            Rectangle rect = new Rectangle(new Point(0, 0), size);             

            CaptureRectangle = rect;
            CaptureBitmap = new Bitmap(CaptureRectangle.Width, CaptureRectangle.Height, PixelFormat.Format32bppArgb);
        }

        public bool UpdatePixels()
        {
            try
            {
                Rectangle rect = Config.Corner.GetCaptureRectangle(CaptureRectangle.Size);

                Point loc = rect.Location;
                Size size = rect.Size;

                if (CaptureBitmap == null || size.Width == 0 || size.Height == 0)
                    return false;
            
                Graphics dest = Graphics.FromImage(CaptureBitmap);
                dest.CopyFromScreen(loc.X, loc.Y, 0, 0, size, CopyPixelOperation.SourceCopy);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        
        public Color GetPixel(int x, int y)
        {
            if (CaptureRectangle.Contains(x, y))
            {
                int xPos = x - CaptureRectangle.Location.X;
                int yPos = y - CaptureRectangle.Location.Y;

                Color color = CaptureBitmap.GetPixel(xPos, yPos);
                return color;
            }

            return Color.Black;
        }
        
        public void DisplayPixels(Control control)
        {
            Size size = CaptureRectangle.Size;

            if (CaptureBitmap == null || size.Width == 0 || size.Height == 0)
                return;

            control.Size = size;
            
            if (control.BackgroundImage != CaptureBitmap)
                control.BackgroundImage = CaptureBitmap;
            else
                control.Invalidate();
        }

        public void DisplayPixels(PictureBox control)
        {
            Size size = CaptureRectangle.Size;

            if (CaptureBitmap == null || size.Width == 0 || size.Height == 0)
                return;

            control.Size = size;

            if (control.Image != CaptureBitmap)
                control.Image = CaptureBitmap;
            else
                control.Invalidate();
        }

        public void UpdateVariables()
        {
            var variables = m_Variables.Values;
            foreach (Variable v in variables)
            {
                Color color = GetPixel(v.m_Point.X, v.m_Point.Y);

                int red   = color.R;
                int green = color.G;
                int blue  = color.B;
                int colorIntensity = Math.Max(Math.Max(red, green), blue);                
                int colorValue = red | green << 8 | blue << 16;

                m_Variables[v.m_Name].m_Color    = color;
                m_Variables[v.m_Name].m_RawValue = colorValue;
            }
        }

        public Color GetColor(string name)
        {
            if (m_Variables.ContainsKey(name))
            {
                Variable v = m_Variables[name];
                return v.m_Color;
            }
            return Color.Black;
        }
                
        public int GetRaw(string name)
        {
            if (m_Variables.ContainsKey(name))
            {
                Variable v = m_Variables[name];
                return v.m_RawValue;
            }
            return 0;
        }
        
    }
}
