using System.Collections.Generic;
using TengriLang.Reader;

namespace TengriLang.Language.Model.AST
{
    public class ExportElement : TreeElement, IElement
    {
        public List<TreeElement> ReturnBlock;
        
        public ExportElement(TreeElement parent, List<TreeElement> returnBlock) : base(parent.File, parent.Position, parent.Line, parent.CharIndex)
        {
            ReturnBlock = returnBlock;
        }

        public string ParseCode(Translator translator, TreeReader reader)
        {
            var cl = File.Replace('\\', '_').Replace('/', '_').Replace(".tengri", "");
            return $"class ExportClass_{cl} {{ public static dynamic Get() {{ return " + translator.Emulate(ReturnBlock, false) + "; } }";
        }
    }
}