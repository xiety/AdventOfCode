using System.Text.RegularExpressions;

namespace A2015.Problem19;

public static class Solver
{
    [GeneratedTest<int>(7, 535)]
    public static int RunA(string[] lines)
        => Parse(lines).Apply(data => data.Transforms
            .SelectMany(transform => Mutate(data.Input, transform.From, transform.To))
            .Distinct().Count());

    static IEnumerable<string> Mutate(string input, string from, string to)
        => input.AllIndexesOf(from)
            .Select(a => Replace(input, from.Length, to, a));

    static string Replace(string input, int fromSize, string to, int n)
        => string.Concat(input.AsSpan(0, n), to.AsSpan(), input.AsSpan(n + fromSize));

    [GeneratedTest<int>(6, 212)]
    public static int RunB(string[] lines)
    {
        var (rules, input) = Parse(lines);
        var reverse = rules
            .OrderByDescending(r => r.To.Length)
            .ToArray();

        var step = 0;
        var current = input;

        do
        {
            var rule = reverse.First(r => current.Contains(r.To, StringComparison.Ordinal));
            var n = current.IndexOf(rule.To, StringComparison.Ordinal);
            current = Replace(current, rule.To.Length, rule.From, n);
            step++;
        }
        while (current != "e");

        return step;
    }

    static ((string From, string To)[] Transforms, string Input) Parse(string[] lines)
    {
        var (a, b) = lines.SplitBy(String.Empty);
        var transforms = a.ToArray(c => CompiledRegs.Regex.Match(c).Apply(d => (d[1], d[2])));
        return (transforms, b[0]);
    }
}

static partial class CompiledRegs
{
    [GeneratedRegex(@$"^(.+) => (.+)$")]
    public static partial Regex Regex { get; }
}
