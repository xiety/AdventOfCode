using Advent.Common;

namespace A2023.Problem14;

public class Solver : IProblemSolver<long>
{
    public long RunA(string filename)
    {
        var map = LoadFile(filename);

        North(map);

        var result = map.EnumeratePositionsOf('O')
                        .Select(a => map.GetWidth() - a.Y)
                        .Sum();

        return result;
    }

    public long RunB(string filename)
    {
        var map = LoadFile(filename);

        var history = new Dictionary<string, long>();

        const long total = 1_000_000_000L;

        for (var i = 0L; i < total; ++i)
        {
            var key = map.ToString(a => a.ToString()).TrimEnd();

            if (history.TryGetValue(key, out var index))
            {
                var required = index + (total - i) % (i - index);
                var requiredKey = history.First(a => a.Value == required).Key;
                var requiredData = requiredKey.Split(Environment.NewLine).ToArray();

                map = MapData.ParseMap(requiredData, c => c);

                break;
            }
            else
            {
                North(map);
                West(map);
                South(map);
                East(map);

                history.Add(key, i);
            }
        }

        var result = CalcResult(map);

        return result;
    }

    private static int CalcResult(char[,] map)
        => map.EnumeratePositionsOf('O')
              .Select(a => map.GetWidth() - a.Y)
              .Sum();

    private static char[,] LoadFile(string filename)
        => MapData.ParseMap(File.ReadAllLines(filename), c => c);

    private static void North(char[,] map)
    {
        var border = new int[map.GetWidth()];

        for (var y = 0; y < map.GetHeight(); ++y)
        {
            for (var x = 0; x < map.GetWidth(); ++x)
            {
                var c = map[x, y];

                if (c == '#')
                {
                    border[x] = y + 1;
                }
                else if (c == 'O')
                {
                    map[x, y] = '.';
                    map[x, border[x]] = 'O';
                    border[x]++;
                }
            }
        }
    }

    private static void South(char[,] map)
    {
        var border = ArrayEx.CreateAndInitialize(map.GetWidth(), map.GetHeight() - 1);

        for (var y = map.GetHeight() - 1; y >= 0; --y)
        {
            for (var x = 0; x < map.GetWidth(); ++x)
            {
                var c = map[x, y];

                if (c == '#')
                {
                    border[x] = y - 1;
                }
                else if (c == 'O')
                {
                    map[x, y] = '.';
                    map[x, border[x]] = 'O';
                    border[x]--;
                }
            }
        }
    }

    private static void West(char[,] map)
    {
        var border = new int[map.GetHeight()];

        for (var x = 0; x < map.GetWidth(); ++x)
        {
            for (var y = 0; y < map.GetHeight(); ++y)
            {
                var c = map[x, y];

                if (c == '#')
                {
                    border[y] = x + 1;
                }
                else if (c == 'O')
                {
                    map[x, y] = '.';
                    map[border[y], y] = 'O';
                    border[y]++;
                }
            }
        }
    }

    private static void East(char[,] map)
    {
        var border = ArrayEx.CreateAndInitialize(map.GetHeight(), map.GetWidth() - 1);

        for (var x = map.GetWidth() - 1; x >= 0; --x)
        {
            for (var y = 0; y < map.GetHeight(); ++y)
            {
                var c = map[x, y];

                if (c == '#')
                {
                    border[y] = x - 1;
                }
                else if (c == 'O')
                {
                    map[x, y] = '.';
                    map[border[y], y] = 'O';
                    border[y]--;
                }
            }
        }
    }
}
