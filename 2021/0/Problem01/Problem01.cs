using Advent.Common;

namespace A2021.Problem01;

public static class Solver
{
    [GeneratedTest<int>(7, 1298)]
    public static int RunA(string[] lines)
        => Run(lines, 1);

    [GeneratedTest<int>(5, 1248)]
    public static int RunB(string[] lines)
        => Run(lines, 3);

    static int Run(string[] lines, int window)
    {
        var items = LoadData(lines);

        var result = 0;

        foreach (var i in (items.Length - window))
        {
            var m1 = 0;
            var m2 = 0;

            foreach (var j in window)
            {
                m1 += items[i + j];
                m2 += items[i + j + 1];
            }

            if (m2 > m1)
                result++;
        }

        return result;
    }

    static int[] LoadData(string[] lines)
        => lines.ToArray(int.Parse);
}
