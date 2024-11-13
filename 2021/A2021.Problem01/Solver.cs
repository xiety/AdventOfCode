using Advent.Common;

namespace A2021.Problem01;

public class Solver : IProblemSolver<int>
{
    public int RunA(string filename)
        => Run(filename, 1);

    public int RunB(string filename)
        => Run(filename, 3);

    static int Run(string filename, int window)
    {
        var items = LoadFile(filename);

        var result = 0;

        for (var i = 0; i < items.Count - window; ++i)
        {
            var m1 = 0;
            var m2 = 0;

            for (var j = 0; j < window; ++j)
            {
                m1 += items[i + j];
                m2 += items[i + j + 1];
            }

            if (m2 > m1)
                result++;
        }

        return result;
    }

    static List<int> LoadFile(string filename)
    {
        var lines = File.ReadAllLines(filename);
        return lines.Select(int.Parse).ToList();
    }
}
