namespace A2022.Problem12;

class SlopePathFinder
{
    public static Pos[]? Find(int[,] map, Pos start, Pos end)
    {
        var star = CalculateStar(map, start, end);

        if (star is null)
            return null;

        return CalculatePath(map, star, start, end);
    }

    static Pos[] CalculatePath(int[,] map, int[,] star, Pos start, Pos end)
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

    static int[,]? CalculateStar(int[,] map, Pos start, Pos end)
    {
        var star = ArrayEx.CreateAndInitialize(map.GetWidth(), map.GetHeight(), -1);
        star.Set(start, 0);

        List<Pos> currentSteps = [start];
        List<Pos> newSteps = [];

        var currentDistance = 1;

        do
        {
            var items =
                from currentStep in currentSteps
                let currentValue = map.Get(currentStep)
                from newStep in map.Offsetted(currentStep)
                where map.Get(newStep) <= currentValue + 1
                where star.Get(newStep) == -1
                select newStep;

            foreach (var newStep in items)
            {
                star.Set(newStep, currentDistance);

                if (newStep == end)
                    return star;

                newSteps.Add(newStep);
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
