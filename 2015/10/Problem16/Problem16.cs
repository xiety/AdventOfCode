using System.Text.RegularExpressions;

namespace A2015.Problem16;

using Ops = Dictionary<string, Func<int, int, bool>>;
using Container = Dictionary<string, int>;

public static class Solver
{
    [GeneratedTest<int>(1, 103)]
    public static int RunA(string[] lines)
        => Run(lines, []);

    [GeneratedTest<int>(2, 405)]
    public static int RunB(string[] lines)
        => Run(lines, OpsB);

    static int Run(string[] lines, Ops ops)
        => Search(SearchData, ops, LoadItems(lines));

    static int Search(Container searches, Ops ops, Item[] items)
        => items.First(a => searches.All(b => CheckLine(ops, a, b))).Number;

    static bool CheckLine(Ops ops, Item item, KeyValuePair<string, int> search)
    {
        if (item.Lines.TryGetValue(search.Key, out var v))
        {
            var op = ops.GetValueOrDefault(search.Key, (a, b) => a == b);

            if (!op(v, search.Value))
                return false;
        }

        return true;
    }

    static Container SearchData
        => new()
        {
            ["children"] = 3,
            ["cats"] = 7,
            ["samoyeds"] = 2,
            ["pomeranians"] = 3,
            ["akitas"] = 0,
            ["vizslas"] = 0,
            ["goldfish"] = 5,
            ["trees"] = 3,
            ["cars"] = 2,
            ["perfumes"] = 1,
        };

    static Ops OpsB
        => new()
        {
            ["cats"] = (a, b) => a > b,
            ["pomeranians"] = (a, b) => a < b,
            ["goldfish"] = (a, b) => a < b,
            ["trees"] = (a, b) => a > b,
        };

    static Item[] LoadItems(string[] lines)
        => CompiledRegs.FromLinesRegex(lines)
            .ToArray(a => new Item(a.Number, a.Names.Zip(a.Values).ToDictionary()));
}

record Item(int Number, Container Lines);
record ItemRaw(int Number, string[] Names, int[] Values);

static partial class CompiledRegs
{
    [GeneratedRegex(@$"^Sue (?<{nameof(ItemRaw.Number)}>\d+): ((?<{nameof(ItemRaw.Names)}>[^:]+): (?<{nameof(ItemRaw.Values)}>\d+)(,\s)?)*$")]
    [MapTo<ItemRaw>]
    public static partial Regex Regex { get; }
}
