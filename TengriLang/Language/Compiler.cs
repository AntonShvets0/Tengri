using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.CSharp;

namespace TengriLang.Language
{
    public class Compiler
    {
        public CompilerErrorCollection CompilerErrors;
        public CompileOptions Options;
        
        public Compiler(CompileOptions options)
        {
            Options = options;
        }

        private List<string> _dll = new List<string>()
        {
            "System.dll",
            "Microsoft.CSharp.dll",
            "System.Core.dll",
            "System.Data.dll",
            "System.Numerics.dll",
            "System.Runtime.Serialization.dll",
            "mscorlib.dll"
        };

        public void Compile()
        {
            var csharpProvider = new CSharpCodeProvider();
            var icc = csharpProvider.CreateCompiler();
            
            var parameters = new CompilerParameters();

            parameters.OutputAssembly = (Options.CompiledOutputPath ?? Options.ProjectFolder) + Options.Name +
                                        (Options.IsExecutable ? ".exe" : ".dll");


            parameters.WarningLevel = 0;
            parameters.CompilerOptions = @"/optimize /win32icon:" + (Options.Icon != null ? Options.ProjectFolder + "/" + Options.Icon : AppDomain.CurrentDomain.BaseDirectory + "/assets/icon.ico");
            

            if (Options.IsExecutable)
            {
                var ns = "FILE_TENGRI_" + Options.PathToClass.Replace('\\', '_').Replace('/', '_').Replace(".tengri", "");

                parameters.GenerateExecutable = true;
                parameters.MainClass = $"{ns}.TENGRI_{Options.MainClass}";
            }
            
            var dll = new List<string>(_dll);
            
            dll.AddRange(Options.Dependencies);

            if (File.Exists(Options.CompiledOutputPath + "/TengriLang.dll"))
            {
                File.Delete(Options.CompiledOutputPath + "/TengriLang.dll");
            }
            
            File.Copy("TengriLang.dll", Options.CompiledOutputPath + "/TengriLang.dll");
            parameters.ReferencedAssemblies.Add("TengriLang.dll");
            
            foreach (var lib in dll)
            {
                if (!_dll.Contains(lib)) 
                {
                    if (File.Exists(Options.CompiledOutputPath + "/" + lib))
                    {
                        File.Delete(Options.CompiledOutputPath + "/" + lib);
                    }

                    File.Copy(Options.ProjectFolder + "/lib/" + lib, Options.CompiledOutputPath + "/" + lib);
                    parameters.ReferencedAssemblies.Add(Assembly.LoadFile(Options.ProjectFolder + "/lib/" + lib).Location);
                }
                else
                {
                    parameters.ReferencedAssemblies.Add(lib);
                }
            }

            var results = icc.CompileAssemblyFromSourceBatch(parameters, Options.Code.ToArray());
            CompilerErrors = results.Errors;
        }
    }
}