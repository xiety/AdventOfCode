using System.Text.RegularExpressions;

namespace A2022.Problem22;

public static class Solver
{
    [GeneratedTest<int>(6032, 50412)]
    public static int RunA(string[] lines)
        => Run(lines, false);

    [GeneratedTest<int>(5031, 130068)]
    public static int RunB(string[] lines, bool isSample)
    {
        if (isSample)
            throw new NotImplementedException();

        return Run(lines, true);
    }

    static int Run(string[] lines, bool isCube)
    {
        var mapText = lines[..^2];
        var map = new Map(mapText);

        var pathText = lines[^1];

        var path = ParsePath(pathText);

        var pos = FindPosition(map);
        var rotation = Rotation.Right;

        Pos[] offsets = [new(1, 0), new(0, 1), new(-1, 0), new(0, -1)];

        foreach (var command in path)
        {
            switch (command)
            {
                case Distance d:
                    foreach (var _ in d.Value)
                    {
                        var offset = offsets[(int)rotation];

                        Pos newPos;
                        var newRotation = rotation;

                        if (isCube)
                            (newPos, newRotation) = CalcPositionCube(pos + offset, rotation);
                        else
                            newPos = CalcPosition(map, pos, offset);

                        if (map[newPos] == ' ')
                            throw new("Wrong territory");

                        if (map[newPos] == '#')
                            break;

                        pos = newPos;
                        rotation = newRotation;
                    }

                    break;

                case Turn t:
                    if (t.Name == "L")
                        rotation--;
                    else
                        rotation++;

                    if (rotation < 0)
                        rotation += 4;
                    else if ((int)rotation >= 4)
                        rotation -= 4;

                    break;
            }
        }

        return (pos.Y + 1) * 1000 + (pos.X + 1) * 4 + (int)rotation;
    }

    static Pos CalcPosition(Map map, Pos pos, Pos offset)
    {
        var newPos = pos + offset;
        var bad = map.IsBad(newPos);

        if (!bad)
            return newPos;

        do
        {
            var d = newPos - offset;

            if (map.IsBad(d))
                break;

            newPos = d;
        }
        while (true);

        return newPos;
    }

    static (Pos, Rotation) CalcPositionCube(Pos pos, Rotation rotation)
        => pos switch
        {
            { X: 49, Y: >= 0 and <= 49 } => (new(0, 149 - pos.Y), Rotation.Right),
            { X: -1, Y: >= 100 and <= 149 } => (new(50, 149 - pos.Y), Rotation.Right),
            { X: >= 50 and <= 99, Y: -1 } => (new(0, pos.X + 100), Rotation.Right),
            { X: -1, Y: >= 150 and <= 199 } => (new(pos.Y - 100, 0), Rotation.Down),
            { X: >= 100 and <= 149, Y: 50 } => (new(99, pos.X - 50), Rotation.Left),
            { X: 100, Y: >= 50 and <= 99 } => (new(pos.Y + 50, 49), Rotation.Up),
            { X: 150, Y: >= 0 and <= 49 } => (new(99, 149 - pos.Y), Rotation.Left),
            { X: 100, Y: >= 100 and <= 149 } => (new(149, 149 - pos.Y), Rotation.Left),
            { X: >= 100 and <= 149, Y: -1 } => (new(pos.X - 100, 199), Rotation.Up),
            { X: >= 0 and <= 49, Y: 200 } => (new(pos.X + 100, 0), Rotation.Down),
            { X: 49, Y: >= 50 and <= 99 } => (new(pos.Y - 50, 100), Rotation.Down),
            { X: >= 0 and <= 49, Y: 99 } => (new(50, pos.X + 50), Rotation.Right),
            { X: >= 50 and <= 99, Y: 150 } => (new(49, pos.X + 100), Rotation.Left),
            { X: 50, Y: >= 150 and <= 199 } => (new(pos.Y - 100, 149), Rotation.Up),
            _ => (pos, rotation),
        };

    static Pos FindPosition(Map map)
    {
        var x = map[0].IndexOf('.');
        return new(x, 0);
    }

    static Base[] ParsePath(string pathText)
        => CompiledRegs.RegexPath()
           .Matches(pathText)
           .ToArray(a => a.Groups[nameof(Distance)].Success
               ? (Base)new Distance(int.Parse(a.Groups[nameof(Distance)].Value))
               : (Base)new Turn(a.Groups[nameof(Turn)].Value));
}

record Base;
record Distance(int Value) : Base;
record Turn(string Name) : Base;

public sealed class Map(string[] data)
{
    public int Width { get; } = data.Max(a => a.Length);
    public int Height { get; } = data.Length;

    public string this[int y] => data[y];
    public char this[int x, int y] => data[y][x];
    public char this[Pos pos] => data[pos.Y][pos.X];

    public bool IsBad(Pos pos)
        => pos.X < 0 || pos.Y < 0 || pos.Y >= Height || pos.X >= data[pos.Y].Length || this[pos] == ' ';
}

static partial class CompiledRegs
{
    [GeneratedRegex(@$"(?:(?<{nameof(Distance)}>\d+)|(?<{nameof(Turn)}>[RL]))")]
    public static partial Regex RegexPath();
}

enum Rotation
{
    Right = 0,
    Down = 1,
    Left = 2,
    Up = 3,
}
