using Advent.Common;

namespace A2023.Problem13;

public static class Solver
{
    [GeneratedTest<long>(405, 30158)]
    public static long RunA(string[] lines)
        => Run(lines, 0);

    [GeneratedTest<long>(400, 36474)]
    public static long RunB(string[] lines)
        => Run(lines, 1);

    static long Run(string[] lines, int min)
    {
        var chunks = lines.SplitBy(String.Empty);

        var r = 0L;

        foreach (var chunk in chunks)
        {
            var map = MapData.ParseMap(chunk.ToArray(), c => c == '#');

            var vs = ProcessVertical(map, min).ToArray();
            r += vs.Sum();

            var hs = ProcessHorizontal(map, min).ToArray();
            r += hs.Sum() * 100;
        }

        return r;
    }

    static IEnumerable<int> ProcessVertical(bool[,] map, int min)
    {
        for (var x = 1; x < map.Width; ++x)
        {
            var bad = 0;

            for (var y = 0; y < map.Height; ++y)
            {
                for (var dx = 0; dx < map.Width; ++dx)
                {
                    if (x - dx < 1)
                        break;

                    if (x + dx >= map.Width)
                        break;

                    if (map[x - dx - 1, y] != map[x + dx, y])
                    {
                        bad++;

                        if (bad > min)
                            break;
                    }
                }

                if (bad > min)
                    break;
            }

            if (bad == min)
                yield return x;
        }
    }

    static IEnumerable<int> ProcessHorizontal(bool[,] map, int min)
    {
        for (var y = 1; y < map.Height; ++y)
        {
            var bad = 0;

            for (var x = 0; x < map.Width; ++x)
            {
                for (var dy = 0; dy < map.Height; ++dy)
                {
                    if (y - dy < 1)
                        break;

                    if (y + dy >= map.Height)
                        break;

                    if (map[x, y - dy - 1] != map[x, y + dy])
                    {
                        bad++;

                        if (bad > min)
                            break;
                    }
                }

                if (bad > min)
                    break;
            }

            if (bad == min)
                yield return y;
        }
    }
}
