namespace A2025.Problem07;

public static class Solver
{
    [GeneratedTest<long>(21, 1592)]
    public static long RunA(string[] lines)
        => Run(lines).Splits;

    [GeneratedTest<long>(40, 17921968177009)]
    public static long RunB(string[] lines)
        => Run(lines).Counts.Sum();

    static Result Run(string[] lines)
        => lines.Aggregate(
            new Result(new long[lines[0].Length], 0L),
            (acc, line) => new(
                line.Zip(acc.Counts).SelectPrevCurrNext((prev, curr, next) =>
                    curr is ('S', _) ? 1 :
                        (curr is ('.', var c) ? c : 0) +
                        (prev is ('^', var p) ? p : 0) +
                        (next is ('^', var n) ? n : 0)
                ).ToArray(),
                acc.Splits + line.Zip(acc.Counts).Count(t => t is ('^', > 0))));

    [GeneratedTest<long>(40, 17921968177009)]
    public static long RunBReversed(string[] lines)
    {
        var start = lines[0].IndexOf('S');
        var space = Enumerable.Repeat(1L, lines[0].Length).ToArray();
        return lines.Reverse().Aggregate(space, (acc, item) =>
            acc.ToArray((a, i) => item[i] == '^' ? acc[i - 1] + acc[i + 1] : a))[start];
    }

    [GeneratedTest<long>(21, 1592)]
    public static long RunAScatter(string[] lines)
        => RunScatter(lines).Splits;

    [GeneratedTest<long>(40, 17921968177009)]
    public static long RunBScatter(string[] lines)
        => RunScatter(lines).Counts.Sum();

    static Result RunScatter(string[] lines)
    {
        var counts = lines[0].ToArray(c => c == 'S' ? 1L : 0L);
        var totalSplits = 0L;

        foreach (var line in lines.Skip(1))
        {
            var next = new long[counts.Length];

            foreach (var i in line.Length)
            {
                var count = counts[i];

                if (count == 0)
                    continue;

                if (line[i] is '^')
                {
                    totalSplits++;
                    next[i - 1] += count;
                    next[i + 1] += count;
                }
                else
                {
                    next[i] += count;
                }
            }

            counts = next;
        }

        return new(counts, totalSplits);
    }
}

record struct Result(long[] Counts, long Splits);
