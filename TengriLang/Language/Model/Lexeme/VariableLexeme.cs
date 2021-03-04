using System.Collections.Generic;
using TengriLang.Language.Model.AST;
using TengriLang.Language.System;
using TengriLang.Reader;
using NotImplementedException = System.NotImplementedException;

namespace TengriLang.Language.Model.Lexeme
{
    public class VariableLexeme : TreeElement, ILexeme, IElement
    {
        public List<ArgsModel> Args = new List<ArgsModel>();
        public string Value;

        public VariableLexeme(string value, string file, StringReader reader)
            : base(file, reader.Position, reader.Line, reader.CharIndex)
        {
            Value = value;
        }

        public string ParseCode(Translator translator, TreeReader reader) => BuildVar(translator);
        public TreeElement ParseElement(TreeBuilder builder, TreeReader reader)
        {
            if (reader.Read() is SpecialLexeme specialLexeme)
            {
                if (specialLexeme.Value == "." || specialLexeme.Value == "[")
                {
                    Args = ParseArrayBrackets(builder, reader);
                }
            }

            // Этот иф нужен, так как в прошлом могли оказаться скобки от массива
            if (reader.Read() is SpecialLexeme newSpecialLexeme)
            {
                
                if (newSpecialLexeme.Value == "{")
                {
                    reader.Next();
                    var block = builder.ParseInBrackets('{', '}');
                    reader.Next();
                    
                    return new InvokeElement("it", this, block[0]);
                }

                if (newSpecialLexeme.Value == ":")
                {
                    if (reader.Read(1) is VariableLexeme variableLexeme && reader.Read(2) is SpecialLexeme special &&
                        special.Value == "{")
                    {
                        if (variableLexeme.Args.Count > 0) Exception("Wrong var name");
                        reader.Next(3);
                        var block = builder.ParseInBrackets('{', '}');
                        reader.Read();
                        
                        return new InvokeElement(variableLexeme.Value, this, block[0]);
                    }
                    else
                    {
                        FuncType type = builder.GetType(Value, builder.ParseAttributes());
                        reader.Next();
                    
                        var body = builder.ParseToNewLine();
                    
                        var declareField = new DeclareFieldElement(this, body, type);
                        var response = declareField.ParseElement(builder, reader);
                        return response ?? declareField;
                    }
                }
                
                if (newSpecialLexeme.Value == "(")
                {
                    reader.Next();
                    var args = builder.ParseInBrackets('(', ')', ',');
                    
                    var callFunction = new CallFunctionElement(this, args);
                    var response = callFunction.ParseElement(builder, reader);
                    return response ?? callFunction;
                }
            }
            
            if (reader.Read() is OperatorLexeme operatorLexeme)
            {
                if (
                    operatorLexeme.Value == "=" 
                    || 
                    (operatorLexeme.Value.Length == 2 && operatorLexeme.Value[1] == '=' && operatorLexeme.Value != "==")
                    )
                {
                    reader.Next(1);
                    var data = builder.ParseToNewLine();
                    
                    var assign = new AssignElement(this, operatorLexeme.Value, data);
                    var response = assign.ParseElement(builder, reader);
                    return response ?? assign;
                }
            }

            return this;
        }

        public string BuildVar(Translator translator)
        {
            var code = $"TENGRI_{Value}";

            foreach (var lexemeArg in Args)
            {
                if (lexemeArg.IsDotUsed) code += $".{translator.Emulate(lexemeArg.Body, false)}";
                else code += $"[{translator.Emulate(lexemeArg.Body, false)}]";
            }

            return code;
        }
        
        private List<ArgsModel> ParseArrayBrackets(TreeBuilder tree, TreeReader reader)
        {
            var args = new List<ArgsModel>();
            
            while (reader.Read() is SpecialLexeme special && (special.Value == "." || special.Value == "["))
            {
                reader.Next();

                if (special.Value == "[")
                {
                    var brackets = tree.ParseInBrackets('[', ']')[0];
                    args.Add(new ArgsModel
                    {
                        IsDotUsed = false,
                        Body = brackets
                    });
                    reader.Next();
                }
                else
                {
                    var element = reader.Read();
                    if (element is VariableLexeme)
                    {
                        args.Add(new ArgsModel {
                            Body = new List<TreeElement> { element },
                            IsDotUsed = true
                        });
                        
                        reader.Next();
                        continue;
                    }

                    element.Exception("Unknown type in current context. Must be var");
                }
            }

            return args;
        }
    }
}