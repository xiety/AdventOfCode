using Advent.Common;

namespace A2024.Problem16;

public class Solver : ISolver<long>
{
    public long RunA(string[] lines, bool isSample)
    {
        var map = MapData.ParseMap(lines, a => $"{a}");

        var start = map.First("S");
        var end = map.First("E");
        var direction = new Pos(1, 0);

        return Find(map, start, direction, end);
    }

    static int Find(string[,] map, Pos start, Pos direction, Pos end)
    {
        var star = ArrayEx.CreateAndInitialize(map.GetWidth(), map.GetHeight(), -1);
        star.Set(start, 0);

        List<(Pos, Pos)> currentSteps = [(start, direction)];
        List<(Pos, Pos)> newSteps = [];

        do
        {
            foreach (var currentStep in currentSteps)
            {
                var oldStar = star.Get(currentStep.Item1);

                foreach (var offset in ArrayEx.Offsets)
                {
                    if (currentStep.Item2 == -offset)
                        continue;

                    var newStep = currentStep.Item1 + offset;

                    if (map.Get(newStep) == "#")
                        continue;

                    var newStar = oldStar + 1;

                    if (currentStep.Item2 != offset)
                        newStar += 1000;

                    if (star.Get(newStep) == -1 || star.Get(newStep) > newStar)
                    {
                        star.Set(newStep, newStar);
                        newSteps.RemoveAll(a => a.Item1 == newStep);
                        newSteps.Add((newStep, offset));
                    }
                }
            }

            if (newSteps.Count == 0)
                break;

            (currentSteps, newSteps) = (newSteps, currentSteps);
            newSteps.Clear();
        }
        while (true);

        return star.Get(end);
    }
}
