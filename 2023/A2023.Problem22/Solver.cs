using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

using Advent.Common;

namespace A2023.Problem22;

public class Solver : IProblemSolver<long>
{
    public long RunA(string filename)
    {
        var bricks = LoadFile(filename);

        FallBricks(bricks);

        return bricks
            .AsParallel()
            .Select(brick_to_remove =>
            {
                var rest = bricks.Where(a => a != brick_to_remove).ToArray();
                var someone_fall = rest.Any(a => CanFall(rest, a) > 0);
                return someone_fall ? 0 : 1;
            })
            .Sum();
    }

    public long RunB(string filename)
    {
        var bricks = LoadFile(filename);

        FallBricks(bricks);

        return bricks
            .AsParallel()
            .Select(brick_to_remove => FallBricks(Cloned(bricks.Where(a => a != brick_to_remove))))
            .Sum();
    }

    private static T[] Cloned<T>(IEnumerable<T> enumerable)
        where T : ICloneable
        => enumerable.Select(a => (T)a.Clone()).ToArray();

    private static Brick[] LoadFile(string filename)
        => CompiledRegs.Regex().FromFile<Item>(filename).Select(Brick.FromItem).ToArray();

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
            var someone_fall = false;

            foreach (var brick in bricks)
            {
                var fall = CanFall(bricks, brick);

                if (fall > 0)
                {
                    brick.From = brick.From with { Z = brick.From.Z - fall };
                    brick.To = brick.To with { Z = brick.To.Z - fall };

                    set.Add(brick.Name);

                    someone_fall = true;
                }
            }

            if (!someone_fall)
                break;
        }
        while (true);

        return set.Count;
    }

    private int CanFall(IEnumerable<Brick> bricks, Brick brick)
    {
        for (var z = brick.From.Z - 1; z > 0; --z)
        {
            foreach (var another_brick in bricks)
            {
                if (another_brick != brick
                 && another_brick.To.Z == z
                 && IntersectIn2D(another_brick, brick))
                {
                    return brick.From.Z - z - 1;
                }
            }
        }

        return brick.From.Z - 1;
    }

    private static bool IntersectIn2D(Brick a, Brick b)
        => b.To.X >= a.From.X
        && b.From.X <= a.To.X
        && b.To.Y >= a.From.Y
        && b.From.Y <= a.To.Y;
}

public record Item(int X1, int Y1, int Z1, int X2, int Y2, int Z2);

public class Brick : ICloneable
{
    public required string Name { get; init; }
    public required Pos3 From { get; set; }
    public required Pos3 To { get; set; }

    public Brick()
    {
    }

    [SetsRequiredMembers]
    protected Brick(Brick b)
    {
        Name = b.Name;
        From = b.From;
        To = b.To;
    }

    public Brick Clone()
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
    //4,9,272~6,9,272
    [GeneratedRegex(@$"^(?<{nameof(Item.X1)}>\d+),(?<{nameof(Item.Y1)}>\d+),(?<{nameof(Item.Z1)}>\d+)~(?<{nameof(Item.X2)}>\d+),(?<{nameof(Item.Y2)}>\d+),(?<{nameof(Item.Z2)}>\d+)$")]
    public static partial Regex Regex();
}
