namespace A2020.Problem10;

public static class Solver
{
    [GeneratedTest<long>(220, 1848)]
    public static long RunA(string[] lines)
        => LoadItems(lines)
            .Chain()
            .Select(a => a.Second - a.First)
            .GroupBy(a => a)
            .Where(g => g.Key is (1 or 3))
            .Aggregate(1L, (acc, a) => acc * a.Count());

    [GeneratedTest<long>(19208, 8099130339328)]
    public static long RunB(string[] lines)
    {
        var items = LoadItems(lines);

        var steps = new long[items.Length];
        steps[0] = 1;

        foreach (var i in items.Length - 1)
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
