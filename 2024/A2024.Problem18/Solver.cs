using Advent.Common;

namespace A2024.Problem18;

public class Solver : ISolver<int, string>
{
    public int RunA(string[] lines, bool isSample)
    {
        var items = LoadData(lines);

        var size = isSample ? 7 : 71;
        var total = isSample ? 12 : 1024;

        var map = CreateMap(items, size, total);

        var path = PathFinder.Find(map, new(0, 0), new(size - 1, size - 1));

        return path is null ? throw new() : path.Length;
    }

    public string RunB(string[] lines, bool isSample)
    {
        var items = LoadData(lines);

        var size = isSample ? 7 : 71;
        var total = isSample ? 12 : 1024;

        var map = CreateMap(items, size, total);

        var result = "";

        for (var i = total; i < items.Length; ++i)
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
        var map = ArrayEx.CreateAndInitialize(size, size, 1);

        for (var i = 0; i < total; ++i)
            map.Set(items[i], -1);

        return map;
    }

    static Pos[] LoadData(string[] lines)
        => lines
        .Select(a => a.Split(',').Select(int.Parse).ToArray())
        .Select(a => new Pos(a[0], a[1]))
        .ToArray();
}
