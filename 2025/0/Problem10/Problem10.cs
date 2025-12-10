using System.Text.RegularExpressions;

using Advent.Common;

namespace A2025.Problem10;

public static class Solver
{
    [GeneratedTest<long>(7, 505)]
    public static long RunA(string[] lines)
        => LoadData(lines).Sum(CalcA);

    static int CalcA(Item item)
    {
        var best = int.MaxValue;
        var start = new string('.', item.Lights.Length);
        return RecurseA(item, new bool[item.Lights.Length], [], [start], [], -1, 0, ref best);
    }

    static int RecurseA(Item item, bool[] lights, Dictionary<string, int> history, string[] direct, int[] path, int lastButton, int depth, ref int currentBest)
    {
        if (depth + 1 > currentBest)
            return int.MaxValue;

        var min = int.MaxValue;

        foreach (var (index, switches) in item.Buttons.Index())
        {
            if (index == lastButton)
                continue;

            bool[] copy = [.. lights];

            foreach (var s in switches)
                copy[s] = !copy[s];

            var key = copy.Select(a => a ? '#' : '.').StringJoin();

            if (direct.Contains(key))
                continue;

            if (key == item.Lights)
            {
                currentBest = depth + 1;
                return currentBest;
            }

            if (!history.TryGetValue(key, out var towin))
            {
                var cur = RecurseA(item, copy, history, [.. direct, key], [.. path, index], index, depth + 1, ref currentBest);

                history[key] = cur == int.MaxValue ? cur : cur - depth - 1;

                if (cur < min)
                    min = cur;
            }
            else if (towin != int.MaxValue)
            {
                var cur = depth + 1 + towin;

                if (cur < currentBest)
                    currentBest = cur;

                if (cur < min)
                    min = cur;
            }
        }

        return min;
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
