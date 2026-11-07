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

    //public static IEnumerable<int[]> Compositions(int total, int parts)
    //{
    //    if (parts == 1)
    //    {
    //        yield return [total];
    //        yield break;
    //    }

    //    for (var i = 0; i <= total; ++i)
    //        foreach (var rest in Compositions(total - i, parts - 1))
    //            yield return [i, .. rest];
    //}

    //public static IEnumerable<int[]> Compositions(int total, int parts) =>
    //    parts == 1
    //        ? (int[][])[[total]]
    //        : Enumerable.Range(0, total + 1)
    //            .SelectMany(i => Compositions(total - i, parts - 1)
    //                             .Select(rest => (int[])[i, .. rest]));

    public static IEnumerable<int[]> Compositions(int total, int parts)
    {
        var buffer = new int[parts];
        return Fill(total, 0);

        IEnumerable<int[]> Fill(int remaining, int index)
        {
            if (index == buffer.Length - 1)
            {
                buffer[index] = remaining;
                yield return buffer;
            }
            else
            {
                for (var i = 0; i <= remaining; ++i)
                {
                    buffer[index] = i;
                    foreach (var item in Fill(remaining - i, index + 1))
                        yield return item;
                }
            }
        }
    }
}
