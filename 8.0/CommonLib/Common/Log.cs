using System;
using System.IO;
using Microsoft.Win32;
using System.Windows.Input;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CommonLib
{
    public class Log
    {
        private StreamWriter m_OutStream;
        private FileStream m_File;
        private bool m_LineDirty;

        public enum LogDetail { Low = 1, Med = 3, High = 7 };
        public LogDetail Detail { get; private set; }
        public enum LogMode { Header, Value };
        public LogMode Mode { get; set; }
        public bool Enabled { get; set; }

        public Log()
        {
            Enabled = false;
            Mode = LogMode.Value;
            Detail = LogDetail.High;
            m_LineDirty = false;
        }

        public void Start()
        {
            if (!Enabled || m_File != null || m_OutStream != null)
                return;

            string timeStamp = DateTime.Now.ToString("d-M-yy-HH-mm-ss");
            string filename = Config.LoggingFolder + "\\" + timeStamp + ".csv";
            
            m_File = new FileStream(filename, FileMode.Create);
            m_LineDirty = false;

            if (!m_File.CanWrite)
            {
                m_File = null;
                m_OutStream = null;
                Enabled = false;
            }            
            else
            {
                m_OutStream = new StreamWriter(m_File);
                Enabled = true;
            }
        }

        public void EndLine()
        {
            if (Enabled && m_LineDirty)
            {
                m_OutStream.WriteLine("");
                m_LineDirty = false;
            }
        }

        public void Write(LogDetail detail, String header, Key value)
        {
            Write(detail, header, value.ToString());
        }

        public void Write(LogDetail detail, String header, bool value)
        {
            Write(detail, header, value ? "TRUE" : "FALSE");
        }

        public void Write(LogDetail detail, String header, String value)
        {
            if (!Enabled || detail < Detail || m_OutStream == null)
                return;

            String text = (Mode == LogMode.Header) ? header : value;

            if (m_LineDirty)
            {
                m_OutStream.Write("," + text);
            }
            else
            {
                m_OutStream.Write(text);
                m_LineDirty = true;
            }
        }

        public void Stop()
        {
            if (m_OutStream != null)
            {
                m_OutStream.Close();
            }
            if (m_File != null)
            {
                m_File.Close();
                m_File = null;
            }
            m_OutStream = null;
        }

        
    }
}
