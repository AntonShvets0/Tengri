using TengriLang.Reader;
using NotImplementedException = System.NotImplementedException;

namespace TengriLang.Language.Model.Lexeme
{
    public class NewLineLexeme : TreeElement, ILexeme
    {
        public NewLineLexeme(string file, StringReader reader)
            : base(file, reader.Position, reader.Line, reader.CharIndex)
        {
            
        }

        public TreeElement ParseElement(TreeBuilder builder, TreeReader reader) => null;
    }
}