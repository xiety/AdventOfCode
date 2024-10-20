using Advent.Common;

namespace A2021.Problem07;

public class Solver : IProblemSolver<long>
{
    public long RunA(string filename)
    {
        var items = LoadFile(filename);
        var result = Find(items, (a, best) => Math.Abs(best - a));
        return result;
    }

    public long RunB(string filename)
    {
        var items = LoadFile(filename);
        var result = Find(items, Dist);
        return result;
    }

    private static IEnumerable<int> LoadFile(string filename)
        => File.ReadAllText(filename)
               .TrimEnd()
               .Split(",")
               .Select(int.Parse);

    private static int Find(IEnumerable<int> items, Func<int, int, int> func)
        => Enumerable.Range(items.Min(), items.Max() - items.Min() + 1)
                     .Select(best => items.Select(a => func(a, best)).Sum())
                     .Min();

    private static int Dist(int a, int b)
    {
        var n = Math.Abs(b - a);
        return (n * (n + 1)) / 2;
    }
}
