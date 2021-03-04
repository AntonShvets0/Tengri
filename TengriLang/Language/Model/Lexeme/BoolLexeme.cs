using TengriLang.Reader;
using NotImplementedException = System.NotImplementedException;

namespace TengriLang.Language.Model.Lexeme
{
    public class BoolLexeme : TreeElement, IElement
    {
        public bool Value;
        
        public BoolLexeme(bool value, string file, StringReader reader)
            : base(file, reader.Position, reader.Line, reader.CharIndex)
        {
            Value = value;
        }

        public string ParseCode(Translator translator, TreeReader reader) => Value ? "true" : "false";
    }
}