using System;
using System.IO;
using Microsoft.Win32;
using System.Windows.Input;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
using System.Globalization;

namespace SimC
{
    public abstract class Importer
    {
        protected Data         m_Data;
        protected StreamReader m_InputStream;
        protected FileStream   m_File;
        protected string       m_Filename;
        
        protected Dictionary<string, Data.Ability> m_AbilityMap;
        protected Dictionary<string, Data.Buff>    m_BuffMap;
        protected List<string>                     m_IgnoredAbilities;
        protected List<string>                     m_IgnoredBuffs;

        public Data Data => m_Data;
        public List<Data.Record> Records => m_Data.Records;

        public Importer()
        {
            m_Data = new Data();
            m_AbilityMap = new Dictionary<string, Data.Ability>();
            m_BuffMap = new Dictionary<string, Data.Buff>();
            m_IgnoredAbilities = new List<string>();
            m_IgnoredBuffs = new List<string>();            
        }

        public abstract bool DoImport();

        public bool Import(string filename)
        {
            m_Filename = filename;
            Open(filename);

            bool result = DoImport();

            Close(result);
            return result;
        }

        public void Export(string filename = null)
        {
            if (filename == null || filename == "")
                filename = Path.GetDirectoryName(m_Filename) + "\\" + Path.GetFileNameWithoutExtension(m_Filename) + ".csv";

            FileStream file = new FileStream(filename, FileMode.Create);
            StreamWriter writer = new StreamWriter(file);

            Print(writer);

            writer.Close();
            file.Close();
        }
        
        public void Print(TextWriter writer = null)
        {
            foreach (Data.Record record in m_Data.Records)
            {
                record.Print(writer);
            }
        }

        protected void Open(string filename)
        {
            m_File = new FileStream(filename, FileMode.Open, FileAccess.Read);
            m_InputStream = new StreamReader(m_File);
        }

        protected void Close(bool showMissingData)
        {
            m_InputStream.Close();
            m_File.Close();

            if (showMissingData)
            {
                if (m_IgnoredAbilities.Count > 0)
                {
                    Console.Out.WriteLine("Ignored Abilities:");
                    foreach (string text in m_IgnoredAbilities)
                        Console.Out.WriteLine($"    {text}");
                }

                if (m_IgnoredBuffs.Count > 0)
                {
                    Console.Out.WriteLine("Ignored Buffs:");
                    foreach (string text in m_IgnoredBuffs)
                        Console.Out.WriteLine($"    {text}");
                }
            }
        }

        protected virtual DateTime ConvertToDateTime(string text)
        {
            CultureInfo provider = CultureInfo.InvariantCulture;
            DateTimeStyles style = DateTimeStyles.None;

            DateTime t;
            bool ok =  DateTime.TryParseExact(text,          "m:ss.fff", provider, style, out t);
            ok = ok || DateTime.TryParseExact(text,       "HH:m:ss.fff", provider, style, out t);
            ok = ok || DateTime.TryParseExact(text,   "M/d HH:m:ss.fff", provider, style, out t);
            ok = ok || DateTime.TryParseExact(text, "MMM d HH:m:ss"    , provider, style, out t);

            return t;
        }

        protected virtual Data.Ability ConvertToCast(string text)
        {
            text = text.Trim();

            Data.Ability ability = Data.Ability.None;
            if (!m_AbilityMap.TryGetValue(text, out ability))
            {
                if (!m_IgnoredAbilities.Contains(text))
                    m_IgnoredAbilities.Add(text);
            }

            return ability;
        }

        protected virtual Data.Buff ConvertToBuff(string text)
        {
            text = text.Trim();

            Data.Buff buff = Data.Buff.None;
            if (!m_BuffMap.TryGetValue(text, out buff))
            {
                if (!m_IgnoredBuffs.Contains(text))
                    m_IgnoredBuffs.Add(text);
            }

            return buff;
        }






    }
}
