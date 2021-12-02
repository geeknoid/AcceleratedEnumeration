namespace Bench;

public static partial class AcceleratedEnumerationExtensions
{
    public readonly ref struct ReadOnlyDictionaryWrapper<TKey, TValue>
        where TKey : notnull
    {
        private readonly IReadOnlyDictionary<TKey, TValue>? _dict;

        public ReadOnlyDictionaryWrapper(IReadOnlyDictionary<TKey, TValue>? dict)
        {
            _dict = dict;
        }

        public DictionaryEnumerator<TKey, TValue> GetEnumerator() => new(_dict);
    }
}
