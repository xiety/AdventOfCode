using System.Collections;

using Advent.Common;

namespace A2019.Problem13;

public class Solver : ISolver<long>
{
    public long RunA(string[] lines, bool isSample)
    {
        var codes = LoadData(lines);
        var map = new Dictionary<Pos, Tile>();

        var cpu = new Cpu(codes, []);
        var outputs = cpu.Interpret();

        foreach (var (x, y, tile) in outputs.Chunk(3))
            map[new((int)x, (int)y)] = (Tile)tile;

        return map.Values.Count(a => a == Tile.Block);
    }

    public long RunB(string[] lines, bool isSample)
    {
        var codes = LoadData(lines);
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

                joystick.Value = (long)Math.Sign(ballPos.X - paddlePos.X);
            }
        }

        return score;
    }

    static long[] LoadData(string[] lines)
        => lines.First().Split(",").Select(long.Parse).ToArray();

    enum Tile
    {
        Empty = 0,
        Wall = 1,
        Block = 2,
        Paddle = 3,
        Ball = 4,
    }
}

public class Cpu
{
    private readonly Dictionary<long, long> memory;
    private readonly IEnumerator<long> inputEnumerator;

    //mutable
    private long position;
    private long relativeBase;

    public Cpu(long[] codes, IEnumerable<long> input)
    {
        memory = codes.ToDictionary<long, long>();
        inputEnumerator = input.GetEnumerator();
    }

    public IEnumerable<long> Interpret()
    {
        relativeBase = 0L;
        position = 0L;

        do
        {
            var code = ReadMemory(position);

            var (op, p1, p2, p3) = ParseCode(code);

            switch (op)
            {
                case 99:
                    yield break;
                case 4:
                    yield return GetValue(position + 1, p1);
                    position += 2;
                    break;
                default:
                    RunOp(op, p1, p2, p3);
                    break;
            }
        }
        while (true);
    }

    private bool RunOp(long op, Mode p1, Mode p2, Mode p3)
        => op switch
        {
            1 => Op(p1, p2, p3, (a, b) => a + b),
            2 => Op(p1, p2, p3, (a, b) => a * b),
            3 => Input(p1),
            5 => Jump(p1, p2, a => a != 0),
            6 => Jump(p1, p2, a => a == 0),
            7 => Op(p1, p2, p3, (a, b) => a < b ? 1 : 0),
            8 => Op(p1, p2, p3, (a, b) => a == b ? 1 : 0),
            9 => MoveRelativeBase(p1),
        };

    static (long, Mode, Mode, Mode) ParseCode(long code)
    {
        var n = code % 100;
        var p1 = (Mode)((code / 100) % 10);
        var p2 = (Mode)((code / 1000) % 10);
        var p3 = (Mode)((code / 10000) % 10);
        return (n, p1, p2, p3);
    }

    bool Input(Mode p1)
    {
        var nextAvailable = inputEnumerator.MoveNext();
        if (!nextAvailable)
            throw new("No input value");
        SetValue(position + 1, p1, inputEnumerator.Current);
        position += 2;
        return true;
    }

    bool Jump(Mode p1, Mode p2, Func<long, bool> func)
    {
        var a = GetValue(position + 1, p1);

        if (func(a))
            position = GetValue(position + 2, p2);
        else
            position += 3;

        return true;
    }

    bool Op(Mode p1, Mode p2, Mode p3, Func<long, long, long> func)
    {
        var a = GetValue(position + 1, p1);
        var b = GetValue(position + 2, p2);
        SetValue(position + 3, p3, func(a, b));
        position += 4;
        return true;
    }

    bool MoveRelativeBase(Mode p1)
    {
        relativeBase += GetValue(position + 1, p1);
        position += 2;
        return true;
    }

    long SetValue(long position, Mode mode, long value)
        => mode switch
        {
            Mode.Address => (memory[ReadMemory(position)] = value),
            Mode.Value => throw new(),
            Mode.Relative => memory[relativeBase + ReadMemory(position)] = value,
        };

    long GetValue(long address, Mode mode)
    {
        var value = ReadMemory(address);

        return mode switch
        {
            Mode.Address => ReadMemory(value),
            Mode.Value => value,
            Mode.Relative => ReadMemory(relativeBase + value),
        };
    }

    long ReadMemory(long position)
        => memory.GetValueOrDefault(position);

    enum Mode
    {
        Address = 0,
        Value = 1,
        Relative = 2,
    }
}

class Joystick : IEnumerable<long>
{
    public long Value { get; set; }

    public IEnumerator<long> GetEnumerator()
    {
        do
        {
            yield return Value;
        }
        while (true);
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
