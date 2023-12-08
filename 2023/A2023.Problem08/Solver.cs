using System.Text.RegularExpressions;

using Advent.Common;

namespace A2023.Problem08;

public class Solver : IProblemSolver<long>
{
    public long RunA(string filename)
    {
        var lines = File.ReadAllLines(filename);
        var path = lines[0].Select(a => a switch { 'L' => 0, 'R' => 1 }).ToArray();
        var nodes = CompiledRegs.Regex().FromLines<Node>(lines[2..]);

        var step = 0;
        var current_node = "AAA";

        do
        {
            var node = nodes.First(a => a.Name == current_node);
            current_node = node.Outputs[path[step % path.Length]];
            step++;
        }
        while (current_node != "ZZZ");

        return step;
    }
}

public record Node(string Name, string[] Outputs);

static partial class CompiledRegs
{
    [GeneratedRegex(@$"^(?<{nameof(Node.Name)}>\w+) = \(((?<{nameof(Node.Outputs)}>\w+), )*(?<{nameof(Node.Outputs)}>\w+)\)$")]
    public static partial Regex Regex();
}
