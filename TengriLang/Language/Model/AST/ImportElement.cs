using System.Collections.Generic;
using TengriLang.Reader;

namespace TengriLang.Language.Model.AST
{
    public class ImportElement : TreeElement, IElement
    {
        public string Namespace;
        
        public ImportElement(TreeElement parent, string ns) : base(parent.File, parent.Position, parent.Line, parent.CharIndex)
        {
            Namespace = "FILE_TENGRI_" + ns.Replace('\\', '_').Replace('/', '_').Replace(".tengri", "");
        }

        public string ParseCode(Translator translator, TreeReader reader)
        {
            Translator.NeedNamespaces.Add(Namespace);
            return null;
        }
    }
}