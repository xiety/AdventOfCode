using Advent.Common;

namespace A2021.Problem01;

public class Solver : IProblemSolver<int>
{
    public int RunA(string fileName)
        => Run(fileName, 1);

    public int RunB(string fileName)
        => Run(fileName, 3);

    private static int Run(string fileName, int window)
    {
        var items = LoadFile(fileName);

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

    private static List<int> LoadFile(string fileName)
    {
        var lines = File.ReadAllLines(fileName);
        return lines.Select(int.Parse).ToList();
    }
}
