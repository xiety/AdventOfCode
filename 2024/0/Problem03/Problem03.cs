using System.Text.RegularExpressions;

namespace A2024.Problem03;

public static class Solver
{
    const string CommandDo = "do()";
    const string CommandDont = "don't()";

    [GeneratedTest<int>(161, 170068701)]
    public static int RunA(string[] lines)
        => lines.Sum(line => CompiledRegs.Regex().Matches(line)
                .Where(a => a.Groups[1].Success && a.Groups[2].Success)
                .Select(m => m.MapTo<Item>())
                .Sum(b => b.Left * b.Right));

    [GeneratedTest<int>(48, 78683433)]
    public static int RunB(string[] lines)
    {
        var sum = 0;
        var enabled = true;

        foreach (var match in lines.SelectMany(line => CompiledRegs.Regex().Matches(line)))
        {
            if (match.Value == CommandDo)
            {
                enabled = true;
            }
            else if (match.Value == CommandDont)
            {
                enabled = false;
            }
            else if (enabled)
            {
                var item = match.MapTo<Item>();
                sum += item.Left * item.Right;
            }
        }

        return sum;
    }
}

record Item(int Left, int Right);

static partial class CompiledRegs
{
    [GeneratedRegex(@$"do\(\)|don't\(\)|mul\((?<{nameof(Item.Left)}>\d{{1,3}}),(?<{nameof(Item.Right)}>\d{{1,3}})\)")]
    public static partial Regex Regex();
}
