using System.Text.RegularExpressions;

using Advent.Common;

namespace A2023.Problem08;

public class Solver : IProblemSolver<long>
{
    public long RunA(string filename)
    {
        var (path, nodes) = LoadFile(filename);

        var step = 0;
        var current_node = nodes.First(a => a.Name == "AAA");

        do
        {
            var path_index = step % path.Length;
            current_node = nodes.First(a => a.Name == current_node.Outputs[path[path_index]]);
            step++;
        }
        while (current_node.Name != "ZZZ");

        return step;
    }

    public long RunB(string filename)
    {
        var (path, nodes) = LoadFile(filename);

        var current_nodes = nodes.Where(a => a.Name.EndsWith('A')).ToArray();
        var all = new List<List<int>>();

        foreach (var start in current_nodes)
        {
            var step = 0;
            var current_node = start;
            var list = new List<int>();
            var already = new List<(Node, int)>();

            do
            {
                var path_index = step % path.Length;

                if (already.Contains((current_node, path_index)))
                    break;

                already.Add((current_node, path_index));

                current_node = nodes.First(b => b.Name == current_node.Outputs[path[path_index]]);
                step++;

                if (current_node.Name.EndsWith('Z'))
                    list.Add(step);
            }
            while (true);

            all.Add(list);
        }

        return MathExtensions.Lcm(all.Select(a => a.Max())); //little cheat with max
    }

    private static (int[] path, List<Node>) LoadFile(string filename)
    {
        var lines = File.ReadAllLines(filename);
        var path = lines[0].Select(a => a switch { 'L' => 0, 'R' => 1 }).ToArray();
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
