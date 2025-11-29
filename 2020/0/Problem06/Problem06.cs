using Advent.Common;

namespace A2020.Problem06;

public static class Solver
{
    [GeneratedTest<int>(11, 6457)]
    public static int RunA(string[] lines)
        => lines
            .SplitBy(string.Empty)
            .Sum(a => a.SelectMany(b => b)
                .Distinct()
                .Count());

    [GeneratedTest<int>(6, 3260)]
    public static int RunB(string[] lines)
        => lines
            .SplitBy(string.Empty)
            .Sum(a => a.SelectMany(b => b)
                .GroupBy(b => b)
                .Count(b => b.Count() == a.Length));
}
