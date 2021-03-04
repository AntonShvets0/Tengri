using System.Collections.Generic;
using TengriLang.Reader;
using NotImplementedException = System.NotImplementedException;

namespace TengriLang.Language.Model.AST
{
    public class WhileElement : TreeElement, IElement
    {
        public List<TreeElement> Condition;
        public List<TreeElement> Block;

        public WhileElement(TreeElement parent, List<TreeElement> condition, List<TreeElement> block) : base(parent.File, parent.Position, parent.Line, parent.CharIndex)
        {
            Condition = condition;
            Block = block;
        }

        public string ParseCode(Translator translator, TreeReader reader)
        {
            return $"while ({translator.Emulate(Condition, true)}) {{{translator.Emulate(Block, true, translator.IsStaticBlock)}}}";
        }
    }
}