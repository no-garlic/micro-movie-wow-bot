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
    public class CombatLog : Importer
    {
        public CombatLog() : base()
        {
            m_AbilityMap["\"Shred\""] = Data.Ability.Shred;
            m_AbilityMap["\"Rake\""] = Data.Ability.Rake;
            m_AbilityMap["\"Rip\""] = Data.Ability.Rip;
            m_AbilityMap["\"Ashamane's Frenzy\""] = Data.Ability.AshamanesFrenzy;
            m_AbilityMap["\"Ferocious Bite\""] = Data.Ability.FerociousBite;
            m_AbilityMap["\"Healing Touch\""] = Data.Ability.HealingTouch;
            m_AbilityMap["\"Savage Roar\""] = Data.Ability.SavageRoar;
            m_AbilityMap["\"Moonfire\""] = Data.Ability.Moonfire;
            m_AbilityMap["\"Tiger's Fury\""] = Data.Ability.TigersFury;
            m_AbilityMap["\"Berserk\""] = Data.Ability.Berserk;
            m_AbilityMap["\"Swipe\""] = Data.Ability.Swipe;
            m_AbilityMap["\"Thrash\""] = Data.Ability.Thrash;
            m_AbilityMap["\"Shadowmeld\""] = Data.Ability.Shadowmeld;

            m_BuffMap["\"Savage Roar\""] = Data.Buff.SavageRoar;
            m_BuffMap["\"Bloodtalons\""] = Data.Buff.Bloodtalons;
            m_BuffMap["\"Predatory Swiftness\""] = Data.Buff.PredatorySwiftness;
            m_BuffMap["\"Tiger's Fury\""] = Data.Buff.TigersFury;
            m_BuffMap["\"Berserk\""] = Data.Buff.Berserk;
            m_BuffMap["\"Clearcasting\""] = Data.Buff.ClearCasting;
        }
        
        public override bool DoImport()
        {
            Data.Buff buffs = Data.Buff.None;

            DateTime prevDateTime = new DateTime(0);
            DateTime initDateTime = new DateTime(0);
            int pos, pos2;
            string line;
            while ((line = m_InputStream.ReadLine()) != null)
            {
                if (line.Length == 0)
                    continue;

                pos = line.IndexOf(' ');
                string date = line.Substring(0, pos);

                pos++;
                pos2 = line.IndexOf(' ', pos);
                string time = line.Substring(pos, pos2 - pos);

                line = line.Remove(0, pos2).Trim();

                string[] tokens = line.Split(',');

                if (tokens.Length <= 10)
                    continue;

                string eventName  = tokens[0];
                string playerName = tokens[2];
                string spellName  = tokens[10];

                if (playerName != "\"Eldryn-Barthilas\"")
                    continue;

                if (eventName == "SPELL_CAST_SUCCESS")
                {
                    Data.Ability cast = ConvertToCast(spellName);
                    if (cast != Data.Ability.None)
                    {
                        DateTime dateTime = ConvertToDateTime(time);
                        Data.Record record = new Data.Record();

                        if (m_Data.Records.Count == 0)
                        {
                            initDateTime = dateTime;
                            prevDateTime = dateTime;
                        }

                        TimeSpan ts = dateTime.Subtract(initDateTime);
                        record.Elapsed = (float) (0.001 * ts.TotalMilliseconds);
                        ts = dateTime.Subtract(prevDateTime);
                        record.Delta = (float) (0.001 * ts.TotalMilliseconds);
                        record.DateTime = dateTime;
                        prevDateTime = dateTime;

                        record.Cast  = cast;
                        record.Buffs = buffs;
                        m_Data.Records.Add(record);
                    }
                    continue;
                }
                else if (eventName == "SPELL_AURA_APPLIED" || eventName == "SPELL_AURA_REFRESH")
                {
                    Data.Buff buff = ConvertToBuff(spellName);
                    if (buff != Data.Buff.None)
                        buffs |= buff;
                    continue;
                }
                else if (eventName == "SPELL_AURA_REMOVED")
                {
                    Data.Buff buff = ConvertToBuff(spellName);
                    if (buff != Data.Buff.None)
                        buffs &= ~buff;
                    continue;
                }
                else
                {
                    continue;
                }
            }

            if (m_Data.Records.Count > 0)
                GetResourceValues();

            return true;
        }

        private void GetResourceValues()
        {
            string path = Path.GetDirectoryName(m_Filename);

            ResourceValues resourceValues = new ResourceValues();
            resourceValues.BuildIndexTable(path);
            resourceValues.Update(this);
        }

    }
}
