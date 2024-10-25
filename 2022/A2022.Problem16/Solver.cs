﻿using System.Text.RegularExpressions;

using Advent.Common;

namespace A2022.Problem16;

public class Solver : IProblemSolver<int>
{

    public int RunA(string filename)
    {
        var items = CompiledRegs.Regex().FromFile<Item>(filename).ToArray();
        var graph = Grapher.CreateGraph(items);
        var solver = new Solver1();
        return solver.Run(graph);
    }

    public int RunB(string filename)
    {
        var items = CompiledRegs.Regex().FromFile<Item>(filename).ToArray();
        var graph = Grapher.CreateGraph(items);
        var solver = new Solver2();
        return solver.Run(graph);
    }
}

public class Solver2
{
    readonly int totalTime = 26;

    GraphNode[] availableWorkingVaults = null!;

    long step;


    public int Run(Graph graph)
    {
        var currentTime = 0;

        var parent = graph.Nodes.First(); //first must be AA

        availableWorkingVaults = graph.Nodes.Where(a => a.Rate > 0).ToArray();

        var path1 = Linked.Empty<string>();
        var path2 = Linked.Empty<string>();
        var releases = Linked.Empty<int>();
        var released = Linked.Empty<Release>();

        path1 = path1.AddBefore(parent.Name);
        var maximum = parent.Connections.AsParallel()
            .Max(child => RecurseElephant(path1, path2, releases, released, currentTime, child, parent, false, false));

        return maximum;
    }

    int globalMaximum = 0;

    private int Recurse(Linked<string> path1, Linked<string> path2, Linked<int> released, Linked<Release> releases, int currentTime, GraphNode parent1, GraphNode parent2, bool opened1, bool opened2)
    {
        if (path1.Value != parent1.Name)
            path1 = path1.AddBefore(parent1.Name);

        step++;

        var copyStep = step;

        if (currentTime >= totalTime - 1 || released.Count == availableWorkingVaults.Length)
        {
            var m = CalculatePreasure(releases, totalTime);

            if (m > globalMaximum)
                globalMaximum = m;

            return m;
        }

        var predict = Predict(releases, currentTime);

        if (predict <= globalMaximum)
            return 0;

        var maximum = 0;
        var moved = false;

        if (!opened1 && parent1.Rate > 0 && !released.Contains(parent1.Id))
        {
            var newReleased = released.AddBefore(parent1.Id);
            var newReleases = releases.AddBefore(new(parent1.Name, parent1.Rate, currentTime + 1));

            maximum = Math.Max(maximum, RecurseElephant(path1, path2, newReleased, newReleases, currentTime, parent1, parent2, true, opened2));
        }

        foreach (var child1 in parent1.Connections)
        {
            if (opened1 || child1.Name != path1.Next?.Value)
            {
                maximum = Math.Max(maximum, RecurseElephant(path1, path2, released, releases, currentTime, child1, parent2, false, opened2));
                moved = true;
            }
        }

        if (!moved)
        {
            maximum = Math.Max(maximum, RecurseElephant(path1, path2, released, releases, currentTime, parent1, parent2, false, opened2));
        }

        return maximum;
    }

    private int RecurseElephant(Linked<string> path1, Linked<string> path2, Linked<int> released, Linked<Release> releases, int currentTime, GraphNode parent1, GraphNode parent2, bool opened1, bool opened2)
    {
        if (path2.Value != parent2.Name)
            path2 = path2.AddBefore(parent2.Name);

        var maximum = 0;
        var moved = false;

        if (!opened2 && parent2.Rate > 0 && !released.Contains(parent2.Id))
        {
            var newReleased = released.AddBefore(parent2.Id);
            var newReleases = releases.AddBefore(new(parent2.Name, parent2.Rate, currentTime + 1));

            maximum = Math.Max(maximum, Recurse(path1, path2, newReleased, newReleases, currentTime + 1, parent1, parent2, opened1, true));
        }

        foreach (var child2 in parent2.Connections)
        {
            if (opened2 || child2.Name != path2.Next?.Value)
            {
                maximum = Math.Max(maximum, Recurse(path1, path2, released, releases, currentTime + 1, parent1, child2, opened1, false));
                moved = true;
            }
        }

        if (!moved)
            maximum = Math.Max(maximum, Recurse(path1, path2, released, releases, currentTime + 1, parent1, parent2, opened1, false));

        return maximum;
    }

    //private static int CalculatePreasure(IEnumerable<Release> releases, int currentTime)
    //    => releases.Select(a => a.Rate * (currentTime - a.FromTime)).Sum();

    private static int CalculatePreasure(Linked<Release> releases, int totalTime)
    {
        var p = releases;
        var res = 0;

        while (!p.End)
        {
            res += p.Value!.Rate * (totalTime - p.Value!.FromTime);
            p = p.Next!;
        }

        return res;
    }

    private int Predict(Linked<Release> releases, int currentTime)
    {
        var preasure = 0;

        foreach (var a in availableWorkingVaults)
        {
            var r = releases.FirstOrDefault(b => b.Name == a.Name);
            var t = r is not null ? r.FromTime : currentTime + 1;
            preasure += a.Rate * (totalTime - t);
        }

        //preasure += CalculatePreasure(releases, totalTime);

        return preasure;
    }

    record Release(string Name, int Rate, int FromTime);
}

public record class Item(string Valve, int Rate, string[] Targets);

static partial class CompiledRegs
{
    [GeneratedRegex(@$"^Valve (?<{nameof(Item.Valve)}>\w+) has flow rate=(?<{nameof(Item.Rate)}>\d+); tunnels? leads? to valves? (?<{nameof(Item.Targets)}>\w\w)(, (?<{nameof(Item.Targets)}>\w\w))*$")]
    public static partial Regex Regex();
}
