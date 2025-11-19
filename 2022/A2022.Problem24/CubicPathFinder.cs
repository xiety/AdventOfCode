namespace A2022.Problem24;

public static class CubicPathFinder
{
    static readonly Pos3[] Deltas =
    [
        new(-1, 0, 1),
        new(0, -1, 1),
        new(1, 0, 1),
        new(0, 1, 1),
        new(0, 0, 1),
    ];

    public static Pos3 CountSteps(Map3d map, Pos3 start, Pos3 finishWithoutZ)
    {
        var star = Array.CreateAndInitialize(map.Size.X, map.Size.Y, map.Size.Z, -1);

        star.Set(start, 0);

        var currentSteps = new List<Pos3> { start };
        var newSteps = new List<Pos3>();
        var currentDistance = 1;

        do
        {
            foreach (var currentStep in currentSteps)
            {
                foreach (var delta in Deltas)
                {
                    var newStep = currentStep + delta;

                    if (star.IsInBounds(newStep) && !map[newStep] && star.Get(newStep) == -1)
                    {
                        star.Set(newStep, currentDistance);

                        if (newStep.X == finishWithoutZ.X && newStep.Y == finishWithoutZ.Y) //ignore Z
                            return newStep;

                        newSteps.Add(newStep);
                    }
                }
            }

            currentSteps.Clear();
            currentSteps.AddRange(newSteps);
            newSteps.Clear();

            currentDistance++;

            if (currentSteps.Count == 0)
                throw new();
        }
        while (true);
    }
}
