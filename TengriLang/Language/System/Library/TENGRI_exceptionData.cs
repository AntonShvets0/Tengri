namespace TengriLang.Language.System.Library
{
    public class TENGRI_exceptionData
    {
        public string TENGRI_message;
        public string TENGRI_source;

        public TENGRI_exceptionData(string message, string source)
        {
            TENGRI_message = message;
            TENGRI_source = source;
        }
    }
}