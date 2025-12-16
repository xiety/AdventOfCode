namespace A2019.Problem05;

public static class Solver
{
    [GeneratedTest<long>(1, 16489636)]
    public static long RunA(string[] lines)
        => Run(lines, [1]);

    [GeneratedTest<long>(999, 9386583)]
    public static long RunB(string[] lines)
        => Run(lines, [5]);

    static long Run(string[] lines, long[] input)
    {
        var codes = CpuCodeLoader.Load(lines);

        var cpu = new Cpu(codes, input);
        var result = cpu.Interpret();

        return result.Last();
    }
}
