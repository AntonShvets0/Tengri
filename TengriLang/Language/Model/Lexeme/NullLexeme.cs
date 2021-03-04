using TengriLang.Reader;

namespace TengriLang.Language.Model.Lexeme
{
    public class NullLexeme : TreeElement, IElement
    {
        public NullLexeme(string file, StringReader reader)
            : base(file, reader.Position, reader.Line, reader.CharIndex)
        {
            
        }
        
        public string ParseCode(Translator translator, TreeReader reader) => "null";
    }
}