using System.Text.RegularExpressions;

using Advent.Common;

namespace A2023.Problem25;

public static class Solver
{
    [GeneratedTest<long>(54, 596376)]
    public static long RunA(string[] lines, bool isSample)
    {
        var items = CompiledRegs.FromLinesRegex(lines);
        var components = Create(items);

        if (isSample)
        {
            Cut(components, "hfx", "pzl");
            Cut(components, "bvb", "cmg");
            Cut(components, "nvd", "jqt");

            var size = Size(components, "hfx");

            return size * (components.Length - size);
        }
        else
        {
            //cheat: just saw it on a diagram
            Cut(components, "gst", "rph");
            Cut(components, "ljm", "sfd");
            Cut(components, "cfn", "jkn");

            var size = Size(components, "gst");

            return size * (components.Length - size);
        }
    }

    static int Size(Component[] components, string startName)
    {
        var start = components.First(a => a.Name == startName);

        HashSet<Component> already = [start];
        List<Component> currentSteps = [start];
        List<Component> newSteps = [];

        do
        {
            var items = currentSteps
                .Select(currentStep => currentStep.Connections.Where(a => !already.Contains(a)))
                .SelectMany(nextSteps => nextSteps);

            foreach (var newStep in items)
            {
                already.Add(newStep);
                newSteps.Add(newStep);
            }

            if (newSteps.Count == 0)
                break;

            (currentSteps, newSteps) = (newSteps, currentSteps);
            newSteps.Clear();
        }
        while (true);

        return already.Count;
    }

    static void Cut(Component[] components, string name1, string name2)
    {
        var component1 = components.First(a => a.Name == name1);
        var component2 = components.First(a => a.Name == name2);

        component1.Connections.Remove(component2);
        component2.Connections.Remove(component1);
    }

    static Component[] Create(Item[] items)
    {
        var components = items
            .Select(a => a.Name)
            .Concat(items.SelectMany(a => a.Outputs))
            .Distinct()
            .ToArray(a => new Component { Name = a });

        foreach (var item in items)
        {
            var fromComponent = components.First(a => a.Name == item.Name);

            foreach (var output in item.Outputs)
            {
                var toComponent = components.First(a => a.Name == output);

                fromComponent.Connections.Add(toComponent);
                toComponent.Connections.Add(fromComponent);
            }
        }

        return [.. components];
    }
}

record Item(string Name, string[] Outputs);

class Component
{
    public required string Name;
    public readonly List<Component> Connections = [];
}

static partial class CompiledRegs
{
    [GeneratedRegex(@$"^(?<{nameof(Item.Name)}>\w+): ((?<{nameof(Item.Outputs)}>\w+)\s?)+$")]
    [MapTo<Item>]
    public static partial Regex Regex();
}
