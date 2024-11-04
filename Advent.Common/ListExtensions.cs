namespace System.Collections.Generic;

public static class ListExtensions
{
    public static void RemoveRange<T>(this List<T> list, IEnumerable<T> items)
    {
        foreach (var item in items)
            list.Remove(item);
    }

    // to make it possible to add elements to a list during enumeration
    public static IEnumerable<T> Enumerate<T>(this List<T> list)
    {
        for (var i = 0; i < list.Count; ++i)
            yield return list[i];
    }
}
