namespace A2015.Problem01;

public static class Solver
{
    [GeneratedTest<int>(3, 138)]
    public static int RunA(string[] lines)
        => lines[0].Count(a => a == '(') * 2 - lines[0].Length;

    [GeneratedTest<int>(5, 1771)]
    public static int RunB(string[] lines)
        => lines[0]
        .Accumulate((Sum: 0, Index: 0), (acc, a) => (acc.Sum + (a == '(' ? 1 : -1), acc.Index + 1))
        .First(a => a.Sum < 0).Index;
}
