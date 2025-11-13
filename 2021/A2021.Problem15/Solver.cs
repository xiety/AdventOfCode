using Advent.Common;

namespace A2021.Problem15;

public class Solver : IProblemSolver<long>
{
    public long RunA(string filename)
    {
        var map = MapData.ParseMap(File.ReadAllLines(filename));

        var path = PathFinder.Find(map, new(0, 0), new(map.Width - 1, map.Height - 1))
            ?? throw new();

        var result = path.Select(map.Get).Sum();

        return result;
    }

    public long RunB(string filename)
    {
        var map = MapData.ParseMap(File.ReadAllLines(filename));

        var bigMap = CreateBigMap(map);

        var path = PathFinder.Find(bigMap, new(0, 0), new(bigMap.Width - 1, bigMap.Height - 1))
            ?? throw new();

        var result = path.Select(bigMap.Get).Sum();

        return result;
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
