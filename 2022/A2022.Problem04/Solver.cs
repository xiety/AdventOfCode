using System.Text.RegularExpressions;

using Advent.Common;

namespace A2022.Problem04;

public class Solver : IProblemSolver<int>
{
    public int RunA(string filename)
        => LoadFile(filename)
            .Count(a => (a.From1 >= a.From2 && a.To1 <= a.To2) || (a.From2 >= a.From1 && a.To2 <= a.To1));

    public int RunB(string filename)
        => LoadFile(filename)
            .Count(a => Interval.IsIntersect(a.From1, a.To1, a.From2, a.To2));

    static Item[] LoadFile(string filename)
        => CompiledRegs.Line().FromFile<Item>(filename);
}

record Item(int From1, int To1, int From2, int To2);

static partial class CompiledRegs
{
    [GeneratedRegex(@$"(?<{nameof(Item.From1)}>\d*)-(?<{nameof(Item.To1)}>\d*),(?<{nameof(Item.From2)}>\d*)-(?<{nameof(Item.To2)}>\d*)")]
    public static partial Regex Line();
}
