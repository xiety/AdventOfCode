using Advent.Common;

namespace A2023.Problem13;

public class Solver : IProblemSolver<long>
{
    public long RunA(string filename)
        => Run(filename, 0);

    public long RunB(string filename)
        => Run(filename, 1);

    static long Run(string filename, int min)
    {
        var chunks = File.ReadAllLines(filename).Split(String.Empty);

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
        for (var x = 1; x < map.GetWidth(); ++x)
        {
            var bad = 0;

            for (var y = 0; y < map.GetHeight(); ++y)
            {
                for (var dx = 0; dx < map.GetWidth(); ++dx)
                {
                    if (x - dx - 1 < 0)
                        break;

                    if (x + dx >= map.GetWidth())
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
        for (var y = 1; y < map.GetHeight(); ++y)
        {
            var bad = 0;

            for (var x = 0; x < map.GetWidth(); ++x)
            {
                for (var dy = 0; dy < map.GetHeight(); ++dy)
                {
                    if (y - dy - 1 < 0)
                        break;

                    if (y + dy >= map.GetHeight())
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
