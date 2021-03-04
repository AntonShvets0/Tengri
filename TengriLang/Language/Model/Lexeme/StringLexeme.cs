using TengriLang.Reader;

namespace TengriLang.Language.Model.Lexeme
{
    public class StringLexeme : TreeElement, IElement
    {
        public string Value;
        
        public StringLexeme(string value, string file, StringReader reader)
            : base(file, reader.Position, reader.Line, reader.CharIndex)
        {
            Value = value;
        }
        
        public StringLexeme(string value, string file, int position, int line, int charIndex)
            : base(file, position, line, charIndex)
        {
            Value = value;
        }

        public string ParseCode(Translator translator, TreeReader reader) => $"\"{Value}\"";
    }
}