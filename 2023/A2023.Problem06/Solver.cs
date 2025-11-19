using Advent.Common;

namespace A2023.Problem06;

public class Solver : IProblemSolver<long>
{
    public long RunA(string filename)
    {
        var lines = File.ReadAllLines(filename);
        var times = ParseA(lines[0]);
        var distances = ParseA(lines[1]);

        return times.Zip(distances)
            .Select(a => Calculate(a.First, a.Second))
            .Mul();
    }

    static long[] ParseA(string line)
        => line.Split(' ', StringSplitOptions.RemoveEmptyEntries).Skip(1).ToArray(long.Parse);

    public long RunB(string filename)
    {
        var lines = File.ReadAllLines(filename);
        var time = ParseB(lines[0]);
        var distance = ParseB(lines[1]);

        return Calculate(time, distance);
    }

    static int Calculate(long time, long distance)
        => EnumerableExtensions
            .LongRange(0, time)
            .Count(b => ((time - b) * b) > distance);

    static long ParseB(string line)
        => long.Parse(String.Concat(line.Split(' ', StringSplitOptions.RemoveEmptyEntries).Skip(1)));
}
