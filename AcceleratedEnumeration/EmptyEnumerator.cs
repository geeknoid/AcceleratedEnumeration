using System.Collections;

namespace Bench;

internal sealed class EmptyEnumerator<T> : IEnumerator<T>
{
    public static readonly EmptyEnumerator<T> Instance = new();
    public T Current => throw new NotSupportedException();
    object IEnumerator.Current => Current!;
    public bool MoveNext() => false;

    public void Dispose()
    {
        // nop
    }

    public void Reset()
    {
        // nop
    }
}
