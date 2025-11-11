namespace System.Collections.Generic;

public static class ListExtensions
{
    extension<T>(List<T> list)
    {
        public void RemoveRange(IEnumerable<T> items)
        {
            foreach (var item in items)
                list.Remove(item);
        }

        // to make it possible to add elements to a list during enumeration
        public IEnumerable<T> Enumerate()
        {
            for (var i = 0; i < list.Count; ++i)
                yield return list[i];
        }
    }
}
