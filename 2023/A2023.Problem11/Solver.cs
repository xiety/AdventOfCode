using Advent.Common;

namespace A2023.Problem11;

public class Solver : IProblemSolver<long>
{
    public long RunA(string filename)
        => Run(filename, 2);

    public long RunB(string filename)
        => Run(filename, Path.GetFileName(filename) == "sample.txt" ? 100 : 1_000_000);

    private long Run(string filename, int resize)
    {
        var map = MapData.ParseMap(File.ReadAllLines(filename), c => c == '#');

        var emptyCols = GetEmptyCols(map);
        var emptyRows = GetEmptyRows(map);

        var stars = GetStars(map, resize, emptyCols, emptyRows).ToArray();
        var totalDistance = 0L;

        for (var i = 0; i < stars.Length - 1; ++i)
            for (var j = i + 1; j < stars.Length; ++j)
                totalDistance += Distance(stars[i], stars[j]);

        return totalDistance;
    }

    private static IEnumerable<Pos> GetStars(bool[,] map, int resize, List<int> emptyCols, List<int> emptyRows)
        => from pos in map.EnumeratePositionsOf(true)
           let dx = emptyCols.Count(a => a < pos.X) * (resize - 1) + pos.X
           let dy = emptyRows.Count(a => a < pos.Y) * (resize - 1) + pos.Y
           select new Pos(dx, dy);

    private static List<int> GetEmptyRows(bool[,] map)
    {
        var emptyRows = new List<int>();

        for (var y = 0; y < map.GetHeight(); ++y)
        {
            var empty = true;

            for (var x = 0; x < map.GetWidth(); ++x)
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

    private static List<int> GetEmptyCols(bool[,] map)
    {
        var emptyCols = new List<int>();

        for (var x = 0; x < map.GetWidth(); ++x)
        {
            var empty = true;

            for (var y = 0; y < map.GetHeight(); ++y)
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

    private static long Distance(Pos from, Pos to)
        => Math.Abs(from.X - to.X) + Math.Abs(from.Y - to.Y);
}
