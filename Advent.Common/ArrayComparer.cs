namespace Advent.Common;

public class ArrayComparer<T> : IComparer<T[]>
    where T : IComparable
{
    //IComparable<T> constraint doesn't work with Enums

    public int Compare(T[]? x, T[]? y)
    {
        if (x == null || y == null || x.Length != y.Length)
            return -1;

        for (var i = 0; i < x.Length; ++i)
        {
            var r = x[i].CompareTo(y[i]);

            if (r != 0)
                return r;
        }

        return 0;
    }
}
