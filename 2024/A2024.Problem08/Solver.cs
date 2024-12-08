using Advent.Common;

namespace A2024.Problem08;

public class Solver : ISolver<int>
{
    public int RunA(string[] lines, bool isSample)
        => Run(lines, 1, false);

    public int RunB(string[] lines, bool isSample)
        => Run(lines, -1, true);

    static int Run(string[] lines, int maxSteps, bool includeSelf)
    {
        var map = MapData.ParseMap(lines, a => $"{a}");

        return map.Enumerate().Where(a => a.Item != ".").Select(a => a.Item).Distinct()
            .SelectMany(x => map.EnumeratePositionsOf(x).ToArray().EnumeratePairs())
            .SelectMany(b => Inspect(map, b.Item1, b.Item2, maxSteps, includeSelf))
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
        var step = 0;

        while ((maxSteps == -1 || step < maxSteps) && map.IsInBounds(p))
        {
            yield return p;
            p += diff;
            step++;
        }
    }
}
