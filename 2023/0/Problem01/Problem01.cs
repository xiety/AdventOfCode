namespace A2023.Problem01;

public static class Solver
{
    [GeneratedTest<int>(142, 54644)]
    public static int RunA(string[] lines)
        => lines.Sum(ProcessA);

    [GeneratedTest<int>(281, 53348)]
    public static int RunB(string[] lines)
        => lines.Sum(ProcessB);

    static int ProcessA(string line)
    {
        var a = line.IndexOfAny(['0', '1', '2', '3', '4', '5', '6', '7', '8', '9']);
        var b = line.LastIndexOfAny(['0', '1', '2', '3', '4', '5', '6', '7', '8', '9']);

        return int.Parse(line[a].ToString() + line[b]);
    }

    static int ProcessB(string line)
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

        var minResult = existing.Select(a => (Pos: line.IndexOf(a.Key, StringComparison.Ordinal), a.Value))
            .MinBy(a => a.Pos).Value;
        var maxResult = existing.Select(a => (Pos: line.LastIndexOf(a.Key, StringComparison.Ordinal), a.Value))
            .MaxBy(a => a.Pos).Value;

        return int.Parse($"{minResult}{maxResult}");
    }
}
