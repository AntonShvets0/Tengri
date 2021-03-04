using System.Collections.Generic;
using TengriLang.Language.Model;

namespace TengriLang.Reader
{
    public class TreeReader : AbstractReader<List<TreeElement>, TreeElement>
    {
        public TreeReader(List<TreeElement> parts):  base(parts) {}
        
        protected override List<TreeElement> AddToT(object part, List<TreeElement> list)
        {
            if (part is TreeElement) list.Add((TreeElement)part);
            if (part is List<TreeElement>) list.AddRange((List<TreeElement>)part);
            return list;
        }

        protected override List<TreeElement> CreateT()
            => new List<TreeElement>();

        public override TreeElement Read(int offset = 0)
            => IsEmpty(offset) ? null : Content[Position + offset];

        public override bool IsEmpty(int offset = 0)
            => Position + offset < 0 || Position + offset > Content.Count - 1;
    }
}