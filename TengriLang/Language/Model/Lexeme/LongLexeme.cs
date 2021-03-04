using TengriLang.Reader;

namespace TengriLang.Language.Model.Lexeme
{
    public class LongLexeme : TreeElement, IElement
    {
        public long Value;
        
        public LongLexeme(long value, string file, StringReader reader)
            : base(file, reader.Position, reader.Line, reader.CharIndex)
        {
            Value = value;
        }

        public string ParseCode(Translator translator, TreeReader reader) => $"{Value}l";
    }
}