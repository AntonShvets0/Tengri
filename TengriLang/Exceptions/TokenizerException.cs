using System;

namespace TengriLang.Exceptions
{
    public class TokenizerException : TengriException
    {
        public TokenizerException(string file, int line, int position, string message) : base(message + $" ({file}:{line}:{position})")
        {
        }
    }
}