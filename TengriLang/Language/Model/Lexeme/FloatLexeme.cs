using TengriLang.Reader;

namespace TengriLang.Language.Model.Lexeme
{
    public class FloatLexeme : TreeElement, IElement
    {
        public float Value;
        
        public FloatLexeme(float value, string file, StringReader reader)
            : base(file, reader.Position, reader.Line, reader.CharIndex)
        {
            Value = value;
        }
        
        public string ParseCode(Translator translator, TreeReader reader) 
            => Value.ToString().Replace(',', '.') + "f";
    }
}