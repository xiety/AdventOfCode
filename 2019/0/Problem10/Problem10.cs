using Advent.Common;

namespace A2019.Problem10;

public static class Solver
{
    [GeneratedTest<long>(210, 278)]
    public static long RunA(string[] lines)
    {
        var map = LoadData(lines);
        var asteroids = map.EnumeratePositionsOf(true).ToArray();
        var rect = Rect.CreateBoundingBox(asteroids);
        return FindObservatory(rect, asteroids).Count;
    }

    [GeneratedTest<long>(802, 1417)]
    public static long RunB(string[] lines)
    {
        var map = LoadData(lines);
        var asteroids = map.EnumeratePositionsOf(true).ToList();
        var rect = Rect.CreateBoundingBox(asteroids);
        var observatory = FindObservatory(rect, asteroids).Pos;
        const int target = 200;

        return EnumerateVaporized(asteroids, rect, observatory)
            .Skip(target - 1)
            .Select(a => a.X * 100 + a.Y)
            .First();
    }

    static IEnumerable<Pos> EnumerateVaporized(List<Pos> asteroids, Rect rect, Pos observatory)
    {
        var ordered = asteroids.OrderBy(a => Angle(a - observatory)).ToList();

        do
        {
            var circles = ordered.ToList();

            do
            {
                var vaporized = circles
                    .First(a => See(rect, circles, observatory, a));

                var shadowed = CastShadow(rect, observatory, vaporized)
                    .Where(circles.Contains);

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
                Count: asteroids.Count(asteroid => See(rect, asteroids, observatory, asteroid))))
        .MaxBy(a => a.Count);

    static IEnumerable<Pos> CastShadow(Rect rect, Pos observatory, Pos asteroid)
    {
        var distance = asteroid - observatory;
        var g = Math.GCD(distance.X, distance.Y);
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
            .Any(other => CastShadow(rect, observatory, other)
                .Any(a => a == asteroid));
    }

    static bool[,] LoadData(string[] lines)
        => MapData.ParseMap(lines, a => a == '#');
}
