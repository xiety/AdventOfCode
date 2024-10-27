namespace System.Collections.Generic;

public static class Linked
{
    public static Linked<T> Empty<T>()
        => new(default, true, null, 0);
}

public class Linked<T>(T? value, bool end, Linked<T>? next, int count) : IEnumerable<T>
{
    public T? Value = value;

    public bool End { get; } = end;

    public Linked<T>? Next { get; } = next;

    public int Count => count;

    public Linked<T> AddBefore(T value)
        => new(value, false, this, count + 1);

    public bool Contains(T search)
    {
        var p = this;

        while (!p.End)
        {
            if (p.Value!.Equals(search))
                return true;

            p = p.Next!;
        }

        return false;
    }

    public Linked<T> AddDistinct(T value)
    {
        return Contains(value)
            ? this
            : AddBefore(value);
    }

    public IEnumerator<T> GetEnumerator()
        => new Enumerator(this);

    IEnumerator IEnumerable.GetEnumerator()
        => new Enumerator(this);

    struct Enumerator(Linked<T> initial) : IEnumerator<T>
    {
        private Linked<T>? linked;

        public readonly T Current => linked!.Value!;

        readonly object IEnumerator.Current => linked!.Value!;

        private bool reseted = true;

        public readonly void Dispose()
        {
        }

        public bool MoveNext()
        {
            if (reseted)
            {
                reseted = false;
                linked = initial;
                return !linked.End;
            }
            else
            {
                if (linked?.Next == null)
                    return false; //WHY THIS IS HAPPENING FROM VISUALIZER???

                linked = linked!.Next;
                return !linked!.End;
            }
        }

        public void Reset()
            => reseted = true;
    }
}
