using Advent.Common;

namespace A2024.Problem10;

public class Solver : ISolver<int>
{
    public int RunA(string[] lines, bool isSample)
    {
        var map = MapData.ParseMap(lines);
        return map.EnumeratePositionsOf(0).Sum(a => FindNumberOfPaths(map, a).Distinct().Count());
    }

    public int RunB(string[] lines, bool isSample)
    {
        var map = MapData.ParseMap(lines);
        return map.EnumeratePositionsOf(0).Sum(a => FindNumberOfPaths(map, a).Count());
    }

    static IEnumerable<Pos> FindNumberOfPaths(int[,] map, Pos start)
    {
        var steps = new List<Pos> { start };
        var newSteps = new List<Pos>();

        do
        {
            foreach (var step in steps)
            {
                var o = map.Get(step);

                foreach (var newStep in map.Offsetted(step))
                {
                    var c = map.Get(newStep);

                    if (c == o + 1)
                    {
                        if (c == 9)
                            yield return newStep;
                        else
                            newSteps.Add(newStep);
                    }
                }
            }

            (steps, newSteps) = (newSteps, steps);
            newSteps.Clear();
        }
        while (steps.Count > 0);
    }
}
