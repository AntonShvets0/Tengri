using System.Collections.Generic;
using TengriLang.Language.Model.Lexeme;
using TengriLang.Reader;
using NotImplementedException = System.NotImplementedException;

namespace TengriLang.Language.Model.AST
{
    public class DeclareFieldElement : TreeElement, IElement, ILexeme
    {
        public string FieldName;
        public List<TreeElement> Body;
        public string ClassName;

        public FuncType Type;

        public DeclareFieldElement(VariableLexeme lexeme, List<TreeElement> body, FuncType type) : base(lexeme.File, lexeme.Position, lexeme.Line,
            lexeme.CharIndex)
        {
            FieldName = lexeme.Value;
            Body = body;
            Type = type;
        }

        public TreeElement ParseElement(TreeBuilder builder, TreeReader reader)
        {
            return null;
        }

        public string ParseCode(Translator translator, TreeReader reader)
        {
            if (translator.IsStaticBlock && FieldName == "init")
            {
                Exception("Creating a constructor in static block!");
            }
            
            string code = (translator.IsStaticBlock ? "static " : "") + $"{translator.TypeToString(Type)} dynamic TENGRI_{FieldName} = {translator.Emulate(Body, false)};";

            if (Body.Count == 1 && Body[0] is DeclareFunctionElement functionElement)
            {
                if (FieldName == "main" && translator.IsStaticBlock)
                {
                    code += "static void Main(string[] args) {{TENGRI_main(args); TENGRI_console.TENGRI_input(null); }}";
                }
                else if (FieldName == "init")
                {
                    code += $"{translator.TypeToString(Type)} TENGRI_{ClassName}(dynamic[] args) {{ TENGRI_init(args); }}";
                }
            }

            return code;
        }
    }
}