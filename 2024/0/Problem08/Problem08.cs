namespace A2024.Problem08;

public static class Solver
{
    [GeneratedTest<int>(14, 240)]
    public static int RunA(string[] lines)
        => Run(lines, 1, false);

    [GeneratedTest<int>(34, 955)]
    public static int RunB(string[] lines)
        => Run(lines, -1, true);

    static int Run(string[] lines, int maxSteps, bool includeSelf)
    {
        var map = MapData.ParseMap(lines, a => $"{a}");

        return map.Enumerate().Where(a => a.Item != ".").Select(a => a.Item).Distinct()
            .SelectMany(x => map.EnumeratePositionsOf(x).ToArray().EnumeratePairs())
            .SelectMany(b => Inspect(map, b.First, b.Second, maxSteps, includeSelf))
            .Distinct()
            .Count();
    }

    static IEnumerable<Pos> Inspect(string[,] map, Pos a, Pos b, int maxSteps, bool includeSelf)
    {
        var diff = a - b;

        (Pos Center, Pos Diff)[] variants = [(a, diff), (a, -diff), (b, diff), (b, -diff)];

        var result = variants
            .SelectMany(x => Ray(map, x.Center, x.Diff, maxSteps));

        if (!includeSelf)
            result = result.Where(x => x != a && x != b);

        return result;
    }

    static IEnumerable<Pos> Ray(string[,] map, Pos start, Pos diff, int maxSteps)
    {
        var p = start + diff;
        for (var step = 0; (maxSteps == -1 || step < maxSteps) && map.IsInBounds(p); ++step)
        {
            yield return p;
            p += diff;
        }
    }
}
