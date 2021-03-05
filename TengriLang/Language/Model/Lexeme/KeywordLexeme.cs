using System.Collections.Generic;
using TengriLang.Language.Model.AST;
using TengriLang.Reader;
using NotImplementedException = System.NotImplementedException;

namespace TengriLang.Language.Model.Lexeme
{
    public class KeywordLexeme : TreeElement, IElement, ILexeme
    {
        public string Value;
        
        public KeywordLexeme(string value, string file, StringReader reader)
            : base(file, reader.Position, reader.Line, reader.CharIndex)
        {
            Value = value;
        }

        public TreeElement ParseElement(TreeBuilder builder, TreeReader reader)
        {
            switch (Value)
            {
                case "for":
                case "while":
                case "do":
                    return ParseLoop(builder, reader);
                case "if":
                    return ParseIf(builder, reader);
                case "return":
                    return new ReturnElement(this, builder.ParseToNewLine());
                case "try":
                    reader.Next();
                    var tryBlock = builder.ParseInBrackets('{', '}')[0];
                    reader.Next();

                    List<TreeElement> catchBlock = new List<TreeElement>();
                    List<TreeElement> finallyBlock = new List<TreeElement>();

                    VariableLexeme variableLexeme = null;
                    
                    if (reader.Read() is KeywordLexeme keywordLexeme && keywordLexeme.Value == "catch")
                    {
                        reader.Next();

                        if (reader.Read() is SpecialLexeme specialLexeme && specialLexeme.Value == ":"
                            && reader.Read(1) is VariableLexeme variable)
                        {
                            if (variable.Args.Count > 0) Exception("Wrong name var in catch block");
                            variableLexeme = variable;
                        }

                        reader.Next(2);
                        catchBlock = builder.ParseInBrackets('{', '}')[0];
                        reader.Next();
                    }

                    if (reader.Read() is KeywordLexeme finallyKeyword && finallyKeyword.Value == "finally")
                    {
                        reader.Next();
                        finallyBlock = builder.ParseInBrackets('{', '}')[0];
                        reader.Next();
                    }
                    
                    return new TryElement(this, variableLexeme, tryBlock, catchBlock, finallyBlock);
                case "import":
                {
                    var block = builder.ParseToNewLine();
                    if (block.Count != 1) Exception("Wrong import!");
                    if (block[0] is StringLexeme lexeme)
                        return new ImportElement(this, lexeme.Value);

                    Exception("Wrong import!");
                    break;
                }
                case "export":
                    return new ExportElement(this, builder.ParseToNewLine());
                case "fun" when reader.Read() is VariableLexeme name:
                    return ParseFun(name, builder, reader);
            }

            return this;
        }

        private TreeElement ParseLoop(TreeBuilder builder, TreeReader reader)
        { 
            if (Value == "while" || Value == "do")
            {
                var cond = ParseToBracket(reader);
                reader.Next();

                var block = builder.ParseInBrackets('{', '}')[0];
                
                if (Value == "while") return new WhileElement(this, cond, block);
                return new DoWhileElement(this, cond, block);
            }
            else if (Value == "for")
            {
                var cond = ParseToBracket(reader);
                reader.Next();

                VariableLexeme variable = null;
                List<TreeElement> startValue = new List<TreeElement>();
                List<TreeElement> endValue = new List<TreeElement>();
                var block = builder.ParseInBrackets('{', '}')[0];
                var dotCount = 0;
                var toStartValue = true;

                for (int i = 0; i < cond.Count; i++)
                {
                    if (cond[i] is VariableLexeme variableLexeme && toStartValue)
                    {
                        variable = variableLexeme;
                        if (variableLexeme.Args.Count > 0) Exception("Wrong for construction!");
                    } else if (i == 0) Exception("Wrong for construction!");
                    else if (cond[i] is SpecialLexeme specialLexeme && toStartValue && specialLexeme.Value == ".")
                    {
                        dotCount++;

                        if (dotCount == 3) toStartValue = false;
                    }
                    else if (i == 1)
                    {
                        if (cond[i] is KeywordLexeme keywordLexeme && keywordLexeme.Value == "in") continue;
                        Exception("Wrong for construction!");
                    } 
                    else
                    {
                        if (toStartValue) startValue.Add(cond[i]);
                        else endValue.Add(cond[i]);
                    }
                }
                
                return new ForElement(variable.Value, startValue, endValue, this, block);
            }

            return null;
        }

        private TreeElement ParseFun(VariableLexeme name, TreeBuilder builder, TreeReader reader)
        {
            reader.Next(2);
            var args = builder.ParseInBrackets('(', ')', ',');
            reader.Next(2);

            var body = builder.ParseInBrackets('{', '}')[0];
            reader.Next();
                
            return new DeclareFunctionElement(this, args, body)
            {
                Name = name.Value
            };
        }
        
        private TreeElement ParseIf(TreeBuilder builder, TreeReader reader)
        {
            var cond = ParseToBracket(reader);
            reader.Next();

            var thenBlock = builder.ParseInBrackets('{', '}')[0];
                
            var ifElement = new IfElement(this, cond, thenBlock);

            while (reader.Read() is KeywordLexeme keywordLexeme &&
                   (keywordLexeme.Value == "elif" || keywordLexeme.Value == "else"))
            {
                reader.Next();

                if (keywordLexeme.Value == "elif")
                {
                    cond = ParseToBracket(reader);
                    reader.Next();

                    var block = builder.ParseInBrackets('{', '}')[0];
                    ifElement.ElifBlocks.Add(cond, block);
                }

                if (keywordLexeme.Value == "else")
                {
                    reader.Next();
                    var elseBlock = builder.ParseInBrackets('{', '}')[0];
                    ifElement.ElseBlock = elseBlock;
                }
            }

            return ifElement;
        }
        
        private List<TreeElement> ParseToBracket(TreeReader reader)
        {
            var openBracketsFunc = 0;
            var openBracketsArray = 0;
            
            return reader.ReadWhile(el =>
            {
                if (el is SpecialLexeme specialLexeme)
                {
                    if (specialLexeme.Value == "{")
                    {
                        if (openBracketsArray < 1 && openBracketsFunc < 1) return false;
                    }
                    if (specialLexeme.Value == "(") openBracketsFunc++;
                    if (specialLexeme.Value == "[") openBracketsArray++;
                    if (specialLexeme.Value == ")") openBracketsFunc--;
                    if (specialLexeme.Value == "]") openBracketsArray++;
                }

                return el;
            });
        }

        public string ParseCode(Translator translator, TreeReader reader)
        {
            switch (Value)
            {
                case "var": return "dynamic ";
                case "continue":
                case "break": return $"{Value};";
                case "this": return Value;
                case "static":
                    if (reader.Read() is BlockElement block && !block.IsFuncBrackets)
                    {
                        reader.Next();
                        var data = translator.Emulate(block.Block, true, true);
                        return data;
                    }
                    break;
            }

            return Value + " ";
        }
    }
}