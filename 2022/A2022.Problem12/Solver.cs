using Advent.Common;

namespace A2022.Problem12;

public class Solver : IProblemSolver<int>
{
    public int RunA(string filename)
    {
        var (map, start, end) = Load(filename);

        var path = SlopePathFinder.Find(map, start, end)
            ?? throw new();

        return path.Length;
    }

    public int RunB(string filename)
    {
        var (map, _, end) = Load(filename);

        var starts = map.EnumeratePositionsOf(0);

        var result = starts
            .Select(a => SlopePathFinder.Find(map, a, end))
            .WhereNotNull()
            .Min(a => a.Length);

        return result;
    }

    public static (int[,], Pos, Pos) Load(string filename)
    {
        var lines = File.ReadAllLines(filename);

        var width = lines[0].Length;
        var height = lines.Length;

        var data = new int[width, height];

        var start = new Pos(0, 0);
        var end = new Pos(0, 0);

        for (var y = 0; y < height; ++y)
        {
            for (var x = 0; x < width; ++x)
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
