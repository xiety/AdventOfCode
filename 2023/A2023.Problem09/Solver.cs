using Advent.Common;

namespace A2023.Problem09;

public class Solver : IProblemSolver<long>
{
    public long RunA(string filename)
        => Run(filename, true);

    public long RunB(string filename)
        => Run(filename, false);

    private static long Run(string filename, bool next)
        => LoadFiles(filename).Select(a => CalcRecurse(a, next)).Sum();

    private static IEnumerable<long[]> LoadFiles(string filename)
        => File.ReadAllLines(filename)
               .Select(a => a.Split(' ').Select(long.Parse).ToArray());

    private static long CalcRecurse(long[] items, bool next)
    {
        var diffs = items.Chain().Select(a => a.Item2 - a.Item1).ToArray();
        var done = diffs.Distinct().Count() == 1;
        var delta = done ? diffs[0] : CalcRecurse(diffs, next);
        var result = next ? items.Last() + delta : items.First() - delta;
        return result;
    }
}
