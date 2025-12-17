using System.Text.RegularExpressions;

namespace A2025.Problem02;

public static class Solver
{
    [GeneratedTest<long>(1227775554, 21898734247)]
    public static long RunA(string[] lines)
        => Run(lines, a => IsValid(a, true));

    [GeneratedTest<long>(4174379265, 28915664389)]
    public static long RunB(string[] lines)
        => Run(lines, a => IsValid(a, false));

    [GeneratedTest<long>(1227775554, 21898734247)]
    public static long RunARegex(string[] lines)
        => Run(lines, a => !CompiledRegs.CheckA().IsMatch(a.ToString()));

    [GeneratedTest<long>(4174379265, 28915664389)]
    public static long RunBRegex(string[] lines)
        => Run(lines, a => !CompiledRegs.CheckB().IsMatch(a.ToString()));

    static long Run(string[] lines, Func<long, bool> isValid)
        => LoadData(lines)
            .SelectMany(a => Enumerable.RangeTo(a.From, a.To + 1))
            .Where(a => !isValid(a))
            .Sum();

    static bool IsValid(long n, bool limit)
        => IsValid(n.ToString(), limit);

    static bool IsValid(string text, bool limit)
        => Enumerable.Primes((limit ? 2 : text.Length) + 1)
            .Select(a => Math.DivRem(text.Length, a))
            .Where(a => a.Remainder == 0)
            .Select(a => a.Quotient)
            .All(size => Enumerable.RangeTo(size, text.Length - size + 1, size)
                .Any(a => text.Substring(a, size) != text[..size]));

    static Item[] LoadData(string[] lines)
        => lines.ToArrayMany(a => a.Split(",", StringSplitOptions.RemoveEmptyEntries)
            .ToArray(CompiledRegs.MapToRegex));
}

record Item(long From, long To);

static partial class CompiledRegs
{
    [GeneratedRegex(@$"^(?<{nameof(Item.From)}>\d+)\-(?<{nameof(Item.To)}>\d+)$")]
    [MapTo<Item>]
    public static partial Regex Regex();

    [GeneratedRegex(@$"^(\d+)\1$")]
    public static partial Regex CheckA();

    [GeneratedRegex(@$"^(\d+)\1+$")]
    public static partial Regex CheckB();
}
