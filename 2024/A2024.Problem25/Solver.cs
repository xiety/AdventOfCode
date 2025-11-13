using Advent.Common;

namespace A2024.Problem25;

public class Solver : ISolver<long>
{
    public long RunA(string[] lines, bool isSample)
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
    {
        foreach (var item in items)
            if (item.GetRow(0).All(b => b == 1))
                yield return item.GetColumns().Select(a => Array.IndexOf(a, 0) - 1).ToArray();
    }

    static IEnumerable<int[]> ParseKeys(int[][,] items)
    {
        foreach (var item in items)
            if (item.GetRow(item.Height - 1).All(b => b == 1))
                yield return item.GetColumns().Select(a => a.Length - Array.IndexOf(a, 1) - 1).ToArray();
    }

    static int[][,] LoadData(string[] lines)
        => lines.SplitBy(String.Empty).Select(a => MapData.ParseMap(a, b => b == '#' ? 1 : 0)).ToArray();
}
