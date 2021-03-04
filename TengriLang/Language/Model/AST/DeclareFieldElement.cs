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
            return translator.IsStaticBlock ? "static " : "" + $"{translator.TypeToString(Type)} dynamic TENGRI_{FieldName} = {translator.Emulate(Body, false)};";
        }
    }
}