using System;
using System.Text;
using System.IO;
using Microsoft.Win32;
using System.Diagnostics;
using System.Collections.Generic;
using CommonLib;

namespace Compiler
{
    public class Binding
    {
        public string Name          { get; set; }
        public string DisplayName   { get; set; }
        public string MacroText     { get; set; }
        public string BindingText   { get; set; }

    }

    public class Addon
    {
        public List<LuaData> m_DataList;
        public List<Binding> m_Bindings;
        public Dictionary<Type, Type> m_Types;

        public Addon()
        {
            m_DataList = new List<LuaData>();
            m_Bindings = new List<Binding>();
            m_Types = new Dictionary<Type, Type>();

            // -----------------------------
            // 
            // Test Values
            //
            /*
            Binding binding1 = new Binding();
            binding1.BindingText = "SHIFT-F1";
            binding1.DisplayName = "Sit";
            binding1.Name = "Sit";
            binding1.MacroText = "/sit";
            m_Bindings.Add(binding1);

            Binding binding2 = new Binding();
            binding2.BindingText = "SHIFT-F2";
            binding2.DisplayName = "Cast Moonfire";
            binding2.Name = "Moonfire";
            binding2.MacroText = "/cast Moonfire";
            m_Bindings.Add(binding2);
            */
            // 
            // -----------------------------
        }

        public void RegisterLuaType<T,L>()
        {
            m_Types.Add(typeof(T), typeof(L));
        }

        public bool Generate(bool beta)
        {
            string wowFolder, addonFolder;
            if (!Utils.GetWOWFolder(beta, out wowFolder, out addonFolder))
                return false;

            if (!Directory.Exists(addonFolder))
                Directory.CreateDirectory(addonFolder);

            if (!GenerateTOC(addonFolder))
                return false;

            if (!BuildDataList())
                return false;

            if (!GenerateLUA(addonFolder))
                return false;

            if (Config.GenerateBindings && m_Bindings.Count > 0)
            {
                if (!GenerateBindings(addonFolder))
                    return false;
            }

            return true;
        }

        public static string GetFilePath(string filename)
        {
            StackFrame stackFrame = new StackFrame(true);
            string path = Path.GetDirectoryName(stackFrame.GetFileName());

            if (path.EndsWith("\\Common"))
            {
                path = path.Remove(path.Length - 7);
            }

            if (filename.Length > 0)
            {
                path += "\\Lua\\" + filename;
            }

            return path;
        }

        private bool GenerateTOC(string addonFolder)
        {
            StreamWriter writer = new StreamWriter($"{addonFolder}\\{Constants.AddonName}.toc");
            if (writer == null)
                return false;

            writer.WriteLine($"## Interface: {Constants.InterfaceVersion}");
            writer.WriteLine($"## Title: {Constants.AddonName}");
            writer.WriteLine($"## Version: {Constants.Version}");
            writer.WriteLine($"## SavedVariablesPerCharacter: {Constants.AddonName}_settings");
            writer.WriteLine($"{Constants.AddonName}.lua");   
            
            if (m_Bindings.Count > 0)             
                writer.WriteLine($"Bindings.lua");                

            writer.Close();

            return true;
        }

        private bool GenerateLUA(string addonFolder)
        {
            StreamWriter writer = new StreamWriter($"{addonFolder}\\{Constants.AddonName}.lua");
            if (writer == null)
                return false;

            if (!WriteHeader(writer))
            {
                writer.Flush();
                return false;
            }

            if (!WriteFrames(writer))
            {
                writer.Flush();
                return false;
            }

            if (!WriteFooter(writer))
            {
                writer.Flush();
                return false;
            }

            writer.Flush();
            writer.Close();

            return true;
        }

        private bool WriteHeader(StreamWriter writer)
        {
            string filename = GetFilePath("Common\\header.lua");

            StreamReader reader = new StreamReader(filename);
            if (reader == null)
                return false;

            string text = reader.ReadToEnd();
            text = text.Replace("{size}",  Config.FrameSize.ToString());
            text = text.Replace("{count}", App.Class.DataCount.ToString());
            text = text.Replace("{xPos}",  0.ToString());
            text = text.Replace("{yPos}",  0.ToString());
            text = text.Replace("{addon}", Constants.AddonName);
            text = text.Trim();

            writer.Write(text);
            writer.WriteLine(@"");

            return true;
        }

        private bool WriteGlobals(StreamWriter writer, string filename)
        {
            StreamReader reader = new StreamReader(filename);
            if (reader == null)
                return false;

            writer.WriteLine(@"");

            string text = reader.ReadToEnd();
            text = text.Trim();

            writer.Write(text);
            writer.WriteLine(@"");

            return true;
        }

        private bool WriteFooter(StreamWriter writer)
        {        
            string filename = GetFilePath("Common\\footer.lua");

            StreamReader reader = new StreamReader(filename);
            if (reader == null)
                return false;

            writer.WriteLine(@"");

            string text = reader.ReadToEnd();
            text = text.Replace("{class}", App.Class.ClassID.ToString());
            text = text.Replace("{addon}", Constants.AddonName);
            text = text.Trim();

            writer.Write(text);
            writer.WriteLine(@"");
            
            return true;
        }

        private bool BuildDataList()
        {
            foreach (Data data in App.Class.DataList)
            {
                LuaData luaData = CreateLuaData(data);
                m_DataList.Add(luaData);
            }

            return true;
        }

        private LuaData CreateLuaData(Data data)
        {
            Type searchType = data.GetType();
            Type foundType = null;

            if (m_Types.TryGetValue(searchType, out foundType))
            {
                return (LuaData) Activator.CreateInstance(foundType, data);
            }

            return null;
        }
        
        private bool WriteFrames(StreamWriter writer)
        {
            writer.WriteLine(@"");

            foreach (LuaData data in m_DataList)
            {
                if (!data.WriteLuaDefinition(writer))
                    return false;
            }

            string globals = GetFilePath("Common\\globals.lua");
            if (!WriteGlobals(writer, globals))
                return false;

            string conversion = GetFilePath("Common\\conversion.lua");
            if (!WriteGlobals(writer, conversion))
                return false;

            foreach (LuaData data in m_DataList)
            {
                if (!data.WriteLuaFunction(writer))
                    return false;
            }

            int thirdFrames    = m_DataList.Count / 3;
            int twoThirdFrames = thirdFrames * 2;
            int frameCounter = 0;

            writer.WriteLine(@"");
            writer.WriteLine(@"local function initFrames1()");

            foreach (LuaData data in m_DataList)
            {
                if (!data.WriteLuaDeclaration(writer))
                    return false;

                frameCounter++;

                if (frameCounter >= thirdFrames)
                {
                    thirdFrames = 100000;

                    writer.WriteLine(@"");
                    writer.WriteLine(@"end");

                    writer.WriteLine(@"");
                    writer.WriteLine(@"local function initFrames2()");
                }
                if (frameCounter >= twoThirdFrames)
                {
                    twoThirdFrames = 100000;

                    writer.WriteLine(@"");
                    writer.WriteLine(@"end");

                    writer.WriteLine(@"");
                    writer.WriteLine(@"local function initFrames3()");
                }
            }

            writer.WriteLine(@"");
            writer.WriteLine(@"end");
            return true;
        }

        private bool GenerateBindings(string addonFolder)
        {
            StreamWriter luaWriter = new StreamWriter($"{addonFolder}\\Bindings.lua");
            if (luaWriter == null)
                return false;

            StreamWriter xmlWriter = new StreamWriter($"{addonFolder}\\Bindings.xml");
            if (luaWriter == null)
                return false;

            bool result = WriteBindings(luaWriter, xmlWriter);

            luaWriter.Flush();
            luaWriter.Close();

            xmlWriter.Flush();
            xmlWriter.Close();

            return result;
        }

        private bool WriteBindings(StreamWriter luaWriter, StreamWriter xmlWriter)
        {
            string bindingframeFilename = GetFilePath("Common\\BindingFrame.lua");
            StreamReader bindingFrameReader = new StreamReader(bindingframeFilename);
            if (bindingFrameReader == null)
                return false;

            string bindingFrameText = bindingFrameReader.ReadToEnd();
            bindingFrameReader.Close();

            luaWriter.WriteLine($"BINDING_HEADER_{Constants.AddonName} = \"{Constants.DisplayName}\"");
            luaWriter.WriteLine(@"");

            foreach (Binding binding in m_Bindings)
            {
                luaWriter.WriteLine($"_G[\"BINDING_NAME_CLICK {binding.Name}:LeftButton\"] = \"{binding.DisplayName}\"");
            }

            luaWriter.WriteLine(@"");

            int frameSize = 24;
            int frameNum = 1;
            int x = 0 - ((m_Bindings.Count - 1) * (frameSize/2));
            foreach (Binding binding in m_Bindings)
            {                
                string text = bindingFrameText;

                text = text.Replace("{frame}", $"frame{frameNum}");
                text = text.Replace("{name}", binding.Name);
                text = text.Replace("{macro}", binding.MacroText);
                text = text.Replace("{x}", x.ToString());
                text = text.Replace("{size}", frameSize.ToString());
                text = text.Trim();

                luaWriter.WriteLine(text);
                luaWriter.WriteLine(@"");

                x += frameSize;
                frameNum++;
            }
            
            xmlWriter.WriteLine(@"<Bindings>");

            foreach (Binding binding in m_Bindings)
            {
                xmlWriter.WriteLine($"<Binding category=\"ADDONS\" name=\"CLICK {binding.Name}:LeftButton\" header=\"{Constants.AddonName}\" default=\"{binding.BindingText}\"/>");
            }

            xmlWriter.WriteLine(@"</Bindings>");
            return true;
        }



    }
}
