using System.Text.RegularExpressions;

namespace A2019.Problem04;

public static class Solver
{
    [GeneratedTest<int>(64, 960)]
    public static int RunA(string[] lines)
        => Run(lines, CheckA);

    [GeneratedTest<int>(51, 626)]
    public static int RunB(string[] lines)
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

        foreach (var a1 in fmin..(fmax + 1))
            foreach (var a2 in a1..10)
                foreach (var a3 in a2..10)
                    foreach (var a4 in a3..10)
                        foreach (var a5 in a4..10)
                            foreach (var a6 in a5..10)
                            {
                                var n = a6 + a5 * 10 + a4 * 100 + a3 * 1_000 + a2 * 10_000 + a1 * 100_000;
                                if (n >= min && n <= max && check(a1, a2, a3, a4, a5, a6))
                                    count++;
                            }

        return count;
    }

    static Item LoadData(string[] lines)
        => CompiledRegs.FromLinesRegex(lines)[0];
}

record Item(int Min, int Max);

static partial class CompiledRegs
{
    [GeneratedRegex(@$"^(?<{nameof(Item.Min)}>\d+)\-(?<{nameof(Item.Max)}>\d+)$")]
    [MapTo<Item>]
    public static partial Regex Regex();
}
