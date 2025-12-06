using Advent.Common;

namespace A2025.Problem06;

public static class Solver
{
    [GeneratedTest<long>(4277556, 6295830249262)]
    public static long RunA(string[] lines)
        => Run(lines, false);

    [GeneratedTest<long>(3263827, 9194682052782)]
    public static long RunB(string[] lines)
        => Run(lines, true);

    static long Run(string[] lines, bool transpose)
        => lines[^1]
            .Index()
            .Where(x => x.Item != ' ')
            .Append((Index: lines[^1].Length + 1, Item: ' '))
            .Chain()
            .Sum(pair =>
                lines[..^1].ToArray(s => s[pair.First.Index..(pair.Second.Index - 1)])
                    .Apply(s => transpose ? s.Transposed() : s)
                    .Where(v => !string.IsNullOrWhiteSpace(v))
                    .Select(long.Parse)
                    .Apply(n => pair.First.Item == '*' ? n.Mul() : n.Sum()));
}
