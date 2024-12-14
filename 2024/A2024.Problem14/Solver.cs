using System.Text.RegularExpressions;

using Advent.Common;

namespace A2024.Problem14;

public class Solver : ISolver<long>
{
    public long RunA(string[] lines, bool isSample)
    {
        var robots = LoadData(lines);

        var (width, height) = isSample ? (11, 7) : (101, 103);

        for (var i = 0; i < 100; ++i)
            MoveRobots(robots, width, height);

        return Count(width, height, robots);
    }

    public long RunB(string[] lines, bool isSample)
    {
        if (isSample)
            return -1;

        var robots = LoadData(lines);
        var (width, height) = (101, 103);
        var tree = ToMap(Data.Tree);

        var seconds = 0;

        do
        {
            MoveRobots(robots, width, height);

            seconds++;

            if (robots.DistinctBy(a => a.Pos).Count() == robots.Length)
            {
                var map = ToMap(robots, width, height);

                if (map.TryFindSubarray(tree, out var _))
                    break;
            }
        }
        while (true);

        return seconds;
    }

    static void MoveRobots(Robot[] robots, int width, int height)
    {
        foreach (var robot in robots)
        {
            robot.Pos += robot.Velocity;
            robot.Pos = new(Wrap(robot.Pos.X, width), Wrap(robot.Pos.Y, height));
        }
    }

    static bool[,] ToMap(Robot[] robots, int width, int height)
    {
        var array = new bool[width, height];

        foreach (var robot in robots)
            array.Set(robot.Pos, true);

        return array;
    }

    static bool[,] ToMap(string text)
        => MapData.ParseMap(text.Split(Environment.NewLine), a => a == '#');

    static int Wrap(int x, int max)
        => x switch
        {
            < 0 => Wrap(x + max, max),
            _ when x >= max => Wrap(x - max, max),
            _ => x
        };

    static long Count(int width, int height, Robot[] robots)
        => GetQuadrants(width, height)
            .Select(a => robots.Count(b => a.Intersects(b.Pos))).MulLong();

    static Rect[] GetQuadrants(int width, int height)
    {
        var widthHalf = width / 2;
        var heightHalf = height / 2;

        return [
            new(new(0, 0), new(widthHalf - 1, heightHalf - 1)),
            new(new(0, heightHalf + 1), new(widthHalf - 1, height - 1)),
            new(new(widthHalf + 1, 0), new(width - 1, heightHalf - 1)),
            new(new(widthHalf + 1, heightHalf + 1), new(width - 1, height - 1))
        ];
    }

    static Robot[] LoadData(string[] lines)
        => CompiledRegs.Regex().FromLines<Item>(lines)
            .Select(a => new Robot { Pos = new(a.Px, a.Py), Velocity = new(a.Vx, a.Vy) }).ToArray();
}

class Robot
{
    public Pos Pos { get; set; }
    public Pos Velocity { get; set; }
}

record Item(int Px, int Py, int Vx, int Vy);

static partial class CompiledRegs
{
    [GeneratedRegex(@$"^p=(?<{nameof(Item.Px)}>\-?\d+),(?<{nameof(Item.Py)}>\-?\d+) v=(?<{nameof(Item.Vx)}>\-?\d+),(?<{nameof(Item.Vy)}>\-?\d+)$")]
    public static partial Regex Regex();
}

class Data
{
    public const string Tree = """
        ###############################
        #.............................#
        #.............................#
        #.............................#
        #.............................#
        #..............#..............#
        #.............###.............#
        #............#####............#
        #...........#######...........#
        #..........#########..........#
        #............#####............#
        #...........#######...........#
        #..........#########..........#
        #.........###########.........#
        #........#############........#
        #..........#########..........#
        #.........###########.........#
        #........#############........#
        #.......###############.......#
        #......#################......#
        #........#############........#
        #.......###############.......#
        #......#################......#
        #.....###################.....#
        #....#####################....#
        #.............###.............#
        #.............###.............#
        #.............###.............#
        #.............................#
        #.............................#
        #.............................#
        #.............................#
        ###############################
        """;
}
