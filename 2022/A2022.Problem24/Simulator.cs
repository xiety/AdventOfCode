﻿namespace A2022.Problem24;

public class Simulator
{
    public static Map3d Create3dMap(string[] data, int sizeZ)
    {
        var size = GetSize(data, sizeZ);
        var blizzards = ParseBlizzards(data).ToArray();
        var map = new Map3d(size);

        RenderSlice(map, 0, blizzards);

        for (var z = 1; z < size.Z; ++z)
        {
            Simulate(map, blizzards);
            RenderSlice(map, z, blizzards);
        }

        return map;
    }

    static Pos3 GetSize(string[] data, int sizeZ)
        => new(data[0].Length, data.Length, sizeZ);

    static void Simulate(Map3d map, Blizzard[] blizzards)
    {
        foreach (var blizzard in blizzards)
        {
            var newPos = blizzard.Pos + blizzard.Delta;

            if (newPos.X == 0)
                newPos = newPos with { X = map.Size.X - 2 };
            else if (newPos.X == map.Size.X - 1)
                newPos = newPos with { X = 1 };

            if (newPos.Y == 0)
                newPos = newPos with { Y = map.Size.Y - 2 };
            else if (newPos.Y == map.Size.Y - 1)
                newPos = newPos with { Y = 1 };

            blizzard.Pos = newPos;
        }
    }

    static void RenderSlice(Map3d map, int z, Blizzard[] blizzards)
    {
        foreach (var blizzard in blizzards)
        {
            map[blizzard.Pos.X, blizzard.Pos.Y, z] = true;
        }

        for (var x = 0; x < map.Size.X; ++x)
        {
            map[x, 0, z] = true;
            map[x, map.Size.Y - 1, z] = true;
        }

        for (var y = 0; y < map.Size.Y; ++y)
        {
            map[0, y, z] = true;
            map[map.Size.X - 1, y, z] = true;
        }
    }

    static IEnumerable<Blizzard> ParseBlizzards(string[] data)
    {
        for (var y = 1; y < data.Length - 1; ++y)
        {
            for (var x = 1; x < data[y].Length - 1; ++x)
            {
                switch (data[y][x])
                {
                    case '>':
                        yield return new(new(x, y), new(+1, 0));
                        break;
                    case '<':
                        yield return new(new(x, y), new(-1, 0));
                        break;
                    case '^':
                        yield return new(new(x, y), new(0, -1));
                        break;
                    case 'v':
                        yield return new(new(x, y), new(0, +1));
                        break;
                }
            }
        }
    }
}

public class Map3d
{
    private readonly bool[,,] data;

    public Pos3 Size => new(data.GetLength(0), data.GetLength(1), data.GetLength(2));

    public Map3d(Pos3 size)
        => data = new bool[size.X, size.Y, size.Z];

    public Map3d(bool[,,] data)
        => this.data = data;

    public bool this[int x, int y, int z]
    {
        get => data[x, y, z];
        set => data[x, y, z] = value;
    }

    public bool this[Pos3 pos]
    {
        get => data[pos.X, pos.Y, pos.Z];
        set => data[pos.X, pos.Y, pos.Z] = value;
    }
}

public class Blizzard(Pos pos, Pos delta)
{
    public Pos Pos { get; set; } = pos;
    public Pos Delta { get; } = delta;
}
