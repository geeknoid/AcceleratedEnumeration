using System.Runtime.CompilerServices;

namespace Bench;

public static partial class AcceleratedEnumerationExtensions
{
    public readonly struct SetWrapper<T>
        where T : notnull
    {
        private readonly ISet<T>? _set;

        public SetWrapper(ISet<T>? set)
        {
            _set = set;
        }

        public SetEnumerator<T> GetEnumerator() => new(_set);
    }

    public struct SetEnumerator<T> : IDisposable
        where T : notnull
    {
        private HashSet<T>.Enumerator _setEnumerator;
        private readonly IEnumerator<T>? _delegatedEnumerator;

        public SetEnumerator(ISet<T>? set)
        {
            if (set == null || set.Count == 0)
            {
                _setEnumerator = default;
                _delegatedEnumerator = EmptyEnumerator<T>.Instance;
            }
            else if (set is HashSet<T> s)
            {
                _setEnumerator = s.GetEnumerator();
                _delegatedEnumerator = null;
            }
            else
            {
                _setEnumerator = default;
                _delegatedEnumerator = set.GetEnumerator();
            }
        }

        public SetEnumerator(IReadOnlySet<T>? set)
        {
            if (set == null || set.Count == 0)
            {
                _setEnumerator = default;
                _delegatedEnumerator = EmptyEnumerator<T>.Instance;
            }
            else if (set is HashSet<T> s)
            {
                _setEnumerator = s.GetEnumerator();
                _delegatedEnumerator = null;
            }
            else
            {
                _setEnumerator = default;
                _delegatedEnumerator = set.GetEnumerator();
            }
        }

        public T Current
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _delegatedEnumerator != null ? _delegatedEnumerator.Current : _setEnumerator.Current;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool MoveNext() => _delegatedEnumerator != null ? _delegatedEnumerator.MoveNext() : _setEnumerator.MoveNext();

        public void Dispose() => _delegatedEnumerator?.Dispose();
    }
}
