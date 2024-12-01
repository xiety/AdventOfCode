using Advent.Common;

namespace A2019.Problem02;

public class Solver : ISolver<long>
{
    public long RunA(string[] lines, bool isSample)
    {
        var codes = CpuCodeLoader.Load(lines);

        if (!isSample)
        {
            codes[1] = 12;
            codes[2] = 2;
        }

        var cpu = new Cpu(codes, codes);
        var output = cpu.Interpret().ToArray();
        var result = cpu.ReadMemory(0);

        return result;
    }

    public long RunB(string[] lines, bool isSample)
    {
        var codes = CpuCodeLoader.Load(lines);

        var target = !isSample ? 19690720 : 100;

        for (var a = 0; a <= 99; ++a)
        {
            for (var b = 0; b <= 99; ++b)
            {
                codes[1] = a;
                codes[2] = b;

                var cpu = new Cpu(codes, [.. codes]);
                var output = cpu.Interpret().ToArray();
                var result = cpu.ReadMemory(0);

                if (result == target)
                    return a * 100 + b;
            }
        }

        throw new();
    }
}
