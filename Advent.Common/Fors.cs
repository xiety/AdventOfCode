namespace System;

public static class Fors
{
    public static IEnumerable<int[]> For(params (int fromIncl, int toExcl)[] ranges)
    {
        var loops = ranges.Select(a => Enumerable.Range(a.fromIncl, a.toExcl - a.fromIncl)).ToArray();

        //return loops.Skip(1)
        //    .Aggregate(
        //        loops.First().Select(c => new List<int>() { c }),
        //        (previous, next) => previous.SelectMany(p => next.Select(d => new List<int>(p) { d }),
        //        a => a.ToArray()));

        return loops.Aggregate(
            seed: new[] { Array.Empty<int>() },
            func: (acc, next) => acc.SelectMany(a => next.Select(b => (int[])[..a, b])).ToArray());
    }
}
