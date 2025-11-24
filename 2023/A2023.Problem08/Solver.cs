using System.Text.RegularExpressions;

using Advent.Common;

namespace A2023.Problem08;

public class Solver : IProblemSolver<long>
{
    public long RunA(string filename)
    {
        var (path, nodes) = LoadFile(filename);

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

    public long RunB(string filename)
    {
        var (path, nodes) = LoadFile(filename);

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

        return Math.Lcm(all.Select(a => a.Max())); //little cheat with max
    }

    static (int[] path, Node[]) LoadFile(string filename)
    {
        var lines = File.ReadAllLines(filename);
        var path = lines[0].ToArray(a => a switch { 'L' => 0, 'R' => 1 });
        var nodes = CompiledRegs.Regex().FromLines<Node>(lines[2..]);
        return (path, nodes);
    }
}

public record Node(string Name, string[] Outputs);

static partial class CompiledRegs
{
    [GeneratedRegex(@$"^(?<{nameof(Node.Name)}>\w+) = \(((?<{nameof(Node.Outputs)}>\w+), )*(?<{nameof(Node.Outputs)}>\w+)\)$")]
    public static partial Regex Regex();
}
