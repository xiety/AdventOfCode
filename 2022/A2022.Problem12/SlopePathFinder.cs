namespace A2022.Problem12;

internal class SlopePathFinder
{
    public static Pos[]? Find(int[,] map, Pos start, Pos end)
    {
        var star = CalculateStar(map, start, end);

        if (star is null)
            return null;

        return CalculatePath(map, star, start, end);
    }

    private static Pos[] CalculatePath(int[,] map, int[,] star, Pos start, Pos end)
    {
        List<Pos> path = [end];

        var currentStep = end;
        var currentDistance = star.Get(end);

        do
        {
            currentStep = map.Offsetted(currentStep)
                .Select(a => new { NewStep = a, Value = star.Get(a) })
                .First(a => a.Value == currentDistance - 1)
                .NewStep;

            if (currentStep == start)
                break;

            path.Add(currentStep);

            currentDistance--;
        }
        while (true);

        return path.Reverse<Pos>().ToArray();
    }

    private static int[,]? CalculateStar(int[,] map, Pos start, Pos end)
    {
        var star = ArrayEx.CreateAndInitialize(map.GetWidth(), map.GetHeight(), -1);
        star.Set(start, 0);

        List<Pos> currentSteps = [start];
        List<Pos> newSteps = [];

        var currentDistance = 1;

        do
        {
            foreach (var currentStep in currentSteps)
            {
                var currentValue = map.Get(currentStep);

                foreach (var newStep in map.Offsetted(currentStep))
                {
                    if (map.Get(newStep) <= currentValue + 1)
                    {
                        if (star.Get(newStep) == -1)
                        {
                            star.Set(newStep, currentDistance);

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

            currentDistance++;
        }
        while (true);
    }
}
