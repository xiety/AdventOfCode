using Advent.Common;

namespace A2021.Problem06;

public class Solver : IProblemSolver<long>
{
    public long RunA(string filename)
        => Run(filename, 80);

    public long RunB(string filename)
        => Run(filename, 256);

    private static long Run(string filename, int days)
    {
        var items = File.ReadAllText(filename)
            .TrimEnd()
            .Split(",")
            .Select(int.Parse);

        const int period = 7;

        var array = new long[period + 2];

        foreach (var item in items)
            array[item]++;

        for (var i = 0; i < days; ++i)
        {
            var temp = array[0];

            for (var j = 1; j < array.Length; ++j)
                array[j - 1] = array[j];

            array[period - 1] += temp;
            array[period + 2 - 1] = temp;
        }

        var result = array.Sum();

        return result;
    }
}
