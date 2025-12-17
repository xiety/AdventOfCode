using System.Text.RegularExpressions;

namespace A2020.Problem14;

using Mask = bool?[];

public static class Solver
{
    [GeneratedTest<long>(165, 17028179706934)]
    public static long RunA(string[] lines)
        => ParseGroups(lines)
            .SelectMany(a => a.Values.Select(b => (Mask: a.Key, b.Address, b.Value)))
            .GroupBy(a => a.Address)
            .Select(a => a.Last())
            .Sum(a => ApplyMask(a.Value, a.Mask, b => b is not false));

    [GeneratedTest<long>(208, 3683236147222)]
    public static long RunB(string[] lines)
        => ParseGroups(lines)
            .SelectMany(a => Spread(a.Key, a.Values))
            .GroupBy(a => a.Address)
            .Select(a => a.Last())
            .Sum(a => a.Value);

    static IEnumerable<ItemSet> Spread(Mask mask, ItemSet[] values)
        => Spread(mask, values, mask.Index().Where(a => a.Item is null).ToArray(a => a.Index));

    static IEnumerable<ItemSet> Spread(Mask mask, ItemSet[] values, int[] indexes)
        => values
            .Select(a => (a.Value, Masked: ApplyMask(a.Address, mask, b => b is not null)))
            .SelectMany(a => Enumerable.BinaryCounting<long>(indexes.Length)
                .Select(b => new ItemSet(GenerateAddress(a.Masked, indexes, b), a.Value)));

    static long GenerateAddress(long masked, int[] indexes, long[] bits)
        => bits
            .Zip(indexes)
            .Select(a => a.First << a.Second)
            .Aggregate(masked, (acc, a) => acc | a);

    static long CreateLong(Mask mask, Func<bool?, bool> predicate)
        => mask.Index().Aggregate(0L, (acc, a) => predicate(a.Item) ? acc | (1L << a.Index) : acc);

    static long ApplyMask(long value, Mask mask, Func<bool?, bool> predicate)
        => (value | CreateLong(mask, a => a is true)) & CreateLong(mask, predicate);

    static IEnumerable<HeaderGrouping<Mask, ItemSet>> ParseGroups(string[] lines)
        => lines
            .Select(ParseLine)
            .GroupByHeader(a => a as Mask, a => a as ItemSet);

    static object ParseLine(string line)
        => CompiledRegs.TryMapToRegexMask(line, out var itemMaskRaw)
            ? ParseMask(itemMaskRaw)
            : CompiledRegs.MapToRegexSet(line);

    static Mask ParseMask(string text)
        => text.Select(a => a switch { 'X' => (bool?)null, '1' => true, '0' => false }).Reverse().ToArray();
}

record ItemSet(long Address, long Value);

static partial class CompiledRegs
{
    [GeneratedRegex(@$"^mask = (.+)$")]
    [MapTo<string>]
    public static partial Regex RegexMask();

    [GeneratedRegex(@$"^mem\[(?<{nameof(ItemSet.Address)}>\d+)\] = (?<{nameof(ItemSet.Value)}>\d+)$")]
    [MapTo<ItemSet>]
    public static partial Regex RegexSet();
}
