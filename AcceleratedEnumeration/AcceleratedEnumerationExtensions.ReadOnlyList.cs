using System.Runtime.CompilerServices;

namespace Bench;

public static partial class AcceleratedEnumerationExtensions
{
    public readonly ref struct ReadOnlyListWrapper<T>
    {
        private readonly IReadOnlyList<T>? _list;

        public ReadOnlyListWrapper(IReadOnlyList<T>? list)
        {
            _list = list;
        }

        public ReadOnlyListEnumerator<T> GetEnumerator() => new(_list);
    }

    public ref struct ReadOnlyListEnumerator<T>
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

        public T Current
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _list![_index];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool MoveNext()
        {
            if (_index < _limit)
            {
                _index++;
                return true;
            }

            return false;
        }
    }
}
