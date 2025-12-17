using System.Text.RegularExpressions;

namespace A2020.Problem12;

public static class Solver
{
    [GeneratedTest<int>(25, 1687)]
    public static int RunA(string[] lines)
        => Solve(lines, new(new(0, 0), new(1, 0)), false);

    [GeneratedTest<int>(286, 20873)]
    public static int RunB(string[] lines)
        => Solve(lines, new(new(0, 0), new(10, 1)), true);

    static int Solve(string[] lines, State initial, bool moveWaypoint)
        => Load(lines)
        .Aggregate(initial, (state, item) => Step(state, item, moveWaypoint))
        .Ship.ManhattanLength;

    static State Step(State state, Item item, bool moveWaypoint)
        => item.Mode switch
        {
            Mode.Left => state with { Waypoint = state.Waypoint.Rotate(-item.Value) },
            Mode.Right => state with { Waypoint = state.Waypoint.Rotate(item.Value) },
            Mode.Forward => state with { Ship = state.Ship + state.Waypoint * item.Value },
            _ => Move(state, item, moveWaypoint),
        };

    static State Move(State state, Item item, bool moveWaypoint)
    {
        var delta = ToDir(item.Mode) * item.Value;

        return moveWaypoint
            ? state with { Waypoint = state.Waypoint + delta }
            : state with { Ship = state.Ship + delta };
    }

    static Pos ToDir(Mode mode)
        => mode switch
        {
            Mode.North => new Pos(0, 1),
            Mode.South => new(0, -1),
            Mode.East => new(1, 0),
            Mode.West => new(-1, 0),
        };

    static Item[] Load(string[] lines)
        => CompiledRegs.FromLinesRegexItem(lines);
}

enum Mode { North, South, East, West, Left, Right, Forward }
record struct State(Pos Ship, Pos Waypoint);
record struct Item([RegexParser<ParseMode, Mode>] Mode Mode, int Value);

static partial class CompiledRegs
{
    [GeneratedRegex(@$"^(?<{nameof(Item.Mode)}>.)(?<{nameof(Item.Value)}>\d+)$")]
    [MapTo<Item>]
    public static partial Regex RegexItem();
}

class ParseMode : IRegexParser<Mode>
{
    public Mode Parse(string text)
        => text switch
        {
            "N" => Mode.North,
            "S" => Mode.South,
            "E" => Mode.East,
            "W" => Mode.West,
            "L" => Mode.Left,
            "R" => Mode.Right,
            "F" => Mode.Forward,
        };
}
