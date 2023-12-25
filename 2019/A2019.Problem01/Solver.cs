using Advent.Common;

namespace A2019.Problem01;

public class Solver : IProblemSolver<long>
{
    public long RunA(string filename)
    {
        var items = File.ReadAllLines(filename).Select(int.Parse).ToArray();

        var result = 0L;

        foreach (var item in items)
        {
            var fuel = (long)Math.Floor(item / (float)3) - 2L;
            Console.WriteLine(fuel);
            result += fuel;
        }

        return result;
    }
}
