using Advent.Common;

namespace A2019.Problem02;

public class Solver : ISolver<int>
{
    public int RunA(string[] lines, bool isSample)
    {
        var codes = LoadData(lines);

        if (!isSample)
        {
            codes[1] = 12;
            codes[2] = 2;
        }

        var cpu = new Cpu(codes, codes);
        var result = cpu.Interpret();

        return result;
    }

    public int RunB(string[] lines, bool isSample)
    {
        var codes = LoadData(lines);

        var target = !isSample ? 19690720 : 100;

        for (var a = 0; a <= 99; ++a)
        {
            for (var b = 0; b <= 99; ++b)
            {
                codes[1] = a;
                codes[2] = b;

                var cpu = new Cpu(codes, [.. codes]);
                var result = cpu.Interpret();

                if (result == target)
                    return a * 100 + b;
            }
        }

        throw new();
    }

    private static int[] LoadData(string[] lines)
        => lines.First().Split(",").Select(int.Parse).ToArray();
}

public class Cpu(int[] codes, int[] memory)
{
    private readonly ResizableArray<int> codes = new(codes);
    private readonly ResizableArray<int> memory = new(memory);

    public int Interpret()
    {
        var position = 0;

        do
        {
            var n = codes[position];

            if (n == 99)
                break;

            position = n switch
            {
                1 => Op(position, (a, b) => a + b),
                2 => Op(position, (a, b) => a * b),
                _ => position,
            };
        }
        while (true);

        return memory[0];
    }

    private int Op(int position, Func<int, int, int> func)
    {
        var a = memory[codes[position + 1]];
        var b = memory[codes[position + 2]];
        memory[codes[position + 3]] = func(a, b);
        position += 4;
        return position;
    }
}
