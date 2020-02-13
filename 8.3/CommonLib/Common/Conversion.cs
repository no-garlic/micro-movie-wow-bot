using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;

namespace CommonLib
{
    public enum ColorChannel { Red, Green, Blue };

    public abstract class Conversion
    {
        public static float Clamp(float value, float min, float max)
        {
            return Math.Min(max, Math.Max(min, value));
        }

        public static int Clamp(int value, int min, int max)
        {
            return Math.Min(max, Math.Max(min, value));
        }

        public static int GetChannelColor(Data data, ColorChannel channel)
        {
            Color color = data.GetColor();
            switch (channel)
            {
                case ColorChannel.Red:
                    return color.R;
                case ColorChannel.Green:
                    return color.G;
                case ColorChannel.Blue:
                    return color.B;
            }
            return 0;
        }

        public static float ToFloat(Data data, ColorChannel channel)
        {
            float value = GetChannelColor(data, channel);
            float fraction = value / 255.0f;
            return fraction;
        }

        public static int ToInt(Data data, ColorChannel channel)
        {
            int value = GetChannelColor(data, channel);
            return value;
        }

        public static int ToInt2(Data data, int bigunit, ColorChannel channel1, ColorChannel channel2)
        {
            int a256 = GetChannelColor(data, channel1);
            int b256 = GetChannelColor(data, channel2);

            int factor = (int) (256.0f / (bigunit + 1));
            int bias   = (256 - factor) - (bigunit * factor);

            int bigpart   = Math.Max(0, (a256 - bias) / factor);
            int smallpart = Math.Max(0, (b256 - bias) / factor);

            int value = (bigpart * bigunit) + smallpart;
            return value;
        }

        public static int ToInt2(Data data, ColorChannel channel1, ColorChannel channel2)
        {
            return ToInt2(data, 255, channel1, channel2);
        }

        public static bool ToBool(Data data, ColorChannel channel)
        {
            int value = GetChannelColor(data, channel);
            return value >= 127;
        }

        public static void ToBools(Data data, ColorChannel channel, out bool a, out bool b)
        {
            bool[] bools = ToBoolArray(data, channel, 2);
            a = bools[0];
            b = bools[1];
        }

        public static void ToBools(Data data, ColorChannel channel, out bool a, out bool b, out bool c, out bool d)
        {
            bool[] bools = ToBoolArray(data, channel, 4);
            a = bools[0];
            b = bools[1];
            c = bools[2];
            d = bools[3];
        }

        public static bool[] ToBoolArray(Data data, ColorChannel channel, int count)
        {
            if (count < 0 || count > 8)
                return null;

            bool[] values = new bool[count];

            int numDiscreteValues = (1 << count);
            
            int value = GetChannelColor(data, channel) * (numDiscreteValues - 1);
            float fvalue = ((float) value / 255.0f);
            value = (int) Math.Round(fvalue);

            for (int i = 0; i < count; ++i)
            {
                int mask = 1 << i;
                int bitset = (value & mask);

                values[i] = (bitset != 0);
            }
            
            return values;
        }

        public static int ToEnumID(Data data, int enumItemCount, ColorChannel channel)
        {
            int value = GetChannelColor(data, channel);

            int factor = (int) (256.0f / (enumItemCount - 1));
            int bias = 256 - (factor * (enumItemCount - 1));

            int index = (value - bias) / factor;
            return index;
        }
        
        public static float ToFloat2(Data data, ColorChannel channel1, ColorChannel channel2)
        {
            int a256 = GetChannelColor(data, channel1);
            int b256 = GetChannelColor(data, channel2);

            int wholepart = a256;
            float fractionpart = (b256 / 256.0f);

            float value = wholepart + fractionpart;            
            return value;
        }

        public static float ToFloat3(Data data, int bigunit)
        {
            Color color = data.GetColor();
            int a256 = color.R;
            int b256 = color.G;
            int c256 = color.B;

            int factor = (int) (256.0f / (bigunit + 1));
            int bias   = (256 - factor) - (bigunit * factor);
            
            int bigpart   = (a256 - bias) / factor;
            int smallpart = (b256 - bias) / factor;
            int wholepart = (bigpart * bigunit) + smallpart;

            float fractionpart = c256 / 256.0f;
            float value = wholepart + fractionpart;
            
            return value;
        }

        public static float ToTime(Data data)
        {
            return ToFloat3(data, 60);
        }

        public static float ToFloat3(Data data)
        {
            return ToFloat3(data, 255);
        }

    }
}
