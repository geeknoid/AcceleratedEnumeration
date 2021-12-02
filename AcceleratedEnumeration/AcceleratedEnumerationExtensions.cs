namespace Bench;

public static partial class AcceleratedEnumerationExtensions
{
    public static EnumerableWrapper<T> AcceleratedEnum<T>(this IEnumerable<T>? enumerable) => new(enumerable);
    public static ReadOnlyListWrapper<T> AcceleratedEnum<T>(this IReadOnlyList<T>? list) => new(list);
    public static ListWrapper<T> AcceleratedEnum<T>(this IList<T>? list) => new(list);
    public static DictionaryWrapper<TKey, TValue> AcceleratedEnum<TKey, TValue>(this IDictionary<TKey, TValue>? dict) where TKey : notnull => new(dict);
    public static ReadOnlyDictionaryWrapper<TKey, TValue> AcceleratedEnum<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue>? dict) where TKey : notnull => new(dict);
    public static SetWrapper<T> AcceleratedEnum<T>(this ISet<T>? set) where T : notnull => new(set);
    public static ReadOnlySetWrapper<T> AcceleratedEnum<T>(this IReadOnlySet<T>? set) where T : notnull => new(set);
}
