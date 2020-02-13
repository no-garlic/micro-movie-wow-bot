using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Drawing;
using System.Numerics;
using CommonLib;

namespace OceanMaster
{
    public class MouseGesture
    {
        public enum Gesture { Movement, Emote, Rubbish };

        public static string    OUTPUT_FOLDER   = "d:\\Logs";
        public static byte      RECORD_ID       = 2;
        public static int       MAX_BUFFER_SIZE = 2 + (256 * 3);

        public int Count { get { return m_Samples.Count; } }
        public uint Duration { get { return GetDuration(); } }
        public float Length { get { return GetVector().Length(); } }
        public float Width { get { return GetWidth(); } }
        public Gesture Classification { get { return GetClassification(); }  }

        private MouseInfo m_Start;
        private List<MouseInfo> m_Samples;

        public MouseGesture()
        {
            m_Start = null;
            m_Samples = new List<MouseInfo>();
        }

        public void Add(MouseArgs args)
        {
            MouseInfo info = new MouseInfo();

            if (m_Start == null)
            {
                m_Start = new MouseInfo();
                m_Start.Position = new Point(args.PositionX, args.PositionY);
                m_Start.Milliseconds = args.Milliseconds;
            }

            info.Position = new Point(args.PositionX - m_Start.Position.X, args.PositionY - m_Start.Position.Y);
            info.Milliseconds = args.Milliseconds - m_Start.Milliseconds;

            if (m_Samples.Count > 0)
            {
                if (m_Samples[m_Samples.Count - 1].Milliseconds != info.Milliseconds)
                {
                    m_Samples.Add(info);
                }
            }
            else
            {
                m_Samples.Add(info);
            }
        }
        
        public bool Save()
        {
            if (m_Samples.Count == 0)
                return false;

            StreamWriter writer = new StreamWriter($"{OUTPUT_FOLDER}\\a.txt");
            if (writer == null)
                return false;

            writer.WriteLine($"<Type={Classification}, Duration={Duration}, Samples={m_Samples.Count}, Length={Length}, Width={Width}>");

            foreach (MouseInfo info in m_Samples)
            {
                string message = $"{info.Milliseconds}, {info.Position.X}, {info.Position.Y}";
                writer.WriteLine(message);
            }

            int x = m_Samples[m_Samples.Count - 1].Position.X;
            int y = m_Samples[m_Samples.Count - 1].Position.Y;

            int totalDistance = (int) Math.Round(Math.Sqrt((x*x) + (y*y)), 0);
            int totalMilliseconds = (int) m_Samples[m_Samples.Count - 1].Milliseconds;

            System.Console.WriteLine($"Distance = {totalDistance}, Milliseconds = {totalMilliseconds}");

            writer.Close();
            return true;
        }

        public void Clear()
        {
            m_Samples.Clear();
            m_Start = null;
        }

        private Gesture GetClassification()
        {
            float duration = Duration;
            float ratio = Length / (float) Math.Max(Width, 0.01);

            if (m_Samples.Count <= 2 || m_Samples.Count > 100)
            {
                return Gesture.Rubbish;
            }
            else if (ratio > 3 && Length > 200.0 && Length < 800 && duration < 1500)
            {
                return Gesture.Movement;
            }
            else if (ratio < 2 && Length > 50.0 && Length < 400 && duration < 500)
            {
                return Gesture.Emote;
            }

            return Gesture.Rubbish;
        }
        
        public bool Send(SerialPort serialPort)
        {
            // byte         : id
            // byte         : count
            // array of:
            //   byte       : delay ms
            //   byte       : delta x (biased)
            //   byte       : delta y (biased)

            // so, for say 50 mouse points, bits = 8 + 8 + (50 * 8 * 3) = 1216 bits
            // at 9600 buad, tx time = 127 milliseconds

            const byte OFFSET = 127;

            if (m_Samples.Count == 0)
                return false;

            int byteCount = 0;
            byte[] buffer = new byte[MAX_BUFFER_SIZE];

            buffer[0]           = RECORD_ID;
            buffer[++byteCount] = (byte) m_Samples.Count;

            MouseInfo previousInfo = null;
            foreach (MouseInfo info in m_Samples)
            {
                if (previousInfo == null)
                {
                    buffer[++byteCount] = (byte) (info.Milliseconds);
                    buffer[++byteCount] = (byte) (OFFSET + (info.Position.X));
                    buffer[++byteCount] = (byte) (OFFSET + (info.Position.Y));
                }
                else
                {
                    buffer[++byteCount] = (byte) (info.Milliseconds - previousInfo.Milliseconds);
                    buffer[++byteCount] = (byte) (OFFSET + (info.Position.X - previousInfo.Position.X));
                    buffer[++byteCount] = (byte) (OFFSET + (info.Position.Y - previousInfo.Position.Y));
                }

                previousInfo = info;
            }

            try
            {
                if (serialPort != null)
                {
                    serialPort.Write(buffer, 0, byteCount);
                }
            }
            catch(Exception)
            {
                return false;
            }

            return true;
        }

        public void Draw(PaintEventArgs e)
        {
            int numPoints = m_Samples.Count;

            if (numPoints < 2)
                return;

            Pen pen = new Pen(Color.Black, 1);
            Point[] points = new Point[numPoints];

            for (int i = 0; i < numPoints; ++i)
            {
                points[i] = m_Samples[i].Position;
                e.Graphics.DrawEllipse(pen, points[i].X, points[i].Y, 3, 3);
            }

            e.Graphics.DrawLines(pen, points);
        }

        private uint GetDuration()
        {
            if (m_Samples.Count >= 1)
                return m_Samples[m_Samples.Count - 1].Milliseconds;

            return 0;
        }


        public void Transform(Point startPosition, Point endPosition)
        {
            Vector2 u = new Vector2(endPosition.X - startPosition.X, endPosition.Y - startPosition.Y);
            Vector2 v = GetVector();

            double lu = u.Length();
            double lv = v.Length();

            double scale = lu / lv;

            u = Vector2.Normalize(u);
            v = Vector2.Normalize(v);

            double dotP = Vector2.Dot(v, u);
            double radians = 0.0 - Math.Acos(dotP);

            RotateAndScaleAndTransform(radians, scale, startPosition);          
        }

        public Vector2 GetVector()
        {
            Vector2 v = new Vector2();

            if (m_Samples.Count >= 2)
            {
                MouseInfo a = m_Samples[0];
                MouseInfo b = m_Samples[m_Samples.Count - 1];

                v.X = b.Position.X - a.Position.X;
                v.Y = b.Position.Y - a.Position.Y;
            }

            return v;
        }

        private float GetWidth()
        {
            double maxWidth = 0.0f;

            if (m_Samples.Count >= 2)
            {
                Point l1 = m_Samples[0].Position;
                Point l2 = m_Samples[m_Samples.Count - 1].Position;

                for (int i = 1; i < m_Samples.Count - 2; ++i)
                {
                    MouseInfo info = m_Samples[i];
                    Point point = info.Position;

                    double d = Math.Abs((l2.X - l1.X)*(l1.Y - point.Y) - (l1.X - point.X)*(l2.Y - l1.Y))/
                        Math.Sqrt(Math.Pow(l2.X - l1.X, 2) + Math.Pow(l2.Y - l1.Y, 2));

                    maxWidth = Math.Max(maxWidth, d);
                }
            }

            return (float) maxWidth;
        }

        public void RotateAndScaleAndTransform(double radians, double scaleFactor, Point offset)
        {
            double cos = Math.Cos(radians);
            double sin = Math.Sin(radians);

            foreach (MouseInfo info in m_Samples)
            {
                double dx = info.Position.X;
                double dy = info.Position.Y;

                double x = cos * dx - sin * dy;
                double y = sin * dx + cos * dy;

                double sx = x * scaleFactor;
                double sy = y * scaleFactor;

                double tx = sx + offset.X;
                double ty = sy + offset.Y;

                info.Position.X = (int) Math.Round(tx);
                info.Position.Y = (int) Math.Round(ty);
            }
        }


    }
}
