namespace System;

public static class Memoization
{
    //public static Func<TP1, TR> Wrap<TP1, TR>(Func<TP1, TR> func)
    //    where TP1 : notnull
    //{
    //    var cache = new Dictionary<TP1, TR>();
    //    return (p1) => cache.GetOrCreate(p1, () => func(p1));
    //}

    //public static Func<TP1, TP2, TR> Wrap<TP1, TP2, TR>(Func<TP1, TP2, TR> func)
    //{
    //    var cache = new Dictionary<(TP1, TP2), TR>();
    //    return (p1, p2) => cache.GetOrCreate((p1, p2), () => func(p1, p2));
    //}

    //public static Func<TP1, TP2, TP3, TR> Wrap<TP1, TP2, TP3, TR>(Func<TP1, TP2, TP3, TR> func)
    //{
    //    var cache = new Dictionary<(TP1, TP2, TP3), TR>();
    //    return (p1, p2, p3) => cache.GetOrCreate((p1, p2, p3), () => func(p1, p2, p3));
    //}

    public static Func<T, TR> WrapRecursive<T, TR>(Func<Func<T, TR>, T, TR> self)
        where T : notnull
    {
        var cache = new Dictionary<T, TR>();
        Func<T, TR> memoized = null!;
        memoized = arg => cache.GetOrCreate(arg, () => self(memoized, arg));
        return memoized;
    }

    public static Func<T1, T2, TR> WrapRecursive<T1, T2, TR>(Func<Func<T1, T2, TR>, T1, T2, TR> self)
    {
        var cache = new Dictionary<(T1, T2), TR>();
        Func<T1, T2, TR> memoized = null!;
        memoized = (p1, p2) => cache.GetOrCreate((p1, p2), () => self(memoized, p1, p2));
        return memoized;
    }

    public static TR RunRecursive<T, TR>(T p1, Func<Func<T, TR>, T, TR> self)
        where T : notnull
        => WrapRecursive(self)(p1);

    public static TR RunRecursive<T1, T2, TR>(T1 p1, T2 p2, Func<Func<T1, T2, TR>, T1, T2, TR> self)
        => WrapRecursive(self)(p1, p2);
}
