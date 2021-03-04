using TengriLang.Reader;

namespace TengriLang.Language.Model
{
    public interface ILexeme
    {
        TreeElement ParseElement(
            TreeBuilder builder,
            TreeReader reader
            );
    }
}