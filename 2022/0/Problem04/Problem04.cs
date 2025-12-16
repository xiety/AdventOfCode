using System.Text.RegularExpressions;

namespace A2022.Problem04;

public static class Solver
{
    [GeneratedTest<int>(2, 542)]
    public static int RunA(string[] lines)
        => LoadData(lines)
            .Count(a => (a.From1 >= a.From2 && a.To1 <= a.To2) || (a.From2 >= a.From1 && a.To2 <= a.To1));

    [GeneratedTest<int>(4, 900)]
    public static int RunB(string[] lines)
        => LoadData(lines)
            .Count(a => Interval.IsIntersect(a.From1, a.To1, a.From2, a.To2));

    static Item[] LoadData(string[] lines)
        => CompiledRegs.FromLinesLine(lines);
}

record Item(int From1, int To1, int From2, int To2);

static partial class CompiledRegs
{
    [GeneratedRegex(@$"(?<{nameof(Item.From1)}>\d*)-(?<{nameof(Item.To1)}>\d*),(?<{nameof(Item.From2)}>\d*)-(?<{nameof(Item.To2)}>\d*)")]
    [MapTo<Item>]
    public static partial Regex Line();
}
