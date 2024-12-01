using Advent.Common;

namespace A2019.Problem05;

public class Solver : ISolver<long>
{
    public long RunA(string[] lines, bool isSample)
        => Run(lines, [1]);

    public long RunB(string[] lines, bool isSample)
        => Run(lines, [5]);

    static long Run(string[] lines, long[] input)
    {
        var codes = CpuCodeLoader.Load(lines);

        var cpu = new Cpu(codes, input);
        var result = cpu.Interpret();

        return result.Last();
    }
}
