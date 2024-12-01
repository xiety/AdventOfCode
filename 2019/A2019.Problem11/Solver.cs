﻿using Advent.Common;

namespace A2019.Problem11;

public class Solver : ISolver<long, string>
{
    public long RunA(string[] lines, bool isSample)
    {
        var map = Run(lines, 0, false);
        return map.Count;
    }

    public string RunB(string[] lines, bool isSample)
    {
        var map = Run(lines, 1, true);

        var bbox = Rect.CreateBoundingBox(map.Keys);
        var c = new int[bbox.Width, bbox.Height];

        foreach (var (pos, value) in map)
            c.Set(pos - bbox.From, (int)value);

        return c.ToString(Environment.NewLine, "", a => a == 1 ? "#" : ".").TrimEnd();
    }

    static Dictionary<Pos, long> Run(string[] lines, int start, bool paintStart)
    {
        var codes = CpuCodeLoader.Load(lines);
        var pos = new Pos(0, 0);
        var dir = Dir.Up;
        var map = new Dictionary<Pos, long>();

        if (paintStart)
            map[pos] = start;

        var inputs = new List<long> { start };
        var cpu = new Cpu(codes, inputs.Enumerate());
        var outputs = cpu.Interpret();

        foreach (var (color, rotate) in outputs.Chunk(2))
        {
            map[pos] = color;

            dir = dir.RotateEnum(rotate == 0 ? -1 : 1);

            pos += dir switch
            {
                Dir.Up => new(0, -1),
                Dir.Right => new(1, 0),
                Dir.Down => new(0, 1),
                Dir.Left => new(-1, 0),
            };

            inputs.Add(map.GetValueOrDefault(pos));
        }

        return map;
    }

    enum Dir
    {
        Up = 0,
        Right = 1,
        Down = 2,
        Left = 3,
    }
}
