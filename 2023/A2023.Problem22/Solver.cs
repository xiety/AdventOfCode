using System.Text.RegularExpressions;

using Advent.Common;

namespace A2023.Problem22;

public class Solver : IProblemSolver<long>
{
    //slow
    public long RunA(string filename)
    {
        var bricks = CompiledRegs.Regex().FromFile<Item>(filename).Select(Brick.FromItem).ToArray();

        FallBricks(bricks);

        var count = 0;

        foreach (var brick_to_remove in bricks)
        {
            var fall = false;

            var rest = bricks.Where(a => a != brick_to_remove).ToArray();

            foreach (var brick in rest)
            {
                if (CanFall(rest, brick))
                {
                    fall = true;
                    break;
                }
            }

            if (!fall)
                count++;
        }

        return count;
    }

    //slow
    public long RunB(string filename)
    {
        var bricks = CompiledRegs.Regex().FromFile<Item>(filename).Select(Brick.FromItem).ToArray();

        FallBricks(bricks);

        var count = 0;
        var total_fall = 0;

        foreach (var brick_to_remove in bricks)
        {
            var fall = FallBricks(bricks.Where(a => a != brick_to_remove).Select(a => a with { }).ToArray());

            total_fall += fall;

            if (fall == 0)
                count++;
        }

        return total_fall;
    }

    private void Dump(Brick[] bricks, string filename)
    {
        var obj = new ObjFile();

        foreach (var brick in bricks)
            obj.AddCube(
                new(brick.From.X, brick.From.Z, brick.From.Y),
                new(brick.To.X + 1, brick.To.Z + 1, brick.To.Y + 1));

        obj.Save(filename);
    }

    private int FallBricks(Brick[] bricks)
    {
        var set = new HashSet<string>();

        do
        {
            var fall = false;

            foreach (var brick in bricks)
            {
                if (CanFall(bricks, brick))
                {
                    brick.From = brick.From with { Z = brick.From.Z - 1 };
                    brick.To = brick.To with { Z = brick.To.Z - 1 };

                    set.Add(brick.Name);

                    fall = true;
                }
            }

            if (!fall)
                break;
        }
        while (true);

        return set.Count;
    }

    private bool CanFall(IEnumerable<Brick> bricks, Brick brick)
    {
        var z_below = brick.From.Z - 1;

        if (z_below == 0)
            return false;

        for (var x = brick.From.X; x <= brick.To.X; ++x)
        {
            for (var y = brick.From.Y; y <= brick.To.Y; ++y)
            {
                var intersect = bricks
                    .Where(a => a != brick)
                    .Where(a => a.From.X <= x && a.To.X >= x
                             && a.From.Y <= y && a.To.Y >= y
                             && a.To.Z == z_below)
                    .Any();

                if (intersect)
                    return false;
            }
        }

        return true;
    }
}

public record Item(int X1, int Y1, int Z1, int X2, int Y2, int Z2);

public record Brick
{
    public required string Name { get; init; }
    public required Pos3 From { get; set; }
    public required Pos3 To { get; set; }

    public override string ToString()
        => $"{Name} ({From.X},{From.Y},{From.Z}) ({To.X},{To.Y},{To.Z})";

    public static Brick FromItem(Item item, int index)
        => new()
        {
            Name = $"B{index:0000}",
            From = new(item.X1, item.Y1, item.Z1),
            To = new(item.X2, item.Y2, item.Z2)
        };
}

static partial class CompiledRegs
{
    //4,9,272~6,9,272
    [GeneratedRegex(@$"^(?<{nameof(Item.X1)}>\d+),(?<{nameof(Item.Y1)}>\d+),(?<{nameof(Item.Z1)}>\d+)~(?<{nameof(Item.X2)}>\d+),(?<{nameof(Item.Y2)}>\d+),(?<{nameof(Item.Z2)}>\d+)$")]
    public static partial Regex Regex();
}
