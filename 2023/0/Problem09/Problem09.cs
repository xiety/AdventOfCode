namespace A2023.Problem09;

public static class Solver
{
    [GeneratedTest<long>(114, 1972648895)]
    public static long RunA(string[] lines)
        => Run(lines, true);

    [GeneratedTest<long>(2, 919)]
    public static long RunB(string[] lines)
        => Run(lines, false);

    static long Run(string[] lines, bool next)
        => LoadDatas(lines).Sum(a => CalcRecurse(a, next));

    static IEnumerable<long[]> LoadDatas(string[] lines)
        => lines.Select(a => a.Split(' ').ToArray(long.Parse));

    static long CalcRecurse(long[] items, bool next)
    {
        var diffs = items.Chain().ToArray(a => a.Second - a.First);
        var done = diffs.Distinct().Count() == 1;
        var delta = done ? diffs[0] : CalcRecurse(diffs, next);
        return next ? items[^1] + delta : items[0] - delta;
    }
}
