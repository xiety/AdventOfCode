using System.Text.RegularExpressions;

using Advent.Common;

namespace A2020.Problem05;

public class Solver : ISolver<int>
{
    public int RunA(string[] lines, bool isSample)
        => lines.Max(GetId);

    public int RunB(string[] lines, bool isSample)
    {
        var ids = lines.Select(GetId).Order().ToArray();
        return ids.Skip(1).Where((a, i) => a != ids[i] + 1).Select(a => a - 1).First();
    }

    static int GetId(string line)
    {
        var p1 = line[..7].Select(a => a == 'B').ToArray();
        var p2 = line[7..].Select(a => a == 'R').ToArray();

        return Parse(p1) * 8 + Parse(p2);
    }

    static int Parse(bool[] array)
    {
        var max = (int)Math.Pow(2, array.Length);

        return array.Aggregate((min: 0, max), (acc, c) => c
            ? (acc.min / 2 + acc.max / 2, acc.max)
            : (acc.min, acc.min / 2 + acc.max / 2)).min;
    }
}
