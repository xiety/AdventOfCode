namespace A2024.Problem10;

public static class Solver
{
    [GeneratedTest<int>(36, 798)]
    public static int RunA(string[] lines)
    {
        var map = MapData.ParseMap(lines);
        return map.EnumeratePositionsOf(0).Sum(a => FindNumberOfPaths(map, a).Distinct().Count());
    }

    [GeneratedTest<int>(81, 1816)]
    public static int RunB(string[] lines)
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
