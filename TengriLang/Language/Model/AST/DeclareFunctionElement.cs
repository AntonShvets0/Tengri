using System;
using System.Collections.Generic;
using TengriLang.Language.Model.Lexeme;
using TengriLang.Language.System;
using TengriLang.Reader;
using NotImplementedException = System.NotImplementedException;

namespace TengriLang.Language.Model.AST
{
    public class DeclareFunctionElement : TreeElement, IElement, ILexeme
    {
        public List<List<TreeElement>> Args;
        public List<TreeElement> Body;
        public string Name = null;
        
        public DeclareFunctionElement(TreeElement parent, List<List<TreeElement>> args, List<TreeElement> body)
            : base(parent.File, parent.Position, parent.Line, parent.CharIndex)
        {
            Body = body;
            Args = args;
            _randomArgs = new Random(new Random().Next(1, 50000 + Body.Count + Args.Count)).Next(0, 100000);
        }

        public TreeElement ParseElement(TreeBuilder builder, TreeReader reader)
        {
            return this;
        }

        public string ParseCode(Translator translator, TreeReader reader)
        {
            string code = Name == null ? $"(TengriData.TengriMethod)({ArgsName} => {{" : $"class SYS_TENGRI_GLOBAL_{Name} {{ public static dynamic TENGRI_{Name}(dynamic[] {ArgsName}) {{";

            var i = 0;
                
            foreach (var arg in Args)
            {
                if (arg.Count == 0) continue;
                if (!(arg[0] is VariableLexeme || arg[0] is AssignElement)) Exception("Wrong args!");
                
                if (arg[0] is AssignElement assign)
                {
                    var val = translator.Emulate(assign.Right, false);

                    code += $"dynamic TENGRI_{assign.Left.Value};if ({ArgsName}.Length - 1 < {i})" + "{TENGRI_" + assign.Left.Value + " = " + val + ";}else{TENGRI_" + assign.Left.Value + $" = {ArgsName}[" + (i) + "];}";
                }
                else
                {
                    var variable = arg[0] as VariableLexeme;
                    code += $"dynamic TENGRI_{variable.Value} = {ArgsName}[{i}];";
                }

                i++;
            }  

            code += translator.Emulate(Body);
            code += "return null;";

            return Name == null ? (code + "})") : code + "}}";
        }

        private int _randomArgs;
        private string ArgsName => $"SYS_TENGRI_{_randomArgs}_ARGS";
    }
}