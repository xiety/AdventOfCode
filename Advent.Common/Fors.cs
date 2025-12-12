namespace Advent.Common;

public static class Fors
{
    public static IEnumerable<int[]> For(params (int fromIncl, int toExcl)[] ranges)
        => ranges
              .Select(a => Enumerable.Range(a.fromIncl, a.toExcl - a.fromIncl))
              .Aggregate(
                  seed: new[] { Array.Empty<int>() },
                  func: (acc, next) => acc.SelectMany(a => next.Select(b => (int[])[.. a, b])).ToArray());

    public static void LoopWhile(Func<bool> func)
    {
        var done = false;

        do
        {
            done = func();
        }
        while (done);
    }
}
