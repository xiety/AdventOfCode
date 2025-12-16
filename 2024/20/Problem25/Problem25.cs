using Advent.Common;

using System.Linq;

namespace A2024.Problem25;

public static class Solver
{
    [GeneratedTest<long>(3, 3356)]
    public static long RunA(string[] lines)
    {
        var items = LoadData(lines);
        var locks = ParseLocks(items).ToArray();
        var keys = ParseKeys(items).ToArray();
        var height = items[0].Height - 2;

        var query = from @lock in locks
                    from key in keys
                    where Check(@lock, key, height)
                    select 1;

        return query.Count();
    }

    static bool Check(int[] @lock, int[] key, int height)
        => @lock.Zip(key).All(a => a.First + a.Second <= height);

    static IEnumerable<int[]> ParseLocks(int[][,] items)
        => from item in items
            where item.GetRow(0).All(b => b == 1)
            select item.GetColumns().ToArray(a => Array.IndexOf(a, 0) - 1);

    static IEnumerable<int[]> ParseKeys(int[][,] items)
        => from item in items
            where item.GetRow(item.Height - 1).All(b => b == 1)
            select item.GetColumns().ToArray(a => a.Length - Array.IndexOf(a, 1) - 1);

    static int[][,] LoadData(string[] lines)
        => lines.SplitBy(String.Empty).ToArray(a => MapData.ParseMap(a, b => b == '#' ? 1 : 0));
}
