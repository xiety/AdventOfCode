using System.Text.RegularExpressions;

using Advent.Common;

namespace A2024.Problem05;

public class Solver : ISolver<int>
{
    public int RunA(string[] lines, bool isSample)
    {
        var (rules, items) = LoadData(lines);
        return SumMid(items.Where(a => Order(rules, a).SequenceEqual(a)));
    }

    public int RunB(string[] lines, bool isSample)
    {
        var (rules, items) = LoadData(lines);
        return SumMid(items.Select(a => new { Ordered = Order(rules, a), Orig = a })
            .Where(a => !a.Ordered.SequenceEqual(a.Orig))
            .Select(a => a.Ordered));
    }

    static int SumMid(IEnumerable<int[]> items)
        => items.Sum(a => a[a.Length / 2]);

    static int[] Order(Rule[] rules, int[] array)
        => array.Order(Comparer<int>.Create((x, y) => Compare(rules, x, y))).ToArray();

    static int Compare(Rule[] rules, int x, int y)
        => rules.Any(a => a.Left == x && a.Right == y) ? -1 : 1;

    private static (Rule[], int[][]) LoadData(string[] lines)
    {
        var (chunk1, chunk2) = lines.SplitBy(String.Empty);
        var rules = CompiledRegs.Regex().FromLines<Rule>(chunk1);
        var items = chunk2.ToArray(a => a.Split(",").ToArray(int.Parse));
        return (rules, items);
    }
}

record Rule(int Left, int Right);

static partial class CompiledRegs
{
    [GeneratedRegex(@$"(?<{nameof(Rule.Left)}>\d+)\|(?<{nameof(Rule.Right)}>\d+)")]
    public static partial Regex Regex();
}
