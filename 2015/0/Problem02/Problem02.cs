namespace A2015.Problem02;

public static class Solver
{
    [GeneratedTest<int>(58 + 43, 1606483)]
    public static int RunA(string[] lines)
    {
        var sizes = LoadData(lines).ToArray(a =>
            a.ToArrayMany((b, i) => a.Index().Where(c => c.Index != i).ToArray(c => c.Item * b)));

        return sizes.Sum(a => a.Sum() + a.Min());
    }

    [GeneratedTest<int>(34 + 14, 3842356)]
    public static int RunB(string[] lines)
        => LoadData(lines)
        .ToArray(a => a.Order().Take(2).Sum() * 2 + a.Mul()).Sum();

    static int[][] LoadData(string[] lines)
        => lines.ToArray(a => a.Split('x').ToArray(int.Parse));
}
