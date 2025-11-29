using Advent.Common;

namespace A2020.Problem01;

public static class Solver
{
    [GeneratedTest<int>(514579, 997899)]
    public static int RunA(string[] lines)
        => Run(lines, 2);

    [GeneratedTest<int>(241861950, 131248694)]
    public static int RunB(string[] lines)
        => Run(lines, 3);

    static int Run(string[] lines, int num)
        => Find(LoadData(lines), num, 2020);

    static int Find(int[] items, int num, int target)
        => items
            .Combinations(num)
            .Where(a => a.Sum() == target)
            .Select(a => a.Mul())
            .First();

    static int[] LoadData(string[] lines)
        => lines.ToArray(int.Parse);
}
