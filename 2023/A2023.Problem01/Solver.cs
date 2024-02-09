using Advent.Common;

namespace A2023.Problem01;

public class Solver : IProblemSolver<int>
{
    public int RunA(string fileName)
    {
        var lines = File.ReadAllLines(fileName);

        var result = lines.Select(ProcessA).Sum();

        return result;
    }

    private int ProcessA(string line)
    {
        var a = line.IndexOfAny(['0', '1', '2', '3', '4', '5', '6', '7', '8', '9']);
        var b = line.LastIndexOfAny(['0', '1', '2', '3', '4', '5', '6', '7', '8', '9']);

        return Int32.Parse(line[a].ToString() + line[b].ToString());
    }

    public int RunB(string fileName)
    {
        var lines = File.ReadAllLines(fileName);

        var result = lines.Select(ProcessB).Sum();

        return result;
    }

    private int ProcessB(string line)
    {
        var dic = new Dictionary<string, int>
        {
            ["one"] = 1,
            ["two"] = 2,
            ["three"] = 3,
            ["four"] = 4,
            ["five"] = 5,
            ["six"] = 6,
            ["seven"] = 7,
            ["eight"] = 8,
            ["nine"] = 9,
            ["0"] = 0,
            ["1"] = 1,
            ["2"] = 2,
            ["3"] = 3,
            ["4"] = 4,
            ["5"] = 5,
            ["6"] = 6,
            ["7"] = 7,
            ["8"] = 8,
            ["9"] = 9,
        };

        var existing = dic.Where(a => line.Contains(a.Key)).ToArray();

        var minResult = existing.Select(a => (Pos: line.IndexOf(a.Key), a.Value)).MinBy(a => a.Pos).Value;
        var maxResult = existing.Select(a => (Pos: line.LastIndexOf(a.Key), a.Value)).MaxBy(a => a.Pos).Value;

        var result = Int32.Parse($"{minResult}{maxResult}");

        return result;
    }
}
