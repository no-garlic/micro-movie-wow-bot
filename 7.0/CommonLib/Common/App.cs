using System;
using System.Windows.Forms;

namespace CommonLib
{
    public static class App
    {
        public static Capture       Capture         { get; private set; }
        public static KeyboardHook  KeyboardHook    { get; private set; }
        public static ClassBase     Class           { get; private set; }
        public static Log           Log             { get; private set; }

        public static void Create()
        {
            Capture         = new Capture();
            KeyboardHook    = new KeyboardHook();
            Class           = new Main();
            Log             = new Log();
        }

        public static void Init()
        {
            App.Class.Create();
            App.Class.Init();

            App.Log.Enabled = Config.LoggingEnabled;

            App.KeyboardHook.HookKeyboard();
            App.KeyboardHook.OnKeyPressed += App.OnKeyPress;
        }
        
        public static bool Update()
        {
            if (App.Capture.UpdatePixels())
            {
                App.Capture.UpdateVariables();
                App.Class.Update();
                return true;
            }
            
            return false;
        }
        
        public static void OnKeyPress(object sender, KeyPressedArgs e)
        {
            App.Class.OnKeyPress(e);
        }

        public static void Close()
        {
            App.Log.Stop();
            App.KeyboardHook.UnHookKeyboard();
            Application.Exit();
        }


    }
}
