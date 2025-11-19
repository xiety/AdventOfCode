using System.Text.RegularExpressions;

using Advent.Common;

namespace A2023.Problem03;

public class Solver : IProblemSolver<int>
{
    public int RunA(string filename)
    {
        var lines = File.ReadAllLines(filename);

        var width = lines[0].Length;
        var height = lines.Length;

#pragma warning disable RCS1077 // Optimize LINQ method call
        var result = lines.Index()
            .SelectMany(a =>
                CompiledRegs.Regex().Matches(a.Item)
                    .Where(m => Fors.For((m.Index - 1, m.Index + m.Length + 1), (a.Index - 1, a.Index + 2))
                        .Select(b => (px: b[0], py: b[1]))
                        .Where(pos => pos.py >= 0 && pos.py < height && pos.px >= 0 && pos.px < width)
                        .Any(pos => lines[pos.py][pos.px]
                            is not ('.' or '0' or '1' or '2' or '3' or '4' or '5' or '6' or '7' or '8' or '9')))
                    .Select(m => int.Parse(m.Value))
            )
            .Sum();
#pragma warning restore RCS1077 // Optimize LINQ method call

        return result;
    }

    public int RunB(string filename)
    {
        var lines = File.ReadAllLines(filename);

        var width = lines[0].Length;
        var height = lines.Length;

        var result = lines.Index()
            .SelectMany(a =>
                CompiledRegs.Regex().Matches(a.Item)
                    .SelectMany(m =>
                        Fors.For((m.Index - 1, m.Index + m.Length + 1), (a.Index - 1, a.Index + 2))
                            .Select(b => (px: b[0], py: b[1]))
                            .Where(pos => pos.py >= 0 && pos.py < height && pos.px >= 0 && pos.px < width)
                            .Where(pos => lines[pos.py][pos.px] is '*')
                            .Select(pos => (pos, value: int.Parse(m.Value)))))
            .GroupBy(a => a.pos)
            .Where(a => a.Count() == 2)
            .Sum(a => a.Mul(b => b.value));

        return result;
    }
}

static partial class CompiledRegs
{
    [GeneratedRegex(@$"\d+")]
    public static partial Regex Regex();
}
