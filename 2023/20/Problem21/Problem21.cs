namespace A2023.Problem21;

public static class Solver
{
    [GeneratedTest<long>(16, 3724)]
    public static long RunA(string[] lines, bool isSample)
    {
        var map = MapData.ParseMap(lines, c => c == '#');
        var start = MapData.FindPos(lines, 'S');

        var currentSteps = new List<Pos> { start };
        var newSteps = new List<Pos>();

        var total = isSample ? 6 : 64;

        foreach (var i in total)
        {
            foreach (var currentStep in currentSteps)
            {
                foreach (var newStep in map.Offsetted(currentStep))
                {
                    var c = map.Get(newStep);

                    if (!c && !newSteps.Contains(newStep))
                    {
                        newSteps.Add(newStep);
                    }
                }
            }

            if (newSteps.Count == 0)
                throw new();

            (currentSteps, newSteps) = (newSteps, currentSteps);
            newSteps.Clear();
        }

        return currentSteps.Count;
    }
}
