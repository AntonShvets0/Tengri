using System.Collections.Generic;
using TengriLang.Reader;

namespace TengriLang.Language.Model.AST
{
    public class ImportElement : TreeElement, IElement
    {
        public string Namespace;
        public string ImportFile;
        
        public ImportElement(TreeElement parent, string ns) : base(parent.File, parent.Position, parent.Line, parent.CharIndex)
        {
            ImportFile = ns.Replace('\\', '_').Replace('/', '_').Replace(".tengri", "");
            Namespace = "FILE_TENGRI_" + ImportFile;
        }

        public string ParseCode(Translator translator, TreeReader reader)
        {
            Translator.NeedNamespaces.Add(Namespace);
            return $"ExportClass_{ImportFile}.Get()" + (translator.InBlock ? ";" : "");
        }
    }
}