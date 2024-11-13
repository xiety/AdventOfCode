using Advent.Common;

namespace A2021.Problem09;

public class Solver : IProblemSolver<long>
{
    public long RunA(string filename)
    {
        var data = MapData.ParseMap(File.ReadAllLines(filename));

        var lowestPoints = LowestPoints(data);

        var result = lowestPoints.Select(a => a.item + 1).Sum();

        return result;
    }

    public long RunB(string filename)
    {
        var data = MapData.ParseMap(File.ReadAllLines(filename));

        var lowestPoints = LowestPoints(data);

        var basins = lowestPoints
            .Select(tuple => CalculateBasin(tuple.pos, data));

        var result = basins
            .OrderDescending()
            .Take(3)
            .Aggregate(1, (acc, item) => acc * item);

        return result;
    }

    static List<(Pos pos, int item)> LowestPoints(int[,] data)
        => data.Enumerate()
               .Where(tuple => !data
                   .Offsetted(tuple.pos)
                   .Any(a => data.Get(a) <= tuple.item))
               .ToList();

    static int CalculateBasin(Pos start, int[,] data)
    {
        var list = new HashSet<Pos>();
        var nextPositions = new List<Pos> { start };

        do
        {
            nextPositions = nextPositions
                .SelectMany(nextPosition => data
                    .Offsetted(nextPosition)
                    .Where(pos => data.Get(pos) < 9)
                    .Where(pos => !list.Contains(pos)))
                .ToList();

            list.UnionWith(nextPositions);
        }
        while (nextPositions.Count > 0);

        return list.Count;
    }
}
