using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

namespace CommonLib
{
    public static partial class App
    {
        public enum Mode { CombatMovie, LittleBattles, OceanMaster};

        public static Mode          CurrentMode     = Mode.CombatMovie;
        public static Capture       Capture         { get; private set; }
        public static KeyboardHook  KeyboardHook    { get; private set; }
        public static bool          PauseOverride   { get; private set; }
        public static ClassBase     Class           { get; private set; }
        public static Log           Log             { get; private set; }

        private static Dictionary<int, Type> m_ClassTypes = new Dictionary<int, Type>();

        public static void RegisterClass<T>(int classID)
        {
            m_ClassTypes[classID] = typeof(T);
        }

        public static bool IsClassRegistered(int classID)
        {
            return m_ClassTypes.ContainsKey(classID);
        }

        public static ICollection<int> GetRegisteredClassIDs()
        {
            ICollection<int> keys = m_ClassTypes.Keys;
            return keys;
        }

        public static ClassBase Create(int classID)
        {
            Type objectType;
            if (m_ClassTypes.TryGetValue(classID, out objectType))
            {
                ClassBase obj = (ClassBase) Activator.CreateInstance(objectType);
                Create(obj);
                return obj;
            }
            return null;
        }

        public static ClassBase Create<T>() where T : ClassBase
        {
            T obj = Activator.CreateInstance<T>();
            Create(obj);
            return obj;
        }

        private static void Create(ClassBase classBase)
        {
            Capture = new Capture();

            Class = classBase;
            classBase.Create();
            classBase.Init();
            
            Log = new Log();
            Log.Enabled = Config.LoggingEnabled;

            KeyboardHook = new KeyboardHook();
            KeyboardHook.HookKeyboard();
            KeyboardHook.OnKeyPressed += App.OnKeyPress;

            PauseOverride = false;
        }
        
        public static bool Update()
        {
            if (Capture.UpdatePixels())
            {
                Capture.UpdateVariables();
                Class.Update();
                return true;
            }
            
            return false;
        }

        public static void TogglePauseOverride()
        {
            PauseOverride = !PauseOverride;
        }
        
        public static void OnKeyPress(object sender, KeyPressedArgs e)
        {
            if (CurrentMode == Mode.CombatMovie)
            {
                Class.OnKeyPress(e);
            }
        }

        public static void Close()
        {
            Capture = null;

            if (Log != null)
            {
                Log.Stop();
                Log = null;
            }

            if (KeyboardHook != null)
            {
                KeyboardHook.OnKeyPressed -= App.OnKeyPress;
                KeyboardHook.UnHookKeyboard();
                KeyboardHook = null;
            }
        }

        public static void Exit()
        {
            Application.Exit();
        }


    }
}
