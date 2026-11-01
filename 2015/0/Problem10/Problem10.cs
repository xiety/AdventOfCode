namespace A2015.Problem10;

public static class Solver
{
    [GeneratedTest<int>(6, 252594)]
    public static int RunA(string[] lines, bool isSample)
        => Calc(lines[0], isSample ? 5 : 40);

    [GeneratedTest<int>(102, 3579328)]
    public static int RunB(string[] lines, bool isSample)
        => Calc(lines[0], isSample ? 15 : 50);

    static int Calc(string text, int total)
        => Enumerable.Repeat(0, total)
            .Aggregate(text, (a, _) => Convert(a))
            .Length;

    static string Convert(string text)
        => string.Concat(text.RunsWithCounts().Select(r => $"{r.Count}{r.Value}"));
}
