namespace System.Collections.Generic;

public class PathFinder
{
    private static readonly Pos[] deltas = [new(-1, 0), new(0, -1), new(1, 0), new(0, 1)];

    public static Pos[]? Find(int[,] map, Pos start, Pos end)
    {
        var star = CalculateStar(map, start, end);

        if (star is null)
            return null;

        return CalculatePath(map, star, start, end);
    }

    private static Pos[] CalculatePath(int[,] map, int[,] star, Pos start, Pos end)
    {
        var step = 0;

        var path = new List<Pos>() { end };
        var currentStep = end;

        do
        {
            currentStep = deltas
                .Select(a => currentStep - a)
                .Where(map.IsInBounds)
                .Select(a => new { NewStep = a, Value = star.Get(a) })
                .Where(a => a.Value != -1)
                .OrderBy(a => a.Value)
                .First()
                .NewStep;

            if (currentStep == start)
                break;

            path.Add(currentStep);

            step++;
        }
        while (true);

        return path.Reverse<Pos>().ToArray();
    }

    private static int[,]? CalculateStar(int[,] map, Pos start, Pos end)
    {
        var step = 0;

        var star = ArrayEx.CreateAndInitialize(map.GetWidth(), map.GetHeight(), -1);

        star.Set(start, 0);

        var currentSteps = new List<Pos> { start };
        var newSteps = new List<Pos>();

        do
        {
            foreach (var currentStep in currentSteps)
            {
                var currentDistance = star.Get(currentStep);

                foreach (var newStep in deltas.Select(a => a + currentStep).Where(map.IsInBounds))
                {
                    var oldStar = star.Get(newStep);
                    var newStar = currentDistance + map.Get(newStep);

                    if (oldStar == -1 || oldStar > newStar)
                    {
                        star.Set(newStep, newStar);

                        if (newStep == end)
                            return star;

                        newSteps.Add(newStep);
                    }
                }
            }

            if (newSteps.Count == 0)
                return null;

            (currentSteps, newSteps) = (newSteps, currentSteps);
            newSteps.Clear();

            step++;
        }
        while (true);
    }
}
