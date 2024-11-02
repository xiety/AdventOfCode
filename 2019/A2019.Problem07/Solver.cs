using Advent.Common;

namespace A2019.Problem07;

public class Solver : ISolver<int>
{
    public int RunA(string[] lines, bool isSample)
    {
        var codes = LoadData(lines);

        return Enumerable.Range(0, 5)
            .Permutations(5)
            .Max(a => a.Aggregate(0, (phase, input) => RunCpu([.. codes], [input, phase])));
    }

    public int RunB(string[] lines, bool isSample)
    {
        var codes = LoadData(lines);

        return Enumerable.Range(5, 5)
            .Permutations(5)
            .Max(a => RunCpuLoop(codes, [.. a]));
    }

    static int RunCpu(int[] codes, int[] inputs)
    {
        var cpu = new Cpu(codes, inputs);
        var output = cpu.Interpret();
        return output.Last();
    }

    static int RunCpuLoop(int[] codes, int[] phases)
    {
        List<int> input1 = [phases[0], 0];

        var cpu1 = new Cpu([.. codes], Enumerate(input1));
        var lastOutput = cpu1.Interpret();

        foreach (var phase in phases.Skip(1))
        {
            var input = lastOutput.Prepend(phase);
            var cpu = new Cpu([.. codes], input);
            lastOutput = cpu.Interpret();
        }

        foreach (var p in lastOutput)
            input1.Add(p);

        return input1.Last();
    }

    // to make it possible to add elements to a list during enumeration
    static IEnumerable<T> Enumerate<T>(List<T> list)
    {
        for (var i = 0; i < list.Count; ++i)
            yield return list[i];
    }

    static int[] LoadData(string[] lines)
        => lines.First().Split(",").Select(int.Parse).ToArray();
}

public class Cpu(int[] codes, IEnumerable<int> input)
{
    private readonly ResizableArray<int> codes = new(codes);
    private readonly ResizableArray<int> memory = new(codes);
    private readonly IEnumerator<int> inputEnumerator = input.GetEnumerator();

    public IEnumerable<int> Interpret()
    {
        var position = 0;

        do
        {
            var (n, p1, p2) = GetOp(position);

            if (n == 99)
                yield break;

            if (n == 4)
            {
                var value = GetValue(position + 1, p1);
                yield return value;
                position += 2;
            }
            else
            {
                position = n switch
                {
                    1 => Op(position, p1, p2, (a, b) => a + b),
                    2 => Op(position, p1, p2, (a, b) => a * b),
                    3 => Input(position),
                    5 => Jump(position, p1, p2, a => a != 0),
                    6 => Jump(position, p1, p2, a => a == 0),
                    7 => Op(position, p1, p2, (a, b) => a < b ? 1 : 0),
                    8 => Op(position, p1, p2, (a, b) => a == b ? 1 : 0),
                    _ => position,
                };
            }
        }
        while (true);
    }

    (int, int, int) GetOp(int position)
    {
        var code = codes[position];

        var n = code % 100;
        var p1 = (code / 100) % 10;
        var p2 = (code / 1000) % 10;

        return (n, p1, p2);
    }

    int Input(int position)
    {
        inputEnumerator.MoveNext();
        memory[codes[position + 1]] = inputEnumerator.Current;
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
