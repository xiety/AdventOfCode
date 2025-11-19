using Advent.Common;

namespace A2022.Problem01;

public class Solver : IProblemSolver<int>
{
    public int RunA(string filename)
    {
        var chunks = LoadFile(filename);

        var sums = Parse(chunks);

        return sums.Max();
    }

    public int RunB(string filename)
    {
        var chunks = LoadFile(filename);

        var sums = Parse(chunks);
        return sums.OrderDescending().Take(3).Sum();
    }

    static IEnumerable<int> Parse(IEnumerable<IEnumerable<string>> chunks)
        => chunks.Select(a => a.Sum(int.Parse));

    static IEnumerable<IEnumerable<string>> LoadFile(string filename)
    {
        var lines = File.ReadAllLines(filename);
        return lines.SplitBy(String.Empty);
    }
}
