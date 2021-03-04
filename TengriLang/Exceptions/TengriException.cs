using System;

namespace TengriLang.Exceptions
{
    public class TengriException : Exception
    {
        public TengriException(string message) : base(message) {}
    }
}