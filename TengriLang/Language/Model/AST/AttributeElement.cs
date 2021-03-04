using TengriLang.Language.Model.Lexeme;

namespace TengriLang.Language.Model.AST
{
    public class AttributeElement : TreeElement
    {
        public string AttributeName;

        public AttributeElement(VariableLexeme var) : base(var.File, var.Position, var.Line, var.CharIndex)
        {
            AttributeName = var.Value;
        }
    }
}