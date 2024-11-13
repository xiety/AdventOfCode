using Advent.Common;

namespace A2022.Problem01;

public class Solver : IProblemSolver<int>
{
    public int RunA(string filename)
    {
        var chunks = LoadFile(filename);

        var sums = Parse(chunks);
        var max = sums.Max();

        return max;
    }

    public int RunB(string filename)
    {
        var chunks = LoadFile(filename);

        var sums = Parse(chunks);
        var max = sums.OrderDescending().Take(3).Sum();

        return max;
    }

    static IEnumerable<int> Parse(IEnumerable<IEnumerable<string>> chunks)
        => chunks.Select(a => a.Select(int.Parse).Sum());

    static IEnumerable<IEnumerable<string>> LoadFile(string filename)
    {
        var lines = File.ReadAllLines(filename);
        var chunks = lines.Split(String.Empty);
        return chunks;
    }
}
