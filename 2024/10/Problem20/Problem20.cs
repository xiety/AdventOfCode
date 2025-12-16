using Advent.Common;

namespace A2024.Problem20;

public static class Solver
{
    [GeneratedTest<int>(44, 1332)]
    public static int RunA(string[] lines, bool isSample)
    {
        var map = LoadData(lines);
        var start = MapData.FindPos(lines, 'S');
        var end = MapData.FindPos(lines, 'E');
        var target = isSample ? 2 : 100;

        var path = PathFinder.Find(map, start, end)!;

        return Run(map, start, path)
            .Count(a => path.Length - a.Value >= target);
    }

    [GeneratedTest<int>(285, 987695)]
    public static int RunB(string[] lines, bool isSample)
    {
        var map = LoadData(lines);
        var start = MapData.FindPos(lines, 'S');
        var end = MapData.FindPos(lines, 'E');
        var target = isSample ? 50 : 100;

        var path = PathFinder.Find(map, start, end)!;

        return RunB(map, start, path)
            .Count(a => path.Length - a.Value >= target);
    }

    static Dictionary<(Pos, Pos), int> Run(int[,] map, Pos start, Pos[] path)
    {
        var dic = new Dictionary<(Pos, Pos), int>();

        var index = -1;

        foreach (var p in path.Prepend(start))
        {
            foreach (var offset1 in ArrayEx.Offsets)
            {
                var p1 = p + offset1;
                var p2 = p1 + offset1;

                if (map.GetOrDefault(p1, -1) == -1 && map.GetOrDefault(p2, -1) == 1)
                {
                    var i2 = Array.IndexOf(path, p2);

                    if (i2 > index)
                    {
                        var newLen = path.Length - (i2 - index - 2);
                        dic.Add((p1, p2), newLen);
                    }
                }
            }

            index++;
        }

        return dic;
    }

    static Dictionary<(Pos, Pos), int> RunB(int[,] map, Pos start, Pos[] path)
    {
        var dic = new Dictionary<(Pos, Pos), int>();
        var index = -1;
        const int cheat = 20;

        foreach (var p in path.Prepend(start))
        {
            for (var dy = -cheat; dy <= cheat; ++dy)
            {
                for (var dx = -cheat; dx <= cheat; ++dx)
                {
                    var delta = new Pos(dx, dy);

                    if (delta.ManhattanLength <= cheat)
                    {
                        var p2 = p + delta;

                        if (!dic.ContainsKey((p, p2)) && map.IsInBounds(p2) && map.Get(p2) == 1)
                        {
                            var i2 = Array.IndexOf(path, p2, index + 1);

                            if (i2 > -1)
                            {
                                var newLen = path.Length - (i2 - index - delta.ManhattanLength);

                                if (newLen < path.Length)
                                    dic[(p, p2)] = newLen;
                            }
                        }
                    }
                }
            }

            index++;
        }

        return dic;
    }

    static int[,] LoadData(string[] lines)
        => MapData.ParseMap(lines, a => a switch { '#' => -1, _ => 1 });
}
