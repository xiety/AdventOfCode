using System.Text.RegularExpressions;

using Advent.Common;

namespace A2019.Problem04;

public class Solver : ISolver<int>
{
    public int RunA(string[] lines, bool isSample)
        => Run(lines, CheckA);

    public int RunB(string[] lines, bool isSample)
        => Run(lines, CheckB);

    static bool CheckA(int a1, int a2, int a3, int a4, int a5, int a6)
        => a1 == a2 || a2 == a3 || a3 == a4 || a4 == a5 || a5 == a6;

    static bool CheckB(int a1, int a2, int a3, int a4, int a5, int a6)
        => (a1 == a2 && a2 != a3)
        || (a2 == a3 && a1 != a2 && a3 != a4)
        || (a3 == a4 && a2 != a3 && a4 != a5)
        || (a4 == a5 && a3 != a4 && a5 != a6)
        || (a5 == a6 && a4 != a5);

    static int Run(string[] lines, Func<int, int, int, int, int, int, bool> check)
    {
        var (min, max) = LoadData(lines);

        var fmin = min / 100_000;
        var fmax = max / 100_000;

        var count = 0;

        for (var a1 = fmin; a1 <= fmax; ++a1)
            for (var a2 = a1; a2 <= 9; ++a2)
                for (var a3 = a2; a3 <= 9; ++a3)
                    for (var a4 = a3; a4 <= 9; ++a4)
                        for (var a5 = a4; a5 <= 9; ++a5)
                            for (var a6 = a5; a6 <= 9; ++a6)
                            {
                                var n = a6 + a5 * 10 + a4 * 100 + a3 * 1_000 + a2 * 10_000 + a1 * 100_000;
                                if (n >= min && n <= max && check(a1, a2, a3, a4, a5, a6))
                                    count++;
                            }

        return count;
    }

    static Item LoadData(string[] lines)
        => CompiledRegs.Regex().FromLines<Item>(lines).First();
}

record Item(int Min, int Max);

static partial class CompiledRegs
{
    [GeneratedRegex(@$"^(?<{nameof(Item.Min)}>\d+)\-(?<{nameof(Item.Max)}>\d+)$")]
    public static partial Regex Regex();
}
