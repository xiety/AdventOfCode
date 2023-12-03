namespace System;

public static class Memoization
{
    public static Func<TP, TR> Wrap<TP, TR>(Func<TP, TR> func)
        where TP : notnull
    {
        var cache = new Dictionary<TP, TR>();

        return p =>
        {
            if (cache.TryGetValue(p, out var r))
            {
                return r;
            }
            else
            {
                r = func(p);
                cache.Add(p, r);
                return r;
            }
        };
    }
}
