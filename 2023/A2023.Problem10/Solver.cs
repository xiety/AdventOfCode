using System.Xml;

using Advent.Common;

namespace A2023.Problem10;

public class Solver : IProblemSolver<long>
{
    public long RunA(string filename)
    {
        var map = MapData.ParseMap(File.ReadAllLines(filename), c => c);

        var foundPath = FoundPath(map);

        map.ForEach(pos =>
        {
            if (!foundPath.Contains(pos))
                map.Set(pos, ' ');
        });

        var text = map.ToString(Environment.NewLine, "", a => a.ToString());

        return foundPath.Length % 2 == 0 ? foundPath.Length / 2 : foundPath.Length / 2 + 1;
    }

    private Pos[] FoundPath(char[,] map)
    {
        var startPos = map.EnumeratePositionsOf('S').First();
        var currentChar = map.Get(startPos);
        var nextChar = 'S';
        var foundPath = Array.Empty<Pos>();

        foreach (var startDir in Enum.GetValues<Dirs>())
        {
            var path = new List<Pos>();
            var length = 0;

            var foundWhole = false;
            var prevDir = startDir;
            var currentPos = startPos + GetOffsetFromDir(startDir);

            if (!map.IsInBounds(currentPos))
                continue;

            currentChar = map.Get(currentPos);

            if (currentChar == '.')
                continue;

            if (!IsConnected(startDir, 'S', currentChar))
                continue;

            path.Add(currentPos);

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

                    nextChar = map.Get(nextPos);

                    if (nextChar == '.')
                        continue;

                    if (IsConnected(dir, currentChar, nextChar))
                    {
                        prevDir = dir;
                        currentChar = nextChar;
                        currentPos = nextPos;
                        found = true;
                        break;
                    }
                }

                if (!found)
                    break;

                path.Add(currentPos);

                length++;

                if (nextChar == 'S')
                {
                    foundWhole = true;
                    break;
                }
            }
            while (true);

            if (foundWhole)
            {
                foundPath = path.ToArray();
                break;
            }
        }

        return foundPath;
    }

    private Dirs Anti(Dirs dir)
        => dir switch
        {
            Dirs.Top => Dirs.Bottom,
            Dirs.Right => Dirs.Left,
            Dirs.Left => Dirs.Right,
            Dirs.Bottom => Dirs.Top,
        };

    private static Pos GetOffsetFromDir(Dirs dir)
        => dir switch
        {
            Dirs.Top => new Pos(0, -1),
            Dirs.Right => new Pos(1, 0),
            Dirs.Left => new Pos(-1, 0),
            Dirs.Bottom => new Pos(0, 1),
        };

    private bool IsConnected(Dirs dir, char a, char b)
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

            foreach (var pa in psa)
            {
                foreach (var pb in psb)
                {
                    var r = (dir, pa, pb) switch
                    {
                        (Dirs.Top, Dirs.Top, Dirs.Bottom) => true,
                        (Dirs.Bottom, Dirs.Bottom, Dirs.Top) => true,
                        (Dirs.Left, Dirs.Left, Dirs.Right) => true,
                        (Dirs.Right, Dirs.Right, Dirs.Left) => true,
                        _ => false,
                    };

                    if (r == true)
                        return true;
                }
            }

            return false;
        }
    }

    private readonly Dictionary<char, Dirs[]> info = new()
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
