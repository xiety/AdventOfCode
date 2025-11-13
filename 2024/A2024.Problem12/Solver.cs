using Advent.Common;

namespace A2024.Problem12;

public class Solver : ISolver<long>
{
    public long RunA(string[] lines, bool isSample)
        => Run(lines).Select(a => a.Item1 * a.Item2.Length).Sum();

    public long RunB(string[] lines, bool isSample)
        => Run(lines).Select(a => a.Item1 * CountSides(a.Item2)).Sum();

    static IEnumerable<(int, Log[])> Run(string[] lines)
    {
        var map = MapData.ParseMap(lines, a => $"{a}");
        var filled = new bool[map.Width, map.Height];

        do
        {
            var startOrNull = filled.EnumeratePositionsOf(false).FirstOrNull();

            if (startOrNull is not Pos start)
                break;

            var current = new bool[map.Width, map.Height];
            Flood(start, filled, current, map);

            var num = current.EnumeratePositionsOf(true).Count();
            var perimeterItems = Perimeter(current).ToArray();

            yield return (num, perimeterItems);
        }
        while (true);
    }

    static int CountSides(Log[] perimeterList)
    {
        var remainder = perimeterList.ToList();
        var perimeter = 0;

        do
        {
            var pos = remainder.First();

            switch (pos.Wall)
            {
                case Wall.Top or Wall.Bottom:
                    remainder.RemoveRange(Ray(remainder, pos, new(-2, 0)));
                    remainder.RemoveRange(Ray(remainder, pos, new(2, 0)));
                    break;
                case Wall.Left or Wall.Right:
                    remainder.RemoveRange(Ray(remainder, pos, new(0, -2)));
                    remainder.RemoveRange(Ray(remainder, pos, new(0, 2)));
                    break;
            }

            remainder.RemoveAt(0);

            perimeter++;
        }
        while (remainder.Count > 0);

        return perimeter;
    }

    static IEnumerable<Log> Ray(IReadOnlyCollection<Log> remainder, Log pos, Pos offset)
    {
        var currentPos = pos.Pos + offset;

        do
        {
            var item = new Log(currentPos, pos.Wall);

            if (remainder.Contains(item))
                yield return item;
            else
                break;

            currentPos += offset;
        }
        while (true);
    }

    static IEnumerable<Log> Perimeter(bool[,] current)
    {
        (Pos, Pos, Pos, Wall, Wall)[] data = [
            (new(-1, 0), new(0, -1), new(0, 1), Wall.Top, Wall.Bottom),
            (new(1, 0), new(0, -1), new(0, 1), Wall.Top, Wall.Bottom),
            (new(0, -1), new(-1, 0), new(1, 0), Wall.Left, Wall.Right),
            (new(0, 1), new(-1, 0), new(1, 0), Wall.Left, Wall.Right),
        ];

        var query = from pos in current.EnumeratePositionsOf(true)
                    let center = pos * 2
                    from fence in ArrayEx.DiagOffsets.Select(a => a + center)
                    from line in data
                    let log = fence + line.Item1
                    let p1 = current.GetOrDefault((log + line.Item2) / 2, false)
                    let p2 = current.GetOrDefault((log + line.Item3) / 2, false)
                    where p1 != p2
                    select new Log(log, p1 && !p2 ? line.Item4 : line.Item5);

        return query.Distinct();
    }

    static void Flood(Pos start, bool[,] filled, bool[,] current, string[,] map)
    {
        var startString = map.Get(start);

        var floodPoints = new List<Pos>() { start };
        var nextPoints = new List<Pos>();

        filled.Set(start, true);
        current.Set(start, true);

        do
        {
            var items =
                from p in floodPoints
                from p2 in map.Offsetted(p)
                where map.Get(p2) == startString && !current.Get(p2)
                select p2;

            items = items.Distinct();

            foreach (var p2 in items)
            {
                filled.Set(p2, true);
                current.Set(p2, true);
                nextPoints.Add(p2);
            }

            (nextPoints, floodPoints) = (floodPoints, nextPoints);
            nextPoints.Clear();
        }
        while (floodPoints.Count > 0);
    }

    enum Wall { Left, Right, Top, Bottom }

    record struct Log(Pos Pos, Wall Wall);
}
