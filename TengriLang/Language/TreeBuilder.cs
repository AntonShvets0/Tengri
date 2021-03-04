using System;
using System.Collections.Generic;
using System.Linq;
using TengriLang.Language.Model;
using TengriLang.Language.Model.AST;
using TengriLang.Language.Model.Lexeme;
using TengriLang.Reader;

namespace TengriLang.Language
{
    public class TreeBuilder
    {
        private TreeReader _reader;
        private string _file;
        private List<TreeElement> _tokens;
        public string AvailableAttributes = "override private public protected";
        public string ClassName;
        
        public TreeBuilder(string file, List<TreeElement> elements)
        {
            _reader = new TreeReader(elements);
            _file = file;
        }

        public List<TreeElement> GetTree()
        {
            _tokens = new List<TreeElement>();

            while (!_reader.IsEmpty())
            {
                var token = GetTreeElement(_reader.Read());
                if (token == null) break;
                _tokens.Add(token);
            }

            return _tokens;
        }

        private TreeElement GetTreeElement(TreeElement element)
        {
            _reader.Next();

            if (element is ILexeme lexeme)
            {
                var response = lexeme.ParseElement(this, _reader);
                if (response == null) return GetTreeElement(_reader.Read());
                
                return response;
            }

            return element;
        }

        public List<TreeElement> ParseToNewLine()
        {
            var openBracketsFunc = 0;
            var openBracketsClass = 0;
            var openBracketsArray = 0;
            
            var elements = _reader.ReadWhile(element =>
            {
                if (element is SpecialLexeme specialLexeme)
                {
                    if (specialLexeme.Value == "(") openBracketsFunc++;
                    else if (specialLexeme.Value == "{") openBracketsClass++;
                    else if (specialLexeme.Value == "}") openBracketsClass--;
                    else if (specialLexeme.Value == "[") openBracketsArray++;
                    else if (specialLexeme.Value == "]") openBracketsArray--;
                    else if (specialLexeme.Value == ")") openBracketsFunc--;
                }
                if (
                    (element is NewLineLexeme || element is SpecialLexeme special && special.Value == ",")
                    && openBracketsClass < 1 && openBracketsFunc < 1 && openBracketsArray < 1) return false;
                return element;
            });
            
            return new TreeBuilder(_file, elements).GetTree();
        }

        public List<AttributeElement> ParseAttributes()
        {
            var list = new List<AttributeElement>();

            while (_tokens.Count > 0 && _tokens[_tokens.Count - 1] is AttributeElement attributeElement)
            {
                list.Add(attributeElement);
                _tokens.RemoveAt(_tokens.Count - 1);
            }
            
            return list;
        }

        public FuncType GetType(string value, List<AttributeElement> attributeElement)
        {
            if (attributeElement.Any(e => e.AttributeName == "private")) return FuncType.Private;
            if (attributeElement.Any(e => e.AttributeName == "protected")) return FuncType.Protected;
            if (attributeElement.Any(e => e.AttributeName == "public")) return FuncType.Public;

            if (value.StartsWith("_"))
            {
                return FuncType.Private;
            } 
            
            if (value.StartsWith("protected"))
            {
                return FuncType.Protected;
            }

            return FuncType.Public;
        }
        
        public List<List<TreeElement>> ParseInBrackets(char startBracket, char endBracket, char delimiter = '\0')
        {
            var openedBrackets = 0;

            var list = new List<List<TreeElement>>()
            {
                new List<TreeElement>()
            };
            
            _reader.ReadWhile(element =>
            {
                if (element is SpecialLexeme special)
                {
                    if (special.Value == startBracket.ToString())
                    {
                        openedBrackets++;
                    }
                    else if (special.Value == endBracket.ToString())
                    {
                        if (openedBrackets == 0)
                        {
                            return false;
                        }

                        openedBrackets--;
                    }
                    else if (special.Value == delimiter.ToString())
                    {
                        list.Add(new List<TreeElement>());
                        return true;
                    }
                }

                list[list.Count - 1].Add(element);
                return true;
            });

            var newElements = new List<List<TreeElement>>();

            foreach (var element in list)
            {
                newElements.Add(new TreeBuilder(_file, element).GetTree());
            }

            return newElements;
        }
    }
}