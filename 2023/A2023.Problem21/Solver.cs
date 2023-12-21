using Advent.Common;

namespace A2023.Problem21;

public class Solver : IProblemSolver<long>
{
    public long RunA(string filename)
    {
        var lines = File.ReadAllLines(filename);
        var map = MapData.ParseMap(lines, c => c == '#');
        var start = MapData.FindPos(lines, 'S');

        var currentSteps = new List<Pos> { start };
        var newSteps = new List<Pos>();

        var total = Path.GetFileName(filename) == "sample.txt" ? 6 : 64;

        for (var i = 0; i < total; ++i)
        {
            foreach (var currentStep in currentSteps)
            {
                foreach (var newStep in map.Offsetted(currentStep))
                {
                    var c = map.Get(newStep);

                    if (!c)
                    {
                        if (!newSteps.Contains(newStep))
                            newSteps.Add(newStep);
                    }
                }
            }

            if (newSteps.Count == 0)
                throw new Exception();

            (currentSteps, newSteps) = (newSteps, currentSteps);
            newSteps.Clear();
        }

        return currentSteps.Count;
    }
}
