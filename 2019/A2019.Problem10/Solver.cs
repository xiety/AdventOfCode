using System.Numerics;

using Advent.Common;

namespace A2019.Problem10;

public class Solver : ISolver<long>
{
    public long RunA(string[] lines, bool isSample)
    {
        var map = LoadData(lines);
        var asteroids = map.EnumeratePositionsOf(true).ToArray();
        var rect = Rect.CreateBoundingBox(asteroids);
        return FindObservatory(rect, asteroids).Count;
    }

    public long RunB(string[] lines, bool isSample)
    {
        var map = LoadData(lines);
        var asteroids = map.EnumeratePositionsOf(true).ToList();
        var rect = Rect.CreateBoundingBox(asteroids);
        var observatory = FindObservatory(rect, asteroids).Pos;
        var target = 200;

        return EnumerateVaporized(asteroids, rect, observatory)
            .Skip(target - 1)
            .Select(a => a.X * 100 + a.Y)
            .First();
    }

    private static IEnumerable<Pos> EnumerateVaporized(List<Pos> asteroids, Rect rect, Pos observatory)
    {
        var ordered = asteroids.OrderBy(a => Angle(a - observatory)).ToList();

        do
        {
            var circles = ordered.ToList();

            do
            {
                var vaporized = circles
                    .Where(a => See(rect, circles, observatory, a))
                    .First();

                var shadowed = CastShadow(rect, observatory, vaporized)
                    .Where(a => circles.Any(b => b == a));

                circles.RemoveRange(shadowed);
                circles.Remove(vaporized);
                ordered.Remove(vaporized);

                yield return vaporized;
            }
            while (circles.Any());
        }
        while (ordered.Any());
    }

    static (Pos Pos, int Count) FindObservatory(Rect rect, IReadOnlyCollection<Pos> asteroids)
        => asteroids
        .Select(observatory => (
                Pos: observatory,
                Count: asteroids.Where(asteroid => See(rect, asteroids, observatory, asteroid)).Count()))
        .MaxBy(a => a.Count);

    static IEnumerable<Pos> CastShadow(Rect rect, Pos observatory, Pos asteroid)
    {
        var distance = asteroid - observatory;
        var g = INumberExtensions.GCD(distance.X, distance.Y);
        var step = new Pos(distance.X / g, distance.Y / g);
        var pos = asteroid;

        do
        {
            pos += step;
            yield return pos;
        }
        while (rect.Intersects(pos));
    }

    static double Angle(Pos direction)
        => Math.PI * 2 - Math.Atan2(direction.X, direction.Y);

    static bool See(Rect rect, IEnumerable<Pos> asteroids, Pos observatory, Pos asteroid)
    {
        if (observatory == asteroid)
            return false;

        return !asteroids
            .Where(other => other != asteroid && other != observatory)
            .Any(other => CastShadow(rect, observatory, other).Any(a => a == asteroid));
    }

    static bool[,] LoadData(string[] lines)
        => MapData.ParseMap(lines, a => a == '#');
}
