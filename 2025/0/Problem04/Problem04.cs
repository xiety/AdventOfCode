using Advent.Common;

namespace A2025.Problem04;

public static class Solver
{
    [GeneratedTest<int>(13, 1474)]
    public static int RunA(string[] lines)
        => Run(lines, true);

    [GeneratedTest<int>(43, 8910)]
    public static int RunB(string[] lines)
        => Run(lines, false);

    static int Run(string[] lines, bool limit)
    {
        var map = LoadData(lines);
        return Enumerable.InfiniteSequence(0, 1)
            .TakeWhile(a => !limit || a < 1)
            .Select(_ => map.EnumeratePositionsOf(true)
                .Where(a => map.Delted(a).Count(map.Get) < 4)
                .ToArray()
                .Count(a => map.Set(a, false)))
            .TakeWhile(a => a > 0)
            .Sum();
    }

    static bool[,] LoadData(string[] lines)
        => MapData.ParseMap(lines, a => a == '@');
}
