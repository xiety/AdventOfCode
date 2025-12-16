using System.Text.RegularExpressions;

namespace A2023.Problem03;

public static class Solver
{
    [GeneratedTest<int>(4361, 539713)]
    public static int RunA(string[] lines)
    {
        var width = lines[0].Length;
        var height = lines.Length;

        return lines.Index()
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
    }

    [GeneratedTest<int>(467835, 84159075)]
    public static int RunB(string[] lines)
    {
        var width = lines[0].Length;
        var height = lines.Length;

        return lines.Index()
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
    }
}

static partial class CompiledRegs
{
    [GeneratedRegex(@$"\d+")]
    public static partial Regex Regex();
}
