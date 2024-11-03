namespace System.Collections.Generic;

public static class ListExtensions
{
    public static void RemoveRange<T>(this List<T> list, IEnumerable<T> items)
    {
        foreach (var item in items)
            list.Remove(item);
    }
}
