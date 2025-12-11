using System.Text.RegularExpressions;

using Advent.Common;

using Graph = System.Collections.Generic.Dictionary<string, Advent.Common.GraphNode>;

namespace A2025.Problem11;

public static class Solver
{
    [GeneratedTest<long>(5, 603)]
    public static long RunA(string[] lines)
        => Find(LoadData(lines)["you"], "out");

    [GeneratedTest<long>(2, 380961604031372)]
    public static long RunB(string[] lines)
    {
        var nodes = LoadData(lines);
        return Array.Create("fft", "dac")
            .Combinations()
            .Sum(w => Array.Create(["svr", .. w, "out"])
                .Chain()
                .Mul(a => Find(nodes[a.First], a.Second)));
    }

    static long Find(GraphNode start, string finish)
        => Memoization.RunRecursive<GraphNode, long>(start,
            (memo, p) => p.Name == finish ? 1 : p.Connections.Sum(memo));

    static Graph CreateGraph(Item[] items)
    {
        var nodes = items.Select(a => a.From)
            .Concat(items.SelectMany(a => a.To))
            .Distinct()
            .ToDictionary(a => a, GraphNode.Create);

        items.SelectMany(item => item.To.Select(to => (item.From, To: to)))
             .Foreach(a => nodes[a.From].Connections.Add(nodes[a.To]));

        return nodes;
    }

    static Graph LoadData(string[] lines)
        => CreateGraph(CompiledRegs.FromLinesRegex(lines));
}

record Item(string From, string[] To);

static partial class CompiledRegs
{
    [GeneratedRegex(@$"^(?<{nameof(Item.From)}>.+)\:\s((?<{nameof(Item.To)}>[^\s]+)\s?)+$")]
    [MapTo<Item>]
    public static partial Regex Regex();
}
