using TengriLang.Reader;

namespace TengriLang.Language.Model
{
    public interface IElement
    {
        string ParseCode(
            Translator translator, 
            TreeReader reader
            );
    }
}