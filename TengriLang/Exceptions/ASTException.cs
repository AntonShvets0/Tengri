using System;
using TengriLang.Language.Model;

namespace TengriLang.Exceptions
{
    public class ASTException : TengriException
    {
        public TreeElement Element;
        
        public ASTException(TreeElement element, string message) 
            : base($"\"{message}\" ({element.File}:{element.Line}:{element.CharIndex})")
        {
            
        }
    }
}