using TengriLang.Language.Model;

namespace TengriLang.Reader.Model
{
    public class ReadWhileResponse
    {
        public readonly bool IsContinue;
        public readonly object Value;

        public ReadWhileResponse(bool isContinue, object value = null)
        {
            Value = value;
            IsContinue = isContinue;
        }
        
        public static implicit operator ReadWhileResponse(bool isContinue) => new ReadWhileResponse(isContinue);
        public static implicit operator ReadWhileResponse(TreeElement value) => new ReadWhileResponse(true, value);
        public static implicit operator ReadWhileResponse(string value) => new ReadWhileResponse(true, value);
        public static implicit operator ReadWhileResponse(char value) => new ReadWhileResponse(true, value);
    }
}