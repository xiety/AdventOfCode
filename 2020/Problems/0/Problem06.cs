using Advent.Common;

namespace A2020.Problem06;

public class Solver : ISolver<int>
{
    public int RunA(string[] lines, bool isSample)
        => lines
            .SplitBy(string.Empty)
            .Sum(a => a.SelectMany(b => b)
                .Distinct()
                .Count());

    public int RunB(string[] lines, bool isSample)
        => lines
            .SplitBy(string.Empty)
            .Sum(a => a.SelectMany(b => b)
                .GroupBy(b => b)
                .Count(b => b.Count() == a.Length));
}
