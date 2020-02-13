using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Input;
using CommonLib;

namespace OceanMaster
{
    public partial class OceanMaster : Form
    {
        public int CurrentScreen { get; private set; }

        private bool m_ClickThroughWindow = false;

        private bool m_MouseDown;
        private bool m_MouseDownMoved;
        private Point m_MousePos;
        private Stopwatch m_Stopwatch;
        private int m_OpacityIndex;
        private Hotkey m_Hotkey;
        private bool m_UseViewer;
        private Control m_ParentControl;
        private MouseHook m_MouseHook;
        private MouseRecorder m_MouseRecorder;

        public OceanMaster()
        {
            InitializeComponent();

            App.Create(Config.DefaultClassID);

            m_Hotkey = new Hotkey();
            m_Stopwatch = new Stopwatch();
            m_MousePos = new Point();
            m_MouseDown = false;
            m_MouseDownMoved = false;
            m_UseViewer = false;
            m_OpacityIndex = 0;
            CurrentScreen = 0;
            m_ParentControl = null;
            m_MouseRecorder = new MouseRecorder();
        }

        private void OceanMaster_Load(object sender, EventArgs e)
        {
            m_Timer.Start();
            
            SetWindowLocation();
            Utils.SetWindowOnTop(this);

            m_Hotkey.RegisterHotKey(ModifierKey.AltCtrl, Keys.H);
            m_Hotkey.RegisterHotKey(ModifierKey.AltCtrl, Keys.Q);
            m_Hotkey.RegisterHotKey(ModifierKey.AltCtrl, Keys.V);
            m_Hotkey.RegisterHotKey(ModifierKey.AltCtrl, Keys.P);
            m_Hotkey.KeyPressed += OnHotkeyPressed;

            m_MouseHook = new MouseHook();
            m_MouseHook.HookMouse();
            m_MouseHook.OnMouseEvent += OnMouseEvent;

            m_ParentControl = this;
            SetChildControlEvents(this);
        }

        private void SetDefaultWindowLocation()
        {
            Rectangle screenBounds = Screen.PrimaryScreen.Bounds;
            Point windowLocation = new Point(0, screenBounds.Height - Size.Height);
            Location = windowLocation;
        }

        private void SetWindowLocation()
        {
            SetDefaultWindowLocation();

            CurrentScreen = 0;

            if (Screen.AllScreens.Length == 1)
            {
                Location = new Point(0, Screen.PrimaryScreen.Bounds.Y);
                return;
            }

            for (int i = 0; i < Screen.AllScreens.Length; ++i)
            {
                Rectangle screenBounds = Screen.AllScreens[i].Bounds;
                if (screenBounds.Left < Screen.PrimaryScreen.Bounds.Left)
                {
                    CurrentScreen = i;
                    Point windowLocation = new Point(-Size.Width, screenBounds.Y);
                    Location = windowLocation;
                    return;
                }
            }
        }

        private void SetChildControlEvents(Control parent)
        {
            foreach (Control control in parent.Controls)
            {
                control.MouseDoubleClick += new MouseEventHandler(OceanMaster_MouseDoubleClick);
                control.KeyPress += new KeyPressEventHandler(OceanMaster_KeyPress);
                control.MouseDown += new MouseEventHandler(OceanMaster_MouseDown);
                control.MouseLeave += new EventHandler(OceanMaster_MouseLeave);
                control.MouseMove += new MouseEventHandler(OceanMaster_MouseMove);
                control.MouseUp += new MouseEventHandler(OceanMaster_MouseUp);

                SetChildControlEvents(control);
            }
        }

        private void OnHotkeyPressed(object sender, KeyPressedEventArgs e)
        {
            if (e.Modifier == ModifierKey.AltCtrl && e.Key == Keys.H)
            {
                if (WindowState != FormWindowState.Minimized)
                {
                    WindowState = FormWindowState.Minimized;
                }
                else
                {
                    WindowState = FormWindowState.Normal;
                }
            }
            if (e.Modifier == ModifierKey.AltCtrl && e.Key == Keys.V)
            {
                ToggleViewer();
            }
            if (e.Modifier == ModifierKey.AltCtrl && e.Key == Keys.Q)
            {
                Close();
            }
            if (e.Modifier == ModifierKey.AltCtrl && e.Key == Keys.P)
            {
                TogglePauseOverride();
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
        
        private void m_Timer_Tick(object sender, EventArgs e)
        {
            if (m_MouseRecorder.Tick())
            {
                pnlDraw.Invalidate();
            }

            int classID = App.Class.ShouldChangeClass();
            if (classID > 0 && App.IsClassRegistered(classID))
            {
                App.Close();
                App.Create(classID);
                return;
            }            

            if (App.Update())
            {
                UpdateInfo();

                Viewer.Instance?.UpdatePictureBox();
                m_Timer.Interval = 10;
                ShowViewer();
            }
            else
            {
                ClearInfo();

                Viewer.Instance?.ClearPictureBox();
                m_Timer.Interval = 500;
            }
        }
        
        private void OceanMaster_FormClosing(object sender, FormClosingEventArgs e)
        {
            m_Hotkey.Close();
            App.Close();
            App.Exit();
        }

        private void OceanMaster_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                m_MouseDown = true;
                m_MouseDownMoved = false;
                m_MousePos = e.Location;
            }
        }

        private void OceanMaster_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                m_MouseDown = false;
                if (m_MouseDownMoved == false)
                {
                    // left click handler
                }
                m_MouseDownMoved = false;
            }
            if (e.Button == MouseButtons.Right)
            {
                ToggleViewer();
            }
        }

        private void OceanMaster_MouseLeave(object sender, EventArgs e)
        {
            m_MouseDown = false;
            m_MouseDownMoved = false;
        }

        private void OceanMaster_MouseMove(object sender, MouseEventArgs e)
        {
            if (m_MouseDown && e.Button == MouseButtons.Left)
            {
                Size delta = new Size(e.Location.X - m_MousePos.X, e.Location.Y - m_MousePos.Y);

                if (Math.Abs(delta.Width) + Math.Abs(delta.Height) >= 16 || m_MouseDownMoved)
                {
                    this.Location = this.Location + delta;
                    m_MouseDownMoved = true;

                    Viewer.Instance?.SetWindowLocation();
                }
            }
        }

        private void OceanMaster_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == KeyInterop.VirtualKeyFromKey(Key.Escape))
            {
                App.Close();
                App.Exit();
            }
        }

        private void UpdateOpacity()
        {
            m_OpacityIndex = (m_OpacityIndex + 1) % 3;
            float[] opacityValues = { 1.0f, 0.6f, 0.3f };
            Opacity = opacityValues[m_OpacityIndex];
        }

        private void OceanMaster_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            // double click handler
        }

        private void TogglePauseOverride()
        {
            App.TogglePauseOverride();
        }
        
        private void ToggleViewer()
        {
            m_UseViewer = !m_UseViewer;

            if (m_UseViewer && Viewer.Instance == null)
            {
                ShowViewer();
            }
            else
            {
                HideViewer();
            }
        }

        private void ShowViewer()
        {
            if (m_UseViewer)
            {
                if (Viewer.Instance == null)
                {
                    Viewer.Instance = new Viewer(this, m_ClickThroughWindow);
                    Viewer.Instance.Show();
                    return;
                }
            }
            else
            {
                HideViewer();
            }
        }

        private void HideViewer()
        {
            if (Viewer.Instance != null)
            {
                Viewer.Instance.Close();
                Viewer.Instance = null;
            }
        }

        private void UpdateInfo()
        {
            if (App.Class.AreMagicNumbersValid() == false)
            {
                ClearInfo();
                return;
            }
        }

        private void ClearInfo()
        {
        }

        public void OnMouseEvent(object sender, MouseArgs e)
        {
            m_MouseRecorder.Update(e);
            pnlDraw.Invalidate();
        }

        private void pnlDraw_Paint(object sender, PaintEventArgs e)
        {
            base.OnPaint(e);
            m_MouseRecorder.Paint(e);
        }
    }
}
