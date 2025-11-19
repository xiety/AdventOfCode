using System.Text.RegularExpressions;

using Advent.Common;

namespace A2020.Problem07;

public class Solver : ISolver<int>
{
    const string rootName = "shiny gold";

    public int RunA(string[] lines, bool isSample)
        => LoadItems(lines).Values.Count(CheckRecurse);

    public int RunB(string[] lines, bool isSample)
        => CountRecurse(LoadItems(lines)[rootName]) - 1;

    static bool CheckRecurse(Item parent)
        => parent.Receipts.Any(a => a.Item.Name == rootName || CheckRecurse(a.Item));

    static int CountRecurse(Item parent)
        => 1 + parent.Receipts.Sum(a => a.Value * CountRecurse(a.Item));

    static Dictionary<string, Item> LoadItems(string[] lines)
    {
        var dic2 = lines.Select(ParseItem2).ToDictionary(a => a.Name);
        var cache = new Dictionary<string, Item>();
        foreach (var item2 in dic2.Values)
            ConvertRecurse(cache, dic2, item2);
        return cache;
    }

    static Item ConvertRecurse(Dictionary<string, Item> cache, Dictionary<string, Item2> dic2, Item2 item2)
        => cache.GetOrCreate(item2.Name, () => new Item(item2.Name, item2.Receipts
                .ToArray(a => new Receipt(a.Value, ConvertRecurse(cache, dic2, dic2[a.Name])))));

    static Item2 ParseItem2(string text)
    {
        var raw = CompiledRegs.Regex1().MapTo<Item1>(text);
        var receipts = CompiledRegs.Regex2().Matches(raw.To).ToArray(a => a.MapTo<Receipt2>());
        return new(raw.From, receipts);
    }
}

record Item1(string From, string To);
record Receipt2(int Value, string Name);
record Item2(string Name, Receipt2[] Receipts);

record Receipt(int Value, Item Item);
record Item(string Name, Receipt[] Receipts);

static partial class CompiledRegs
{
    [GeneratedRegex(@$"^(?<{nameof(Item1.From)}>.+) bags contain (?<{nameof(Item1.To)}>.+)$")]
    public static partial Regex Regex1();

    [GeneratedRegex(@$"(?<{nameof(Receipt2.Value)}>\d+) (?<{nameof(Receipt2.Name)}>.+?) bags?")]
    public static partial Regex Regex2();
}
