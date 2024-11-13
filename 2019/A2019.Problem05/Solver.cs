using Advent.Common;

namespace A2019.Problem05;

public class Solver : ISolver<int>
{
    public int RunA(string[] lines, bool isSample)
        => Run(lines, [1]);

    public int RunB(string[] lines, bool isSample)
        => Run(lines, [5]);

    static int Run(string[] lines, int[] input)
    {
        var codes = LoadData(lines);

        var cpu = new Cpu(codes, codes, input);
        var result = cpu.Interpret();

        return result.Last();
    }

    static int[] LoadData(string[] lines)
        => lines.First().Split(",").Select(int.Parse).ToArray();
}

public class Cpu(int[] codes, int[] memory, int[] input)
{
    readonly ResizableArray<int> codes = new(codes);
    readonly ResizableArray<int> memory = new(memory);
    readonly Queue<int> input = new(input);
    readonly Queue<int> output = new();

    public int[] Interpret()
    {
        var position = 0;

        do
        {
            var (n, p1, p2) = GetOp(position);

            if (n == 99)
                break;

            position = n switch
            {
                1 => Op(position, p1, p2, (a, b) => a + b),
                2 => Op(position, p1, p2, (a, b) => a * b),
                3 => Input(position),
                4 => Output(position, p1),
                5 => Jump(position, p1, p2, a => a != 0),
                6 => Jump(position, p1, p2, a => a == 0),
                7 => Op(position, p1, p2, (a, b) => a < b ? 1 : 0),
                8 => Op(position, p1, p2, (a, b) => a == b ? 1 : 0),
                _ => position,
            };
        }
        while (true);

        return output.ToArray();
    }

    (int, int, int) GetOp(int position)
    {
        var code = codes[position];

        var n = code % 100;
        var p1 = (code / 100) % 10;
        var p2 = (code / 1000) % 10;

        return (n, p1, p2);
    }

    int Output(int position, int p1)
    {
        var value = GetValue(position + 1, p1);
        output.Enqueue(value);
        position += 2;
        return position;
    }

    int Input(int position)
    {
        var value = input.Dequeue();
        memory[codes[position + 1]] = value;
        position += 2;
        return position;
    }

    int Jump(int position, int p1, int p2, Func<int, bool> func)
    {
        var a = GetValue(position + 1, p1);

        if (func(a))
            position = GetValue(position + 2, p2);
        else
            position += 3;

        return position;
    }

    int Op(int position, int p1, int p2, Func<int, int, int> func)
    {
        var a = GetValue(position + 1, p1);
        var b = GetValue(position + 2, p2);
        memory[codes[position + 3]] = func(a, b);
        position += 4;
        return position;
    }

    int GetValue(int position, int mode)
        => mode == 0 ? memory[codes[position]] : codes[position];
}
