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

        var min = Int32.MaxValue;
        var minResult = "";

        var max = Int32.MinValue;
        var maxResult = "";

        foreach (var pair in dic)
        {
            var n = line.IndexOf(pair.Key);

            if (n > -1 && n < min)
            {
                min = n;
                minResult = pair.Value.ToString();
            }

            var nl = line.LastIndexOf(pair.Key);

            if (nl > -1 && nl > max)
            {
                max = nl;
                maxResult = pair.Value.ToString();
            }
        }

        var result = Int32.Parse(minResult + maxResult);

        return result;
    }
}
