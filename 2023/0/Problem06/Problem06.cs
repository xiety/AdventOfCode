namespace A2023.Problem06;

public static class Solver
{
    [GeneratedTest<long>(288, 1084752)]
    public static long RunA(string[] lines)
    {
        var times = ParseA(lines[0]);
        var distances = ParseA(lines[1]);

        return times.Zip(distances)
            .Select(a => Calculate(a.First, a.Second))
            .Mul();
    }

    static long[] ParseA(string line)
        => line.Split(' ', StringSplitOptions.RemoveEmptyEntries).Skip(1).ToArray(long.Parse);

    [GeneratedTest<long>(71503, 28228952)]
    public static long RunB(string[] lines)
    {
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
