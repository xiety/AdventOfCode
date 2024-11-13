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

        var result = lines.Indexed()
            .SelectMany(a =>
                CompiledRegs.Regex().Matches(a.item)
                    .Where(m => Fors.For((m.Index - 1, m.Index + m.Length + 1), (a.index - 1, a.index + 2))
                        .Select(b => (px: b[0], py: b[1]))
                        .Where(pos => pos.py >= 0 && pos.py < height && pos.px >= 0 && pos.px < width)
                        .Any(pos => lines[pos.py][pos.px]
                            is not ('.' or '0' or '1' or '2' or '3' or '4' or '5' or '6' or '7' or '8' or '9')))
                    .Select(m => int.Parse(m.Value))
            )
            .Sum();

        return result;
    }

    public int RunB(string filename)
    {
        var lines = File.ReadAllLines(filename);

        var width = lines[0].Length;
        var height = lines.Length;

        var result = lines.Indexed()
            .SelectMany(a =>
                CompiledRegs.Regex().Matches(a.item)
                    .SelectMany(m =>
                        Fors.For((m.Index - 1, m.Index + m.Length + 1), (a.index - 1, a.index + 2))
                            .Select(b => (px: b[0], py: b[1]))
                            .Where(pos => pos.py >= 0 && pos.py < height && pos.px >= 0 && pos.px < width)
                            .Where(pos => lines[pos.py][pos.px] is '*')
                            .Select(pos => (pos, value: int.Parse(m.Value)))))
            .GroupBy(a => a.pos)
            .Where(a => a.Count() == 2)
            .Select(a => a.Mul(b => b.value))
            .Sum();

        return result;
    }
}

static partial class CompiledRegs
{
    [GeneratedRegex(@$"\d+")]
    public static partial Regex Regex();
}
