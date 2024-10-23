using System.Text.RegularExpressions;

using Advent.Common;

namespace A2021.Problem02;

public class Solver : IProblemSolver<int>
{
    public int RunA(string filename)
    {
        static Pos Process(Pos pos, Item item)
            => item.Dir switch
            {
                "up" => pos with { Y = pos.Y - item.Number },
                "down" => pos with { Y = pos.Y + item.Number },
                "forward" => pos with { X = pos.X + item.Number },
            };

        var items = LoadFile(filename);
        var pos = items.Aggregate(new Pos(0, 0), Process);
        var result = pos.X * pos.Y;

        return result;
    }

    public int RunB(string filename)
    {
        static (int, Pos) Process((int Aim, Pos Pos) data, Item item)
            => item.Dir switch
            {
                "up" => (data.Aim - item.Number, data.Pos),
                "down" => (data.Aim + item.Number, data.Pos),
                "forward" => (data.Aim,
                    new(data.Pos.X + data.Aim * item.Number,
                        data.Pos.Y + item.Number)),
            };

        var items = LoadFile(filename);
        var (aim, pos) = items.Aggregate((0, new Pos(0, 0)), Process);
        var result = pos.X * pos.Y;

        return result;
    }

    private static List<Item> LoadFile(string filename)
    {
        var lines = File.ReadAllLines(filename);

        return lines
            .Select(CompiledRegs.Regex().MapTo<Item>)
            .ToList();
    }
}

record Item(string Dir, int Number);

static partial class CompiledRegs
{
    [GeneratedRegex(@$"^(?<{nameof(Item.Dir)}>\w+) (?<{nameof(Item.Number)}>\d+)$")]
    public static partial Regex Regex();
}
