namespace System;

public static class Memoization
{
    public static Func<TP1, TR> Wrap<TP1, TR>(Func<TP1, TR> func)
        where TP1 : notnull
    {
        var cache = new Dictionary<TP1, TR>();
        return (p1) => cache.GetOrCreate(p1, () => func(p1));
    }

    public static Func<TP1, TP2, TR> Wrap<TP1, TP2, TR>(Func<TP1, TP2, TR> func)
    {
        var cache = new Dictionary<(TP1, TP2), TR>();
        return (p1, p2) => cache.GetOrCreate((p1, p2), () => func(p1, p2));
    }

    public static Func<TP1, TP2, TP3, TR> Wrap<TP1, TP2, TP3, TR>(Func<TP1, TP2, TP3, TR> func)
    {
        var cache = new Dictionary<(TP1, TP2, TP3), TR>();
        return (p1, p2, p3) => cache.GetOrCreate((p1, p2, p3), () => func(p1, p2, p3));
    }
}
