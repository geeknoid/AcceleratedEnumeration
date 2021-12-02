namespace Bench;

public static partial class AcceleratedEnumerationExtensions
{
    public readonly struct ReadOnlySetWrapper<T>
        where T : notnull
    {
        private readonly IReadOnlySet<T>? _set;

        public ReadOnlySetWrapper(IReadOnlySet<T>? set)
        {
            _set = set;
        }

        public SetEnumerator<T> GetEnumerator() => new(_set);
    }
}
