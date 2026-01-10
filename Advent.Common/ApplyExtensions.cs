namespace System;

public static class ApplyExtensions
{
    extension<T>(T obj)
    {
        public TR Apply<TR>(Func<T, TR> func)
            => func(obj);

        public TR? ApplyIfNotNull<TR>(Func<T, TR> func)
            => obj is null ? default : func(obj);
    }

    extension<T>(T? obj)
        where T : struct
    {
        public TR? ApplyIfNotNull<TR>(Func<T, TR> func)
            => !obj.HasValue ? default : func(obj.Value);
    }

    extension<T1, T2>((T1, T2) t)
    {
        public TR Apply<TR>(Func<T1, T2, TR> func)
            => func(t.Item1, t.Item2);
    }

    extension<T1, T2, T3>((T1, T2, T3) t)
    {
        public TR Apply<TR>(Func<T1, T2, T3, TR> func)
            => func(t.Item1, t.Item2, t.Item3);
    }
}
