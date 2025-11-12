using Advent.Common;

namespace A2020.Problem01;

public class Solver : ISolver<int>
{
    public int RunA(string[] lines, bool isSample)
        => Run(lines, 2);

    public int RunB(string[] lines, bool isSample)
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
        => lines.Select(Int32.Parse).ToArray();
}
