using Advent.Common;

namespace A2023.Problem09;

public class Solver : IProblemSolver<long>
{
    public long RunA(string filename)
        => Run(filename, true);

    public long RunB(string filename)
        => Run(filename, false);

    static long Run(string filename, bool next)
        => LoadFiles(filename).Sum(a => CalcRecurse(a, next));

    static IEnumerable<long[]> LoadFiles(string filename)
        => File.ReadAllLines(filename)
               .Select(a => a.Split(' ').ToArray(long.Parse));

    static long CalcRecurse(long[] items, bool next)
    {
        var diffs = items.Chain().ToArray(a => a.Second - a.First);
        var done = diffs.Distinct().Count() == 1;
        var delta = done ? diffs[0] : CalcRecurse(diffs, next);
        return next ? items[^1] + delta : items[0] - delta;
    }
}
