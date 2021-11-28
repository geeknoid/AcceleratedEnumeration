using System.Collections;

namespace Bench;

public static class AcceleratedEnumerationExtensions
{
    public static EnumerableWrapper<T> AcceleratedEnum<T>(this IEnumerable<T>? enumerable) => new(enumerable);
    public static ReadOnlyListWrapper<T> AcceleratedEnum<T>(this IReadOnlyList<T>? list) => new(list);
    public static ListWrapper<T> AcceleratedEnum<T>(this IList<T>? list) => new(list);
    public static DictionaryWrapper<TKey, TValue> AcceleratedEnum<TKey, TValue>(this IDictionary<TKey, TValue>? dict) where TKey : notnull => new(dict);

    public readonly struct EnumerableWrapper<T>
    {
        private readonly IEnumerable<T>? _enumerable;

        public EnumerableWrapper(IEnumerable<T>? enumerable)
        {
            _enumerable = enumerable;
        }

        public EnumerableEnumerator<T> GetEnumerator() => new(_enumerable);
    }

    public readonly struct ReadOnlyListWrapper<T>
    {
        private readonly IReadOnlyList<T>? _list;

        public ReadOnlyListWrapper(IReadOnlyList<T>? list)
        {
            _list = list;
        }

        public ReadOnlyListEnumerator<T> GetEnumerator() => new(_list);
    }

    public readonly struct ListWrapper<T>
    {
        private readonly IList<T>? _list;

        public ListWrapper(IList<T>? list)
        {
            _list = list;
        }

        public ListEnumerator<T> GetEnumerator() => new(_list);
    }

    public readonly struct DictionaryWrapper<TKey, TValue>
        where TKey : notnull
    {
        private readonly IDictionary<TKey, TValue>? _dict;

        public DictionaryWrapper(IDictionary<TKey, TValue>? dict)
        {
            _dict = dict;
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => _dict != null && _dict.Count != 0 ? _dict.GetEnumerator() : EmptyEnumerator<KeyValuePair<TKey, TValue>>.Instance;
    }

    public struct EnumerableEnumerator<T> : IEnumerator<T>
    {
        private enum Mode
        {
            DelegatedEnumerator,
            ReadOnlyList,
            List,
            Array,
            Empty,
        }

        private readonly Mode _mode;
        private readonly IEnumerator<T>? _delegatedEnumerator;
        private readonly IReadOnlyList<T>? _readonlyList;
        private readonly IList<T>? _list;
        private readonly T[]? _array;
        private readonly int _limit;
        private int _index;

        public EnumerableEnumerator(IEnumerable<T>? enumerable)
        {
            _index = -1;
            switch (enumerable)
            {
                case T[] arr:
                    _mode = Mode.Array;
                    _delegatedEnumerator = null;
                    _readonlyList = null;
                    _list = null;
                    _array = arr;
                    _limit = arr.Length - 1;

                    if (_limit < 0)
                    {
                        _mode = Mode.Empty;
                    }
                    break;

                case IReadOnlyList<T> rol:
                    _mode = Mode.ReadOnlyList;
                    _delegatedEnumerator = null;
                    _readonlyList = rol;
                    _list = null;
                    _array = null;
                    _limit = rol.Count - 1;

                    if (_limit < 0)
                    {
                        _mode = Mode.Empty;
                    }
                    break;

                case IList<T> l:
                    _mode = Mode.List;
                    _delegatedEnumerator = null;
                    _readonlyList = null;
                    _list = l;
                    _array = null;
                    _limit = l.Count - 1;

                    if (_limit < 0)
                    {
                        _mode = Mode.Empty;
                    }
                    break;

                case IReadOnlyCollection<T> readonlyCol when readonlyCol.Count == 0:
                case ICollection<T> col when col.Count == 0:
                    _mode = Mode.Empty;
                    _delegatedEnumerator = EmptyEnumerator<T>.Instance;
                    _readonlyList = null;
                    _list = null;
                    _array = null;
                    _limit = -1;
                    break;

                default:
                    _mode = Mode.DelegatedEnumerator;
                    _delegatedEnumerator = enumerable?.GetEnumerator() ?? EmptyEnumerator<T>.Instance;
                    _readonlyList = null;
                    _list = null;
                    _array = null;
                    _limit = -1;
                    break;
            }
        }

        public void Reset()
        {
            if (_delegatedEnumerator == null)
            {
                _index = -1;
            }
            else
            {
                _delegatedEnumerator.Reset();
            }
        }

        public T Current => _mode switch
        {
            Mode.ReadOnlyList => _readonlyList![_index],
            Mode.List => _list![_index],
            Mode.Array => _array![_index],
            _ => _delegatedEnumerator!.Current,
        };

        public bool MoveNext()
        {
            if (_index < _limit)
            {
                _index++;
                return true;
            }

            return _delegatedEnumerator != null && _delegatedEnumerator.MoveNext();
        }

        object IEnumerator.Current => Current!;

        public void Dispose()
        {
            // nop
        }
    }

    public struct ReadOnlyListEnumerator<T> : IEnumerator<T>
    {
        private readonly IReadOnlyList<T>? _list;
        private readonly int _limit;
        private int _index;

        public ReadOnlyListEnumerator(IReadOnlyList<T>? list)
        {
            _list = list;
            _index = -1;
            _limit = list != null ? list.Count - 1 : -1;
        }

        public void Reset() => _index = -1;
        public T Current => _list![_index];
        object IEnumerator.Current => Current!;

        public bool MoveNext()
        {
            if (_index < _limit)
            {
                _index++;
                return true;
            }

            return false;
        }

        public void Dispose()
        {
            // nop
        }
    }

    public struct ListEnumerator<T> : IEnumerator<T>
    {
        private readonly IList<T>? _list;
        private readonly int _limit;
        private int _index;

        public ListEnumerator(IList<T>? list)
        {
            _list = list;
            _index = -1;
            _limit = list != null ? list.Count - 1 : -1;
        }

        public void Reset() => _index = -1;
        public T Current => _list![_index];
        object IEnumerator.Current => Current!;

        public bool MoveNext()
        {
            if (_index < _limit)
            {
                _index++;
                return true;
            }

            return false;
        }

        public void Dispose()
        {
            // nop
        }
    }
}
