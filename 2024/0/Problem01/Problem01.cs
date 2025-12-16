using System.Text.RegularExpressions;

using Advent.Common;

namespace A2024.Problem01;

public static class Solver
{
    [GeneratedTest<int>(11, 1151792)]
    public static int RunA(string[] lines)
    {
        var (left, right) = LoadData(lines);

        return left.Order().Zip(right.Order())
            .Sum(a => Math.Abs(a.First - a.Second));
    }

    [GeneratedTest<int>(31, 21790168)]
    public static int RunB(string[] lines)
    {
        var (left, right) = LoadData(lines);

        return left
            .Sum(a => a * right.Count(b => b == a));
    }

    static (int[], int[]) LoadData(string[] lines)
    {
        var items = CompiledRegs.FromLinesRegex(lines);
        var left = items.Select(a => a.Left);
        var right = items.Select(a => a.Right);
        return ([.. left], [.. right]);
    }
}

record Item(int Left, int Right);

static partial class CompiledRegs
{
    [GeneratedRegex(@$"^(?<{nameof(Item.Left)}>\d+)   (?<{nameof(Item.Right)}>\d+)$")]
    [MapTo<Item>]
    public static partial Regex Regex();
}
