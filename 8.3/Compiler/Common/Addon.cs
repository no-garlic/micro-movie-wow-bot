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
        public Dictionary<Type, Type> m_Types;
        public string m_ClassName;

        public static bool m_CopiedLibs = false;

        public Addon()
        {
            m_DataList = new List<LuaData>();
            m_Types = new Dictionary<Type, Type>();
            m_ClassName = Constants.AddonName;
        }

        public void RegisterLuaType<T,L>()
        {
            m_Types.Add(typeof(T), typeof(L));
        }

        public bool Generate(ClassBase classBase)
        {
            m_ClassName = Utils.GetClassOrSpecNameFromID(classBase.ClassID);

            if (m_ClassName.Length == 0)
                m_ClassName = classBase.GetType().Name;

            string wowFolder, addonFolder;
            if (!Utils.GetWOWFolder(out wowFolder, out addonFolder))
                return false;

            if (!Directory.Exists(addonFolder))
                Directory.CreateDirectory(addonFolder);

            if (!BuildDataList(classBase))
                return false;

            if (!GenerateLUA(addonFolder))
                return false;

            if (!CopyMainFile(addonFolder))
                return false;

            if (!GenerateTOC(addonFolder))
                return false;
            
            if (!CopyLibraries(addonFolder))
                return false;

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
            writer.WriteLine($"## Version: {Constants.Version}");
            writer.WriteLine($"## Title: {Constants.AddonName}");
            writer.WriteLine($"## SavedVariablesPerCharacter: {Constants.AddonName}_settings");
            writer.WriteLine("Libs\\LibStub\\LibStub.lua");
            writer.WriteLine("Libs\\AceAddon-3.0\\AceAddon-3.0.xml");
            writer.WriteLine("ClassHelper.lua");

            string[] files = System.IO.Directory.GetFiles(addonFolder, "*.lua");
            foreach (string filename in files)
            {
                string name = Path.GetFileName(filename);

                if (name.ToLower() != "classhelper.lua")
                    writer.WriteLine($"{name}");
            }
            
            writer.Close();
            return true;
        }
        
        private bool GenerateLUA(string addonFolder)
        {
            StreamWriter writer = new StreamWriter($"{addonFolder}\\{m_ClassName}.lua");
            if (writer == null)
                return false;

            if (!WriteHeader(writer))
            {
                writer.Flush();
                writer.Close();
                return false;
            }

            if (!WriteFrames(writer))
            {
                writer.Flush();
                writer.Close();
                return false;
            }

            if (!WriteFooter(writer))
            {
                writer.Flush();
                writer.Close();
                return false;
            }

            writer.Flush();
            writer.Close();

            return true;
        }

        private bool CopyMainFile(string addonFolder)
        {
            string sourceFile = GetFilePath("Common\\ClassHelper.lua");
            string destFile = $"{addonFolder}\\ClassHelper.lua";

             StreamWriter writer = new StreamWriter(destFile);
            if (writer == null)
                return false;

            StreamReader reader = new StreamReader(sourceFile);
            if (reader == null)
                return false;

            string modules = "";
            foreach (int classID in App.GetRegisteredClassIDs())
            {
                string className = Utils.GetClassOrSpecNameFromID(classID);

                modules += $"{className}Helper = ClassHelper:NewModule(\"{className}Helper\")\n";
                modules += $"local {className}Helper = {className}Helper\n\n";
            }
            modules = modules.Trim();

            string helpers = "";
            foreach (int classID in App.GetRegisteredClassIDs())
            {
                string className = Utils.GetClassOrSpecNameFromID(classID);
                string condition = (helpers.Length == 0) ? "    if" : "    elseif";

                helpers += $"{condition} (classIndex == {classID}) then\n";
                helpers += $"       ClassHelper:EnableModule(\"{className}Helper\")\n";
            }
            helpers += "    end\n";
            helpers = helpers.Trim();

            string text = reader.ReadToEnd();
            text = text.Replace("{helpers}", helpers);
            text = text.Replace("{modules}", modules);
            text = text.Trim();

            writer.WriteLine(@"");
            writer.Write(text);
            writer.WriteLine(@"");

            writer.Close();
            reader.Close();
            
            return true;
        }

        private bool WriteHeader(StreamWriter writer)
        {
            string filename = GetFilePath("Common\\header.lua");

            StreamReader reader = new StreamReader(filename);
            if (reader == null)
                return false;

            string helperName = Utils.GetClassOrSpecNameFromID(App.Class.ClassID);
            helperName += "Helper";

            string text = reader.ReadToEnd();
            text = text.Replace("{size}",  Config.FrameSize.ToString());
            text = text.Replace("{count}", App.Class.DataCount.ToString());
            text = text.Replace("{xPos}",  0.ToString());
            text = text.Replace("{yPos}",  0.ToString());
            text = text.Replace("{addon}", Constants.AddonName);
            text = text.Replace("{helper}", helperName);
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

            string helperName = Utils.GetClassOrSpecNameFromID(App.Class.ClassID);
            helperName += "Helper";

            writer.WriteLine(@"");

            string text = reader.ReadToEnd();
            text = text.Replace("{class}", App.Class.ClassID.ToString());
            text = text.Replace("{addon}", Constants.AddonName);
            text = text.Replace("{helper}", helperName);
            text = text.Trim();

            writer.Write(text);
            writer.WriteLine(@"");
            
            return true;
        }
        
        private bool BuildDataList(ClassBase classBase)
        {
            foreach (Data data in classBase.DataList)
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
                if (!data.WriteLuaFunction(writer))
                    return false;
            }

            writer.WriteLine(@"");
            writer.WriteLine(@"local function initFrames()");

            foreach (LuaData data in m_DataList)
            {
                if (!data.WriteLuaDeclaration(writer))
                    return false;
            }

            writer.WriteLine(@"end");
            return true;
        }

        private bool CopyLibraries(string addonFolder)
        {
            if (m_CopiedLibs)
                return true;

            string sourceLibs = GetFilePath("Libs");
            string addonLibs = $"{addonFolder}\\Libs";

            if (Directory.Exists(addonLibs))
                return true;

            Directory.CreateDirectory(addonLibs);
            Utils.CopyFiles(sourceLibs, addonLibs);
            m_CopiedLibs = true;

            return true;
        }


    }
}
