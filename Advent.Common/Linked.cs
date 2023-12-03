namespace System.Collections.Generic;

public static class Linked
{
    public static Linked<T> Empty<T>()
        => new(default, true, null, 0);
}

public class Linked<T>(T? value, bool end, Linked<T>? next, int count) : IEnumerable<T>
{
    public T? Value = value;

    private readonly bool end = end;
    private readonly Linked<T>? next = next;

    public bool End => end;
    public Linked<T>? Next => next;
    public int Count => count;

    public Linked<T> AddBefore(T value)
        => new(value, false, this, count + 1);

    public bool Contains(T search)
    {
        var p = this;

        while (!p.end)
        {
            if (p.Value!.Equals(search))
                return true;

            p = p.Next!;
        }

        return false;
    }

    public Linked<T> AddDistinct(T value)
    {
        if (Contains(value))
            return this;

        return AddBefore(value);
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
                return !linked.end;
            }
            else
            {
                if (linked == null || linked.next == null)
                    return false; //WHY THIS IS HAPPENING FROM VISUALIZER???

                linked = linked!.next;
                return !linked!.end;
            }
        }

        public void Reset()
            => reseted = true;
    }
}
