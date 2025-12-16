using Advent.Common;

namespace A2022.Problem12;

public static class Solver
{
    [GeneratedTest<int>(31, 456)]
    public static int RunA(string[] lines)
    {
        var (map, start, end) = LoadData(lines);

        var path = SlopePathFinder.Find(map, start, end)
            ?? throw new();

        return path.Length;
    }

    [GeneratedTest<int>(29, 454)]
    public static int RunB(string[] lines)
    {
        var (map, _, end) = LoadData(lines);

        var starts = map.EnumeratePositionsOf(0);

        return starts
            .Select(a => SlopePathFinder.Find(map, a, end))
            .WhereNotNull()
            .Min(a => a.Length);
    }

    public static (int[,], Pos, Pos) LoadData(string[] lines)
    {
        var width = lines[0].Length;
        var height = lines.Length;

        var data = new int[width, height];

        var start = new Pos(0, 0);
        var end = new Pos(0, 0);

        foreach (var y in height)
        {
            foreach (var x in width)
            {
                var c = lines[y][x];

                if (c == 'S')
                {
                    start = new(x, y);
                    c = 'a';
                }
                else if (c == 'E')
                {
                    end = new(x, y);
                    c = 'z';
                }

                data[x, y] = (c - 'a');
            }
        }

        return (data, start, end);
    }
}
