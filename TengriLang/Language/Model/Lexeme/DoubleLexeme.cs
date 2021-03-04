using TengriLang.Reader;

namespace TengriLang.Language.Model.Lexeme
{
    public class DoubleLexeme : TreeElement, IElement
    {
        public double Value;
        
        public DoubleLexeme(float value, string file, StringReader reader)
            : base(file, reader.Position, reader.Line, reader.CharIndex)
        {
            Value = value;
        }

        public string ParseCode(Translator translator, TreeReader reader) => Value.ToString().Replace(',', '.');
    }
}