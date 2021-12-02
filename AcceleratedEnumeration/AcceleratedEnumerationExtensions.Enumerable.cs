using System.Runtime.CompilerServices;

namespace Bench;

public static partial class AcceleratedEnumerationExtensions
{
    public readonly struct EnumerableWrapper<T>
    {
        private readonly IEnumerable<T>? _enumerable;

        public EnumerableWrapper(IEnumerable<T>? enumerable)
        {
            _enumerable = enumerable;
        }

        public EnumerableEnumerator<T> GetEnumerator() => new(_enumerable);
    }

    public struct EnumerableEnumerator<T> : IDisposable
    {
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
                    _delegatedEnumerator = null;
                    _readonlyList = null;
                    _list = null;
                    _array = arr;
                    _limit = arr.Length - 1;
                    break;

                case IReadOnlyList<T> rol:
                    _delegatedEnumerator = null;
                    _readonlyList = rol;
                    _list = null;
                    _array = null;
                    _limit = rol.Count - 1;
                    break;

                case IList<T> l:
                    _delegatedEnumerator = null;
                    _readonlyList = null;
                    _list = l;
                    _array = null;
                    _limit = l.Count - 1;
                    break;

                case IReadOnlyCollection<T> readonlyCol when readonlyCol.Count == 0:
                case ICollection<T> col when col.Count == 0:
                    _delegatedEnumerator = EmptyEnumerator<T>.Instance;
                    _readonlyList = null;
                    _list = null;
                    _array = null;
                    _limit = -1;
                    break;

                default:
                    _delegatedEnumerator = enumerable?.GetEnumerator();
                    _readonlyList = null;
                    _list = null;
                    _array = null;
                    _limit = -1;
                    break;
            }
        }

        public T Current
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                if (_readonlyList != null)
                {
                    return _readonlyList[_index];
                }

                if (_list != null)
                {
                    return _list[_index];
                }

                if (_array != null)
                {
                    return _array[_index];
                }

                return _delegatedEnumerator!.Current;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool MoveNext()
        {
            if (_index < _limit)
            {
                _index++;
                return true;
            }

            return _delegatedEnumerator != null && _delegatedEnumerator.MoveNext();
        }

        public void Dispose() => _delegatedEnumerator?.Dispose();
    }
}
