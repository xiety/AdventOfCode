using Advent.Common;

namespace A2022.Problem06;

public static class Solver
{
    [GeneratedTest<int>(7, 1892)]
    public static int RunA(string[] lines)
        => Run(lines, 4);

    [GeneratedTest<int>(19, 2313)]
    public static int RunB(string[] lines)
        => Run(lines, 14);

    static int Run(string[] lines, int len)
    {
        var line = lines[0];
        var result = 0;

        foreach (var i in (line.Length - len))
        {
            var num = line[i..(i + len)].Distinct().Count();

            if (num == len)
            {
                result = i + len;
                break;
            }
        }

        return result;
    }
}
