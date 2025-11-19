using Advent.Common;

namespace A2023.Problem17;

public class Solver : IProblemSolver<long>
{
    public long RunA(string filename)
        => Run(filename, 1, 3);

    public long RunB(string filename)
        => Run(filename, 4, 10);

    static long Run(string filename, int min, int max)
    {
        var map = LoadFile(filename);
        return CalculateDistance(map, Pos.Zero, new Pos(map.Width - 1, map.Height - 1), min, max);
    }

    static int CalculateDistance(int[,] map, Pos start, Pos end, int min, int max)
    {
        var startKey = new StarKey(start, Pos.Zero);
        var startStar = new StarData(0, 0);
        var star = new Dictionary<StarKey, List<StarData>> { [startKey] = [startStar] };

        var currentSteps = new Dictionary<StarKey, List<StarData>> { [startKey] = [startStar] };
        var newSteps = new Dictionary<StarKey, List<StarData>>();

        var bestEnd = int.MaxValue;

        do
        {
            foreach (var currentStep in currentSteps)
            {
                foreach (var offset in ArrayEx.Offsets)
                {
                    if (offset == -currentStep.Key.Offset)
                        continue;

                    var newPos = currentStep.Key.Pos + offset;

                    if (!map.IsInBounds(newPos))
                        continue;

                    foreach (var currentStepData in currentStep.Value)
                    {
                        if (offset != currentStep.Key.Offset
                         && currentStepData.OffsetSteps < min
                         && currentStep.Key.Offset != Pos.Zero)
                            continue;

                        var c = map.Get(newPos);

                        if (c == -1)
                            continue;

                        var newValue = currentStepData.Value + c;

                        if (newValue >= bestEnd)
                            continue;

                        var maxlen = Math.Abs(end.X - newPos.X) + Math.Abs(end.Y - newPos.Y) + newValue;

                        if (maxlen >= bestEnd)
                            continue;

                        var offsetSteps = currentStep.Key.Offset == offset ? currentStepData.OffsetSteps + 1 : 1;

                        if (offsetSteps > max)
                            continue;

                        var newKey = new StarKey(newPos, offset);

                        var starList = star.GetOrCreate(newKey, () => []);

                        if (starList.Any(a => a.OffsetSteps == offsetSteps && a.Value <= newValue))
                            continue;

                        if (newPos == end)
                        {
                            if (offsetSteps >= min)
                                bestEnd = newValue;
                        }
                        else
                        {
                            var newStepsList = newSteps.GetOrCreate(newKey, () => []);

                            newStepsList.RemoveAll(a => a.OffsetSteps == offsetSteps && a.Value >= newValue);

                            var newData = new StarData(offsetSteps, newValue);

                            newStepsList.Add(newData);
                            starList.Add(newData);
                        }
                    }
                }
            }

            (currentSteps, newSteps) = (newSteps, currentSteps);
            newSteps.Clear();
        }
        while (currentSteps.Count > 0);

        return bestEnd;
    }

    static int[,] LoadFile(string filename)
        => MapData.ParseMap(File.ReadAllLines(filename), c => int.Parse($"{c}"));
}

public record StarKey(Pos Pos, Pos Offset);
public record StarData(int OffsetSteps, int Value);
