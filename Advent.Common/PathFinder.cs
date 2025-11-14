namespace Advent.Common;

public static class PathFinder
{
    public static int? Length(int[,] map, Pos start, Pos end)
    {
        var star = CalculateStar(map, start, end);

        if (star is null)
            return null;

        return star.Get(end);
    }

    public static Pos[]? Find(int[,] map, Pos start, Pos end)
    {
        var star = CalculateStar(map, start, end);

        if (star is null)
            return null;

        return CalculatePath(map, star, start, end);
    }

    public static Pos[][] FindAll(int[,] map, Pos start, Pos end)
    {
        var star = CalculateStar(map, start, end);

        if (star is null)
            return [];

        return CalculatePaths(map, star, start, end);
    }

    static Pos[] CalculatePath(int[,] map, int[,] star, Pos start, Pos end)
    {
        var path = new List<Pos>() { end };
        var currentStep = end;

        do
        {
            currentStep = map
                .Offsetted(currentStep)
                .Select(a => new { NewStep = a, Value = star.Get(a) })
                .Where(a => a.Value != -1)
                .OrderBy(a => a.Value)
                .First()
                .NewStep;

            if (currentStep == start)
                break;

            path.Add(currentStep);
        }
        while (true);

        return path.Reverse<Pos>().ToArray();
    }

    static Pos[][] CalculatePaths(int[,] map, int[,] star, Pos start, Pos end)
    {
        List<List<Pos>> paths = [[end]];
        List<List<Pos>> newPaths = [];

        do
        {
            foreach (var path in paths)
            {
                if (path[^1] != start)
                {
                    var currentSteps = map
                        .Offsetted(path[^1])
                        .Select(a => new { NewStep = a, Value = star.Get(a) })
                        .Where(a => a.Value != -1)
                        .MinAllBy(a => a.Value)
                        .Select(a => a.NewStep);

                    foreach (var (index, currentStep) in currentSteps.Index())
                    {
                        var newPath = path.Append(currentStep).ToList();
                        newPaths.Add(newPath);
                    }
                }
            }

            if (newPaths.Count == 0)
                break;

            (paths, newPaths) = (newPaths, paths);
            newPaths.Clear();
        }
        while (true);

        return paths.ToArray(a => a[..^1].Reverse<Pos>().ToArray());
    }

    static int[,]? CalculateStar(int[,] map, Pos start, Pos end)
    {
        var star = Array.CreateAndInitialize(map.Width, map.Height, -1);

        star.Set(start, 0);

        var currentSteps = new List<Pos> { start };
        var newSteps = new List<Pos>();

        do
        {
            foreach (var currentStep in currentSteps)
            {
                var currentDistance = star.Get(currentStep);

                foreach (var newStep in map.Offsetted(currentStep))
                {
                    var oldStar = star.Get(newStep);

                    var c = map.Get(newStep);

                    if (c != -1)
                    {
                        var newStar = currentDistance + c;

                        if (oldStar == -1 || oldStar > newStar)
                        {
                            star.Set(newStep, newStar);

                            if (newStep == end)
                                return star;

                            newSteps.Add(newStep);
                        }
                    }
                }
            }

            if (newSteps.Count == 0)
                return null;

            (currentSteps, newSteps) = (newSteps, currentSteps);
            newSteps.Clear();
        }
        while (true);
    }
}
