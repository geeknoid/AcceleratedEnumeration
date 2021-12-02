using System.Runtime.CompilerServices;

namespace Bench;

public static partial class AcceleratedEnumerationExtensions
{
    public readonly ref struct DictionaryWrapper<TKey, TValue>
        where TKey : notnull
    {
        private readonly IDictionary<TKey, TValue>? _dict;

        public DictionaryWrapper(IDictionary<TKey, TValue>? dict)
        {
            _dict = dict;
        }

        public DictionaryEnumerator<TKey, TValue> GetEnumerator() => new(_dict);
    }

    public ref struct DictionaryEnumerator<TKey, TValue>
        where TKey : notnull
    {
        private Dictionary<TKey, TValue>.Enumerator _dictEnumerator;
        private readonly IEnumerator<KeyValuePair<TKey, TValue>>? _delegatedEnumerator;

        public DictionaryEnumerator(IDictionary<TKey, TValue>? dict)
        {
            if (dict == null || dict.Count == 0)
            {
                _dictEnumerator = default;
                _delegatedEnumerator = EmptyEnumerator<KeyValuePair<TKey, TValue>>.Instance;
            }
            else if (dict is Dictionary<TKey, TValue> d)
            {
                _dictEnumerator = d.GetEnumerator();
                _delegatedEnumerator = null;
            }
            else
            {
                _dictEnumerator = default;
                _delegatedEnumerator = dict.GetEnumerator();
            }
        }

        public DictionaryEnumerator(IReadOnlyDictionary<TKey, TValue>? dict)
        {
            if (dict == null || dict.Count == 0)
            {
                _dictEnumerator = default;
                _delegatedEnumerator = EmptyEnumerator<KeyValuePair<TKey, TValue>>.Instance;
            }
            else if (dict is Dictionary<TKey, TValue> d)
            {
                _dictEnumerator = d.GetEnumerator();
                _delegatedEnumerator = null;
            }
            else
            {
                _dictEnumerator = default;
                _delegatedEnumerator = dict.GetEnumerator();
            }
        }

        public KeyValuePair<TKey, TValue> Current
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _delegatedEnumerator != null ? _delegatedEnumerator.Current : _dictEnumerator.Current;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool MoveNext() => _delegatedEnumerator != null ? _delegatedEnumerator.MoveNext() : _dictEnumerator.MoveNext();

        public void Dispose() => _delegatedEnumerator?.Dispose();
    }
}
