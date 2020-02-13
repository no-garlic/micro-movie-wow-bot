using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Input;
using System.Collections.Generic;

namespace CommonLib
{
    public class KeyboardHook
    {
        private const int WH_KEYBOARD_LL = 13;

        private const int WM_KEYDOWN = 0x0100;
        private const int WM_KEYUP   = 0x0101;

        private const int KEYEVENTF_EXTENDEDKEY = 0x0001;
        private const int KEYEVENTF_KEYUP       = 0x0002;

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);
 
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);
 
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);
 
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);
 
        [DllImport("user32.dll")]
        public static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, uint dwExtraInfo);

        private Stopwatch m_Stopwatch;

        public delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);
         
        private Dictionary<int, int> m_RemappedKeys;
        private LowLevelKeyboardProc m_Proc;
        private IntPtr m_Hook = IntPtr.Zero;

        public event EventHandler<KeyPressedArgs> OnKeyPressed;

        public KeyboardHook()
        {
            m_Proc = HookCallback;
            m_RemappedKeys = new Dictionary<int, int>();
            m_Stopwatch = new Stopwatch();
        }
        
        public void HookKeyboard()
        {
            m_Hook = SetHook(m_Proc);
        }
 
        public void UnHookKeyboard()
        {
            UnhookWindowsHookEx(m_Hook);
        }
 
        private IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc, GetModuleHandle(curModule.ModuleName), 0);
            }
        }
 
        private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (OnKeyPressed == null)
            {
                return CallNextHookEx(m_Hook, nCode, wParam, lParam);
            }

            int keyMsg = (int) wParam;
            if (nCode >= 0 && (keyMsg == WM_KEYDOWN || keyMsg <= WM_KEYUP))
            {
                int vkCode = Marshal.ReadInt32(lParam);

                if (keyMsg == WM_KEYDOWN)
                {
                    Key vkKey = KeyInterop.KeyFromVirtualKey(vkCode);
                    KeyPressedArgs eventArgs = new KeyPressedArgs(vkKey);
                    OnKeyPressed(this, eventArgs);

                    if (eventArgs.HasChanged)
                    {
                        int vkNewCode = KeyInterop.VirtualKeyFromKey(eventArgs.Key);
                        if (m_RemappedKeys.ContainsKey(vkCode) == false)
                            m_RemappedKeys.Add(vkCode, vkNewCode);

                        m_Stopwatch.Restart();

                        keybd_event((byte) vkNewCode, 0, 0 | 0, 0);

                        return (IntPtr) 1;
                    }
                }
                else if (keyMsg == WM_KEYUP)
                {
                    int vkNewCode = 0;
                    if (m_RemappedKeys.TryGetValue(vkCode, out vkNewCode))
                    {
                        m_RemappedKeys.Remove(vkCode);

                        m_Stopwatch.Stop();
                        Console.Out.WriteLine($"Key Press Duration: {m_Stopwatch.ElapsedMilliseconds} ms");

                        keybd_event((byte) vkNewCode, 0, 0 | KEYEVENTF_KEYUP, 0);
                        return (IntPtr) 1;
                    }
                }
            }
            return CallNextHookEx(m_Hook, nCode, wParam, lParam);
        }
    }
       
    public class KeyPressedArgs : EventArgs
    {
        private Key m_OldKey;
        private Key m_NewKey;
        
        public Key Key
        {
            get { return m_NewKey;  }
            set { m_NewKey = value; }
        }

        public bool HasChanged
        {
            get { return (m_NewKey != m_OldKey); }
        }

        public KeyPressedArgs(Key key)
        {
            m_OldKey = key;
            m_NewKey = key;
        }
    }
    
    public class GameKey
    {        
        public Key Key { get; set; }

        public GameKey(Key key)
        {
            Key = key;
        }

        public bool WasPressed(Key pressed)
        {
            return (pressed == Key);
        }
        

    }


}
