using TengriLang.Exceptions;

namespace TengriLang.Language.Model
{
    public abstract class TreeElement
    {
        public int Position { get; }
        public int Line { get; }
        
        public int CharIndex { get; }
        public string File { get; }

        public TreeElement(string file, int position, int line, int charIndex)
        {
            File = file;
            Position = position;
            Line = line;
            CharIndex = charIndex;
        }

        public void Exception(string message)
        {
            throw new ASTException(this, message);
        }
    }
}