using System;

namespace TengriLang.Language.System.Library
{
    public class TENGRI_exception : Exception
    {
        public TENGRI_exception(dynamic[] args) : base(args[0] as string)
        {
            
        }
    }
}