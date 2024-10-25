using Advent.Common;

namespace A2022.Problem15;

public class Solver : IProblemSolver<long>
{
    public long RunA(string filename)
    {
        var items = Loader.Load(filename);

        var targetY = filename.Contains("sample") ? 10 : 2_000_000; // ugh

        var minX = items.Min(a => a.Sensor.X - (a.Sensor - a.Beacon).ManhattanLength()) - 1;
        var maxX = items.Max(a => a.Sensor.X + (a.Sensor - a.Beacon).ManhattanLength()) + 1;

        return Enumerable.Range(minX, maxX - minX + 1).AsParallel().Select(x =>
        {
            var pos = new Pos(x, targetY);

            var collide = items
                .Where(a => a.Beacon != pos)
                .Any(a => a.Collide(pos));

            return collide ? 1 : 0;
        }).Sum();
    }

    public long RunB(string filename)
    {
        var items = Loader.Load(filename);

        var max = filename.Contains("sample") ? new Pos(20, 20) : new Pos(4_000_000, 4_000_000);

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
        {
            Console.WriteLine(p);
            return p.X * 4_000_000L + p.Y;
        }

        throw new ArgumentOutOfRangeException();
    }

    private static Pos[] CreateBeaconOffsets(int d, int n)
        => [
               new(-d - 1 + n, -n),
               new(-d - 1 + n, n),
               new(d + 1 - n, -n),
               new(d + 1 - n, n),
        ];
}

static class PosExtensions
{
    public static int ManhattanLength(this Pos pos)
        => Math.Abs(pos.X) + Math.Abs(pos.Y);
}

public class Item
{
    public Pos Sensor { get; }
    public Pos Beacon { get; }
    public int BeaconDistance { get; }
    public Rect Rect { get; }

    public Item(Pos sensor, Pos beacon)
    {
        Sensor = sensor;
        Beacon = beacon;
        BeaconDistance = (Beacon - Sensor).ManhattanLength();
        Rect = new(Sensor - new Pos(BeaconDistance, BeaconDistance), Sensor + new Pos(BeaconDistance, BeaconDistance));
    }

    public bool Collide(Pos pos)
        => Rect.Intersects(pos)
        && (Beacon == pos || Sensor == pos || (Sensor - pos).ManhattanLength() <= BeaconDistance);
}
