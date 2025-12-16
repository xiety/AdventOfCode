using System.Text.RegularExpressions;

using Advent.Common;

namespace A2023.Problem18;

public static class Solver
{
    [GeneratedTest<long>(62, 106459)]
    public static long RunA(string[] lines)
    {
        var items = CompiledRegs.FromLinesRegex(lines);

        var (minX, maxX, minY, maxY) = GetMinMax(items);

        var width = maxX - minX + 1;
        var height = maxY - minY + 1;

        var map = new bool[width + 2, height + 2];

        var pos = Pos.Zero;

        foreach (var newpos in items.Select(item => pos + GetOffset(item)))
        {
            for (var y = Math.Min(pos.Y, newpos.Y); y < (Math.Max(pos.Y, newpos.Y) + 1); ++y)
                for (var x = Math.Min(pos.X, newpos.X); x < (Math.Max(pos.X, newpos.X) + 1); ++x)
                    map[x - minX + 1, y - minY + 1] = true;

            pos = newpos;
        }

        var borderCount = map.EnumeratePositionsOf(true).Count();

        map.Flood(Pos.Zero);

        var floodCount = map.EnumeratePositionsOf(true).Count();

        return (map.Width * map.Height - floodCount) + borderCount;
    }

    [GeneratedTest<long>(952408144115, 63806916814808)]
    public static long RunBAlternative(string[] lines)
    {
        var items = CompiledRegs.FromLinesRegex(lines);

        var points = items
            .Select(ParseB)
            .Select(GetOffset)
            .Aggregate<Pos, Pos[]>([Pos.Zero], (acc, a) => [.. acc, acc[^1] + a]);

        return Math.Abs(points.Chain()
            .Sum(a =>
                (long)a.First.X * a.Second.Y - (long)a.First.Y * a.Second.X
              + Math.Abs((long)(a.Second.X - a.First.X + 1) * (a.Second.Y - a.First.Y + 1)))) / 2L + 1L;
    }

    [GeneratedTest<long>(952408144115, 63806916814808)]
    public static long RunB(string[] lines)
    {
        var items = CompiledRegs.FromLinesRegex(lines);

        items = items.ToArray(ParseB);

        var possibleX = new List<int>();
        var possibleY = new List<int>();

        var (minX, maxX, minY, maxY) = GetMinMax(items);

        var pos = Pos.Zero;

        var edges = new List<(Pos, Pos)>();

        foreach (var newpos in items.Select(item => pos + GetOffset(item)))
        {
            edges.Add((pos, newpos));

            pos = newpos;

            if (!possibleX.Contains(pos.X))
                possibleX.Add(pos.X);

            if (!possibleY.Contains(pos.Y))
                possibleY.Add(pos.Y);
        }

        possibleX.Add(minX - 10);
        possibleX.Add(maxX + 10);

        possibleY.Add(minY - 10);
        possibleY.Add(maxY + 10);

        possibleX.Sort();
        possibleY.Sort();

        var volume = 0L;

        var fillMap = new bool[possibleX.Count, possibleY.Count];

        foreach (var iy in (possibleY.Count - 1))
        {
            foreach (var ix in (possibleX.Count - 1))
            {
                var square = new Rect(
                    new(possibleX[ix],
                        possibleY[iy]),
                    new(possibleX[ix + 1] - 1,
                        possibleY[iy + 1] - 1)
                );

                var middleX = square.From.X + (square.To.X - square.From.X) / 2;
                var middleY = square.From.Y + (square.To.Y - square.From.Y) / 2;
                var middle = new Pos(middleX, middleY);

                var intersectionsX = edges.Count(a => IntersectsRay(a.Item1, a.Item2, middle, true));
                var intersectionsY = edges.Count(a => IntersectsRay(a.Item1, a.Item2, middle, false));

                var inside = intersectionsX % 2 == 1 && intersectionsY % 2 == 1;

                if (inside)
                    volume += square.Area;

                fillMap[ix, iy] = inside;
            }
        }

        return PixelHunting(possibleX, possibleY, volume, fillMap);
    }

    static long PixelHunting(List<int> possibleX, List<int> possibleY, long volume, bool[,] fillMap)
    {
        foreach (var iy in possibleY.Count)
        {
            foreach (var ix in possibleX.Count)
            {
                if (!fillMap[ix, iy])
                {
                    if (ix > 0 && fillMap[ix - 1, iy])
                        volume += Math.Abs(possibleY[iy] - possibleY[iy + 1]);

                    if (iy > 0 && fillMap[ix, iy - 1])
                        volume += Math.Abs(possibleX[ix] - possibleX[ix + 1]);

                    if (ix > 0 && iy > 0 && fillMap[ix, iy - 1] && fillMap[ix - 1, iy])
                        volume--;

                    if (ix > 0 && iy > 0 && fillMap[ix - 1, iy - 1] && !fillMap[ix, iy - 1] && !fillMap[ix - 1, iy])
                        volume++;
                }
            }
        }

        return volume;
    }

    static (int minX, int maxX, int minY, int maxY) GetMinMax(Item[] items)
    {
        var (minX, maxX, minY, maxY) = (0, 0, 0, 0);

        var pos = Pos.Zero;

        foreach (var item in items)
        {
            pos += GetOffset(item);

            minX = Math.Min(minX, pos.X);
            maxX = Math.Max(maxX, pos.X);
            minY = Math.Min(minY, pos.Y);
            maxY = Math.Max(maxY, pos.Y);
        }

        return (minX, maxX, minY, maxY);
    }

    static bool IntersectsRay(Pos item1, Pos item2, Pos middle, bool hor)
    {
        if (hor)
        {
            if (item1.Y == item2.Y)
                return false;

            var minY = Math.Min(item1.Y, item2.Y);
            var maxY = Math.Max(item1.Y, item2.Y);

            if (minY > middle.Y || maxY < middle.Y)
                return false;

            if (item1.X < middle.X)
                return false;

            return true;
        }

        if (item1.X == item2.X)
            return false;

        var minX = Math.Min(item1.X, item2.X);
        var maxX = Math.Max(item1.X, item2.X);

        if (minX > middle.X || maxX < middle.X)
            return false;

        if (item1.Y < middle.Y)
            return false;

        return true;
    }

    static Item ParseB(Item item)
    {
        var length = int.Parse(item.Color[..5], System.Globalization.NumberStyles.HexNumber);

        var dir = item.Color[5] switch
        {
            '0' => "R",
            '1' => "D",
            '2' => "L",
            '3' => "U",
        };

        return new(dir, length, "");
    }

    static Pos GetOffset(Item item)
        => item.Dir switch
        {
            "U" => new(0, -item.Number),
            "D" => new(0, item.Number),
            "L" => new(-item.Number, 0),
            "R" => new(item.Number, 0),
        };
}

record Item(string Dir, int Number, string Color);

static partial class CompiledRegs
{
    [GeneratedRegex(@$"^(?<{nameof(Item.Dir)}>\w) (?<{nameof(Item.Number)}>\d+) \(#(?<{nameof(Item.Color)}>......)\)$")]
    [MapTo<Item>]
    public static partial Regex Regex();
}
