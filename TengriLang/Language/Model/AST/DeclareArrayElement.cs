using System.Collections.Generic;
using TengriLang.Language.Model.Lexeme;
using TengriLang.Language.System;
using TengriLang.Reader;

namespace TengriLang.Language.Model.AST
{
    public class DeclareArrayElement : TreeElement, IElement
    {
        public Dictionary<string, List<TreeElement>> Array;

        public DeclareArrayElement(TreeElement parent, Dictionary<string, List<TreeElement>> array) : base(parent.File, parent.Position,
            parent.Line, parent.CharIndex)
        {
            Array = array;
        }
        
        public string ParseCode(Translator translator, TreeReader reader)
        {
            var code = "new TengriArray(new Dictionary<dynamic, dynamic> {";
            var elements = new List<string>();

            foreach (var arrItem in Array)
            {
                elements.Add($"{{ \"{arrItem.Key.Replace("\"", "\\\"")}\", {translator.Emulate(arrItem.Value, false)} }}");
            }

            return code + $"{string.Join(",", elements)}}})";
        }
    }
}