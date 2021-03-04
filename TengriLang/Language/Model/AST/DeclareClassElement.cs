using System.Collections.Generic;
using TengriLang.Language.Model.Lexeme;
using TengriLang.Reader;
using NotImplementedException = System.NotImplementedException;

namespace TengriLang.Language.Model.AST
{
    public class DeclareClassElement : TreeElement, IElement, ILexeme
    {
        public string ClassName;
        public List<TreeElement> Body;

        public FuncType Type;

        public DeclareClassElement(VariableLexeme lexeme, List<TreeElement> body, FuncType type) : base(lexeme.File, lexeme.Position, lexeme.Line,
            lexeme.CharIndex)
        {
            ClassName = lexeme.Value;
            Body = body;
            Type = type;
        }

        public TreeElement ParseElement(TreeBuilder builder, TreeReader reader)
        {
            return this;
        }

        public string ParseCode(Translator translator, TreeReader reader)
        {
            return "public class TENGRI_" + ClassName + " {" + translator.Emulate(Body) + "}";
        }
    }
}