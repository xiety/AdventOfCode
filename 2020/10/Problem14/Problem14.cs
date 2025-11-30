using System.Collections;
using System.Text.RegularExpressions;

using Advent.Common;

namespace A2020.Problem14;

public static class Solver
{
    [GeneratedTest<long>(165, 17028179706934)]
    public static long RunA(string[] lines)
        => ParseGroups(lines)
            .SelectMany(a => a.Values.Select(b => (a.Key.Mask, b.Address, b.Value)))
            .GroupBy(a => a.Address)
            .Select(a => a.Last())
            .Sum(a => ApplyMask(a.Mask, a.Value));

    static long ApplyMask(bool?[] mask, long value)
    {
        var valueBits = new BitArray(BitConverter.GetBytes(value));

        for (var i = 0; i < mask.Length; ++i)
            if (mask[i] is bool m)
                valueBits.Set(i, m);

        return BitConverter.ToInt64(valueBits.ToBytes());
    }

    static IEnumerable<HeaderGrouping<ItemMask, ItemSet>> ParseGroups(string[] lines)
        => lines
            .Select(ParseLine)
            .GroupByHeader(a => a as ItemMask, a => a as ItemSet);

    static Item ParseLine(string line)
        => CompiledRegs.TryMapToRegexMask(line, out var itemMaskRaw)
            ? new ItemMask(itemMaskRaw.Mask.Select(a => a switch { 'X' => (bool?)null, '1' => true, '0' => false }).Reverse().ToArray())
            : CompiledRegs.MapToRegexSet(line);
}

record Item();
record ItemMask(bool?[] Mask) : Item;
record ItemSet(long Address, long Value) : Item;
record ItemMaskRaw(string Mask);

static partial class CompiledRegs
{
    [GeneratedRegex(@$"^mask = (?<{nameof(ItemMaskRaw.Mask)}>.+)$")]
    [MapTo<ItemMaskRaw>]
    public static partial Regex RegexMask();

    [GeneratedRegex(@$"^mem\[(?<{nameof(ItemSet.Address)}>\d+)\] = (?<{nameof(ItemSet.Value)}>\d+)$")]
    [MapTo<ItemSet>]
    public static partial Regex RegexSet();
}
