using System.Text.RegularExpressions;

using Advent.Common;

namespace A2023.Problem08;

public static class Solver
{
    [GeneratedTest<long>(6, 14893)]
    public static long RunA(string[] lines)
    {
        var (path, nodes) = LoadData(lines);

        var step = 0;
        var currentNode = nodes.First(a => a.Name == "AAA");

        do
        {
            var pathIndex = step % path.Length;
            currentNode = nodes.First(a => a.Name == currentNode.Outputs[path[pathIndex]]);
            step++;
        }
        while (currentNode.Name != "ZZZ");

        return step;
    }

    [GeneratedTest<long>(6, 10241191004509)]
    public static long RunB(string[] lines)
    {
        var (path, nodes) = LoadData(lines);

        var currentNodes = nodes.Where(a => a.Name.EndsWith('A')).ToArray();
        var all = new List<List<int>>();

        foreach (var start in currentNodes)
        {
            var step = 0;
            var currentNode = start;
            var list = new List<int>();
            var already = new HashSet<(Node, int)>();

            do
            {
                var pathIndex = step % path.Length;

                if (!already.Add((currentNode, pathIndex)))
                    break;

                currentNode = nodes.First(b => b.Name == currentNode.Outputs[path[pathIndex]]);
                step++;

                if (currentNode.Name.EndsWith('Z'))
                    list.Add(step);
            }
            while (true);

            all.Add(list);
        }

        return Math.LCM(all.Select(a => (long)a.Max())); //little cheat with max
    }

    static (int[] path, Node[]) LoadData(string[] lines)
    {
        var path = lines[0].ToArray(a => a switch { 'L' => 0, 'R' => 1 });
        var nodes = CompiledRegs.FromLinesRegex(lines[2..]);
        return (path, nodes);
    }
}

public record Node(string Name, string[] Outputs);

static partial class CompiledRegs
{
    [GeneratedRegex(@$"^(?<{nameof(Node.Name)}>\w+) = \(((?<{nameof(Node.Outputs)}>\w+), )*(?<{nameof(Node.Outputs)}>\w+)\)$")]
    [MapTo<Node>]
    public static partial Regex Regex();
}
