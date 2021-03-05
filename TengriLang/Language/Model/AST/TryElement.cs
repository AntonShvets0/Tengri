using System.Collections.Generic;
using TengriLang.Language.Model.Lexeme;
using TengriLang.Reader;
using NotImplementedException = System.NotImplementedException;

namespace TengriLang.Language.Model.AST
{
    public class TryElement : TreeElement, IElement
    {
        public List<TreeElement> TryBlock;
        public List<TreeElement> CatchBlock;
        public List<TreeElement> FinallyBlock;

        public VariableLexeme Variable;

        
        public TryElement(TreeElement parent, VariableLexeme variable, List<TreeElement> tryBlock, List<TreeElement> catchBlock, List<TreeElement> finallyBlock) : base(parent.File, parent.Position, parent.Line, parent.CharIndex)
        {
            TryBlock = tryBlock;
            CatchBlock = catchBlock;
            Variable = variable;
            FinallyBlock = finallyBlock;
        }

        public string ParseCode(Translator translator, TreeReader reader)
        {
            var variable = Variable == null ? "ex" : Variable.Value;
            
            return $"try {{ {translator.Emulate(TryBlock)} }} " +
                   $"catch (Exception SYS_TENGRI_{variable})" +
                   $"{{ var TENGRI_{variable} = new TENGRI_exceptionData(SYS_TENGRI_{variable}.Message, SYS_TENGRI_{variable}.Source); {translator.Emulate(CatchBlock)} }}" 
                   + (FinallyBlock.Count != 0 ? $"finally {{{translator.Emulate(FinallyBlock)}}}" : "");
        }
    }
}