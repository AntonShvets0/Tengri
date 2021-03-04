using System.Collections.Generic;

namespace TengriLang.Language
{
    public class CompileOptions
    {
        public bool IsExecutable = false;

        public string ProjectFolder;
        
        public string PathToClass;
        public string MainClass;

        public List<string> Code = new List<string>();

        public string Name;
        public string Icon;
        public string CompiledOutputPath;
        
        public List<string> Dependencies = new List<string>();
    }
}