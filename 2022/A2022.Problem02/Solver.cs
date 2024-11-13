using Advent.Common;

namespace A2022.Problem02;

public class Solver : IProblemSolver<int>
{
    public int RunA(string filename)
    {
        var parsed = LoadFile(filename);
        var results = parsed.Select(a => prices[a.Item2] + wins[a]);
        return results.Sum();
    }

    public int RunB(string filename)
    {
        var parsed = LoadFile(filename);

        var outcome = new Dictionary<char, int>
        {
            ['X'] = 0,
            ['Y'] = 3,
            ['Z'] = 6,
        };

        var results = parsed
            .Select(a =>
            {
                var req = outcome[a.Item2];
                var our = wins.First(b => b.Key.Item1 == a.Item1 && b.Value == req).Key.Item2;
                return req + prices[our];
            });

        return results.Sum();
    }

    static IEnumerable<(char, char)> LoadFile(string filename)
    {
        var lines = File.ReadAllLines(filename);
        var parsed = lines.Select(Parse);
        return parsed;
    }

    readonly Dictionary<(char, char), int> wins = new()
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

    readonly Dictionary<char, int> prices = new()
    {
        ['X'] = 1,
        ['Y'] = 2,
        ['Z'] = 3,
    };

    static (char, char) Parse(string text)
        => (text[0], text[2]);
}
