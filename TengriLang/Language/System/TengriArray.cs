using System.Collections.Generic;
using System.Linq;

namespace TengriLang.Language.System
{
    public class TengriArray
    {
        private Dictionary<dynamic, dynamic> _values;
        public int TENGRI_length => _values.Count;

        public TengriArray(Dictionary<dynamic, dynamic> values)
        {
            _values = values;
        }

        public IEnumerable<TengriField> TENGRI_invoke()
        {
            foreach (var value in _values)
            {
                yield return new TengriField
                {
                    TENGRI_key = value.Key,
                    TENGRI_value = value.Value
                };
            }
        }

        public dynamic this[dynamic key]
        {
            get => _values.ContainsKey(key) ? _values[key] : null;
            set
            {
                if (_values.ContainsKey(key))
                {
                    _values[key] = value;
                    return;
                }
            
                _values.Add(key, value);
            }
        }
    }
}