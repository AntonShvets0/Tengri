using System.Collections.Generic;
using System.Linq;
using TengriLang.Language.Model.AST;
using TengriLang.Reader;
using NotImplementedException = System.NotImplementedException;

namespace TengriLang.Language.Model.Lexeme
{
    public class SpecialLexeme : TreeElement, IElement, ILexeme
    {
        public string Value;
        
        public SpecialLexeme(string value, string file, StringReader reader)
            : base(file, reader.Position, reader.Line, reader.CharIndex)
        {
            Value = value;
        }
        
        public string ParseCode(Translator translator, TreeReader reader) => Value;
        public TreeElement ParseElement(TreeBuilder builder, TreeReader reader)
        {
            switch (Value)
            {
                case "@":
                    if (reader.Read() is VariableLexeme variableLexeme)
                    {
                        if (!builder.AvailableAttributes.Split(' ').Contains(variableLexeme.Value))
                        {
                            variableLexeme.Exception($"Unknown attribute \"{variableLexeme.Value}\"");
                        }
                        
                        reader.Next();
                        return new AttributeElement(variableLexeme);
                    }

                    return null;
                case "{":
                    return new BlockElement(this, builder.ParseInBrackets('{', '}')[0]);;
                case "[":
                    return ParseArray(builder, reader);
                case ",": return this;
                default: return null;
            }
        }

        private DeclareArrayElement ParseArray(TreeBuilder builder, TreeReader reader)
        {
            var block = builder.ParseInBrackets('[', ']')[0];
            var data = new Dictionary<string, List<TreeElement>>();

            var index = 0;
            
            foreach (var treeElement in block)
            {
                if (treeElement is DeclareFieldElement fieldElement)
                {
                    data.Add(fieldElement.FieldName, fieldElement.Body);
                } 
                else
                {
                    if (treeElement is SpecialLexeme specialLexeme && specialLexeme.Value == ",")
                    {
                        index++;
                        continue;
                    }
                    
                    if (data.ContainsKey(index.ToString()))
                    {
                        data[index.ToString()].Add(treeElement);
                    }
                    else
                    {
                        data.Add(index.ToString(), new List<TreeElement> { treeElement });
                    }
                }
            }
            
            return new DeclareArrayElement(this, data);
        }
    }
}