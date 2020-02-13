using System;
using System.IO;
using Microsoft.Win32;
using System.Windows.Input;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace SimC
{
    public class SimCraft : Importer
    {
        public SimCraft() : base()
        {
            m_AbilityMap["shred"] = Data.Ability.Shred;
            m_AbilityMap["rake"] = Data.Ability.Rake;
            m_AbilityMap["rip"] = Data.Ability.Rip;
            m_AbilityMap["ashamanes_frenzy"] = Data.Ability.AshamanesFrenzy;
            m_AbilityMap["ferocious_bite"] = Data.Ability.FerociousBite;
            m_AbilityMap["healing_touch"] = Data.Ability.HealingTouch;
            m_AbilityMap["savage_roar"] = Data.Ability.SavageRoar;
            m_AbilityMap["moonfire"] = Data.Ability.Moonfire;
            m_AbilityMap["lunar_inspiration"] = Data.Ability.Moonfire;
            m_AbilityMap["tigers_fury"] = Data.Ability.TigersFury;
            m_AbilityMap["berserk"] = Data.Ability.Berserk;
            m_AbilityMap["swipe"] = Data.Ability.Swipe;
            m_AbilityMap["thrash"] = Data.Ability.Thrash;
            m_AbilityMap["shadowmeld"] = Data.Ability.Shadowmeld;

            m_BuffMap["savage_roar"] = Data.Buff.SavageRoar;
            m_BuffMap["bloodtalons"] = Data.Buff.Bloodtalons;
            m_BuffMap["bloodtalons(2)"] = Data.Buff.Bloodtalons;
            m_BuffMap["predatory_swiftness"] = Data.Buff.PredatorySwiftness;
            m_BuffMap["tigers_fury"] = Data.Buff.TigersFury;
            m_BuffMap["berserk"] = Data.Buff.Berserk;
            m_BuffMap["clearcasting"] = Data.Buff.ClearCasting;
        }
        
        public override bool DoImport()
        {
            bool   timestampFound = false;
            bool   sectionFound   = false;
            bool   recordFound    = false;
            bool   recordSkip     = true;
            int    valueIndex     = 0;

            string timestampStart = "<li><b>Timestamp:</b>";
            string sectionStart   = "<h3 class=\"toggle\">Sample Sequence Table</h3>";
            string sectionEnd     = "</table>";
            string recordStart    = "<tr>";
            string recordEnd      = "</tr>";

            Data.Record record = null;
            float prevElapsed = 0.0f;
            DateTime timestamp = new DateTime(0);

            string line;
            while ((line = m_InputStream.ReadLine()) != null)
            {
                if (line.Length == 0)
                    continue;

                if (!timestampFound)
                {
                    if (line.StartsWith(timestampStart))
                    {
                        timestamp = ParseTimestamp(line);
                        timestampFound = true;
                    }
                }

                if (line == sectionStart)
                {                   
                    sectionFound = true;
                    continue;
                }

                if (sectionFound && line == sectionEnd)
                {                   
                    return true;
                }

                if (!sectionFound)
                {
                    recordFound = false;
                    valueIndex = 0;
                    continue;
                }

                if (line == recordStart)
                {
                    recordFound = true;
                    valueIndex = 0;
                    if (recordSkip)
                    {
                        recordSkip = false;
                    }
                    else
                    {
                        record = new Data.Record();
                    }
                    continue;
                }

                if (line == recordEnd)
                {
                    recordFound = false;
                    valueIndex = 0;

                    if (record != null)
                    {
                        m_Data.Records.Add(record);
                        record = null;
                    }
                    
                    continue;
                }

                if (recordFound && record != null && recordSkip == false)
                {
                    valueIndex++;
                    string text = TrimHTML(line);

                    switch (valueIndex)
                    {
                        case 1:
                        {
                            DateTime dt = ConvertToDateTime(text);
                            if (dt.ToBinary() > 0)
                            {
                                record.Elapsed = (0.001f * dt.Millisecond) + (1.0f * dt.Second) + (60.0f * dt.Minute) + (3600.0f * dt.Hour);
                                record.Delta = record.Elapsed - prevElapsed;
                                prevElapsed = record.Elapsed;

                                TimeSpan ts = new TimeSpan(0, dt.Hour, dt.Minute, dt.Second, dt.Millisecond);
                                record.DateTime = timestamp.Add(ts);
                            }
                            else
                            {
                                record = null;
                            }
                            break;
                        }

                        case 2:
                        {
                            record.Cast = ConvertToCast(text);
                            if (record.Cast == Data.Ability.None)
                                record = null;
                            break;
                        }

                        case 4:
                        {
                            if (!ConvertToResource(text, ref record))
                                record = null;
                            break;
                        }

                        case 5:
                        {
                            record.Buffs = ConvertToBuff(text);
                            break;
                        }
                    }
                }
            }

            return false;
        }
        
        private string TrimHTML(string text)
        {
            string step1 = Regex.Replace(text, @"<[^>]+>|&nbsp;", "").Trim();
            string step2 = Regex.Replace(step1, @"\s{2,}", " ");
            return step2;
        }

        private DateTime ParseTimestamp(string text)
        {
            text = text.Substring(22);
            int pos = text.IndexOf(' ');
            if (pos > 0)
                text = text.Substring(pos + 1);

            pos = text.LastIndexOf(' ');
            if (pos > 0)
                text = text.Substring(0, pos);

            DateTime dt = base.ConvertToDateTime(text);
            return dt;
        }

        protected override DateTime ConvertToDateTime(string text)
        {
            if (text == "Pre")
                return new DateTime(0);

            return base.ConvertToDateTime(text);
        }        
        
        protected override Data.Ability ConvertToCast(string text)
        {
            if (text == "Waiting" || text == "auto_attack")
                return Data.Ability.None;

            return base.ConvertToCast(text);
        }

        protected override Data.Buff ConvertToBuff(string text)
        {
            text = text.Replace(',', ';');

            int i = text.IndexOf("; ");
            while (i >= 0)
            {
                text = text.Remove(i + 1, 1);
                i = text.IndexOf("; ");
            }

            Data.Buff allBuffs = Data.Buff.None;

            string[] buffNames = text.Split(';');
            foreach (string buffName in buffNames)
            {
                Data.Buff buff = base.ConvertToBuff(buffName);
                if (buff != Data.Buff.None)
                    allBuffs |= buff;
            }

            return allBuffs;
        }

        private bool ConvertToResource(string text, ref Data.Record record)
        {
            bool ok = true;

            double energy, energyMax;
            double combo,  comboMax;

            string[] s = text.Split('|');
            if (s.Length != 3) return false;

            string[] v = s[0].Split(':');
            if (v.Length != 2) return false;
            string[] e = v[0].Split('/');
            if (e.Length != 2) return false;
            ok |= Double.TryParse(e[0], out energy);
            ok |= Double.TryParse(e[1], out energyMax);

            v = s[2].Split(':');
            if (v.Length != 2) return false;
            e = v[0].Split('/');
            if (e.Length != 2) return false;
            ok |= Double.TryParse(e[0], out combo);
            ok |= Double.TryParse(e[1], out comboMax);

            if (ok)
            {
                record.Energy = (int) energy;
                record.MaxEnergy = (int) energyMax;
                record.Combo = (int) combo;
                record.MaxCombo = (int) comboMax;
            }

            return ok;
        }



    }
}
