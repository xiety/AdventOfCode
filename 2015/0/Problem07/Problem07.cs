using System.Numerics;
using System.Text.RegularExpressions;

namespace A2015.Problem07;

public static class Solver
{
    [GeneratedTest<ushort>(65079, 3176)]
    public static ushort RunA(string[] lines, bool isSample)
    {
        var items = LoadItems(lines);
        return Calc<ushort>(items, isSample ? "i" : "a");
    }

    [GeneratedTest<ushort>(456, 14710)]
    public static ushort RunB(string[] lines, bool isSample)
    {
        var items = LoadItems(lines);
        var value = Calc<ushort>(items, isSample ? "i" : "a");

        items.Replace(isSample ? "y" : "b", a => a with { Right = Convert.ToString(value) });

        return Calc<ushort>(items, isSample ? "i" : "a");
    }

    static T Calc<T>(Dictionary<string, Item> items, string outputName)
        where T : IBinaryInteger<T>, IParsable<T>
        => Memoization.RunRecursive<string, T>(outputName, (memo, outputName) =>
        {
            if (T.TryParse(outputName, null, out var value))
                return value;

            var item = items[outputName];

            return item.Operation switch
            {
                "AND" => memo(item.Left) & memo(item.Right),
                "OR" => memo(item.Left) | memo(item.Right),
                "RSHIFT" => memo(item.Left) >> int.CreateChecked(memo(item.Right)),
                "LSHIFT" => memo(item.Left) << int.CreateChecked(memo(item.Right)),
                "NOT" => ~memo(item.Right),
                _ => memo(item.Right),
            };
        });

    static Dictionary<string, Item> LoadItems(string[] lines)
        => CompiledRegs.FromLinesRegex(lines).ToDictionary(a => a.Output);
}

record Item(string Left, string Operation, string Right, string Output);

static partial class CompiledRegs
{
    [GeneratedRegex(@$"^((?<{nameof(Item.Left)}>[a-z0-9]+) )?((?<{nameof(Item.Operation)}>[A-Z]+) )?(?<{nameof(Item.Right)}>[a-z0-9]+) -\> (?<{nameof(Item.Output)}>[a-z]+)")]
    [MapTo<Item>]
    public static partial Regex Regex();
}
