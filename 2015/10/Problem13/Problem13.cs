using System.Text.RegularExpressions;

namespace A2015.Problem13;

public static class Solver
{
    [GeneratedTest<int>(330, 709)]
    public static int RunA(string[] lines)
        => Run(lines, 0);

    [GeneratedTest<int>(286, 668)]
    public static int RunB(string[] lines)
        => Run(lines, 1);

    static int Run(string[] lines, int additional)
    {
        var items = LoadItems(lines);
        var scores = CreateScores(items, additional);
        return Recurse([], scores.GetLength(0), scores);
    }

    static int Recurse(int[] table, int num, int[,] scores)
    {
        if (table.Length == num)
            return Score(table, scores);

        return Enumerable.Range(0, num)
            .Where(a => !table.Contains(a))
            .Max(a => Recurse([.. table, a], num, scores));
    }

    static int[,] CreateScores(ItemRaw[] items, int additional)
    {
        var peopleDict = items.Select(a => a.From)
            .Distinct()
            .Select((a, i) => (a, i))
            .ToDictionary();

        var num = peopleDict.Count + additional;

        var scores = new int[num, num];

        foreach (var item in items)
        {
            var from = peopleDict[item.From];
            var to = peopleDict[item.To];

            scores[from, to] = item.Mode == "gain" ? item.Quantity : -item.Quantity;
        }

        return scores;
    }

    static int Score(int[] table, int[,] scores)
        => table
            .Select(a => (table[a], table[Math.Mod(a + 1, table.Length)]))
            .Sum(a => scores[a.Item1, a.Item2] + scores[a.Item2, a.Item1]);

    static ItemRaw[] LoadItems(string[] lines)
        => CompiledRegs.FromLinesRegex(lines);
}

record ItemRaw(string From, string To, string Mode, int Quantity);

static partial class CompiledRegs
{
    [GeneratedRegex(@$"^(?<{nameof(ItemRaw.From)}>.+) would (?<{nameof(ItemRaw.Mode)}>(lose|gain)) (?<{nameof(ItemRaw.Quantity)}>\d+) happiness units by sitting next to (?<{nameof(ItemRaw.To)}>.+)\.$")]
    [MapTo<ItemRaw>]
    public static partial Regex Regex();
}
