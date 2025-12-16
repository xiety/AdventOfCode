namespace A2022.Problem01;

public static class Solver
{
    [GeneratedTest<int>(24000, 71506)]
    public static int RunA(string[] lines)
        => LoadData(lines).Max();

    [GeneratedTest<int>(45000, 209603)]
    public static int RunB(string[] lines)
        => LoadData(lines).OrderDescending().Take(3).Sum();

    static IEnumerable<int> LoadData(string[] lines)
        => lines.SplitBy(String.Empty)
            .Select(a => a.Sum(int.Parse));
}
