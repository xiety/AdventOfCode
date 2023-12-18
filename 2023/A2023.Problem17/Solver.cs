using Advent.Common;

namespace A2023.Problem17;

public class Solver : IProblemSolver<long>
{
    public long RunA(string filename)
    {
        var map = MapData.ParseMap(File.ReadAllLines(filename), c => int.Parse($"{c}"));
        var result = CalculateDistance(map, new Pos(0, 0), new Pos(map.GetWidth() - 1, map.GetHeight() - 1), 1, 3, false);
        return result.distance;
    }

    public long RunB(string filename)
    {
        var map = MapData.ParseMap(File.ReadAllLines(filename), c => int.Parse($"{c}"));
        var result = CalculateDistance(map, new Pos(0, 0), new Pos(map.GetWidth() - 1, map.GetHeight() - 1), 4, 10, false);
        return result.distance;
    }

    public static (List<Pos> path, int distance) CalculateDistance(int[,] map, Pos start, Pos end, int min, int max, bool collectPath)
    {
        var step = 0;

        var startKey = new StarKey(start, Pos.Zero);
        var startStar = new StarData(0, 0, [start]);
        var star = new Dictionary<StarKey, List<StarData>> { [startKey] = [startStar] };

        var currentSteps = new Dictionary<StarKey, List<StarData>>() { [startKey] = [startStar] };
        var newSteps = new Dictionary<StarKey, List<StarData>>();

        var bestEnd = int.MaxValue;
        var bestPath = new List<Pos>();

        var count = 0;

        do
        {
            foreach (var currentStep in currentSteps)
            {
                foreach (var offset in ArrayExtensions.Offsets)
                {
                    if (offset == -currentStep.Key.Offset)
                        continue;

                    var newPos = currentStep.Key.Pos + offset;

                    if (!map.IsInBounds(newPos))
                        continue;

                    foreach (var currentStepData in currentStep.Value)
                    {
                        if (offset != currentStep.Key.Offset && currentStepData.OffsetSteps < min && currentStep.Key.Offset != Pos.Zero)
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
                            {
                                bestEnd = newValue;

                                if (collectPath)
                                    bestPath = [.. currentStepData.Path, newPos];
                            }

                            continue;
                        }

                        var newStepsList = newSteps.GetOrCreate(newKey, () => []);

                        newStepsList.RemoveAll(a => a.OffsetSteps == offsetSteps && a.Value >= newValue);

                        var path = collectPath ? [.. currentStepData.Path, newPos] : currentStepData.Path;

                        var newData = new StarData(offsetSteps, newValue, path);

                        newStepsList.Add(newData);
                        starList.Add(newData);

                        count++;
                    }
                }
            }

            (currentSteps, newSteps) = (newSteps, currentSteps);
            newSteps.Clear();

            step++;
        }
        while (currentSteps.Count > 0);

        return (bestPath, bestEnd);
    }
}

public record class StarKey(Pos Pos, Pos Offset);
public record class StarData(int OffsetSteps, int Value, List<Pos> Path);
