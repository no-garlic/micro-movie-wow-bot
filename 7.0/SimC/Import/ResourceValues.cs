using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using CommonLib;

namespace SimC
{
    public class ResourceValues
    {
        public class ResourceEntry
        {
            public DateTime m_DateTime;
            public int m_Energy;
            public int m_MaxEnergy;
            public int m_Combo;
            public int m_MaxCombo;
        }

        public class IndexTableEntry
        {
            public DateTime m_First;
            public DateTime m_Last;
            public string   m_FileName;
            public List<ResourceEntry> m_Data;
        }

        List<IndexTableEntry> m_IndexTable;

        public ResourceValues()
        {
            m_IndexTable = new List<IndexTableEntry>();
        }

        public void BuildIndexTable(string folder = null)
        {
            if (folder == null || folder == "")
                folder = Config.LoggingFolder;

            IEnumerable<string> filenames = System.IO.Directory.EnumerateFiles(folder);
            foreach (string filename in filenames)
            {
                string[] tokens = filename.Split('-');
                if (tokens.Length != 6)
                    continue;

                FileStream file = new FileStream(filename, FileMode.Open, FileAccess.Read);
                StreamReader reader = new StreamReader(file);

                string first = null, last = null, line = null;
                while ((line = reader.ReadLine()) != null)
                {
                    if (first == null)
                        first = line;
                    last = line;
                }

                if (first != null && last != null)
                {
                    DateTime dtFirst, dtLast;
                    if (GetDateTime(first, out dtFirst) && GetDateTime(last, out dtLast))
                    {
                        dtLast = dtLast.AddMilliseconds(500);

                        IndexTableEntry entry = new IndexTableEntry();
                        entry.m_First = dtFirst;
                        entry.m_Last  = dtLast;
                        entry.m_FileName = filename;
                        m_IndexTable.Add(entry);
                    }
                }

                reader.Close();
                file.Close();
            }
        }

        private bool GetDateTime(string text, out DateTime dateTime)
        {
            int pos = text.IndexOf(',');
            if (pos >= 0)
                text = text.Substring(0, pos);

            System.Globalization.CultureInfo provider = System.Globalization.CultureInfo.InvariantCulture;
            System.Globalization.DateTimeStyles style = System.Globalization.DateTimeStyles.None;
            if (DateTime.TryParseExact(text, "M/d HH:m:ss.fff", provider, style, out dateTime))
                return true;

            return false;
        }

        private IndexTableEntry GetIndexTableEntry(DateTime dateTime)
        {
            foreach (IndexTableEntry indexTableEntry in m_IndexTable)
            {
                if (indexTableEntry.m_First <= dateTime && indexTableEntry.m_Last >= dateTime)
                {
                    return indexTableEntry;
                }
            }
            return null;
        }

        private ResourceEntry GetResourceEntry(DateTime dateTime)
        {
            IndexTableEntry indexTableEntry = GetIndexTableEntry(dateTime);
            if (indexTableEntry == null)
                return null;

            if (indexTableEntry.m_Data == null)
                LoadResourceData(indexTableEntry);

            if (indexTableEntry.m_Data.Count == 0)
                return null;

            ResourceEntry prevEntry = indexTableEntry.m_Data[0];
            foreach (ResourceEntry entry in indexTableEntry.m_Data)
            {
                if (dateTime > prevEntry.m_DateTime && dateTime <= entry.m_DateTime)
                    return prevEntry;

                prevEntry = entry;
            }

            if (prevEntry != null)
            {
                DateTime next = prevEntry.m_DateTime.AddMilliseconds(500);

                if (dateTime > prevEntry.m_DateTime && dateTime <= next)
                    return prevEntry;
            }
            return null;
        }

        private void LoadResourceData(IndexTableEntry indexTableEntry)
        {
            indexTableEntry.m_Data = new List<ResourceEntry>();

            FileStream file = new FileStream(indexTableEntry.m_FileName, FileMode.Open, FileAccess.Read);
            StreamReader reader = new StreamReader(file);

            string line = null;
            while ((line = reader.ReadLine()) != null)
            {
                string[] tokens = line.Split(',');
                if (tokens.Length != 38)
                    continue;

                DateTime datetime;
                GetDateTime(tokens[0], out datetime);

                ResourceEntry entry = new ResourceEntry();
                entry.m_DateTime  = datetime;
                entry.m_Combo     = System.Convert.ToInt32(tokens[19]);
                entry.m_MaxCombo  = System.Convert.ToInt32(tokens[20]);
                entry.m_Energy    = System.Convert.ToInt32(tokens[21]);
                entry.m_MaxEnergy = System.Convert.ToInt32(tokens[22]);

                indexTableEntry.m_Data.Add(entry);
            }
            
            reader.Close();
            file.Close();
        }

        public void Update(Importer importer)
        {
            foreach (Data.Record record in importer.Records)
            {
                ResourceEntry resourceEntry = GetResourceEntry(record.DateTime);
                if (resourceEntry != null)
                {
                    record.Combo     = resourceEntry.m_Combo;
                    record.MaxCombo  = resourceEntry.m_MaxCombo;
                    record.Energy    = resourceEntry.m_Energy;
                    record.MaxEnergy = resourceEntry.m_MaxEnergy;
                }
            }
        }


    }
}
