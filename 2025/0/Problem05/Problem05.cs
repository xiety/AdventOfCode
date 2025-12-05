using Advent.Common;

namespace A2025.Problem05;

public static class Solver
{
    [GeneratedTest<long>(3, 615)]
    public static long RunA(string[] lines)
    {
        var (ranges, items) = LoadData(lines);
        return items
            .Count(a => CheckRanges(ranges, a));
    }

    [GeneratedTest<long>(14, 353716783056994)]
    public static long RunB(string[] lines)
    {
        var (ranges, _) = LoadData(lines);
        return ranges.Select(a => a.From)
            .Concat(ranges.Select(a => a.To + 1))
            .Distinct().Order().Chain()
            .Where(a => CheckRanges(ranges, a.First))
            .Sum(a => a.Second - a.First);
    }

    static bool CheckRanges(ItemRange[] ranges, long n)
        => ranges.Any(b => b.From <= n && b.To >= n);

    static (ItemRange[], long[]) LoadData(string[] lines)
    {
        var parts = lines.SplitBy(String.Empty).ToArray();
        var ranges = parts[0].ToArray(a => a.Split('-').ToArray(long.Parse).Apply(a => new ItemRange(a[0], a[1])));
        var items = parts[1].ToArray(long.Parse);
        return (ranges, items);
    }
}

record struct ItemRange(long From, long To);
