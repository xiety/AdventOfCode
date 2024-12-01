using System.Collections;

using Advent.Common;

namespace A2019.Problem13;

public class Solver : ISolver<long>
{
    public long RunA(string[] lines, bool isSample)
    {
        var codes = CpuCodeLoader.Load(lines);
        var map = new Dictionary<Pos, Tile>();

        var cpu = new Cpu(codes, []);
        var outputs = cpu.Interpret();

        foreach (var (x, y, tile) in outputs.Chunk(3))
            map[new((int)x, (int)y)] = (Tile)tile;

        return map.Values.Count(a => a == Tile.Block);
    }

    public long RunB(string[] lines, bool isSample)
    {
        var codes = CpuCodeLoader.Load(lines);
        codes[0] = 2;

        var score = 0L;
        var ballPos = Pos.Zero;
        var paddlePos = Pos.Zero;

        var joystick = new Joystick();
        var cpu = new Cpu(codes, joystick);
        var outputs = cpu.Interpret();

        foreach (var (x, y, value) in outputs.Chunk(3))
        {
            if (x == -1)
            {
                score = value;
            }
            else
            {
                var pos = new Pos((int)x, (int)y);
                var tile = (Tile)value;

                if (tile == Tile.Ball)
                    ballPos = pos;
                else if (tile == Tile.Paddle)
                    paddlePos = pos;

                joystick.Value = Math.Sign(ballPos.X - paddlePos.X);
            }
        }

        return score;
    }

    enum Tile
    {
        Empty = 0,
        Wall = 1,
        Block = 2,
        Paddle = 3,
        Ball = 4,
    }
}
