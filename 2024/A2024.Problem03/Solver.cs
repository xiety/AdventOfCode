using System.Text.RegularExpressions;

using Advent.Common;

namespace A2024.Problem03;

public class Solver : ISolver<int>
{
    const string CommandDo = "do()";
    const string CommandDont = "don't()";

    public int RunA(string[] lines, bool isSample)
    {
        var regex = CompiledRegs.Regex();

        return lines.Sum(line => regex.Matches(line)
                .Where(a => a.Groups[1].Success && a.Groups[2].Success)
                .Select(m => m.MapTo<Item>())
                .Sum(b => b.Left * b.Right));
    }

    public int RunB(string[] lines, bool isSample)
    {
        var regex = CompiledRegs.Regex();
        var sum = 0;
        var enabled = true;

        foreach (var match in lines.SelectMany(line => regex.Matches(line)))
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
