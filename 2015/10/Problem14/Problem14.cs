using System.Text.RegularExpressions;

namespace A2015.Problem14;

public static class Solver
{
    [GeneratedTest<int>(1120, 2640)]
    public static int RunA(string[] lines, bool isSample)
    {
        var items = LoadItems(lines);
        var totalTime = isSample ? 1000 : 2503;
        return items.Max(a => Calc(totalTime, a));
    }

    [GeneratedTest<int>(689, 1102)]
    public static int RunB(string[] lines, bool isSample)
    {
        var items = LoadItems(lines);
        var totalTime = isSample ? 1000 : 2503;

        var states = items.ToArray(a => new State(a));

        foreach (var _ in totalTime)
        {
            foreach (var state in states)
                Step(state);

            foreach (var state in states.MaxAllBy(a => a.Distance))
                state.Points++;
        }

        return states.Max(a => a.Points);
    }

    static void Step(State state)
    {
        state.Time++;

        if (state.IsResting)
        {
            if (state.Time == state.Item.Rest)
            {
                state.IsResting = false;
                state.Time = 0;
            }
        }
        else
        {
            state.Distance += state.Item.Speed;

            if (state.Time == state.Item.Time)
            {
                state.IsResting = true;
                state.Time = 0;
            }
        }
    }

    static int Calc(int totalTime, Item item)
    {
        var loopTime = item.Time + item.Rest;
        var loopsNum = totalTime / loopTime;
        var loopedPart = (loopsNum * loopTime);
        var rest = totalTime - loopedPart;
        var restAction = Math.Min(rest, item.Time);
        var loopedDistance = loopsNum * (item.Speed * item.Time);
        return loopedDistance + restAction * item.Speed;
    }

    static Item[] LoadItems(string[] lines)
        => CompiledRegs.FromLinesRegex(lines);
}

class State(Item Item)
{
    public Item Item { get; init; } = Item;
    public int Time { get; set; }
    public int Distance { get; set; }
    public bool IsResting { get; set; }
    public int Points { get; set; }
}

record Item(int Speed, int Time, int Rest);

static partial class CompiledRegs
{
    [GeneratedRegex(@$"^.+ can fly (?<{nameof(Item.Speed)}>\d+) km/s for (?<{nameof(Item.Time)}>\d+) seconds, but then must rest for (?<{nameof(Item.Rest)}>\d+) seconds.$")]
    [MapTo<Item>]
    public static partial Regex Regex();
}
