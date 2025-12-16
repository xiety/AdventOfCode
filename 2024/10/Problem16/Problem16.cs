namespace A2024.Problem16;

public static class Solver
{
    [GeneratedTest<long>(11048, 83432)]
    public static long RunA(string[] lines)
    {
        var map = MapData.ParseMap(lines, a => $"{a}");

        var start = map.FindValue("S");
        var end = map.FindValue("E");
        var direction = new Pos(1, 0);

        var star = CalculateStar(map, start, direction, end);

        return star.Where(a => a.Key.Item1 == end).Min(a => a.Value);
    }

    [GeneratedTest<long>(64, 467)]
    public static long RunB(string[] lines)
    {
        var map = MapData.ParseMap(lines, a => $"{a}");

        var start = map.FindValue("S");
        var end = map.FindValue("E");
        var direction = new Pos(1, 0);

        var star = CalculateStar(map, start, direction, end);

        return CalculatePath(star, start, end);
    }

    static int CalculatePath(Dictionary<(Pos, Pos, Pos), int> star, Pos start, Pos end)
    {
        var allStepsEnd = star
            .Where(a => a.Key.Item1 == end)
            .ToArray();

        var minEnd = allStepsEnd.Min(a => a.Value);

        var currentSteps = allStepsEnd
            .Where(a => a.Value == minEnd)
            .ToList(a => (a.Key.Item1, a.Key.Item2));

        List<(Pos, Pos)> newSteps = [];
        HashSet<Pos> paths = [start, end];

        do
        {
            foreach (var currentStep in currentSteps)
            {
                var newPos = currentStep.Item1 - currentStep.Item2;

                var allSteps = star
                    .Where(a => a.Key.Item1 == newPos && a.Key.Item3 == currentStep.Item2)
                    .ToArray();

                if (allSteps.Length == 0)
                    continue;

                var min = allSteps.Min(a => a.Value);

                var nextSteps = allSteps
                    .Where(a => a.Value == min)
                    .ToArray(a => a.Key);

                foreach (var nextStep in nextSteps)
                {
                    var newValue = star.GetValueOrDefault(nextStep, -1);

                    if (newValue == -1)
                        continue;

                    paths.Add(nextStep.Item1);
                    newSteps.Add((nextStep.Item1, nextStep.Item2));
                }
            }

            if (newSteps.Count == 0)
                break;

            (currentSteps, newSteps) = (newSteps, currentSteps);
            newSteps.Clear();
        }
        while (true);

        return paths.Count;
    }

    static Dictionary<(Pos, Pos, Pos), int> CalculateStar(string[,] map, Pos start, Pos direction, Pos end)
    {
        var star = new Dictionary<(Pos, Pos, Pos), int>
        {
            { (start, new Pos(1, 0), new Pos(1, 0)), 0 },
            { (start, new Pos(1, 0), new Pos(0, -1)), 1000 },
            { (start, new Pos(1, 0), new Pos(0, 1)), 1000 },
            { (start, new Pos(1, 0), new Pos(-1, 0)), 2000 },
        };

        List<(Pos, Pos, int)> currentSteps = [
            (start, new Pos(1, 0), 0),
            (start, new Pos(0, -1), 1000),
            (start, new Pos(0, 1), 1000),
            (start, new Pos(-1, 0), 2000)
        ];

        List<(Pos, Pos, int)> newSteps = [];

        do
        {
            foreach (var currentStep in currentSteps)
            {
                var newStep = currentStep.Item1 + currentStep.Item2;

                if (map.Get(newStep) == "#")
                    continue;

                foreach (var offset2 in ArrayEx.Offsets)
                {
                    if (offset2 == -currentStep.Item2)
                        continue;

                    if (map.Get(newStep + offset2) == "#" && newStep != end)
                        continue;

                    var rotate = 0;

                    if (offset2 != currentStep.Item2 && newStep != end)
                        rotate = 1000;

                    var key = (newStep, currentStep.Item2, offset2);

                    var newStar = currentStep.Item3 + rotate + 1;

                    var toStar = star.GetValueOrDefault(key, -1);

                    if (toStar == -1 || toStar > newStar)
                    {
                        star[key] = newStar;

                        if (newStep != end)
                            newSteps.Add((newStep, offset2, newStar));
                    }
                }
            }

            if (newSteps.Count == 0)
                break;

            (currentSteps, newSteps) = (newSteps, currentSteps);
            newSteps.Clear();
        }
        while (true);

        return star;
    }
}
