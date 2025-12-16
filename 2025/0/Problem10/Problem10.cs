using System.Text.RegularExpressions;

namespace A2025.Problem10;

public static class Solver
{
    [GeneratedTest<long>(7, 505)]
    public static long RunARecursion(string[] lines)
        => LoadData(lines).Sum(CalcRecursion);

    static int CalcRecursion(Item item)
    {
        var target = item.Lights
            .Select((c, i) => c == '#' ? 1 << i : 0)
            .Aggregate(0, (acc, bit) => acc | bit);

        var buttons = item.Buttons
            .ToArray(b => b.Aggregate(0, (mask, lightIdx) => mask | (1 << lightIdx)));

        return Memoization.RunRecursive<int, int, int>(0, 0,
            (recurse, index, mask) => index == buttons.Length
                    ? ((mask == target) ? 0 : short.MaxValue)
                    : Math.Min(
                        recurse(index + 1, mask),
                        recurse(index + 1, mask ^ buttons[index]) + 1));
    }

    [GeneratedTest<long>(7, 505)]
    public static long RunA(string[] lines)
        => LoadData(lines).Sum(CalcA);

    static int CalcA(Item item)
    {
        var target = item.Lights.Index()
            .Where(x => x.Item == '#').Select(x => x.Index).ToHashSet();

        return Enumerable.Range(0, 1 << item.Buttons.Length)
            .Select(index => item.Buttons
                .Where((_, i) => (index & (1 << i)) != 0)
                .ToList())
            .Select(subset => (
                subset.Count,
                Lights: subset
                    .SelectMany(b => b)
                    .GroupBy(i => i)
                    .Where(g => g.Count() % 2 != 0)
                    .Select(g => g.Key)
                    .ToHashSet()))
            .Where(x => x.Lights.SetEquals(target))
            .Min(x => x.Count);
    }

    [GeneratedTest<long>(33, 20002)]
    public static long RunB(string[] lines)
        => LoadData(lines).Sum(CalcB);

    static long CalcB(Item item)
    {
        var matrix = item.Jolts.ToArray((_, index) => item.Buttons
            .ToArray(b => b.Contains(index) ? 1 : 0));

        return LinearSolver.Run(item)!.Sum();
    }

    static Item[] LoadData(string[] lines)
        => lines.ToArray(a =>
        {
            var m = CompiledRegs.Regex().Match(a);
            var lights = m.Groups[nameof(Item.Lights)].Value;
            var buttons = m.Groups[nameof(Item.Buttons)].Captures.ToArray(a => a.Value.Split(",").ToArray(int.Parse));
            var jolts = m.Groups[nameof(Item.Jolts)].Value.Split(",").ToArray(int.Parse);

            return new Item(lights, buttons, jolts);
        });
}

record Item(string Lights, int[][] Buttons, int[] Jolts);

static partial class CompiledRegs
{
    [GeneratedRegex(@$"^\[(?<{nameof(Item.Lights)}>.+?)\] (\((?<{nameof(Item.Buttons)}>.+?)\) )+\{{(?<{nameof(Item.Jolts)}>.+?)\}}$")]
    public static partial Regex Regex();
}
