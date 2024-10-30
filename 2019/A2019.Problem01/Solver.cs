using Advent.Common;

namespace A2019.Problem01;

public class Solver : ISolver<int>
{
    public int RunA(string text, bool isSample)
    {
        var items = LoadData(text);

        return items
            .Select(CalcFuel)
            .Sum();
    }

    public int RunB(string text, bool isSample)
    {
        var items = LoadData(text);

        return items
            .Select(CalcTotalFuel)
            .Sum();
    }

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

    private static int[] LoadData(string text)
        => text.ToLines().Select(int.Parse).ToArray();
}
