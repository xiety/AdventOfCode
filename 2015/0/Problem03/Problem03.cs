namespace A2015.Problem03;

public static class Solver
{
    [GeneratedTest<int>(8, 2565)]
    public static int RunA(string[] lines)
        => LoadData(lines).Sum(a => EnumerateHouses(a).Distinct().Count());

    [GeneratedTest<int>(16, 2639)]
    public static int RunB(string[] lines)
        => LoadData(lines).Sum(a => EnumerateHouses(a.Even)
            .Concat(EnumerateHouses(a.Odd)).Distinct().Count());

    static IEnumerable<Pos> EnumerateHouses(IEnumerable<Pos> offsets)
        => offsets.Accumulate(new Pos(0, 0), (prev, offset) => prev + offset);

    static Pos ToOffset(char c)
        => c switch
        {
            '>' => new(1, 0),
            '<' => new(-1, 0),
            '^' => new(0, -1),
            'v' => new(0, 1),
        };

    static Pos[][] LoadData(string[] lines)
        => lines.ToArray(a => a.ToArray(ToOffset));
}
