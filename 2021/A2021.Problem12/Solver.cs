﻿using Advent.Common;

namespace A2021.Problem12;

public class Solver : IProblemSolver<long>
{
    public long RunA(string filename)
        => Run(filename, true);

    public long RunB(string filename)
        => Run(filename, false);

    static long Run(string filename, bool single)
    {
        var graph = ParseGraph(File.ReadAllLines(filename).ToArray(ParseLine));

        var calc = new Calculator(graph, single);
        var result = calc.Calculate();

        return result;
    }

    static Item ParseLine(string line)
    {
        var n = line.IndexOf('-');
        return new(line[..n], line[(n + 1)..]);
    }

    static Graph ParseGraph(Item[] items)
    {
        var nodes = Enumerable
            .Concat(items.Select(a => a.From), items.Select(a => a.To))
            .ToArray(a => new GraphNode { Name = a, CaveType = CalcCaveType(a) });

        foreach (var item in items)
        {
            var from = nodes.First(a => a.Name == item.From);
            var to = nodes.First(a => a.Name == item.To);

            from.Connections.Add(to);
            to.Connections.Add(from);
        }

        return new()
        {
            Start = nodes.First(a => a.Name == "start"),
            End = nodes.First(a => a.Name == "end"),
        };
    }

    static CaveType CalcCaveType(string name)
    {
        if (name is "start" or "end")
            return CaveType.StartOrEnd;

        return name.ToLower() == name
            ? CaveType.Small
            : CaveType.Large;
    }
}

public record Item(string From, string To);
