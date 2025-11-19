using Advent.Common;

namespace A2023.Problem14;

public class Solver : IProblemSolver<long>
{
    public long RunA(string filename)
    {
        var map = LoadFile(filename);

        North(map);

        return map.EnumeratePositionsOf('O')
                  .Sum(a => map.Width - a.Y);
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

        return CalcResult(map);
    }

    static int CalcResult(char[,] map)
        => map.EnumeratePositionsOf('O')
              .Sum(a => map.Width - a.Y);

    static char[,] LoadFile(string filename)
        => MapData.ParseMap(File.ReadAllLines(filename), c => c);

    static void North(char[,] map)
    {
        var border = new int[map.Width];

        for (var y = 0; y < map.Height; ++y)
        {
            for (var x = 0; x < map.Width; ++x)
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

    static void South(char[,] map)
    {
        var border = Array.CreateAndInitialize1D(map.Width, initValue: map.Height - 1);

        for (var y = map.Height - 1; y >= 0; --y)
        {
            for (var x = 0; x < map.Width; ++x)
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

    static void West(char[,] map)
    {
        var border = new int[map.Height];

        for (var x = 0; x < map.Width; ++x)
        {
            for (var y = 0; y < map.Height; ++y)
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

    static void East(char[,] map)
    {
        var border = Array.CreateAndInitialize1D(map.Height, initValue: map.Width - 1);

        for (var x = map.Width - 1; x >= 0; --x)
        {
            for (var y = 0; y < map.Height; ++y)
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
