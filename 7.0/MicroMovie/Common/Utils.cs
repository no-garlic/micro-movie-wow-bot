using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Windows.Input;
using System.Runtime.InteropServices;
using CommonLib;

namespace MicroMovie
{
    public class Utils
    {
        public static class WS_EX
        {
            public static int Transparent = 0x20;
            public static int Layered     = 0x80000;
        }

        public enum GWL
        {
            ExStyle = -20
        }
        
        public enum LWA
        {
            ColorKey = 0x1,
            Alpha    = 0x2
        }

        [DllImport("user32.dll")] 
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        [DllImport("user32.dll", EntryPoint = "GetWindowLong")]
        public static extern int GetWindowLong(IntPtr hWnd, GWL nIndex);

        [DllImport("user32.dll", EntryPoint = "SetWindowLong")]
        public static extern int SetWindowLong(IntPtr hWnd, GWL nIndex, int dwNewLong);

        [DllImport("user32.dll", EntryPoint = "SetLayeredWindowAttributes")]
        public static extern bool SetLayeredWindowAttributes(IntPtr hWnd, int crKey, byte alpha, LWA dwFlags);

        private static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
        private const UInt32 SWP_NOSIZE = 0x0001;
        private const UInt32 SWP_NOMOVE = 0x0002;
        private const UInt32 TOPMOST_FLAGS = SWP_NOMOVE | SWP_NOSIZE;

        public static void SetWindowOnTop(Form form)
        {
            SetWindowPos(form.Handle, HWND_TOPMOST, 0, 0, 0, 0, TOPMOST_FLAGS);
        }

        public static void SetWindowClickThrough(Form form)
        {
            int wl = GetWindowLong(form.Handle, GWL.ExStyle);
            wl = wl | WS_EX.Layered | WS_EX.Transparent;
            SetWindowLong(form.Handle, GWL.ExStyle, wl);
            SetLayeredWindowAttributes(form.Handle, 0, 128, LWA.Alpha);
        }

        public static void CopyImageToBitmap(Image from, Bitmap to)
        {            
            to.SetResolution(from.HorizontalResolution, from.VerticalResolution);

            using (var graphics = Graphics.FromImage(to))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    Rectangle bitmapRect = new Rectangle(0, 0, to.Width, to.Height);
                    graphics.DrawImage(from, bitmapRect, 0, 0, from.Width, from.Height, GraphicsUnit.Pixel, wrapMode);        
                }
            }
        }

        public static void AddBordersToBitmap(Bitmap bitmap, int rectCount)
        {
            using (var graphics = Graphics.FromImage(bitmap))
            {
                for (int i = 0; i < rectCount; ++i)
                {
                    Rectangle rect = new Rectangle((i * bitmap.Width) / rectCount, 0, (bitmap.Width / rectCount), bitmap.Height);
                    graphics.DrawRectangle(new Pen(Brushes.White), rect);
                }
            }
        }


    }
}
