using Advent.Common;

namespace A2025.Problem01;

public static class Solver
{
    const int Start = 50;
    const int Size = 100;

    [GeneratedTest<int>(3, 1139)]
    public static int RunA(string[] lines)
        => LoadData(lines)
            .Accumulate(Start, (acc, item) => Math.Mod(acc + item, Size))
            .Count(a => a == 0);

    [GeneratedTest<int>(6, 6684)]
    public static int RunB(string[] lines)
        => LoadData(lines)
            .Accumulate((Pos: Start, Count: 0), (acc, item) =>
            {
                var n = acc.Pos + item;
                var count = Math.Abs(n / Size) + (n <= 0 && acc.Pos != 0 ? 1 : 0);
                return (Math.Mod(n, Size), count);
            })
            .Sum(a => a.Count);

    static int[] LoadData(string[] lines)
        => lines.ToArray(a => (a[0] == 'L' ? -1 : 1) * int.Parse(a[1..]));
}
