using System.Collections.Generic;
using TengriLang.Reader;

namespace TengriLang.Language.Model.AST
{
    public class BlockElement : TreeElement, IElement
    {
        public List<TreeElement> Block;

        public BlockElement(TreeElement parent, List<TreeElement> block) : base(parent.File, parent.Position, parent.Line, parent.CharIndex)
        {
            Block = block;
        }

        public string ParseCode(Translator translator, TreeReader reader)
        {
            return "{" + translator.Emulate(Block, true, translator.IsStaticBlock) + "}";
        }
    }
}