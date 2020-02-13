using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Input;
using CommonLib;

namespace MovieMaker
{
    public partial class MovieMaker : Form
    {
        public int CurrentScreen { get; private set; }

        private bool m_ClickThroughWindow = false;

        private bool m_MouseDown;
        private bool m_MouseDownMoved;
        private Point m_MousePos;
        private Stopwatch m_Stopwatch;
        private long m_ElapsedTime;
        private long m_Counter;
        private int m_OpacityIndex;
        private Hotkey m_Hotkey;
        private bool m_UseViewer;
        private Control m_ParentControl;

        public MovieMaker()
        {
            InitializeComponent();

            App.Create(Config.DefaultClassID);

            m_Hotkey = new Hotkey();
            m_Stopwatch = new Stopwatch();
            m_MousePos = new Point();
            m_MouseDown = false;
            m_MouseDownMoved = false;
            m_UseViewer = false;
            m_ElapsedTime = 0;
            m_Counter = 0;
            m_OpacityIndex = 0;
            CurrentScreen = 0;
            m_ParentControl = null;
        }

        private void MicroMovie_Load(object sender, EventArgs e)
        {
            m_Timer.Start();
            
            SetWindowLocation();
            Utils.SetWindowOnTop(this);

            m_Hotkey.RegisterHotKey(ModifierKey.AltCtrl, Keys.H);
            m_Hotkey.RegisterHotKey(ModifierKey.AltCtrl, Keys.Q);
            m_Hotkey.RegisterHotKey(ModifierKey.AltCtrl, Keys.V);
            m_Hotkey.RegisterHotKey(ModifierKey.AltCtrl, Keys.P);
            m_Hotkey.KeyPressed += OnHotkeyPressed;

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
            UpdateTooltip();

            foreach (Control control in parent.Controls)
            {
                control.MouseDoubleClick += new MouseEventHandler(MicroMovie_MouseDoubleClick);
                control.KeyPress += new KeyPressEventHandler(MicroMovie_KeyPress);
                control.MouseDown += new MouseEventHandler(MicroMovie_MouseDown);
                control.MouseLeave += new EventHandler(MicroMovie_MouseLeave);
                control.MouseMove += new MouseEventHandler(MicroMovie_MouseMove);
                control.MouseUp += new MouseEventHandler(MicroMovie_MouseUp);

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

        private void UpdateTooltip()
        {
            string tooltip = "\nCtrl-Alt-Q : Quit\nCtrl-Alt-H : Hide";
            tooltip += "\nCtrl-Alt-V : View\nCtrl-Alt-P : Pause";
            m_ToolTip.SetToolTip(m_ParentControl, tooltip);
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
            ShowFrameRate();

            int classID = App.Class.ShouldChangeClass();
            if (classID > 0 && App.IsClassRegistered(classID))
            {
                App.Close();
                App.Create(classID);
                return;
            }            

            if (App.Update())
            {
                Viewer.Instance?.UpdatePictureBox();

                ClassSpecBase myClassSpec = App.Class.GetActiveClassSpec() as ClassSpecBase;
                ShowClassSpec(myClassSpec);
                ShowBinding(myClassSpec);
                ShowBossMode(myClassSpec);

                if (myClassSpec == null || App.Class.IsNotOK)
                    m_Timer.Interval = 1000;
                else
                    m_Timer.Interval = 10;

                ShowViewer();
            }
            else
            {
                ShowClassSpec(null);
                ShowBinding(null);
                ShowBossMode(null);
                Viewer.Instance?.ClearPictureBox();
                m_Timer.Interval = 5000;
            }
        }
        
        private void ShowClassSpec(ClassSpecBase myClassSpec)
        {
            if (myClassSpec != null && App.Class.IsOK)
            {
                m_Status.Text = "Connected";
                m_Status.ForeColor = Color.DarkGreen;

                m_Options.Text = myClassSpec.Talents;
                m_Options.ForeColor = Color.DarkGreen;

                switch (myClassSpec.SpecID)
                {
                    case 103: m_CurrentForm.Image = m_ImageList.Images[1]; break;
                    case 104: m_CurrentForm.Image = m_ImageList.Images[2]; break;
                }
            }
            else
            {
                m_Status.Text = "Waiting";
                m_Status.ForeColor = Color.DarkRed;
                m_CurrentForm.Image = m_ImageList.Images[0];

                m_Options.Text = "None";
                m_Options.ForeColor = Color.DarkRed;
            }
        }

        private void ShowBinding(ClassSpecBase myClassSpec)
        {
            if (myClassSpec != null && myClassSpec.IsReady)
            {
                m_Ok.Text = "Active";
                m_Ok.ForeColor = Color.DarkGreen;
            }
            else
            {
                m_Ok.Text = "Inactive";
                m_Ok.ForeColor = Color.DarkRed;
            }
        }

        private void ShowFrameRate()
        {
            if (m_Stopwatch.IsRunning)
            {
                m_Counter++;
                m_ElapsedTime += m_Stopwatch.ElapsedMilliseconds;
                m_Stopwatch.Reset();
                m_Stopwatch.Restart();

                if (m_ElapsedTime >= 1000 && m_Counter > 0)
                {
                    long fps = (long)((float)m_Counter * 1000 / (float)m_ElapsedTime);
                    m_ElapsedTime -= 1000;
                    m_Counter = 0;

                    if (fps > 1)
                    {
                        m_Fps.Text = "" + fps + " fps";
                        m_Fps.ForeColor = Color.Black;
                    }
                    else
                    {
                        m_Fps.Text = "0 fps";
                        m_Fps.ForeColor = Color.Black;
                    }
                }
            }
            else
            {
                m_Counter = 0;
                m_ElapsedTime = 0;
                m_Stopwatch.Reset();
                m_Stopwatch.Start();
            }
        }

        private void ShowBossMode(ClassSpecBase myClassSpec)
        {
            if (myClassSpec == null || App.Class == null || App.Class.IsNotOK)
            {
                m_BossModeText.Text  = "";
                m_BossMode.BackColor = m_BossMode.Parent.BackColor;
                return;
            }

            string text = "";
            if (myClassSpec.FightingABoss)
            {
                text = "[B] ";
                m_BossMode.BackColor = System.Drawing.SystemColors.MenuHighlight;
            }
            else
            {
                m_BossMode.BackColor = m_BossMode.Parent.BackColor;
            }

            if (myClassSpec.IsPaused || App.PauseOverride)
            {
                m_BossMode.BackColor = Color.Red;
            }

            float ttd = myClassSpec.TimeToDie;
            if (ttd <= 0.0f)
            {
                m_BossModeText.Text = text + "-----";
            }
            else
            {
                ttd = 0.1f * (float) ((int) (ttd * 10.0f));
                m_BossModeText.Text = text + $"{ttd} sec";
            }
        }

        private void MicroMovie_FormClosing(object sender, FormClosingEventArgs e)
        {
            m_Hotkey.Close();
            App.Close();
            App.Exit();
        }

        private void MicroMovie_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                m_MouseDown = true;
                m_MouseDownMoved = false;
                m_MousePos = e.Location;
            }
        }

        private void MicroMovie_MouseUp(object sender, MouseEventArgs e)
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

        private void MicroMovie_MouseLeave(object sender, EventArgs e)
        {
            m_MouseDown = false;
            m_MouseDownMoved = false;
        }

        private void MicroMovie_MouseMove(object sender, MouseEventArgs e)
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

        private void MicroMovie_KeyPress(object sender, KeyPressEventArgs e)
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

        private void MicroMovie_MouseDoubleClick(object sender, MouseEventArgs e)
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
        
        private void MicroMovie_MouseClick(object sender, MouseEventArgs e)
        {
        }
    }
}
