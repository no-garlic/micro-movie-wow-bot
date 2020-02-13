using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Input;
using System.Collections.Generic;

namespace CommonLib
{
    public class MouseHook
    {
        private const int WH_MOUSE_LL = 14;

        private const int WM_LBUTTONDOWN = 0x0201;
        private const int WM_LBUTTONUP   = 0x0202;
        private const int WM_MOUSEMOVE   = 0x0200;
        private const int WM_MOUSEWHEEL  = 0x020A;
        private const int WM_RBUTTONDOWN = 0x0204;
        private const int WM_RBUTTONUP   = 0x0205;

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelMouseProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        [StructLayout(LayoutKind.Sequential)]
        private struct POINT
        {
            public int x;
            public int y;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct MSLLHOOKSTRUCT
        {
            public POINT pt;
            public uint mouseData;
            public uint flags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        public delegate IntPtr LowLevelMouseProc(int nCode, IntPtr wParam, IntPtr lParam);

        private LowLevelMouseProc m_Proc;
        private IntPtr m_Hook = IntPtr.Zero;

        public event EventHandler<MouseArgs> OnMouseEvent;

        private bool m_ButtonStateL;
        private bool m_ButtonStateR;

        public MouseHook()
        {
            m_Proc = HookCallback;
            m_ButtonStateL = false;
            m_ButtonStateR = false;
        }

        public void HookMouse()
        {
            m_Hook = SetHook(m_Proc);
        }

        public void UnHookKeyboard()
        {
            UnhookWindowsHookEx(m_Hook);
        }

        private IntPtr SetHook(LowLevelMouseProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_MOUSE_LL, proc, GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            int mouseMsg = (int) wParam;
            if (nCode >= 0 && (mouseMsg == WM_MOUSEMOVE || mouseMsg == WM_LBUTTONDOWN || 
                mouseMsg == WM_LBUTTONUP || mouseMsg == WM_RBUTTONDOWN || mouseMsg == WM_RBUTTONUP))
            {
                MSLLHOOKSTRUCT hookStruct = (MSLLHOOKSTRUCT) Marshal.PtrToStructure(lParam, typeof(MSLLHOOKSTRUCT));

                switch (mouseMsg)
                {
                    case WM_LBUTTONDOWN: m_ButtonStateL = true; break;
                    case WM_LBUTTONUP:   m_ButtonStateL = false; break;
                    case WM_RBUTTONDOWN: m_ButtonStateR = true; break;
                    case WM_RBUTTONUP:   m_ButtonStateR = false; break;
                }

                if (OnMouseEvent != null)
                {
                    MouseArgs eventArgs = new MouseArgs();
                    eventArgs.Message = mouseMsg;
                    eventArgs.Milliseconds = hookStruct.time;
                    eventArgs.PositionX = hookStruct.pt.x;
                    eventArgs.PositionY = hookStruct.pt.y;
                    eventArgs.ButtonStateL = m_ButtonStateL;
                    eventArgs.ButtonStateR = m_ButtonStateR;
                    OnMouseEvent(this, eventArgs);
                }
            }
            return CallNextHookEx(m_Hook, nCode, wParam, lParam);
        }
    }


    public class MouseArgs : EventArgs
    {
        public int  Message;
        public uint Milliseconds;
        public int  PositionX;
        public int  PositionY;
        public bool ButtonStateL;
        public bool ButtonStateR;

        public MouseArgs()
        {
        }
    }

}

