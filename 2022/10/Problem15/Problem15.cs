using System.Text.RegularExpressions;

using Advent.Common;

namespace A2022.Problem15;

public static class Solver
{
    [GeneratedTest<long>(26, 5335787)]
    public static long RunA(string[] lines, bool isSample)
    {
        var items = LoadData(lines);

        var targetY = isSample ? 10 : 2_000_000; // ugh

        var minX = items.Min(a => a.Sensor.X - (a.Sensor - a.Beacon).ManhattanLength) - 1;
        var maxX = items.Max(a => a.Sensor.X + (a.Sensor - a.Beacon).ManhattanLength) + 1;

        return Enumerable.Range(minX, maxX - minX + 1).AsParallel().Select(x =>
        {
            var pos = new Pos(x, targetY);

            var collide = items
                .Any(a => a.Beacon != pos && a.Collide(pos));

            return collide ? 1 : 0;
        }).Sum();
    }

    [GeneratedTest<long>(56000011, 13673971349056)]
    public static long RunB(string[] lines, bool isSample)
    {
        var items = LoadData(lines);

        var max = isSample ? new Pos(20, 20) : new Pos(4_000_000, 4_000_000);

        var rect = new Rect(Pos.Zero, max);

        var result = items
            .AsParallel()
            .SelectMany(parent => Enumerable.Range(0, parent.BeaconDistance)
                .Select(n => CreateBeaconOffsets(parent.BeaconDistance, n))
                .SelectMany(offsets =>
                    offsets
                        .Select(offset => parent.Sensor + offset)
                        .Where(pos => rect.Intersects(pos) && !items.Any(a => a.Collide(pos)))))
            .FirstOrNull();

        if (result is Pos p)
            return p.X * 4_000_000L + p.Y;

        throw new();
    }

    static Pos[] CreateBeaconOffsets(int d, int n)
        => [
               new(-d - 1 + n, -n),
               new(-d - 1 + n, n),
               new(d + 1 - n, -n),
               new(d + 1 - n, n),
        ];

    public static Item[] LoadData(string[] lines)
        => lines
            .Select(CompiledRegs.MapToRegex)
            .ToArray(a => new Item(new(a.SensorX, a.SensorY), new(a.BeaconX, a.BeaconY)));
}

public class Item
{
    public Pos Sensor { get; }
    public Pos Beacon { get; }
    public int BeaconDistance { get; }

    Rect Rect { get; }

    public Item(Pos sensor, Pos beacon)
    {
        Sensor = sensor;
        Beacon = beacon;
        BeaconDistance = (Beacon - Sensor).ManhattanLength;
        Rect = new(Sensor - new Pos(BeaconDistance, BeaconDistance), Sensor + new Pos(BeaconDistance, BeaconDistance));
    }

    public bool Collide(Pos pos)
        => Rect.Intersects(pos)
        && (Beacon == pos || Sensor == pos || (Sensor - pos).ManhattanLength <= BeaconDistance);
}

record ParsedItem(int SensorX, int SensorY, int BeaconX, int BeaconY);

static partial class CompiledRegs
{
    [GeneratedRegex(@$"Sensor at x=(?<{nameof(ParsedItem.SensorX)}>-?\d+), y=(?<{nameof(ParsedItem.SensorY)}>-?\d+): closest beacon is at x=(?<{nameof(ParsedItem.BeaconX)}>-?\d+), y=(?<{nameof(ParsedItem.BeaconY)}>-?\d+)")]
    [MapTo<ParsedItem>]
    public static partial Regex Regex();
}
