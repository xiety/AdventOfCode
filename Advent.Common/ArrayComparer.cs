namespace Advent.Common;

public class ArrayComparer<T> : IComparer<T[]>
    where T : IComparable
{
    //IComparable<TV> constraint doesn't work with Enums

    public int Compare(T[]? x, T[]? y)
    {
        if (x == null || y == null || x.Length != y.Length)
            return -1;

        return x.Select((t, i) => t.CompareTo(y[i])).FirstOrDefault(r => r != 0);
    }
}
