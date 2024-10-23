using System.Text.RegularExpressions;

using Advent.Common;

namespace A2022.Problem06;

public class Solver : IProblemSolver<int>
{
    public int RunA(string filename)
        => Run(filename, 4);

    public int RunB(string filename)
        => Run(filename, 14);

    private int Run(string filename, int len)
    {
        var line = File.ReadAllText(filename);
        var result = 0;

        for (var i = 0; i < line.Length - len; ++i)
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
