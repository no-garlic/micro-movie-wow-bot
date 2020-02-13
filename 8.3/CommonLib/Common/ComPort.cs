using System;
using System.Runtime.InteropServices;
using System.Windows.Input;
using System.IO.Ports;

namespace CommonLib
{
    public class ComPort
    {
        private SerialPort m_Port;
        public bool IsOpen { get { return m_Port != null && m_Port.IsOpen; } }

        private byte m_Handshake = 0;

        struct SerialData
        {
            public byte message;
            public byte key;
            public byte modifiers;
            public byte handshake;
            public int duration;
        }

        public ComPort()
        {
        }

        public bool Open(int portNumber)
        {
            if (m_Port != null)
            {
                return false;
            }

            string portName = $"COM{portNumber}";
            m_Port = new SerialPort(portName);

            try
            {
                m_Port.Open();
            }
            catch
            {
                m_Port.Dispose();
            }            

            if (m_Port.IsOpen)
            {
                return true;
            }

            Close();
            return false;
        }

        public void Close()
        {
            m_Port?.Close();
            m_Port?.Dispose();
        }

        byte [] StructureToByteArray(object obj)
        {
            int len = Marshal.SizeOf(obj);
            byte [] arr = new byte[len];
            IntPtr ptr = Marshal.AllocHGlobal(len);
            Marshal.StructureToPtr(obj, ptr, true);
            Marshal.Copy(ptr, arr, 0, len);
            Marshal.FreeHGlobal(ptr);
            return arr;
        }

        public bool PressKey(byte key, byte modifier, int milliseconds)
        {
            if (m_Port == null)
            {
                return false;
            }

            if (!m_Port.IsOpen)
            {
                return false;
            }

            SerialData serialData = new SerialData();
            serialData.message = Arduino.MSG_KEY;
            serialData.key = key;
            serialData.modifiers = modifier;
            serialData.handshake = ++m_Handshake;
            serialData.duration = milliseconds;

            byte[] buffer = StructureToByteArray(serialData);
            int length = buffer.GetLength(0);

            m_Port.Write(buffer, 0, 8);
            byte handshake = (byte) m_Port.ReadByte();

            if (handshake != m_Handshake)
                return false;

            return true;
        }


    }
}
