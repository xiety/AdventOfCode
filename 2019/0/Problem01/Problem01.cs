using Advent.Common;

namespace A2019.Problem01;

public static class Solver
{
    [GeneratedTest<int>(33583 + 654 + 2 + 2, 3311492)]
    public static int RunA(string[] lines)
        => LoadData(lines).Sum(CalcFuel);

    [GeneratedTest<int>(50346 + 966 + 2, 4964376)]
    public static int RunB(string[] lines)
        => LoadData(lines).Sum(CalcTotalFuel);

    static int CalcFuel(int n)
        => Math.Max(0, (n / 3) - 2);

    static int CalcTotalFuel(int n)
    {
        var current = n;
        var total = 0;

        do
        {
            var fuel = CalcFuel(current);
            total += fuel;
            current = fuel;
        }
        while (current > 0);

        return total;
    }

    static int[] LoadData(string[] lines)
        => lines.ToArray(int.Parse);
}
