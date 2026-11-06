using System.Text.RegularExpressions;

namespace A2015.Problem14;

public static class Solver
{
    [GeneratedTest<int>(1120, 2640)]
    public static int RunA(string[] lines, bool isSample)
        => LoadItems(lines).Max(item => Calc(TotalTime(isSample), item));

    [GeneratedTest<int>(689, 1102)]
    public static int RunB(string[] lines, bool isSample)
    {
        var items = LoadItems(lines);

        return Enumerable.Range(1, TotalTime(isSample))
            .Aggregate(new int[items.Length], (points, t) => Step(points, items, t))
            .Max();
    }

    static int[] Step(int[] points, Item[] items, int time)
    {
        var distances = items.ToArray(item => Calc(time, item));
        var max = distances.Max();
        return points.ToArray((p, i) => distances[i] == max ? p + 1 : p);
    }

    static int Calc(int totalTime, Item item)
    {
        var (loops, rest) = Math.DivRem(totalTime, item.Time + item.Rest);
        return (loops * item.Time + Math.Min(rest, item.Time)) * item.Speed;
    }

    static int TotalTime(bool isSample)
        => isSample ? 1000 : 2503;

    static Item[] LoadItems(string[] lines)
        => CompiledRegs.FromLinesRegex(lines);
}

record Item(int Speed, int Time, int Rest);

static partial class CompiledRegs
{
    [GeneratedRegex(@$"^.+ can fly (?<{nameof(Item.Speed)}>\d+) km/s for (?<{nameof(Item.Time)}>\d+) seconds, but then must rest for (?<{nameof(Item.Rest)}>\d+) seconds.$")]
    [MapTo<Item>]
    public static partial Regex Regex();
}
