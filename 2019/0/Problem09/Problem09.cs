using Advent.Common;

namespace A2019.Problem09;

public static class Solver
{
    [GeneratedTest<long>(1125899906842624, 3409270027)]
    public static long RunA(string[] lines)
        => Run(lines, [1]);

    [GeneratedTest<long>(1125899906842624, 82760)]
    public static long RunB(string[] lines)
        => Run(lines, [2]);

    static long Run(string[] lines, long[] inputs)
    {
        var codes = CpuCodeLoader.Load(lines);
        var cpu = new Cpu(codes, inputs);
        var output = cpu.Interpret();
        return output.Single();
    }
}
