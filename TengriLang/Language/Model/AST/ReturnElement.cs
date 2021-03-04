using System.Collections.Generic;
using TengriLang.Reader;

namespace TengriLang.Language.Model.AST
{
    public class ReturnElement : TreeElement, IElement
    {
        public List<TreeElement> ReturnBlock;

        public ReturnElement(TreeElement parent, List<TreeElement> block) : base(parent.File, parent.Position, parent.Line, parent.CharIndex)
        {
            ReturnBlock = block;
        }

        public string ParseCode(Translator translator, TreeReader reader)
        {
            return "return " + translator.Emulate(ReturnBlock, false, translator.IsStaticBlock) + ";";
        }
    }
}