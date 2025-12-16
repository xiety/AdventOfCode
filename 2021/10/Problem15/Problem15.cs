namespace A2021.Problem15;

public static class Solver
{
    [GeneratedTest<long>(40, 415)]
    public static long RunA(string[] lines)
    {
        var map = MapData.ParseMap(lines);

        var path = PathFinder.Find(map, new(0, 0), new(map.Width - 1, map.Height - 1))
            ?? throw new();

        return path.Sum(map.Get);
    }

    [GeneratedTest<long>(315, 2864)]
    public static long RunB(string[] lines)
    {
        var map = MapData.ParseMap(lines);

        var bigMap = CreateBigMap(map);

        var path = PathFinder.Find(bigMap, new(0, 0), new(bigMap.Width - 1, bigMap.Height - 1))
            ?? throw new();

        return path.Sum(bigMap.Get);
    }

    static int[,] CreateBigMap(int[,] map)
    {
        var width = map.Width;
        var height = map.Height;

        var bigMap = new int[width * 5, height * 5];

        foreach (var (dy, dx, y, x) in Fors.For((0, 5), (0, 5), (0, height), (0, width)))
            bigMap[x + dx * width, y + dy * width] = Rotate(map[x, y], dx + dy, 10);

        return bigMap;
    }

    static int Rotate(int n, int d, int max)
        => ((n + d - 1) % (max - 1)) + 1;
}
