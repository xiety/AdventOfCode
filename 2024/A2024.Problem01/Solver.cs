using System.Text.RegularExpressions;

using Advent.Common;

namespace A2024.Problem01;

public class Solver : ISolver<int>
{
    public int RunA(string[] lines, bool isSample)
    {
        var (left, right) = LoadData(lines);

        return left.Order().Zip(right.Order())
            .Sum(a => Math.Abs(a.First - a.Second));
    }

    public int RunB(string[] lines, bool isSample)
    {
        var (left, right) = LoadData(lines);

        return left
            .Sum(a => a * right.Count(b => b == a));
    }

    static (IEnumerable<int>, IEnumerable<int>) LoadData(string[] lines)
    {
        var items = CompiledRegs.Regex().FromLines<Item>(lines);
        var left = items.Select(a => a.Left);
        var right = items.Select(a => a.Right);
        return (left, right);
    }
}

record Item(int Left, int Right);

static partial class CompiledRegs
{
    [GeneratedRegex(@$"^(?<{nameof(Item.Left)}>\d+)   (?<{nameof(Item.Right)}>\d+)$")]
    public static partial Regex Regex();
}
