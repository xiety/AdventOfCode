
using Advent.Common;

namespace A2020.Problem15;

public static class Solver
{
    [GeneratedTest<int>(436, 260)]
    public static int RunA(string[] lines)
        => Run(lines, 2020);

    [GeneratedTest<long>(175594, 950)]
    public static int RunB(string[] lines)
        => Run(lines, 30000000);

    static int Run(string[] lines, int total)
    {
        var items = LoadData(lines);
        var dic = items.Take(..^1).Select((i, n) => (i, n)).ToDictionary();

        return Enumerable.RangeTo(items.Length, total)
            .Aggregate(items[^1], (prev, index) =>
            {
                var last = dic.GetValueOrDefault(prev, -1);
                dic[prev] = index - 1;
                return last == -1 ? 0 : index - 1 - last;
            });
    }

    static int[] LoadData(string[] lines)
        => lines[0].Split(',').ToArray(int.Parse);
}
