using System.Collections.Generic;
using System.Linq;
using TengriLang.Language.Model.Lexeme;
using TengriLang.Reader;

namespace TengriLang.Language.Model.AST
{
    public class AssignElement : TreeElement, IElement, ILexeme
    {
        public VariableLexeme Left;
        public string Operator;
        public List<TreeElement> Right;

        public AssignElement(VariableLexeme left, string op, List<TreeElement> right) : base(left.File, left.Position,
            left.Line, left.CharIndex)
        {
            Left = left;
            Operator = op;
            Right = right;
        }

        public TreeElement ParseElement(TreeBuilder builder, TreeReader reader)
        {
            if (Right.Count == 1 && Right[0] is BlockElement block && !block.IsFuncBrackets)
            {
                var data = block.Block.Where(e => e is DeclareFieldElement);
                foreach (var obj in data)
                {
                    var declareField = obj as DeclareFieldElement;
                    declareField.ClassName = Left.Value;
                }

                var declareClass = new DeclareClassElement(Left, block.Block, FuncType.Public);
                return declareClass.ParseElement(builder, reader) ?? declareClass;
            }

            return null;
        }

        public string ParseCode(Translator translator, TreeReader reader)
        {
            var variable = Left.BuildVar(translator);
            if (translator.ClassName != null)
                return $"{variable} {Operator} {translator.Emulate(Right, false)}" +
                       (translator.InBlock ? ";" : "");
            else if (translator.ClassName == null && Operator == "=") return $"class SYS_TENGRI_GLOBAL_{Left.Value} {{ public static dynamic {variable} = {translator.Emulate(Right, false)};}}";
            return null;
        }
    }
}