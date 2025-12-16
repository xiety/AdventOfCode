#pragma warning disable CS0162 // Unreachable code detected
using System.Text.RegularExpressions;

using Advent.Common;

namespace A2022.Problem16;

public static class Solver
{
    [GeneratedTest<int>(1651, 1617)]
    public static int RunA(string[] lines)
    {
        throw new NotImplementedException(); //too slow
        var items = CompiledRegs.FromLinesRegex(lines);
        var graph = Grapher.CreateGraph(items);
        var solver = new Solver1();
        return solver.Run(graph);
    }

    [GeneratedTest<int>(1707, 2171)]
    public static int RunB(string[] lines)
    {
        throw new NotImplementedException();
        var items = CompiledRegs.FromLinesRegex(lines);
        var graph = Grapher.CreateGraph(items);
        var solver = new Solver2();
        return solver.Run(graph);
    }
}

public record Item(string Valve, int Rate, string[] Targets);

static partial class CompiledRegs
{
    [GeneratedRegex(@$"^Valve (?<{nameof(Item.Valve)}>\w+) has flow rate=(?<{nameof(Item.Rate)}>\d+); tunnels? leads? to valves? (?<{nameof(Item.Targets)}>\w\w)(, (?<{nameof(Item.Targets)}>\w\w))*$")]
    [MapTo<Item>]
    public static partial Regex Regex();
}
