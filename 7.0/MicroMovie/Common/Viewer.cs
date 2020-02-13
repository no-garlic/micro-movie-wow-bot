using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Input;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using CommonLib;

namespace MicroMovie
{
    public partial class Viewer : Form
    {
        private MicroMovie m_Owner;
        private bool m_ClickThroughWindow;
        private Bitmap m_Bitmap;

        public Viewer(MicroMovie owner, bool clickThroughWindow)
        {
            m_Owner = owner;
            m_ClickThroughWindow = clickThroughWindow;
            InitializeComponent();
        }

        private void Viewer_Load(object sender, EventArgs e)
        {
            SetWindowSize();
            SetWindowLocation();
            Utils.SetWindowOnTop(this);
        }

        private void SetWindowSize()
        {
            int IDEAL_SIZE_SCREEN_FACTOR = 3;

            int screenMax = Screen.AllScreens[m_Owner.CurrentScreen].Bounds.Width;
            int dataCount = Math.Max(1, App.Class.DataCount);

            int width  = dataCount;
            int height = 1;

            int screenIdeal = screenMax / IDEAL_SIZE_SCREEN_FACTOR;
            while (width < screenIdeal - dataCount)
            {
                width  += dataCount;
                height += 1;
            }

            m_Border.Location = new Point(1, 1);
            m_Border.Size = new Size(width + 6, height + 6);

            m_PictureBox.Size = new Size(width, height);
            m_PictureBox.Location = new Point(2, 2);
            
            Width  = width += 8;
            Height = height += 8;

            m_Bitmap = new Bitmap(width, height, PixelFormat.Format32bppArgb);

            using (var graphics = Graphics.FromImage(m_Bitmap))
            {
                graphics.Clear(Color.Black);
            }

            m_PictureBox.Image = m_Bitmap;
        }

        public void SetWindowLocation()
        {
            Location = m_Owner.Location + new Size(m_Owner.Size.Width - Width, m_Owner.Size.Height);
        }

        public void UpdatePictureBox()
        {
            SetWindowLocation();

            Utils.CopyImageToBitmap(App.Capture.CaptureBitmap, m_Bitmap);
            Utils.AddBordersToBitmap(m_Bitmap, App.Class.DataCount);
            m_PictureBox.Image = m_Bitmap;
            m_PictureBox.Invalidate();
        }

        public void ClearPictureBox()
        {
            using (var graphics = Graphics.FromImage(m_Bitmap))
            {
                graphics.Clear(Color.Black);
            }
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            if (m_ClickThroughWindow)
            {
                Utils.SetWindowClickThrough(this);
            }
        }

        private void Viewer_FormClosing(object sender, FormClosingEventArgs e)
        {
            m_Owner.OnViewerClosed();
        }

        private void Viewer_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == KeyInterop.VirtualKeyFromKey(Key.Escape))
            {
                App.Close();
            }
        }
    }
}
