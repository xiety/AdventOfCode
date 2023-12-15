using Advent.Common;

namespace A2023.Problem15;

public class Solver : IProblemSolver<long>
{
    public long RunA(string filename)
    {
        var items = File.ReadAllText(filename).TrimEnd().Split(",");

        return items.Sum(Hash);
    }

    private long Hash(string text)
    {
        var ret = 0;

        var bytes = System.Text.Encoding.ASCII.GetBytes(text);

        foreach (var b in bytes)
        {
            ret += b;
            ret *= 17;
            ret %= 256;
        }

        return ret;
    }
}
