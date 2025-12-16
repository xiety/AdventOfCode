using Advent.Common;

namespace A2023.Problem11;

public static class Solver
{
    [GeneratedTest<long>(374, 9233514)]
    public static long RunA(string[] lines)
        => Run(lines, 2);

    [GeneratedTest<long>(8410, 363293506944)]
    public static long RunB(string[] lines, bool isSample)
        => Run(lines, isSample ? 100 : 1_000_000);

    static long Run(string[] lines, int resize)
    {
        var map = MapData.ParseMap(lines, c => c == '#');

        var emptyCols = GetEmptyCols(map);
        var emptyRows = GetEmptyRows(map);

        var stars = GetStars(map, resize, emptyCols, emptyRows).ToArray();

        return stars.EnumeratePairs().Sum(a => Distance(a.First, a.Second));
    }

    static IEnumerable<Pos> GetStars(bool[,] map, int resize, List<int> emptyCols, List<int> emptyRows)
        => from pos in map.EnumeratePositionsOf(true)
           let dx = emptyCols.Count(a => a < pos.X) * (resize - 1) + pos.X
           let dy = emptyRows.Count(a => a < pos.Y) * (resize - 1) + pos.Y
           select new Pos(dx, dy);

    static List<int> GetEmptyRows(bool[,] map)
    {
        var emptyRows = new List<int>();

        for (var y = 0; y < map.Height; ++y)
        {
            var empty = true;

            for (var x = 0; x < map.Width; ++x)
            {
                if (map[x, y])
                {
                    empty = false;
                    break;
                }
            }

            if (empty)
                emptyRows.Add(y);
        }

        return emptyRows;
    }

    static List<int> GetEmptyCols(bool[,] map)
    {
        var emptyCols = new List<int>();

        for (var x = 0; x < map.Width; ++x)
        {
            var empty = true;

            for (var y = 0; y < map.Height; ++y)
            {
                if (map[x, y])
                {
                    empty = false;
                    break;
                }
            }

            if (empty)
                emptyCols.Add(x);
        }

        return emptyCols;
    }

    static long Distance(Pos from, Pos to)
        => Math.Abs(from.X - to.X) + Math.Abs(from.Y - to.Y);
}
