using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;
using TengriLang.Exceptions;
using TengriLang.Language;
using TengriLang.Language.Model;

namespace Tengri
{
    public class TengriConsole
    {
        private static TimeWatch _timeWatch = new TimeWatch();
        
        private static int _countLines = 0;
        private static bool _isRunStatistics;
        private static string _projectFolder;
        private static Dictionary<string, string> _hashes = new Dictionary<string, string>();
        private static CompileOptions _compileOptions;
        
        public const string VERSION = "2.0";

        static void Main(string[] args)
        {
            var config = CheckDataAndGetOptions(args);
            if (config == null) return;
            
            _compileOptions = BuildOptions(args, config);
            
            Console.WriteLine("Reading files...");
            _timeWatch = new TimeWatch();
            var files = Parse(args[0]);
            _timeWatch.Elapsed("reading files in project");
            
            Console.WriteLine("Hashing files...");

            var code = new List<string>();
            foreach (var hash in _hashes)
            {
                var newHash = Md5.GenerateHash(hash.Value + VERSION);

                if (File.Exists(".temp/" + newHash))
                {
                    code.Add(File.ReadAllText(".temp/" + newHash));
                    files.Remove(hash.Key);
                }
            }
            
            _timeWatch.Elapsed("hashing files");
            
            if (!Compile(_compileOptions, TranslateCode(BuildTree(files), code))) return;

            Console.WriteLine("Success! Compiled to " + (_compileOptions.CompiledOutputPath + "/" + _compileOptions.Name + (_compileOptions.IsExecutable ? ".exe" : ".dll")));
            if (_isRunStatistics)
            {
                _timeWatch.PrintStatistics();
                Console.WriteLine("Count lines: " + _countLines);
            }
        }

        private static JObject CheckDataAndGetOptions(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine($"Tengri Language. Powered by lordoverlord0\nVersion: {VERSION}\nUse: tengri [path to project] [enable or disable statistics (true/false)]");
                return null;
            }

            _projectFolder = args[0];
            _isRunStatistics = false;
            if (args.Length > 1)
            {
                if (args[1] != "true" && args[1] != "false")
                {
                    Console.WriteLine("Error: Wrong syntax. Statistics field must be 'true' of 'false'");
                    return null;
                }
                
                _isRunStatistics = args[1] == "true";
            }

            if (!Directory.Exists(args[0]))
            {
                Console.WriteLine("Error: Folder not exists");
                return null;
            }

            if (!File.Exists(args[0] + "/app.config"))
            {
                Console.WriteLine("Error: app.config not exists");
                return null;
            }

            var config = JObject.Parse(File.ReadAllText(args[0] + "/app.config"));
            if (config["name"] == null)
            {
                Console.WriteLine("Error: property \"name\" not found in app.config");
                return null;
            }
            
            return config;
        }
        
        private static CompileOptions BuildOptions(string[] args, JObject config) 
        {
            return new CompileOptions
            {
                ProjectFolder = args[0],
                CompiledOutputPath = args[0] + "/output/",
                IsExecutable = config["mainClass"] != null,
                PathToClass = config["mainClass"]?["file"]?.ToString(),
                MainClass = config["mainClass"]?["class"]?.ToString(),
                Name = config["name"].ToString(),
                Dependencies = config["depends"]?.ToList()?.Select(e => e.ToString())?.ToList() ?? new List<string>(),
                Icon = config["icon"]?.ToString()
            };
        }

        private static bool Compile(CompileOptions compileOptions, List<string> strings)
        {
            Console.WriteLine("Compiling...");
            compileOptions.Code = strings;
            var compiler = new Compiler(compileOptions);
            compiler.Compile();
            _timeWatch.Elapsed("compiling");
            
            foreach (CompilerError error in compiler.CompilerErrors)
            {
                Console.WriteLine($"Error {error.ErrorNumber}: {error.ErrorText} ({error.FileName.Replace(".cs", ".tengri")})");
                return false;
            }

            return true;
        }

        private static List<string> TranslateCode(Dictionary<string, List<TreeElement>> tree, List<string> cacheCode)
        {
            Console.WriteLine("Translate to csharp code...");
            
            var strings = new List<string>(cacheCode);
            
            foreach (var treeElement in tree)
            {
                var translator = new Translator(treeElement.Key, treeElement.Value);
                var data = translator.GetCode();
                File.WriteAllText(".temp/" + Md5.GenerateHash(_compileOptions.Name + treeElement.Key + VERSION), data);
                strings.Add(data);
            }
            
            _timeWatch.Elapsed("translate to csharp code");
            return strings;
        }
        
        private static Dictionary<string, List<TreeElement>> BuildTree(Dictionary<string, string> data)
        {
            Console.WriteLine("Finding lexemes and building tree..");

            try
            {
                var fileTokens = new Dictionary<string, List<TreeElement>>();

                foreach (var file in data)
                {
                    var lexer = new Lexer(file.Key, file.Value);
                    var tokens = lexer.GetTokens();
                    _timeWatch.Elapsed("finding lexemes");
                    var builder = new TreeBuilder(file.Key, tokens);
                    fileTokens.Add(file.Key, builder.GetTree());
                    _timeWatch.Elapsed("creating tree");
                }

                return fileTokens;
            }
            catch (TengriException ex)
            {
                if (ex is ASTException astException)
                {
                    Console.WriteLine($"Error while constructing syntax tree: {astException.Message}");
                }
                else
                {
                    Console.WriteLine($"Error while finding lexems: {ex.Message}");
                }

                Console.ReadLine();
                Environment.Exit(0);
            }

            return null;
        }

        private static Dictionary<string, string> Parse(string path, string additionalData = "")
        {
            var files = Directory.GetFiles(path + "/" + additionalData);
            var parsedFiles = new Dictionary<string, string>();
            
            foreach (var file in files)
            {
                if (!file.EndsWith(".tengri")) continue;
                var delimiter = additionalData.Length > 0 ? "/" : "";

                var data = File.ReadAllText(file);
                var pathFile = additionalData + delimiter + file.Substring(_projectFolder.Length + 1);
                _hashes.Add(pathFile, Md5.GenerateHash(_compileOptions.Name + pathFile));
                
                parsedFiles.Add(pathFile,
                    data);
            }

            if (_isRunStatistics)
            {
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                
                foreach (var file in parsedFiles)
                {
                    _countLines += file.Value.Split('\n').Length;
                }

                stopwatch.Stop();
                if (_timeWatch.TimeElapsedData.ContainsKey("counting the number of lines"))
                {
                    _timeWatch.TimeElapsedData["counting the number of lines"] += stopwatch.ElapsedMilliseconds;
                }
                else
                {
                    _timeWatch.TimeElapsedData.Add("counting the number of lines", stopwatch.ElapsedMilliseconds);
                }
            }

            var folders = Directory.GetDirectories(path + "/" + additionalData);
            foreach (var folder in folders)
            {
                Dictionary<string, string> parsed = new Dictionary<string, string>();
                
                if (additionalData.Length == 0)
                {
                    parsed = Parse(path, folder.Substring(_projectFolder.Length + 1));
                }
                else
                {
                    parsed = Parse(path, additionalData + "/" + folder.Substring(_projectFolder.Length + 1));
                }

                foreach (var parse in parsed)
                {
                    parsedFiles.Add(parse.Key, parse.Value);
                }
            }

            return parsedFiles;
        }
    }
}