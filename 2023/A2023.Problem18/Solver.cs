using System.Text.RegularExpressions;

using Advent.Common;

namespace A2023.Problem18;

public class Solver : IProblemSolver<long>
{
    public long RunA(string filename)
    {
        var items = CompiledRegs.Regex().FromFile<Item>(filename);

        var (minX, maxX, minY, maxY) = GetMinMax(items);

        var width = maxX - minX + 1;
        var height = maxY - minY + 1;

        var map = new bool[width + 2, height + 2];

        var pos = Pos.Zero;

        foreach (var item in items)
        {
            var newpos = pos + GetOffset(item);

            for (var y = Math.Min(pos.Y, newpos.Y); y <= Math.Max(pos.Y, newpos.Y); ++y)
                for (var x = Math.Min(pos.X, newpos.X); x <= Math.Max(pos.X, newpos.X); ++x)
                    map[x - minX + 1, y - minY + 1] = true;

            pos = newpos;
        }

        var borderCount = map.EnumeratePositionsOf(true).Count();

        map.Flood(Pos.Zero);

        var floodCount = map.EnumeratePositionsOf(true).Count();

        var result = (map.GetWidth() * map.GetHeight() - floodCount) + borderCount;

        return result;
    }

    public long RunBAlternative(string filename)
    {
        var items = CompiledRegs.Regex().FromFile<Item>(filename);

        var points = items
            .Select(ParseB)
            .Select(GetOffset)
            .Aggregate<Pos, Pos[]>([Pos.Zero], (acc, a) => [.. acc, acc[^1] + a]);

        //Shoelace and Pick
        return Math.Abs(points.Pairs(true)
            .Select(a =>
                (long)a.Item1.X * a.Item2.Y - (long)a.Item1.Y * a.Item2.X
                + Math.Abs(new Rect(a.Item1, a.Item2).Volume))
            .Sum()) / 2L + 1L;
    }

    public long RunB(string filename)
    {
        var items = CompiledRegs.Regex().FromFile<Item>(filename);

        items = items.Select(ParseB).ToList();

        var possibleX = new List<int>();
        var possibleY = new List<int>();

        var (minX, maxX, minY, maxY) = GetMinMax(items);

        var pos = Pos.Zero;

        var lines = new List<(Pos, Pos)>();

        foreach (var item in items)
        {
            var newpos = pos + GetOffset(item);

            lines.Add((pos, newpos));

            pos = newpos;

            if (!possibleX.Contains(pos.X))
                possibleX.Add(pos.X);

            if (!possibleY.Contains(pos.Y))
                possibleY.Add(pos.Y);
        }

        Console.WriteLine($"{possibleX.Count} {possibleY.Count}");

        possibleX.Add(minX - 10);
        possibleX.Add(maxX + 10);

        possibleY.Add(minY - 10);
        possibleY.Add(maxY + 10);

        possibleX.Sort();
        possibleY.Sort();

        var volume = 0L;

        var fillMap = new bool[possibleX.Count, possibleY.Count];

        for (var iy = 0; iy < possibleY.Count - 1; ++iy)
        {
            for (var ix = 0; ix < possibleX.Count - 1; ++ix)
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

                var intersectionsX = lines.Count(a => IntersectsRay(a.Item1, a.Item2, middle, true));
                var intersectionsY = lines.Count(a => IntersectsRay(a.Item1, a.Item2, middle, false));

                var inside = intersectionsX % 2 == 1 && intersectionsY % 2 == 1;

                if (inside)
                    volume += square.Volume;

                fillMap[ix, iy] = inside;
            }
        }

        //pixel huting
        for (var iy = 0; iy < possibleY.Count; ++iy)
        {
            for (var ix = 0; ix < possibleX.Count; ++ix)
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

    private static (int minX, int maxX, int minY, int maxY) GetMinMax(List<Item> items)
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

    private bool IntersectsRay(Pos item1, Pos item2, Pos middle, bool hor)
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
        else
        {
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
    }

    private Item ParseB(Item item)
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

    private static Pos GetOffset(Item item)
        => item.Dir switch
        {
            "U" => new Pos(0, -item.Number),
            "D" => new Pos(0, item.Number),
            "L" => new Pos(-item.Number, 0),
            "R" => new Pos(item.Number, 0),
        };
}

record Item(string Dir, int Number, string Color);

static partial class CompiledRegs
{
    [GeneratedRegex(@$"^(?<{nameof(Item.Dir)}>\w) (?<{nameof(Item.Number)}>\d+) \(#(?<{nameof(Item.Color)}>......)\)$")]
    public static partial Regex Regex();
}
