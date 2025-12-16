using System.Text.RegularExpressions;

namespace A2020.Problem02;

public static class Solver
{
    [GeneratedTest<int>(2, 638)]
    public static int RunA(string[] lines)
        => Run(lines, CheckA);

    [GeneratedTest<int>(1, 699)]
    public static int RunB(string[] lines)
        => Run(lines, CheckB);

    static int Run(string[] lines, Func<Item, bool> predicate)
        => LoadData(lines).Count(predicate);

    static bool CheckA(Item item)
    {
        var num = item.Text.Count(a => a == item.Letter);
        return num >= item.From && num <= item.To;
    }

    static bool CheckB(Item a)
    {
        return Check(a.From - 1) ^ Check(a.To - 1);
        bool Check(int pos) => pos < a.Text.Length && a.Text[pos] == a.Letter;
    }

    static Item[] LoadData(string[] lines)
        => CompiledRegs.FromLinesRegex(lines);
}

record Item(int From, int To, char Letter, string Text);

static partial class CompiledRegs
{
    [GeneratedRegex(@$"^(?<{nameof(Item.From)}>\d+)-(?<{nameof(Item.To)}>\d+)\s(?<{nameof(Item.Letter)}>.)\:\s(?<{nameof(Item.Text)}>.+)$")]
    [MapTo<Item>]
    public static partial Regex Regex();
}
