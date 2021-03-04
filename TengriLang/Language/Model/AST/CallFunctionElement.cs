using System.Collections.Generic;
using System.Linq;
using TengriLang.Language.Model.Lexeme;
using TengriLang.Reader;

namespace TengriLang.Language.Model.AST
{
    public class CallFunctionElement : TreeElement, IElement, ILexeme
    {
        public VariableLexeme Value;
        public List<List<TreeElement>> Args;

        public CallFunctionElement(VariableLexeme variableLexeme, List<List<TreeElement>> args) : base(variableLexeme.File,
            variableLexeme.Position, variableLexeme.Line, variableLexeme.CharIndex)
        {
            Value = variableLexeme;
            Args = args;
        }

        public TreeElement ParseElement(TreeBuilder builder, TreeReader reader)
        {
            if (reader.Read(1) is SpecialLexeme specialLexeme)
            {
                if (specialLexeme.Value == ":")
                {
                    reader.Next(2);
                    FuncType type = builder.GetType(Value.Value, builder.ParseAttributes());

                    var body = builder.ParseToNewLine().FirstOrDefault();
                    if (body != null && body is BlockElement blockElement)
                    {
                        var declareField = new DeclareFunctionElement(this, blockElement.Block, type);
                        var response = declareField.ParseElement(builder, reader);
                        return response ?? declareField;
                    }

                    Exception("Wrong declaring function!");
                }
            }

            return null;
        }

        public string ParseCode(Translator translator, TreeReader reader)
        {
            var data = new List<string>();
            
            foreach (var arg in Args)
            {
                data.Add(translator.Emulate(arg, false));
            }

            if (
                Value.Args.Count == 1 
                && Value.Args[0].IsDotUsed 
                && (Value.Args[0].Body[0] is VariableLexeme variableLexeme && variableLexeme.Value == "init")
                )
            {
                return $"new TENGRI_{Value.Value}(new dynamic[] {{{string.Join(",", data)}}})" + (translator.InBlock ? ";" : "");
            }
            
            var main = Value.BuildVar(translator);
            
            return $"{main}(new dynamic[] {{{string.Join(",", data)}}})" + (translator.InBlock ? ";" : "");
        }
    }
}