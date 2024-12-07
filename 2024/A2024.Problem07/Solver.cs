using System.Text.RegularExpressions;

using Advent.Common;

namespace A2024.Problem07;

delegate long Op(long a, long b);

public class Solver : ISolver<long>
{
    public long RunA(string[] lines, bool isSample)
    {
        Op[] ops = [
            (a, b) => a + b,
            (a, b) => a * b,
        ];

        return Run(lines, ops);
    }

    public long RunB(string[] lines, bool isSample)
    {
        Op[] ops = [
            (a, b) => a + b,
            (a, b) => a * b,
            (a, b) => long.Parse($"{a}{b}"),
        ];

        return Run(lines, ops);
    }

    static long Run(string[] lines, Op[] ops)
        => LoadData(lines).Where(a => Check(ops, a)).Sum(a => a.Result);

    static bool Check(Op[] ops, Item item)
        => Recurse(ops, item, item.Values[0], 1);

    static bool Recurse(Op[] ops, Item item, long value, long index)
        => index == item.Values.Length
            ? value == item.Result
            : ops.Any(a => Recurse(ops, item, a(value, item.Values[index]), index + 1));

    static List<Item> LoadData(string[] lines)
        => CompiledRegs.Regex().FromLines<Item>(lines);
}

record Item(long Result, long[] Values);

static partial class CompiledRegs
{
    [GeneratedRegex(@$"(?<{nameof(Item.Result)}>\d+)\: (?<{nameof(Item.Values)}>\d+)( (?<{nameof(Item.Values)}>\d+))*")]
    public static partial Regex Regex();
}
