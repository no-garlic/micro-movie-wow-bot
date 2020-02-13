using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Input;
using CommonLib;

namespace LittleBattles
{
    public partial class LittleBattles : Form
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
        private PetDirector m_PetDirector;
        private Queue<string> m_MessageLog;

        public LittleBattles()
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
            m_MessageLog = new Queue<string>();
            m_PetDirector = new PetDirector(ShowMessage);
        }

        private void LittleBattles_Load(object sender, EventArgs e)
        {
            m_Timer.Start();
            
            SetWindowLocation();
            Utils.SetWindowOnTop(this);

            m_Hotkey.RegisterHotKey(ModifierKey.AltCtrl, Keys.H);
            m_Hotkey.RegisterHotKey(ModifierKey.AltCtrl, Keys.Q);
            m_Hotkey.RegisterHotKey(ModifierKey.AltCtrl, Keys.V);
            m_Hotkey.RegisterHotKey(ModifierKey.AltCtrl, Keys.P);

            m_Hotkey.RegisterHotKey(ModifierKey.AltCtrl, Keys.A);
            m_Hotkey.RegisterHotKey(ModifierKey.AltCtrl, Keys.O);
            m_Hotkey.RegisterHotKey(ModifierKey.AltCtrl, Keys.S);

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
                control.MouseDoubleClick += new MouseEventHandler(LittleBattles_MouseDoubleClick);
                control.KeyPress += new KeyPressEventHandler(LittleBattles_KeyPress);
                control.MouseDown += new MouseEventHandler(LittleBattles_MouseDown);
                control.MouseLeave += new EventHandler(LittleBattles_MouseLeave);
                control.MouseMove += new MouseEventHandler(LittleBattles_MouseMove);
                control.MouseUp += new MouseEventHandler(LittleBattles_MouseUp);

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
            if (e.Modifier == ModifierKey.AltCtrl && e.Key == Keys.A)
            {
                if (btnAuto.Enabled)
                {
                    btnAuto_Click(sender, e);
                }                
            }
            if (e.Modifier == ModifierKey.AltCtrl && e.Key == Keys.O)
            {
                if (btnSingle.Enabled)
                {
                    btnSingle_Click(sender, e);
                }
            }
            if (e.Modifier == ModifierKey.AltCtrl && e.Key == Keys.S)
            {
                if (btnStop.Enabled)
                {
                    btnStop_Click(sender, e);
                }
            }
        }

        private void UpdateTooltip()
        {
            string tooltip = "\nCtrl-Alt-Q : Quit";
            tooltip += "\nCtrl-Alt-H : Hide";
            tooltip += "\nCtrl-Alt-V : View";
            tooltip += "\nCtrl-Alt-P : Pause";
            tooltip += "\nCtrl-Alt-A : Auto";
            tooltip += "\nCtrl-Alt-O : Once";
            tooltip += "\nCtrl-Alt-S : Stop";
            m_ToolTip.SetToolTip(m_ParentControl, tooltip);
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            if (m_ClickThroughWindow)
            {
                Utils.SetWindowClickThrough(this);
            }

            txtLog.Focus();
        }

        private void m_Timer_Tick(object sender, EventArgs e)
        {
            if (m_PetDirector.IsRunning)
            {
                btnStop.Enabled = true;
                btnAuto.Enabled = false;
                btnSingle.Enabled = false;
            }
            else
            {
                btnStop.Enabled = false;
                btnAuto.Enabled = true;
                btnSingle.Enabled = true;
            }

            m_PetDirector.UseSafariHat = cbSafariHat.Checked;
            m_PetDirector.UsePetTreat = cbPetTreat.Checked;
            m_PetDirector.UseLesserTreat = cbLesserTreat.Checked;

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
                m_PetDirector.Update();

                Viewer.Instance?.UpdatePictureBox();
                m_Timer.Interval = 10;
                ShowViewer();
            }
            else
            {
                ClearInfo();

                Viewer.Instance?.ClearPictureBox();
                m_Timer.Interval = 5000;
            }
        }
        
        private void LittleBattles_FormClosing(object sender, FormClosingEventArgs e)
        {
            m_Hotkey.Close();
            App.Close();
            App.Exit();
        }

        private void LittleBattles_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                m_MouseDown = true;
                m_MouseDownMoved = false;
                m_MousePos = e.Location;
            }
        }

        private void LittleBattles_MouseUp(object sender, MouseEventArgs e)
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

        private void LittleBattles_MouseLeave(object sender, EventArgs e)
        {
            m_MouseDown = false;
            m_MouseDownMoved = false;
        }

        private void LittleBattles_MouseMove(object sender, MouseEventArgs e)
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

        private void LittleBattles_KeyPress(object sender, KeyPressEventArgs e)
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

        private void LittleBattles_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (Height > 400)
            {
                Height = 167;
                txtLog.Visible = false;
            }
            else
            {
                Height = 455;
                txtLog.Visible = true;
            }
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
            if (m_MessageLog.Count > 0)
            {
                string message = m_MessageLog.Dequeue();
                
                txtLog.AppendText(message);
                txtLog.AppendText("\r\n");
            }

            bool valid = App.Class.AreMagicNumbersValid();
            if (!valid)
            {
                ClearInfo();
                return;
            }

            pnlDataValid.BackColor = Color.LightGreen;
            pnlInBattle.BackColor = App.Class.PetInfo.IsInBattle ? Color.LightGreen : Color.Pink;
            pnlTurnReady.BackColor = App.Class.PetInfo.IsTurnReady ? Color.LightGreen : Color.Pink;

            pnlSafariHat.BackColor = cbSafariHat.Checked ? (App.Class.PetInfo.IsSafariHat ? Color.LightGreen : Color.Pink) : Color.Gray;
            pnlLesserTreat.BackColor = cbLesserTreat.Checked ? (App.Class.PetInfo.IsPetTreat ? Color.LightGreen : Color.Pink) : Color.Gray;
            pnlPetTreat.BackColor = cbPetTreat.Checked ? (App.Class.PetInfo.IsLesserTreat ? Color.LightGreen : Color.Pink) : Color.Gray;

            pnlTeensy.BackColor = Color.LightGreen;
            
            float cooldown = App.Class.PetInfo.ReviveCooldown;
            TimeSpan span = new TimeSpan(0, 0, (int) Math.Round(cooldown));
            string cooldownString = string.Format("{0}:{1:00}", (int) span.TotalMinutes, span.Seconds);
            txtReviveCooldown.Text = $"Revive: {cooldownString}";

            pnlRevive.BackColor = (cooldown < 0.01f) ? Color.LightGreen : Color.Pink;
        }

        private void ClearInfo()
        {
            pnlDataValid.BackColor = Color.Pink;
            pnlInBattle.BackColor = Color.Pink;
            pnlTurnReady.BackColor = Color.Pink;

            pnlSafariHat.BackColor = Color.Pink;
            pnlLesserTreat.BackColor = Color.Pink;
            pnlPetTreat.BackColor = Color.Pink;

            pnlTeensy.BackColor = Color.LightGreen;

            txtReviveCooldown.Text = "Revive: 0:00";
            pnlRevive.BackColor = Color.Pink;
        }

        public void ShowMessage(string message)
        {
            m_MessageLog.Enqueue(message);
        }

        private void btnAuto_Click(object sender, EventArgs e)
        {
            if (App.Class.AreMagicNumbersValid() == false)
            {
                MessageBox.Show("No connection to the game found", "Error", MessageBoxButtons.OK);
                return;
            }

            txtLog.Clear();

            bool result = m_PetDirector.Start("auto");
            if (result == false)
            {
                MessageBox.Show("Failed to start the script", "Error", MessageBoxButtons.OK);
            }
        }

        private void btnSingle_Click(object sender, EventArgs e)
        {
            if (App.Class.AreMagicNumbersValid() == false)
            {
                MessageBox.Show("No connection to the game found", "Error", MessageBoxButtons.OK);
                return;
            }

            txtLog.Clear();

            bool result = m_PetDirector.Start("single");
            if (result == false)
            {
                MessageBox.Show("Failed to start the script", "Error", MessageBoxButtons.OK);
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            m_PetDirector.Stop();
        }

    }
}
