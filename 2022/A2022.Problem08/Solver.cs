using Advent.Common;

namespace A2022.Problem08;

public class Solver : IProblemSolver<long>
{
    public long RunA(string filename)
    {
        var map = LoadFile(filename);

        return map.EnumeratePositions()
            .Count(a => IsVisibleTree(map, a));
    }

    public long RunB(string filename)
    {
        var map = LoadFile(filename);

        return map.EnumeratePositions()
            .Max(a => Scenic(map, a));
    }

    static bool IsVisibleTree(int[,] map, Pos p)
        => ArrayEx.Offsets
               .Select(a => IsVisibleTreeDirection(map, p, a))
               .FirstOrDefault(a => a, false);

    static int Scenic(int[,] map, Pos p)
        => ArrayEx.Offsets
               .Select(a => CalculateScenicDirection(map, p, a)).Mul();

    static int CalculateScenicDirection(int[,] map, Pos treePos, Pos delta)
    {
        var visible = IsVisibleTreeDirection(map, treePos, delta);

        if (visible)
        {
            return EnumerateDirection(map, treePos, delta)
                .Count();
        }
        else
        {
            var tree = map.Get(treePos);

            return EnumerateDirection(map, treePos, delta)
                .Select(map.Get)
                .TakeWhile(a => a < tree)
                .Count() + 1;
        }
    }

    static bool IsVisibleTreeDirection(int[,] map, Pos treePos, Pos delta)
    {
        var tree = map.Get(treePos);

        return !EnumerateDirection(map, treePos, delta)
            .Select(map.Get)
            .Any(a => a >= tree);
    }

    static IEnumerable<Pos> EnumerateDirection<T>(T[,] array, Pos p, Pos d)
    {
        p += d;

        while (array.IsInBounds(p))
        {
            yield return p;
            p += d;
        }
    }

    private static int[,] LoadFile(string filename)
        => MapData.ParseMap(File.ReadAllLines(filename));
}
