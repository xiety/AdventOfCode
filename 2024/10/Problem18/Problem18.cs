using Advent.Common;

namespace A2024.Problem18;

public static class Solver
{
    [GeneratedTest<int>(22, 308)]
    public static int RunA(string[] lines, bool isSample)
    {
        var items = LoadData(lines);

        var size = isSample ? 7 : 71;
        var total = isSample ? 12 : 1024;

        var map = CreateMap(items, size, total);

        var path = PathFinder.Find(map, new(0, 0), new(size - 1, size - 1));

        return path is null ? throw new() : path.Length;
    }

    [GeneratedTest<string>("6,1", "46,28")]
    public static string RunB(string[] lines, bool isSample)
    {
        var items = LoadData(lines);

        var size = isSample ? 7 : 71;
        var total = isSample ? 12 : 1024;

        var map = CreateMap(items, size, total);

        var result = "";

        foreach (var i in total..items.Length)
        {
            map.Set(items[i], -1);

            var path = PathFinder.Find(map, new(0, 0), new(size - 1, size - 1));

            if (path is null)
            {
                result = $"{items[i].X},{items[i].Y}";
                break;
            }
        }

        return result;
    }

    static int[,] CreateMap(Pos[] items, int size, int total)
    {
        var map = Array.CreateAndInitialize(size, size, 1);

        foreach (var i in total)
            map.Set(items[i], -1);

        return map;
    }

    static Pos[] LoadData(string[] lines)
        => lines
        .Select(a => a.Split(',').ToArray(int.Parse))
        .ToArray(a => new Pos(a[0], a[1]));
}
