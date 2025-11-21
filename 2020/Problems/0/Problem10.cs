using Advent.Common;

namespace A2020.Problem10;

public class Solver : ISolver<long>
{
    public long RunA(string[] lines, bool isSample)
    {
        var items = LoadItems(lines);

        return items
            .Chain()
            .Select(a => a.Second - a.First)
            .GroupBy(a => a)
            .Where(g => g.Key is (1 or 3))
            .Aggregate(1L, (acc, a) => acc * a.Count());
    }

    public long RunB(string[] lines, bool isSample)
    {
        var items = LoadItems(lines);

        var steps = new long[items.Length];
        steps[0] = 1;

        for (var i = 0; i < items.Length - 1; ++i)
            for (var j = i + 1; j <= i + 3 && j < items.Length && items[j] - items[i] <= 3; ++j)
                steps[j] += steps[i];

        return steps[^1];
    }

    static int[] LoadItems(string[] lines)
    {
        var items = lines.Select(int.Parse).Order().ToArray();
        return [0, .. items, items[^1] + 3];
    }
}
