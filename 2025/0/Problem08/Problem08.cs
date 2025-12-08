using System.Text.RegularExpressions;

using Advent.Common;

namespace A2025.Problem08;

public static class Solver
{
    [GeneratedTest<long>(40, 66640)]
    public static long RunA(string[] lines, bool isSample)
    {
        var total = isSample ? 10 : 1000;
        var items = LoadData(lines);
        var dsu = new Dsu(items.Length);

        GetPairs(items).OrderBy(a => a.D2).Take(total)
            .Foreach(a => dsu.Union(a.Index1, a.Index2));

        return dsu.Sizes.OrderDescending().Take(3).Mul();
    }

    [GeneratedTest<long>(25272, 78894156)]
    public static long RunB(string[] lines)
    {
        var items = LoadData(lines);
        var dsu = new Dsu(items.Length);

        var pair = GetPairs(items).OrderBy(a => a.D2)
            .Do(a => dsu.Union(a.Index1, a.Index2))
            .First(_ => dsu.Count == 1);

        return items[pair.Index1].X * items[pair.Index2].X;
    }

    static IEnumerable<Pair> GetPairs(Pos3[] items)
    {
        for (var i = 0; i < items.Length - 1; ++i)
            for (var j = i + 1; j < items.Length; ++j)
                yield return new(i, j, (items[i] - items[j]).LengthSquared);
    }

    static Pos3[] LoadData(string[] lines)
        => CompiledRegs.FromLinesRegex(lines);
}

record Pair(int Index1, int Index2, long D2);

static partial class CompiledRegs
{
    [GeneratedRegex(@$"^(?<{nameof(Pos3.X)}>\d+),(?<{nameof(Pos3.Y)}>\d+),(?<{nameof(Pos3.Z)}>\d+)$")]
    [MapTo<Pos3>]
    public static partial Regex Regex();
}
