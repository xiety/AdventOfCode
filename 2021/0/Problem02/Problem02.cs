using System.Text.RegularExpressions;

using Advent.Common;

namespace A2021.Problem02;

public static class Solver
{
    [GeneratedTest<int>(150, 2322630)]
    public static int RunA(string[] lines)
    {
        var items = CompiledRegs.FromLinesRegex(lines);
        var pos = items.Aggregate(new Pos(0, 0), Process);
        return pos.X * pos.Y;

        static Pos Process(Pos pos, Item item)
            => item.Dir switch
            {
                "up" => pos with { Y = pos.Y - item.Number },
                "down" => pos with { Y = pos.Y + item.Number },
                "forward" => pos with { X = pos.X + item.Number },
            };
    }

    [GeneratedTest<int>(900, 2105273490)]
    public static int RunB(string[] lines)
    {
        var items = CompiledRegs.FromLinesRegex(lines);
        var (_, pos) = items.Aggregate((0, new Pos(0, 0)), Process);
        return pos.X * pos.Y;

        static (int, Pos) Process((int Aim, Pos Pos) data, Item item)
            => item.Dir switch
            {
                "up" => (data.Aim - item.Number, data.Pos),
                "down" => (data.Aim + item.Number, data.Pos),
                "forward" => (data.Aim,
                    new(data.Pos.X + data.Aim * item.Number,
                        data.Pos.Y + item.Number)),
            };
    }
}

record Item(string Dir, int Number);

static partial class CompiledRegs
{
    [GeneratedRegex(@$"^(?<{nameof(Item.Dir)}>\w+) (?<{nameof(Item.Number)}>\d+)$")]
    [MapTo<Item>]
    public static partial Regex Regex();
}
