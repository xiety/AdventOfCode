using System.Numerics;
using System.Text.RegularExpressions;

using Advent.Common;

namespace A2019.Problem12;

public class Solver : ISolver<long>
{
    public long RunA(string[] lines, bool isSample)
    {
        var total = isSample ? 100 : 1000;

        var items = LoadData(lines);

        for (var i = 0; i < total; ++i)
            SimulateStep(items);

        return Energy(items);
    }

    public long RunB(string[] lines, bool isSample)
    {
        var items = LoadData(lines);

        var simulation = Simulate(items);

        // TODO: simulation is running three times here
        var rx = simulation.Select(a => a[0].Pos.X).FindRepeat();
        var ry = simulation.Select(a => a[0].Pos.Y).FindRepeat();
        var rz = simulation.Select(a => a[0].Pos.Z).FindRepeat();

        return INumberExtensions.LCM<long>([rx, ry, rz]);
    }

    static IEnumerable<Planet[]> Simulate(Planet[] items)
    {
        do
        {
            SimulateStep(items);
            yield return items;
        }
        while (true);
    }

    static void SimulateStep(Planet[] items)
    {
        foreach (var (item1, item2) in items.EnumeratePairs())
        {
            var change = new Pos3(
                Math.Sign(item2.Pos.X - item1.Pos.X),
                Math.Sign(item2.Pos.Y - item1.Pos.Y),
                Math.Sign(item2.Pos.Z - item1.Pos.Z));

            item1.Velocity += change;
            item2.Velocity -= change;
        }

        foreach (var item in items)
            item.Pos += item.Velocity;
    }

    static long Energy(Planet[] items)
        => items.Sum(a => a.Pos.ManhattanLength * a.Velocity.ManhattanLength);

    static Planet[] LoadData(string[] lines)
        => CompiledRegs.Regex().FromLines<Pos3>(lines)
               .Select(a => new Planet { Pos = a }).ToArray();
}

record Planet
{
    public Pos3 Pos { get; set; }
    public Pos3 Velocity { get; set; }
}

static partial class CompiledRegs
{
    [GeneratedRegex(@$"^\<x=(?<{nameof(Pos3.X)}>-?\d+), y=(?<{nameof(Pos3.Y)}>-?\d+), z=(?<{nameof(Pos3.Z)}>-?\d+)\>$")]
    public static partial Regex Regex();
}
