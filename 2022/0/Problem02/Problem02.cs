using Advent.Common;

namespace A2022.Problem02;

public static class Solver
{
    [GeneratedTest<int>(15, 12772)]
    public static int RunA(string[] lines)
        => LoadData(lines)
            .Sum(a => prices[a.Item2] + wins[a]);

    [GeneratedTest<int>(12, 11618)]
    public static int RunB(string[] lines)
        => LoadData(lines)
            .Sum(a =>
            {
                var req = outcome[a.Item2];
                var our = wins.First(b => b.Key.Item1 == a.Item1 && b.Value == req).Key.Item2;
                return req + prices[our];
            });

    static IEnumerable<(char, char)> LoadData(string[] lines)
        => lines.Select(Parse);

    static readonly Dictionary<(char, char), int> wins = new()
    {
        [('A', 'Z')] = 0,
        [('B', 'X')] = 0,
        [('C', 'Y')] = 0,

        [('A', 'X')] = 3,
        [('B', 'Y')] = 3,
        [('C', 'Z')] = 3,

        [('A', 'Y')] = 6,
        [('B', 'Z')] = 6,
        [('C', 'X')] = 6,
    };

    static readonly Dictionary<char, int> prices = new()
    {
        ['X'] = 1,
        ['Y'] = 2,
        ['Z'] = 3,
    };

    static readonly Dictionary<char, int> outcome = new()
    {
        ['X'] = 0,
        ['Y'] = 3,
        ['Z'] = 6,
    };

    static (char, char) Parse(string text)
        => (text[0], text[2]);
}
