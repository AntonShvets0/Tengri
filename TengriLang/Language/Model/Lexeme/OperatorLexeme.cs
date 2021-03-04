using TengriLang.Reader;

namespace TengriLang.Language.Model.Lexeme
{
    public class OperatorLexeme : TreeElement, IElement
    {
        public string Value;
        
        public OperatorLexeme(string value, string file, StringReader reader)
            : base(file, reader.Position, reader.Line, reader.CharIndex)
        {
            Value = value;
        }
        
        public string ParseCode(Translator translator, TreeReader reader) => Value;
    }
}