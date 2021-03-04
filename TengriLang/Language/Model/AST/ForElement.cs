using System.Collections.Generic;
using TengriLang.Language.Model.Lexeme;
using TengriLang.Reader;
using NotImplementedException = System.NotImplementedException;

namespace TengriLang.Language.Model.AST
{
    public class ForElement : TreeElement, IElement
    {
        public List<TreeElement> Block;
        public string VarName;
        public List<TreeElement> StartValue;
        public List<TreeElement> ToValue;

        public ForElement(string var, List<TreeElement> startVar, List<TreeElement> toVar, TreeElement parent, List<TreeElement> block) : base(parent.File, parent.Position, parent.Line, parent.CharIndex)
        {
            Block = block;
            VarName = var;
            StartValue = startVar;
            ToValue = toVar;
        }

        public string ParseCode(Translator translator, TreeReader reader)
        {
            return $"for (dynamic TENGRI_{VarName} = {translator.Emulate(StartValue, false)}; TENGRI_{VarName} < {translator.Emulate(ToValue, false)}; TENGRI_{VarName}++) {{{translator.Emulate(Block, true, translator.IsStaticBlock)}}}";
        }
    }
}