using System;
using System.Windows.Forms;
using System.Diagnostics;
using System.Windows.Input;
using System.Threading;

namespace CommonLib
{
    public class PetDirector
    {
        public byte KEY_AUTO     = Arduino.KEY_F9;
        public byte KEY_WALK     = Arduino.KEY_W;
        public byte KEY_TARGET   = Arduino.KEYPAD_0;
        public byte KEY_INTERACT = Arduino.KEYPAD_1;
        public byte KEY_HEAL     = Arduino.KEYPAD_2;
        public byte KEY_BANDAGE  = Arduino.KEYPAD_3;
        public byte KEY_TREAT    = Arduino.KEYPAD_4;
        public byte KEY_LTREAT   = Arduino.KEYPAD_5;
        public byte KEY_SAFARI   = Arduino.KEYPAD_6;

        public bool IsRunning      { get; set; }
        public bool IsStopping     { get; set; }

        public bool UseSafariHat   { get; set; }
        public bool UsePetTreat    { get; set; }
        public bool UseLesserTreat { get; set; }

        private Random  m_Random  = new Random();
        private ComPort m_ComPort = new ComPort();
        private Thread  m_Thread;

        public delegate void LoggingFunction(string message);
        private LoggingFunction m_LoggingFunction;

        public PetDirector(LoggingFunction loggingFunction)
        {
            m_LoggingFunction = loggingFunction;
            m_ComPort.Open(4);
            IsRunning = false;
            IsStopping = false;
            UseSafariHat = false;
            UsePetTreat = false;
            UseLesserTreat = false;
        }
        
        public bool Start(string scriptname)
        {
            if (m_Thread != null)
                return false;

            m_Thread = new Thread(ThreadStartFunc);
            m_Thread.IsBackground = true;
            m_Thread.Start(scriptname);
            
            return true;
        }

        public void Stop()
        {
            if (m_Thread == null)
                return;

            IsStopping = true;
        }

        public void Update()
        {
            if (m_Thread != null)
            {
                if (m_Thread.ThreadState == System.Threading.ThreadState.Stopped)
                {
                    m_Thread = null;
                }
            }
        }

        private void ThreadStartFunc(object parameter)
        {
            Thread.Sleep(2000);

            string scriptname = parameter as string;
            scriptname = scriptname.ToLower();

            if (scriptname == "single")
            {
                IsRunning = true;
                DoSingleBattle();
            }
            else if (scriptname == "auto")
            {
                IsRunning = true;
                DoMultipleBattles();
            }

            IsRunning = false;
            IsStopping = false;
        }

        private void RandomSleep(int milliseconds, int range = -1)
        {
            if (range < 0)
            {
                range = (int)(milliseconds * 0.25);
            }

            int millisecondsMin = m_Random.Next(milliseconds - range, milliseconds);
            int millisecondsMax = m_Random.Next(milliseconds, milliseconds + range);
            int sleepTime = m_Random.Next(millisecondsMin, millisecondsMax);
            Thread.Sleep(sleepTime);
        }

        private bool RandomKeyPress(byte key, int milliseconds, int range = -1)
        {
            return RandomKeyPress(key, Arduino.KEY_NONE, milliseconds, range);
        }

        private bool RandomKeyPress(byte key, byte modifier, int milliseconds, int range = -1)
        {
            if (range < 0)
            {
                range = (int)(milliseconds * 0.25);
            }

            int millisecondsMin = m_Random.Next(milliseconds - range, milliseconds);
            int millisecondsMax = m_Random.Next(milliseconds, milliseconds + range);
            int keyPressTime = m_Random.Next(millisecondsMin, millisecondsMax);

            bool result = m_ComPort.PressKey(key, modifier, keyPressTime);
            return result;
        }

        private void Log(string message)
        {
            if (m_LoggingFunction != null)
            {
                m_LoggingFunction(message);
            }
        }

        private PET PetInfo  => App.Class.PetInfo;
        private bool IsValid => App.Class.AreMagicNumbersValid();

        private void DoSingleBattle()
        {
            Log("Battle Started");
            
            int turnNumber = 1;

            while (IsValid && PetInfo.IsInBattle)
            {
                if (PetInfo.IsTurnReady)
                {
                    Log($"Turn {turnNumber}");
                    RandomSleep(400);
                    RandomKeyPress(KEY_AUTO, 100, 37);
                    turnNumber++;

                    for (int i = 0; i < 10; ++i)
                    {
                        if (PetInfo.IsTurnReady == false || PetInfo.IsInBattle == false || IsValid == false)
                            break;

                        RandomSleep(200);
                    }
                }

                RandomSleep(500);

                if (IsStopping)
                {
                    Log("Stopped");
                    return;
                }
            }

            Log("Battle Finished");
        }

        private void GetIntoBattle()
        {
            Log("Walk Forward");
            RandomKeyPress(KEY_WALK, 1267, 25);
            RandomSleep(800);

            if (IsStopping)
            {
                Log("Stopped");
                return;
            }

            Log("Heal Pets");
            HealPets();

            if (IsStopping)
            {
                Log("Stopped");
                return;
            }

            FeedPets();
            
            if (IsStopping)
            {
                Log("Stopped");
                return;
            }

            Log("Target Pet Trainer");
            RandomKeyPress(KEY_TARGET, 100, 37);
            RandomSleep(956, 192);

            if (IsStopping)
            {
                Log("Stopped");
                return;
            }

            Log("Interact with Pet Trainer");
            RandomKeyPress(KEY_INTERACT, 100, 37);
            RandomSleep(1334, 193);

            Log("Wait for Intro");
            for (int i = 0; i < 25; ++i)
            {
                if (PetInfo.IsTurnReady == true && PetInfo.IsInBattle == true && IsValid == true)
                    break;

                if (IsStopping)
                {
                    Log("Stopped");
                    return;
                }

                RandomSleep(500);
            }

            RandomSleep(500);
            if (PetInfo.IsTurnReady == true && PetInfo.IsInBattle == true && IsValid == true)
            {
                Log("Intro Sequence Finished");
            }
            else
            {
                Log("Failed to Start Battle");
                IsStopping = true;
            }
        }

        private void HealPets()
        {
            if (App.Class.PetInfo.ReviveCooldown < 15.0f)
            {
                int sleepTime = (int) App.Class.PetInfo.ReviveCooldown;
                sleepTime++;
                sleepTime *= 1000;
                sleepTime += 500;

                Log($"Waiting for Revive Cooldown ({App.Class.PetInfo.ReviveCooldown} seconds)");
                Thread.Sleep(sleepTime);
            }

            if (App.Class.PetInfo.ReviveCooldown == 0.0f)
            {
                RandomKeyPress(KEY_HEAL, 100, 37);
            }
            else
            {
                RandomKeyPress(KEY_BANDAGE, 100, 37);
            }

            RandomSleep(1417, 192);
        }

        private void FeedPets()
        {
            if (UseSafariHat && App.Class.PetInfo.IsSafariHat == false)
            {
                Log("Equipping Safari Hat");
                RandomKeyPress(KEY_SAFARI, 100, 37);
                RandomSleep(1417, 192);
            }

            if (UsePetTreat && App.Class.PetInfo.IsPetTreat == false)
            {
                Log("Using Pet Treat");
                RandomKeyPress(KEY_TREAT, 100, 37);
                RandomSleep(1417, 192);
            }

            if (UseLesserTreat && App.Class.PetInfo.IsLesserTreat == false)
            {
                Log("Using Lesser Pet Treat");
                RandomKeyPress(KEY_LTREAT, 100, 37);
                RandomSleep(1417, 192);
            }
        }

        private void DoMultipleBattles()
        {
            while (IsStopping == false && IsValid == true)
            {
                if (PetInfo.IsInBattle)
                {
                    DoSingleBattle();

                    if (IsStopping)
                    {
                        Log("Stopped");
                        return;
                    }

                    Log("Wait for Outro");
                    RandomSleep(2462, 192);
                }

                if (IsStopping)
                {
                    Log("Stopped");
                    return;
                }

                if (!PetInfo.IsInBattle)
                {
                    GetIntoBattle();

                    if (IsStopping)
                    {
                        Log("Stopped");
                        return;
                    }
                }
            }
        }





    }
}
