namespace A2021.Problem07;

public static class Solver
{
    [GeneratedTest<long>(37, 340052)]
    public static long RunA(string[] lines)
    {
        var items = LoadData(lines);
        return Find(items, (a, best) => Math.Abs(best - a));
    }

    [GeneratedTest<long>(168, 92948968)]
    public static long RunB(string[] lines)
    {
        var items = LoadData(lines);
        return Find(items, Dist);
    }

    static int Find(int[] items, Func<int, int, int> func)
        => Enumerable.Range(items.Min(), items.Max() - items.Min() + 1)
                     .Min<int, int>(best => items.Sum(a => func(a, best)));

    static int Dist(int a, int b)
    {
        var n = Math.Abs(b - a);
        return n * (n + 1) / 2;
    }

    static int[] LoadData(string[] lines)
        => lines[0]
            .TrimEnd()
            .Split(",")
            .ToArray(int.Parse);
}
