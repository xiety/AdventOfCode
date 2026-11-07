using System.Text.RegularExpressions;

namespace A2015.Problem15;

public static class Solver
{
    [GeneratedTest<int>(62842880, 18965440)]
    public static int RunA(string[] lines)
    {
        var items = CompiledRegs.FromLinesRegex(lines);
        var data = CollectData(items);

        return Fors.Compositions(100, data[0].Length)
            .Max(array => data.Mul(a => Score(a, array)));
    }

    [GeneratedTest<int>(57600000, 15862900)]
    public static int RunB(string[] lines)
    {
        var items = CompiledRegs.FromLinesRegex(lines);
        var data = CollectData(items);
        var cals = items.ToArray(a => a.Calories);

        return Fors.Compositions(100, items.Length)
            .Where(array => Score(cals, array) == 500)
            .Max(array => data.Mul(a => Score(a, array)));
    }

    static int Score(int[] counts, int[] item)
        => Math.Max(0, counts.Zip(item).Sum(p => p.First * p.Second));

    static int[][] CollectData(ItemRaw[] items)
        => items
            .ToArray(a => (int[])[a.Capacity, a.Durability, a.Flavor, a.Texture])
            .Transposed();
}

record ItemRaw(int Capacity, int Durability, int Flavor, int Texture, int Calories);

static partial class CompiledRegs
{
    [GeneratedRegex(@$"^.+: capacity (?<{nameof(ItemRaw.Capacity)}>-?\d+), durability (?<{nameof(ItemRaw.Durability)}>-?\d+), flavor (?<{nameof(ItemRaw.Flavor)}>-?\d+), texture (?<{nameof(ItemRaw.Texture)}>-?\d+), calories (?<{nameof(ItemRaw.Calories)}>-?\d+)$")]
    [MapTo<ItemRaw>]
    public static partial Regex Regex();
}
