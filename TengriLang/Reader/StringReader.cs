namespace TengriLang.Reader
{
    public class StringReader : AbstractReader<string, char>
    {
        public int Line { get; protected set; }
        public int CharIndex { get; protected set; }
        
        protected override string CreateT() 
            => "";
        
        protected override string AddToT(object part, string list)
            => list += part;
        
        public override char Read(int offset = 0) 
            => IsEmpty(offset) ? '\0' : Content[Position + offset];

        public override bool IsEmpty(int offset = 0)
            => Position + offset < 0 || Position + offset > Content.Length - 1;

        public override void Next(int count = 1)
        {
            base.Next(count);
            
            if (Read() == '\n')
            {
                Line++;
                CharIndex = 0;
            }
            else
            {
                CharIndex++;
            }
        }

        public StringReader(string content) : base(content) {}
    }
}