using Advent.Common;

namespace A2023.Problem09;

public class Solver : IProblemSolver<long>
{
    public long RunA(string filename)
    {
        var lines = File.ReadAllLines(filename)
            .Select(a => a.Split(' ').Select(long.Parse).ToArray());

        return lines.Select(CalcRecurse).Sum();
    }

    private long CalcRecurse(long[] items)
    {
        var diffs = items.Pairs(true).Select(a => a.Item2 - a.Item1).ToArray();
        var done = diffs.Distinct().Count() == 1;
        var delta = done ? diffs[0] : CalcRecurse(diffs);
        var result = items.Last() + delta;

        return result;
    }
}
