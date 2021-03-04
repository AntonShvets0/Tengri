using System.Collections.Generic;
using TengriLang.Language.Model.Lexeme;
using TengriLang.Reader;
using NotImplementedException = System.NotImplementedException;

namespace TengriLang.Language.Model.AST
{
    public class InvokeElement : TreeElement, IElement
    {
        public List<TreeElement> Block;
        public string VarName;
        public VariableLexeme Array;

        public InvokeElement(string var, VariableLexeme parent, List<TreeElement> block) : base(parent.File, parent.Position, parent.Line, parent.CharIndex)
        {
            Block = block;
            Array = parent;
            VarName = var;
        }

        public string ParseCode(Translator translator, TreeReader reader)
        {
            return $"foreach (dynamic TENGRI_{VarName} in {Array.BuildVar(translator)}.TENGRI_invoke()) {{{translator.Emulate(Block, true, translator.IsStaticBlock)}}}";
        }
    }
}