using System.Text.RegularExpressions;

namespace A2015.Problem05;

public static class Solver
{
    [GeneratedTest<int>(2, 238)]
    public static int RunA(string[] lines)
    {
        char[] vowels = ['a', 'e', 'i', 'o', 'u'];
        string[] bads = ["ab", "cd", "pq", "xy"];

        return lines.Count(line => line.Where(vowels.Contains).Skip(2).Any()
                                && line.Chain().Any(b => b.First == b.Second)
                                && !bads.Any(line.Contains));
    }

    [GeneratedTest<int>(2, 69)]
    public static int RunB(string[] lines)
        => lines.Count(line => line.Index().SkipLast(1).Any(x => line[(x.Index + 2)..].Contains(line[x.Index..(x.Index + 2)]))
                            && line.Index().SkipLast(2).Any(x => x.Item == line[x.Index + 2]));

    [GeneratedTest<int>(2, 238)]
    public static int RunARegex(string[] lines)
        => lines.Count(CompiledRegs.RegexA.IsMatch);

    [GeneratedTest<int>(2, 69)]
    public static int RunBRegex(string[] lines)
        => lines.Count(CompiledRegs.RegexB.IsMatch);
}

static partial class CompiledRegs
{
    [GeneratedRegex(@"^(?=.*(.)\1)(?=.*[aeiou].*[aeiou].*[aeiou])(?!.*(ab|cd|pq|xy))")]
    public static partial Regex RegexA { get; }

    [GeneratedRegex(@"^(?=.*(..).*\1)(?=.*(.).\2)")]
    public static partial Regex RegexB { get; }
}
