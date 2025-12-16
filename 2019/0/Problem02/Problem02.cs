namespace A2019.Problem02;

public static class Solver
{
    [GeneratedTest<long>(3500, 3058646)]
    public static long RunA(string[] lines, bool isSample)
    {
        var codes = CpuCodeLoader.Load(lines);

        if (!isSample)
        {
            codes[1] = 12;
            codes[2] = 2;
        }

        var cpu = new Cpu(codes, codes);
        var output = cpu.Interpret().ToArray();
        return cpu.ReadMemory(0);
    }

    [GeneratedTest<long>(0, 8976)]
    public static long RunB(string[] lines, bool isSample)
    {
        var codes = CpuCodeLoader.Load(lines);

        var target = !isSample ? 19690720 : 100;

        foreach (var a in 100)
        {
            foreach (var b in 100)
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
