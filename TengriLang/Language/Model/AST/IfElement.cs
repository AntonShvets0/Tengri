using System.Collections.Generic;
using TengriLang.Reader;
using NotImplementedException = System.NotImplementedException;

namespace TengriLang.Language.Model.AST
{
    public class IfElement : TreeElement, IElement
    {
        public List<TreeElement> Condition;
        
        public Dictionary<List<TreeElement>, List<TreeElement>> ElifBlocks = new Dictionary<List<TreeElement>, List<TreeElement>>();
        
        public List<TreeElement> ThenBlock;
        public List<TreeElement> ElseBlock;

        public IfElement(TreeElement parent, List<TreeElement> condition, List<TreeElement> then) : base(parent.File, parent.Position, parent.Line, parent.CharIndex)
        {
            Condition = condition;
            ThenBlock = then;
        }

        public string ParseCode(Translator translator, TreeReader reader)
        {
            string code = $"if ({translator.Emulate(Condition, false)})";
            code += "{" + translator.Emulate(ThenBlock, true, translator.IsStaticBlock) + "}";

            foreach (var elifBlock in ElifBlocks)
            {
                code += $"else if ({translator.Emulate(elifBlock.Key)})";
                code += "{" + translator.Emulate(elifBlock.Value, true, translator.IsStaticBlock) + "}";
            }

            if (ElseBlock != null)
            {
                code += "else {" + translator.Emulate(ElseBlock, true, translator.IsStaticBlock) + "}";
            }

            return code;
        }
    }
}