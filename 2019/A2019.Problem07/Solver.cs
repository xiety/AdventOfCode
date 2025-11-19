using Advent.Common;

namespace A2019.Problem07;

public class Solver : ISolver<long>
{
    public long RunA(string[] lines, bool isSample)
    {
        var codes = CpuCodeLoader.Load(lines);

        return Enumerable.Range(0, 5).Select(a => (long)a)
            .ToArray()
            .Permutations(5)
            .Max(a => a.Aggregate(0L, (phase, input) => RunCpu([.. codes], [input, phase])));
    }

    public long RunB(string[] lines, bool isSample)
    {
        var codes = CpuCodeLoader.Load(lines);

        return Enumerable.Range(5, 5)
            .ToArray()
            .Permutations(5)
            .Max(a => RunCpuLoop(codes, [.. a]));
    }

    static long RunCpu(long[] codes, long[] inputs)
    {
        var cpu = new Cpu(codes, inputs);
        var output = cpu.Interpret();
        return output.Last();
    }

    static long RunCpuLoop(long[] codes, long[] phases)
    {
        List<long> input1 = [phases[0], 0];

        var cpu1 = new Cpu([.. codes], input1.Enumerate());
        var lastOutput = cpu1.Interpret();

        foreach (var phase in phases.Skip(1))
        {
            var input = lastOutput.Prepend(phase);
            var cpu = new Cpu([.. codes], input);
            lastOutput = cpu.Interpret();
        }

        input1.AddRange(lastOutput);

        return input1[^1];
    }
}
