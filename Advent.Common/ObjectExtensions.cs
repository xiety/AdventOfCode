namespace System;

public static class ObjectExtensions
{
    extension<T>(T obj)
    {
        public TR Apply<TR>(Func<T, TR> func)
            => func(obj);
    }
}
