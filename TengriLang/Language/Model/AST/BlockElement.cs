using System.Collections.Generic;
using TengriLang.Language.Model.Lexeme;
using TengriLang.Reader;
using NotImplementedException = System.NotImplementedException;

namespace TengriLang.Language.Model.AST
{
    public class BlockElement : TreeElement, IElement, ILexeme
    {
        public List<TreeElement> Block;
        public bool IsFuncBrackets = false;

        public BlockElement(TreeElement parent, List<TreeElement> block) : base(parent.File, parent.Position, parent.Line, parent.CharIndex)
        {
            Block = block;
        }

        public TreeElement ParseElement(TreeBuilder builder, TreeReader reader)
        {
            if (reader.Read(1) is SpecialLexeme specialLexeme && specialLexeme.Value == "{" && IsFuncBrackets)
            {
                reader.Next(2);
                var args = new List<List<TreeElement>>()
                {
                    new List<TreeElement>()
                };

                foreach (var treeElement in Block)
                {
                    if (treeElement is SpecialLexeme special && special.Value == ",")
                    {
                        args.Add(new List<TreeElement>());
                    }
                    else
                    {
                        args[args.Count - 1].Add(treeElement);
                    }
                }
                
                var func = new DeclareFunctionElement(this, args, builder.ParseInBrackets('{', '}')[0]);
                return func.ParseElement(builder, reader);
            }

            return this;
        }

        public string ParseCode(Translator translator, TreeReader reader)
        {
            return "{" + translator.Emulate(Block, true, translator.IsStaticBlock) + "}";
        }
    }
}