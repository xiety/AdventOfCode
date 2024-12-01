using Advent.Common;

namespace A2019.Problem09;

public class Solver : ISolver<long>
{
    public long RunA(string[] lines, bool isSample)
        => Run(lines, [1]);

    public long RunB(string[] lines, bool isSample)
        => Run(lines, [2]);

    static long Run(string[] lines, long[] inputs)
    {
        var codes = CpuCodeLoader.Load(lines);
        var cpu = new Cpu(codes, inputs);
        var output = cpu.Interpret();
        return output.Single();
    }
}
