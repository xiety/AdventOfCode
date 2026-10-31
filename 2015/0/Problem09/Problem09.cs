using System.Text.RegularExpressions;

namespace A2015.Problem09;

public static class Solver
{
    [GeneratedTest<int>(605, 117)]
    public static int RunA(string[] lines)
        => Calc(lines, a => a.Min());

    [GeneratedTest<int>(982, 909)]
    public static int RunB(string[] lines)
        => Calc(lines, a => a.Max());

    static int Calc(string[] lines, Func<IEnumerable<int>, int> aggregate)
    {
        var nodes = LoadNodes(lines);

        return aggregate(nodes
            .SelectMany((_, i) => nodes.Skip(i + 1), (from, to) => (from, to))
            .Select(pair => aggregate(Recurse([], pair.from, pair.to, nodes.Length, 0))));
    }

    static IEnumerable<int> Recurse(List<GraphNode> visited, GraphNode parent, GraphNode target, int total, int distance)
    {
        if (visited.Count + 1 == total)
            return [distance];
        if (parent == target)
            return [];

        return parent.Connections
            .Where(c => !visited.Contains(c.Destination))
            .SelectMany(c => Recurse([..visited, parent], c.Destination, target, total, distance + c.Distance));
    }

    static GraphNode[] LoadNodes(string[] lines)
    {
        var connections = CompiledRegs.FromLinesRegex(lines);

        var nodes = connections.Select(c => c.From)
            .Concat(connections.Select(c => c.To))
            .Distinct()
            .ToArray(GraphNode.Create);

        Connect(connections, nodes);

        return nodes;
    }

    static void Connect(Connection[] connections, GraphNode[] nodes)
    {
        var lookup = nodes.ToDictionary(n => n.Name);

        foreach (var c in connections)
        {
            var from = lookup[c.From];
            var to = lookup[c.To];
            from.Connections.Add(new(to, c.Distance));
            to.Connections.Add(new(from, c.Distance));
        }
    }
}

record Connection(string From, string To, int Distance);

static partial class CompiledRegs
{
    [GeneratedRegex(@$"^(?<{nameof(Connection.From)}>.+) to (?<{nameof(Connection.To)}>.+) = (?<{nameof(Connection.Distance)}>\d+)$")]
    [MapTo<Connection>]
    public static partial Regex Regex();
}
