using System.Text.RegularExpressions;

using Advent.Common;
using Advent.Common.Graph;

namespace A2019.Problem06;

public class Solver : ISolver<int>
{
    public int RunA(string[] lines, bool isSample)
    {
        var connections = LoadData(lines);
        var moons = connections.Select(a => a.Moon).Distinct().ToArray();
        return moons.Select(a => RecurseCount(connections, a, 0)).Sum();
    }

    public int RunB(string[] lines, bool isSample)
    {
        var connections = LoadData(lines);
        var nodes = CreateGraph(connections);

        var start = nodes.First(a => a.Name == "YOU");
        var end = nodes.First(a => a.Name == "SAN");

        var star = GraphPathFinder.CalculateStar(nodes, start, end);

        return star[end] - 2;
    }

    static GraphNode[] CreateGraph(List<Connection> connections)
    {
        var nodes = connections.Select(a => a.Center)
            .Concat(connections.Select(a => a.Moon))
            .Select(a => new GraphNode { Name = a }).ToArray();

        foreach (var connection in connections)
        {
            var from = nodes.First(a => a.Name == connection.Center);
            var to = nodes.First(a => a.Name == connection.Moon);

            from.Connections.Add(to);
            to.Connections.Add(from);
        }

        return nodes;
    }

    static int RecurseCount(List<Connection> connections, string moon, int number)
    {
        var connection = connections.FirstOrDefault(a => a.Moon == moon);

        return connection is not null
            ? RecurseCount(connections, connection.Center, number + 1)
            : number;
    }

    static List<Connection> LoadData(string[] lines)
        => CompiledRegs.Regex().FromLines<Connection>(lines);
}

record Connection(string Center, string Moon);

static partial class CompiledRegs
{
    [GeneratedRegex(@$"^(?<{nameof(Connection.Center)}>.+)\)(?<{nameof(Connection.Moon)}>.+)$")]
    public static partial Regex Regex();
}
