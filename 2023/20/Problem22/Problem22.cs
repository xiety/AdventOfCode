using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace A2023.Problem22;

public static class Solver
{
    [GeneratedTest<long>(5, 405)]
    public static long RunA(string[] lines)
    {
        var bricks = LoadData(lines);

        FallBricks(bricks);

        return bricks
            .AsParallel()
            .Select(brickToRemove =>
            {
                var rest = bricks.Where(a => a != brickToRemove).ToArray();
                var someoneFall = rest.Any(a => CanFall(rest, a) > 0);
                return someoneFall ? 0 : 1;
            })
            .Sum();
    }

    [GeneratedTest<long>(7, 61297)]
    public static long RunB(string[] lines)
    {
        var bricks = LoadData(lines);

        FallBricks(bricks);

        return bricks
            .AsParallel()
            .Select(brickToRemove => FallBricks(Cloned(bricks.Where(a => a != brickToRemove))))
            .Sum();
    }

    static T[] Cloned<T>(IEnumerable<T> enumerable)
        where T : ICloneable
        => enumerable.ToArray(a => (T)a.Clone());

    static Brick[] LoadData(string[] lines)
        => CompiledRegs.FromLinesRegex(lines).ToArray(Brick.FromItem);

    static int FallBricks(Brick[] bricks)
    {
        var set = new HashSet<string>();

        do
        {
            var someoneFall = false;

            foreach (var brick in bricks)
            {
                var fall = CanFall(bricks, brick);

                if (fall > 0)
                {
                    brick.From = brick.From with { Z = brick.From.Z - fall };
                    brick.To = brick.To with { Z = brick.To.Z - fall };

                    set.Add(brick.Name);

                    someoneFall = true;
                }
            }

            if (!someoneFall)
                break;
        }
        while (true);

        return set.Count;
    }

    static int CanFall(Brick[] bricks, Brick brick)
    {
        for (var z = brick.From.Z - 1; z > 0; --z)
        {
            if (bricks.Any(anotherBrick => anotherBrick != brick
                                        && anotherBrick.To.Z == z
                                        && IntersectIn2D(anotherBrick, brick)))
            {
                return brick.From.Z - z - 1;
            }
        }

        return brick.From.Z - 1;
    }

    static bool IntersectIn2D(Brick a, Brick b)
        => b.To.X >= a.From.X
        && b.From.X <= a.To.X
        && b.To.Y >= a.From.Y
        && b.From.Y <= a.To.Y;
}

record Item(int X1, int Y1, int Z1, int X2, int Y2, int Z2);

sealed class Brick : ICloneable
{
    public required string Name { get; init; }
    public required Pos3 From { get; set; }
    public required Pos3 To { get; set; }

    Brick()
    {
    }

    [SetsRequiredMembers]
    Brick(Brick b)
    {
        Name = b.Name;
        From = b.From;
        To = b.To;
    }

    Brick Clone()
        => new(this);

    object ICloneable.Clone()
        => Clone();

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
    [GeneratedRegex(@$"^(?<{nameof(Item.X1)}>\d+),(?<{nameof(Item.Y1)}>\d+),(?<{nameof(Item.Z1)}>\d+)~(?<{nameof(Item.X2)}>\d+),(?<{nameof(Item.Y2)}>\d+),(?<{nameof(Item.Z2)}>\d+)$")]
    [MapTo<Item>]
    public static partial Regex Regex();
}
