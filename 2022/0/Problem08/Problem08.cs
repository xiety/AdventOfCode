namespace A2022.Problem08;

public static class Solver
{
    [GeneratedTest<long>(21, 1845)]
    public static long RunA(string[] lines)
    {
        var map = MapData.ParseMap(lines);

        return map.EnumeratePositions()
            .Count(a => IsVisibleTree(map, a));
    }

    [GeneratedTest<long>(8, 230112)]
    public static long RunB(string[] lines)
    {
        var map = MapData.ParseMap(lines);

        return map.EnumeratePositions()
            .Max(a => Scenic(map, a));
    }

    static bool IsVisibleTree(int[,] map, Pos p)
        => ArrayEx.Offsets
               .Select(a => IsVisibleTreeDirection(map, p, a))
               .FirstOrDefault(a => a, false);

    static int Scenic(int[,] map, Pos p)
        => ArrayEx.Offsets
               .Select(a => CalculateScenicDirection(map, p, a)).Mul();

    static int CalculateScenicDirection(int[,] map, Pos treePos, Pos delta)
    {
        var visible = IsVisibleTreeDirection(map, treePos, delta);

        if (visible)
        {
            return EnumerateDirection(map, treePos, delta)
                .Count();
        }

        var tree = map.Get(treePos);

        return EnumerateDirection(map, treePos, delta)
            .Select(map.Get)
            .TakeWhile(a => a < tree)
            .Count() + 1;
    }

    static bool IsVisibleTreeDirection(int[,] map, Pos treePos, Pos delta)
    {
        var tree = map.Get(treePos);

        return !EnumerateDirection(map, treePos, delta)
            .Select(map.Get)
            .Any(a => a >= tree);
    }

    static IEnumerable<Pos> EnumerateDirection<T>(T[,] array, Pos p, Pos d)
    {
        p += d;

        while (array.IsInBounds(p))
        {
            yield return p;
            p += d;
        }
    }
}
