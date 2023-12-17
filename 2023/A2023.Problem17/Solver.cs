using System.Net.Http.Headers;

using Advent.Common;

namespace A2023.Problem17;

public class Solver : IProblemSolver<long>
{
    public long RunA(string filename)
    {
        var map = MapData.ParseMap(File.ReadAllLines(filename), c => int.Parse($"{c}"));
        var result = CalculateDistance(map, new Pos(0, 0), new Pos(map.GetWidth() - 1, map.GetHeight() - 1));
        return result;
    }

    public static int CalculateDistance(int[,] map, Pos start, Pos end)
    {
        var step = 0;

        var startKey = new StarKey(start, new(0, 0));
        var startStar = new StarData(0, 0);
        var star = new Dictionary<StarKey, List<StarData>> { [startKey] = [startStar] };

        var currentSteps = new List<(StarKey Key, StarData Value)>() { (startKey, startStar) };
        var newSteps = new List<(StarKey Key, StarData Value)>();

        var bestEnd = int.MaxValue;

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

                    var c = map.Get(newPos);

                    if (c != -1)
                    {
                        var newValue = currentStep.Value.Value + c;

                        if (newValue < bestEnd)
                        {
                            var offsetSteps = currentStep.Key.Offset == offset ? currentStep.Value.OffsetSteps + 1 : 0;

                            if (offsetSteps < 3)
                            {
                                if (newPos == end)
                                    bestEnd = newValue;

                                newSteps.RemoveAll(a => a.Key.Pos == newPos && a.Key.Offset == offset && ((a.Value.OffsetSteps == offsetSteps && a.Value.Value > newValue) || (a.Value.OffsetSteps > offsetSteps && a.Value.Value == newValue)));

                                var newKey = new StarKey(newPos, offset);

                                star.TryGetValue(newKey, out var starList);

                                if (starList is null || !starList.Any(a => (a.OffsetSteps == offsetSteps && a.Value < newValue) || (a.OffsetSteps < offsetSteps && a.Value == newValue)))
                                {
                                    var newData = new StarData(offsetSteps, newValue);

                                    newSteps.Add((newKey, newData));

                                    if (starList is null)
                                    {
                                        starList = [];
                                        star[newKey] = starList;
                                    }

                                    starList.Add(newData);

                                    count++;
                                }
                            }
                        }
                    }
                }
            }

            if (newSteps.Count == 0)
                break;

            (currentSteps, newSteps) = (newSteps, currentSteps);
            newSteps.Clear();

            step++;
        }
        while (true);

        return bestEnd;
    }
}

public record class StarKey(Pos Pos, Pos Offset);
public record class StarData(int OffsetSteps, int Value);
