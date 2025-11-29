using Advent.Common;

namespace A2020.Problem03;

public static class Solver
{
    [GeneratedTest<long>(7, 187)]
    public static long RunA(string[] lines)
    {
        var map = LoadData(lines);
        return CountTrees(map, new(3, 1));
    }

    [GeneratedTest<long>(336, 4723283400)]
    public static long RunB(string[] lines)
    {
        var map = LoadData(lines);
        Pos[] slopes = [new(1, 1), new(3, 1), new(5, 1), new(7, 1), new(1, 2)];
        return slopes.Select(a => CountTrees(map, a)).MulLong();
    }

    static int CountTrees(bool[,] map, Pos slope)
    {
        var pos = new Pos(0, 0);
        var count = 0;

        do
        {
            if (map.Get(pos))
                count++;

            pos += slope;
            pos = pos with { X = pos.X % map.Width };
        }
        while (pos.Y < map.Height);

        return count;
    }

    static bool[,] LoadData(string[] lines)
        => MapData.ParseMap(lines, a => a == '#');
}
