using System;
using TengriLang.Reader.Model;

namespace TengriLang.Reader
{
    /**
     * Main class of reader content (AST tree, string)
     */
    public abstract class AbstractReader<T, T2>
    {
        public int Position { get; protected set; }
        public T Content { get; }
        
        public delegate ReadWhileResponse ReadWhileDelegate(T2 element);

        public abstract bool IsEmpty(int offset = 0);
        public abstract T2 Read(int offset = 0);

        protected abstract T CreateT();
        protected abstract T AddToT(object part, T list);
        
        public AbstractReader(T data)
        {
            Content = data;
        }

        public virtual void Next(int count = 1)
        {
            Position += count;
        }

        public T ReadWhile(ReadWhileDelegate readWhileDelegate)
        {
            var list = CreateT();
            
            while (!IsEmpty())
            {
                var part = Read();
                if (part == null) break;
                
                var response = readWhileDelegate(part);

                if (response.Value != null)
                {
                    if (response.Value is T2 || response.Value is T) list = AddToT(response.Value, list);
                    else throw new InvalidCastException("ReadWhileDelegate returns wrong value");
                }
                
                if (!response.IsContinue) break;
                Next();
            }

            return list;
        }
    }
}