namespace A2021.Problem06;

public static class Solver
{
    [GeneratedTest<long>(5934, 391888)]
    public static long RunA(string[] lines)
        => Run(lines, 80);

    [GeneratedTest<long>(26984457539, 1754597645339)]
    public static long RunB(string[] lines)
        => Run(lines, 256);

    static long Run(string[] lines, int days)
    {
        var items = lines[0]
            .TrimEnd()
            .Split(",")
            .Select(int.Parse);

        const int period = 7;

        var array = new long[period + 2];

        foreach (var item in items)
            array[item]++;

        foreach (var i in days)
        {
            var temp = array[0];

            foreach (var j in 1..array.Length)
                array[j - 1] = array[j];

            array[period - 1] += temp;
            array[period + 2 - 1] = temp;
        }

        return array.Sum();
    }
}
