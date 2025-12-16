using System.Text.RegularExpressions;

using Advent.Common;

namespace A2024.Problem07;

delegate long Op(long a, long b);

public static class Solver
{
    [GeneratedTest<long>(3749, 882304362421)]
    public static long RunA(string[] lines)
    {
        Op[] ops = [
            (a, b) => a + b,
            (a, b) => a * b,
        ];

        return Run(lines, ops);
    }

    [GeneratedTest<long>(11387, 145149066755184)]
    public static long RunB(string[] lines)
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

    static Item[] LoadData(string[] lines)
        => CompiledRegs.FromLinesRegex(lines);
}

record Item(long Result, long[] Values);

static partial class CompiledRegs
{
    [GeneratedRegex(@$"(?<{nameof(Item.Result)}>\d+)\: (?<{nameof(Item.Values)}>\d+)( (?<{nameof(Item.Values)}>\d+))*")]
    [MapTo<Item>]
    public static partial Regex Regex();
}
