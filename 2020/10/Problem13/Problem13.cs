using System.Numerics;

using Advent.Common;

namespace A2020.Problem13;

public static class Solver
{
    [GeneratedTest<long>(295, 203)]
    public static long RunA(string[] lines)
    {
        var (target, items) = LoadData(lines);

        return items
            .OfType<int>()
            .Select(a => (Item: a, Diff: Math.Mod(-target, a)))
            .Select(a => (a.Diff, Result: a.Diff * a.Item))
            .MinBy(a => a.Diff)
            .Result;
    }

    [GeneratedTest<long>(1068781, 905694340256752)]
    public static long RunB(string[] lines)
        => ToBusses(LoadData(lines).Items)
            .Aggregate((N: 0L, Step: 1L),
                (acc, bus) => (Enumerable.InfiniteSequence(acc.N, acc.Step)
                        .First(a => (a + bus.Index) % bus.Value == 0),
                    acc.Step * bus.Value))
            .N;

    [GeneratedTest<long>(1068781, 905694340256752)]
    public static long RunBCRT(string[] lines)
    {
        var (_, items) = LoadData(lines);
        var busses = ToBusses(items);
        var M = busses.Mul(a => (long)a.Value);

        return busses.Skip(1).SumMod(M, bus =>
        {
            var Mi = M / bus.Value;
            var inverse = (int)BigInteger.ModPow(Mi, bus.Value - 2, bus.Value);
            return -bus.Index * Mi * inverse;
        });
    }

    static Bus[] ToBusses(int?[] items)
            => items
                .Index()
                .Where(a => a.Item is int)
                .Select(a => (Item: a.Item!.Value, a.Index))
                .ToArray(a => new Bus(a.Item, a.Index));

    static Data LoadData(string[] lines)
    {
        var target = int.Parse(lines[0]);
        var items = lines[1].Split(',').ToArray(a => a == "x" ? (int?)null : int.Parse(a));
        return new(target, items);
    }
}

readonly record struct Data(int Target, int?[] Items);
readonly record struct Bus(int Value, int Index);
