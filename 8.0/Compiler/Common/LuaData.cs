using System;
using System.IO;
using Microsoft.Win32;
using System.Diagnostics;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using CommonLib;

namespace Compiler
{
    public class LuaData
    {
        protected Data m_Data;

        public LuaData(Data data)
        {
            m_Data = data;
        }

        public virtual string GetLuaFileName()
        {
            string name = Constants.Names[m_Data.NameIndex];
            return name + ".lua";
        }

        protected virtual string GetLuaName()
        {
            string name = Constants.Names[m_Data.NameIndex];

            string luaName = "";

            if (name.Length > 0)
                luaName += Char.ToLower(name[0]);

            if (name.Length > 1)
                luaName += name.Substring(1);

            return luaName;
        }

        protected StreamReader ReadLuaFile(string filename)
        {
            string pathname = Addon.GetFilePath(filename);
            if (File.Exists(pathname))
            {
                StreamReader reader = new StreamReader(pathname);
                return reader;
            }
            return null;
        }

        public string GetSafeName(string name)
        {
            for (int i = 0; i < name.Length; ++i)
            {
                char c = name[i];

                if ((c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z') || (c >= '0' && c <= '9'))
                {
                    if (i == 0)
                    {
                        string s = new string(c, 1);
                        name = s + name.Substring(1);
                    }

                    continue;
                }

                name = name.Replace(c, '_');
            };

            return name;
        }
        
        public virtual string GetLuaAddonName()
        {
            return Constants.AddonName;
        }

        public virtual string GetLuaFrameName()
        {
            return GetSafeName(GetLuaName() + "Frame");
        }

        public virtual string GetLuaFunctionName()
        {
            return GetSafeName(GetLuaName() + "Update");
        }

        public virtual bool WriteLuaDeclaration(StreamWriter writer)
        {
            StreamReader reader = ReadLuaFile("Common\\Frame.lua");
            if (reader == null)
                return false;

            while (!reader.EndOfStream)
            {
                string text = reader.ReadLine();
                text = ResolveLuaTokens(text);
                if (text.Length > 0)
                    writer.WriteLine("    " + text);
            }
            
            return true;
        }

        public virtual bool WriteLuaFunction(StreamWriter writer)
        {
            string filename = "Frames\\" + GetLuaFileName();
            StreamReader reader = ReadLuaFile(filename);
            if (reader == null)
                return false;

            writer.WriteLine(@"");
            string text = reader.ReadToEnd();
            text = ResolveLuaTokens(text);
            writer.Write(text);
            writer.WriteLine(@"");
            return true;
        }

        private string GetAnchorString()
        {
            string text = $"\"{Config.Corner.ScriptToken}\"";
            return text;
        }

        private string GetOffsetString()
        {
            int i = m_Data.Index * Config.Corner.Scalar;

            if (Config.Corner.Flip)
            {
                i = ((App.Class.DataCount - 1) - m_Data.Index) * Config.Corner.Scalar;
            }

            string text = $"{i}";
            return text;
        }

        private string GetCornerString()
        {
            int i = m_Data.Index * Config.Corner.Scalar;

            if (Config.Corner.Flip)
            {
                i = ((App.Class.DataCount - 1) - m_Data.Index) * Config.Corner.Scalar;
            }

            string text = $"\"{Config.Corner.ScriptToken}\", {i} * size, 0";
            return text;
        }

        public virtual string ResolveLuaTokens(string text)
        {            
            string helperName = Utils.GetClassOrSpecNameFromID(App.Class.ClassID);
            helperName += "Helper";

            text = text.Replace("{name}", GetSafeName(GetLuaName()));
            text = text.Replace("{prev}", GetSafeName(GetLuaName() + "PrevColor"));
            text = text.Replace("{frame}", GetLuaFrameName());
            text = text.Replace("{point}", GetCornerString());
            text = text.Replace("{anchor}", GetAnchorString());
            text = text.Replace("{offset}", GetOffsetString());
            text = text.Replace("{index}", m_Data.Index.ToString());
            text = text.Replace("{function}", GetLuaFunctionName());
            text = text.Replace("{addon}", GetLuaAddonName());
            text = text.Replace("{helper}", helperName);

            return text.Trim();
        }
    }


    public class LuaDataBase<T> : LuaData where T : Data
    {
        public T Data { get { return m_Data as T; } }

        public LuaDataBase(Data data) : base(data)
        {
        }

    }


}
