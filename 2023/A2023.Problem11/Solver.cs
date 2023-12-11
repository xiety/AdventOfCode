using System.Reflection.Metadata;
using System.Xml;

using Advent.Common;

namespace A2023.Problem11;

public class Solver : IProblemSolver<long>
{
    public long RunA(string filename)
    {
        var map = MapData.ParseMap(File.ReadAllLines(filename), c => c == '#');

        var expanded = Expand(map);
        var stars = expanded.EnumeratePositionsOf(-1).ToArray();
        var totalDistance = 0L;

        for (var i = 0; i < stars.Length - 1; ++i)
            for (var j = i + 1; j < stars.Length; ++j)
                totalDistance += Distance(stars[i], stars[j]);

        return totalDistance;
    }

    private int[,] Expand(bool[,] map)
    {
        var emptyCols = new List<int>();
        var emptyRows = new List<int>();

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

        var expanded = ArrayEx
            .CreateAndInitialize(map.GetWidth() + emptyCols.Count, map.GetHeight() + emptyRows.Count, 1);

        var ey = 0;

        for (var y = 0; y < map.GetHeight(); ++y)
        {
            var ex = 0;

            for (var x = 0; x < map.GetWidth(); ++x)
            {
                expanded[ex, ey] = map[x, y] ? -1 : 1;

                if (emptyCols.Contains(x))
                    ex++;

                ex++;
            }

            if (emptyRows.Contains(y))
                ey++;

            ey++;
        }

        return expanded;
    }

    private static long Distance(Pos from, Pos to)
        => Math.Abs(from.X - to.X) + Math.Abs(from.Y - to.Y);
}
