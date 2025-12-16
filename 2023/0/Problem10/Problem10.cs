namespace A2023.Problem10;

public static class Solver
{
    [GeneratedTest<long>(8, 7005)]
    public static long RunA(string[] lines)
    {
        var map = MapData.ParseMap(lines, c => c);
        var foundPath = FoundPath(map);
        return foundPath.Length % 2 == 0 ? foundPath.Length / 2 : foundPath.Length / 2 + 1;
    }

    [GeneratedTest<long>(10, 417)]
    public static long RunB(string[] lines)
    {
        var map = MapData.ParseMap(lines, c => c);

        var foundPath = FoundPath(map);

        var newmap = Array.CreateAndInitialize(map.Width, map.Height, ' ');

        foreach (var step in foundPath)
            newmap.Set(step.pos, step.value);

        var enlarged = Enlarged(newmap);

        enlarged.Flood(Pos.Zero);

        return Calc(enlarged);
    }

    static long Calc(bool[,] enlarged)
    {
        var square = 0;

        for (var x = 2; x < enlarged.Width - 1; x += 3)
            for (var y = 2; y < enlarged.Height - 1; y += 3)
                if (!enlarged.Get(new Pos(x, y)))
                    square++;

        return square;
    }

    static bool[,] Enlarged(char[,] map)
    {
        var large = new bool[map.Width * 3 + 2, map.Height * 3 + 2];

        map.ForEach(pos =>
        {
            var c = map.Get(pos);
            var pattern = patterns[c];

            foreach (var x in 3)
                foreach (var y in 3)
                    if (pattern[y][x] == '#')
                        large[x + pos.X * 3 + 1, y + pos.Y * 3 + 1] = true;
        });

        return large;
    }

    static readonly Dictionary<char, string[]> patterns = new()
    {
        ['F'] = ["...",
                 ".##",
                 ".#."],
        ['L'] = [".#.",
                 ".##",
                 "..."],
        ['7'] = ["...",
                 "##.",
                 ".#."],
        ['J'] = [".#.",
                 "##.",
                 "..."],
        ['|'] = [".#.",
                 ".#.",
                 ".#."],
        ['-'] = ["...",
                 "###",
                 "..."],
        [' '] = ["...",
                 "...",
                 "..."],
    };

    static (Pos pos, char value)[] FoundPath(char[,] map)
    {
        var startPos = map.EnumeratePositionsOf('S').First();
        var foundPath = Array.Empty<(Pos, char)>();

        foreach (var startDir in Enum.GetValues<Dirs>())
        {
            var path = new List<(Pos, char)>();

            var foundWhole = false;
            var prevDir = startDir;
            var currentPos = startPos + GetOffsetFromDir(startDir);

            if (!map.IsInBounds(currentPos))
                continue;

            var currentChar = map.Get(currentPos);

            if (currentChar == '.')
                continue;

            if (!IsConnected(startDir, 'S', currentChar))
                continue;

            path.Add((currentPos, currentChar));

            do
            {
                var found = false;

                foreach (var dir in Enum.GetValues<Dirs>())
                {
                    if (dir == Anti(prevDir))
                        continue;

                    var offset = GetOffsetFromDir(dir);

                    var nextPos = currentPos + offset;

                    if (!map.IsInBounds(nextPos))
                        continue;

                    var nextChar = map.Get(nextPos);

                    if (nextChar == '.')
                        continue;

                    if (IsConnected(dir, currentChar, nextChar))
                    {
                        if (nextChar == 'S')
                        {
                            Dirs[] check = [startDir, Anti(dir)];
                            nextChar = info.First(a => a.Value.All(b => check.Contains(b))).Key;
                            foundWhole = true;
                        }

                        prevDir = dir;
                        currentChar = nextChar;
                        currentPos = nextPos;
                        found = true;
                        break;
                    }
                }

                if (!found)
                    break;

                path.Add((currentPos, currentChar));
            }
            while (!foundWhole);

            if (foundWhole)
            {
                foundPath = [.. path];
                break;
            }
        }

        return foundPath;
    }

    static Dirs Anti(Dirs dir)
        => dir switch
        {
            Dirs.Top => Dirs.Bottom,
            Dirs.Right => Dirs.Left,
            Dirs.Left => Dirs.Right,
            Dirs.Bottom => Dirs.Top,
        };

    static Pos GetOffsetFromDir(Dirs dir)
        => dir switch
        {
            Dirs.Top => new Pos(0, -1),
            Dirs.Right => new Pos(1, 0),
            Dirs.Left => new Pos(-1, 0),
            Dirs.Bottom => new Pos(0, 1),
        };

    static bool IsConnected(Dirs dir, char a, char b)
    {
        if (a == 'S')
        {
            var psb = info[b];
            return psb.Contains(Anti(dir));
        }
        else
        {
            var psa = info[a];

            if (b == 'S')
                return psa.Contains(dir);

            var psb = info[b];

            var items =
                from pa in psa
                from pb in psb
                select (dir, pa, pb) switch
                {
                    (Dirs.Top, Dirs.Top, Dirs.Bottom) => true,
                    (Dirs.Bottom, Dirs.Bottom, Dirs.Top) => true,
                    (Dirs.Left, Dirs.Left, Dirs.Right) => true,
                    (Dirs.Right, Dirs.Right, Dirs.Left) => true,
                    _ => false,
                };

            return items.Any(r => r);
        }
    }

    static readonly Dictionary<char, Dirs[]> info = new()
    {
        ['|'] = [Dirs.Top, Dirs.Bottom],
        ['-'] = [Dirs.Left, Dirs.Right],
        ['L'] = [Dirs.Top, Dirs.Right],
        ['J'] = [Dirs.Top, Dirs.Left],
        ['7'] = [Dirs.Left, Dirs.Bottom],
        ['F'] = [Dirs.Right, Dirs.Bottom],
    };
}

public enum Dirs
{
    Top,
    Right,
    Bottom,
    Left,
}
