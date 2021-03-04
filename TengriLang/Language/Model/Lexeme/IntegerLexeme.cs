using TengriLang.Reader;

namespace TengriLang.Language.Model.Lexeme
{
    public class IntegerLexeme : TreeElement, IElement
    {
        public int Value;
        
        public IntegerLexeme(int value, string file, StringReader reader)
            : base(file, reader.Position, reader.Line, reader.CharIndex)
        {
            Value = value;
        }

        public string ParseCode(Translator translator, TreeReader reader) => Value.ToString();
    }
}