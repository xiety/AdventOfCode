using Advent.Common;

namespace A2024.Problem06;

public class Solver : ISolver<int>
{
    public int RunA(string[] lines, bool isSample)
    {
        var (map, start) = LoadData(lines);
        return EnumeratePath(map, start)
            .Select(a => a.Pos)
            .Distinct()
            .Count();
    }

    public int RunB(string[] lines, bool isSample)
    {
        var (map, start) = LoadData(lines);
        var path = EnumeratePath(map, start)
            .Select(a => a.Pos)
            .Where(a => a != start)
            .Distinct()
            .ToArray();

        return path.Count(a => Check(map, start, a));
    }

    private static bool Check(bool[,] map, Pos start, Pos obst)
    {
        map.Set(obst, true);
        var loopIndex = EnumeratePath(map, start).FindLoopIndex();
        map.Set(obst, false);

        return loopIndex != -1;
    }

    static IEnumerable<(Pos Pos, Dir Dir)> EnumeratePath(bool[,] map, Pos start)
    {
        var dir = Dir.Up;
        var pos = start;

        do
        {
            var newPos = pos + Offset(dir);
            var c = map.GetOrDefault(newPos, false);

            if (c)
            {
                dir = dir.RotateEnum(1);
            }
            else
            {
                yield return (pos, dir);
                pos = newPos;
            }
        }
        while (map.IsInBounds(pos));
    }

    static Pos Offset(Dir dir)
        => dir switch
        {
            Dir.Up => new(0, -1),
            Dir.Right => new(1, 0),
            Dir.Down => new(0, 1),
            Dir.Left => new(-1, 0),
        };

    static (bool[,], Pos) LoadData(string[] lines)
    {
        var start = MapData.FindPos(lines, '^');
        var map = MapData.ParseMap(lines, c => c == '#');
        return (map, start);
    }

    enum Dir
    {
        Up = 0,
        Right = 1,
        Down = 2,
        Left = 3,
    }
}
