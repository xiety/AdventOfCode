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
        foreach (var x in 1..map.Width)
        {
            var bad = 0;

            foreach (var y in map.Height)
            {
                foreach (var dx in map.Width)
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
        foreach (var y in 1..map.Height)
        {
            var bad = 0;

            foreach (var x in map.Width)
            {
                foreach (var dy in map.Height)
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
