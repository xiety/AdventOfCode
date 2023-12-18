using System.Text.RegularExpressions;

using Advent.Common;

namespace A2023.Problem18;

public class Solver : IProblemSolver<long>
{
    public long RunA(string filename)
    {
        var items = CompiledRegs.Regex().FromFile<Item>(filename);

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

        var width = maxX - minX + 1;
        var height = maxY - minY + 1;

        var map = new bool[width + 2, height + 2];

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
    //U 2 (#7a21e3)
    [GeneratedRegex(@$"^(?<{nameof(Item.Dir)}>\w) (?<{nameof(Item.Number)}>\d+) \(#(?<{nameof(Item.Color)}>......)\)$")]
    public static partial Regex Regex();
}
