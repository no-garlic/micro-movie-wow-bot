using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using CommonLib;

namespace MicroMovie
{
    [Flags]
    public enum ModifierKey : uint
    {
        None         = 0,
        Alt          = 1,
        Ctrl         = 2,
        Shift        = 4,
        AltCtrl      = Alt | Ctrl,
        AltShift     = Alt | Shift,
        AltCtrlShift = Alt | Ctrl | Shift,
        CtrlShift    = Ctrl | Shift
    }

    public class KeyPressedEventArgs : EventArgs
    {
        internal KeyPressedEventArgs(ModifierKey modifier, Keys key)
        {
            Modifier = modifier;
            Key = key;
        }

        public ModifierKey Modifier { get; }
        public Keys Key { get; }
    }

    public class Hotkey
    {
        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);
        
        private readonly Window m_Window = new Window();
        private int m_CurrentId;

        public event EventHandler<KeyPressedEventArgs> KeyPressed;

        public Hotkey()
        {
            m_Window.KeyPressed += delegate (object sender, KeyPressedEventArgs args)
            {
                KeyPressed?.Invoke(this, args);
            };
        }

        public void RegisterHotKey(ModifierKey modifier, Keys key)
        {
            m_CurrentId = m_CurrentId + 1;

            if (!RegisterHotKey(m_Window.Handle, m_CurrentId, (uint)modifier, (uint)key))
                throw new InvalidOperationException("Couldn’t register the hot key.");
        }

        private sealed class Window : NativeWindow
        {
            private static int WM_HOTKEY = 0x0312;

            public event EventHandler<KeyPressedEventArgs> KeyPressed;
            
            public Window()
            {
                CreateHandle(new CreateParams());
            }

            protected override void WndProc(ref Message m)
            {
                base.WndProc(ref m);

                if (m.Msg == WM_HOTKEY)
                {
                    Keys key = (Keys)(((int)m.LParam >> 16) & 0xFFFF);

                    ModifierKey modifier = (ModifierKey)((int)m.LParam & 0xFFFF);

                    KeyPressed?.Invoke(this, new KeyPressedEventArgs(modifier, key));
                }
            }

            public void Close()
            {
                DestroyHandle();
            }

            public static Keys toKey(string keystr)
            {
                return (Keys)Enum.Parse(typeof(Keys), keystr);
            }

            public static ModifierKey toModifier(string keystr)
            {
                return (ModifierKey)Enum.Parse(typeof(ModifierKey), keystr);
            }
        }
        
        public void Close()
        {
            for (int i = m_CurrentId; i > 0; i--)
            {
                UnregisterHotKey(m_Window.Handle, i);
            }

            m_Window.Close();
        }


    }
}
