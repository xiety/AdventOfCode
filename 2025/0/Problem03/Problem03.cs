namespace A2025.Problem03;

public static class Solver
{
    [GeneratedTest<long>(357, 17330)]
    public static long RunA(string[] lines)
        => Run(lines, 2);

    [GeneratedTest<long>(3121910778619, 171518260283767)]
    public static long RunB(string[] lines)
        => Run(lines, 12);

    static long Run(string[] lines, int num)
        => LoadData(lines).Sum(a => Calc(a, num));

    static long Calc(int[] array, int num)
        => Enumerable.Range(0, num)
            .Accumulate(-1, (prev, c)
                => Enumerable.RangeTo(prev + 1, array.Length - num + c + 1)
                             .MaxBy(i => array[i]))
            .Skip(1)
            .Aggregate(0L, (acc, c) => acc * 10 + array[c]);

    static int[][] LoadData(string[] lines)
        => lines.ToArray(a => a.ToArray(b => int.Parse(b.ToString())));
}
