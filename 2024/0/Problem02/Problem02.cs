using Advent.Common;

namespace A2024.Problem02;

public static class Solver
{
    [GeneratedTest<int>(2, 213)]
    public static int RunA(string[] lines)
    {
        var items = LoadData(lines);
        return items.Count(Check);
    }

    [GeneratedTest<int>(4, 285)]
    public static int RunB(string[] lines)
    {
        var items = LoadData(lines);
        return items.Count(CheckRemove);
    }

    static bool CheckRemove(int[] item)
        => Check(item) || item.Where((_, i) => Check(item.Where((_, idx) => idx != i).ToArray())).Any();

    static bool Check(int[] item)
    {
        if (!item.SequenceEqual(item.Order()) && !item.SequenceEqual(item.OrderDescending()))
            return false;

        var diffs = item.Chain().ToArray(a => Math.Abs(a.Second - a.First));
        var maxDiff = diffs.Max();
        var minDiff = diffs.Min();

        return minDiff >= 1 && maxDiff <= 3;
    }

    static int[][] LoadData(string[] lines)
        => lines.ToArray(a => a.Split(' ').ToArray(int.Parse));
}
