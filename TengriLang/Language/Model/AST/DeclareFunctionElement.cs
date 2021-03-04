using System.Collections.Generic;
using TengriLang.Language.Model.Lexeme;
using TengriLang.Reader;
using NotImplementedException = System.NotImplementedException;

namespace TengriLang.Language.Model.AST
{
    public class DeclareFunctionElement : TreeElement, IElement, ILexeme
    {
        public FuncType FuncType;
        public string Value;
        public List<List<TreeElement>> Args;
        public List<TreeElement> Body;

        public string ClassName;

        public DeclareFunctionElement(CallFunctionElement functionElement, List<TreeElement> body, FuncType type)
            : base(functionElement.File, functionElement.Position, functionElement.Line, functionElement.CharIndex)
        {
            Body = body;
            FuncType = type;
            Value = functionElement.Value.Value;
            Args = functionElement.Args;
        }

        public TreeElement ParseElement(TreeBuilder builder, TreeReader reader)
        {
            return this;
        }

        public string ParseCode(Translator translator, TreeReader reader)
        {
            if (translator.IsStaticBlock && Value == "init")
            {
                Exception("Creating a constructor in static block!");
            }
            
            string code = "";

            if (Value == "main" && translator.IsStaticBlock)
            {
                code += $"static void Main(string[] args) {{TENGRI_{Value}(args);}}";
            }
            
            if (Value == "init")
            {
                code += $"{translator.TypeToString(FuncType)} TENGRI_{ClassName}(dynamic[] TENGRI_SYS_ARGS)" + "{";
            }
            else
            {
                code += (translator.IsStaticBlock ? "static " : "") +
                       $"{translator.TypeToString(FuncType)} dynamic TENGRI_{Value}(dynamic[] TENGRI_SYS_ARGS) " + "{";
            }

            var i = 0;
                
            foreach (var arg in Args)
            {
                if (arg.Count == 0) continue;
                if (!(arg[0] is VariableLexeme || arg[0] is AssignElement)) Exception("Wrong args!");
                
                if (arg[0] is AssignElement assign)
                {
                    var val = translator.Emulate(assign.Right, false);

                    code += $"dynamic TENGRI_{assign.Left.Value};if (TENGRI_SYS_ARGS.Length - 1 < {i})" + "{TENGRI_" + assign.Left.Value + " = " + val + ";}else{TENGRI_" + assign.Left.Value + " = TENGRI_SYS_ARGS[" + (i) + "];}";
                }
                else
                {
                    var variable = arg[0] as VariableLexeme;
                    code += $"dynamic TENGRI_{variable.Value} = TENGRI_SYS_ARGS[{i}];";
                }

                i++;
            }  

            code += translator.Emulate(Body);

            if (Value == "main" && translator.IsStaticBlock)
            {
                // Это чтобы консоль не закрывалась
                code += "TENGRI_console.TENGRI_input(null);";
            }

            if (Value != "init")
            {
                code += "return null;";
            }
            
            return code + "}";
        }
    }
}