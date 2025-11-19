using Advent.Common;

namespace A2021.Problem10;

public class Solver : IProblemSolver<long>
{
    static readonly Dictionary<char, int> DicScores1 = new()
    {
        [')'] = 3,
        [']'] = 57,
        ['}'] = 1197,
        ['>'] = 25137,
    };

    static readonly Dictionary<char, int> DicScores2 = new()
    {
        ['('] = 1,
        ['['] = 2,
        ['{'] = 3,
        ['<'] = 4,
    };

    static readonly Dictionary<char, int> DicCloses = new()
    {
        [')'] = '(',
        [']'] = '[',
        ['}'] = '{',
        ['>'] = '<',
    };

    public long RunA(string filename)
    {
        var lines = File.ReadAllLines(filename);

        var score = 0;

        foreach (var line in lines)
        {
            var stack = new Stack<char>();

            foreach (var c in line)
            {
                if (DicCloses.ContainsValue(c))
                {
                    stack.Push(c);
                }
                else
                {
                    var open = stack.Pop();

                    if (open != DicCloses[c])
                    {
                        score += DicScores1[c];
                        break;
                    }
                }
            }
        }

        return score;
    }

    public long RunB(string filename)
    {
        var lines = File.ReadAllLines(filename);

        var scores = new List<long>();

        foreach (var line in lines)
        {
            var stack = new Stack<char>();

            var isError = false;

            foreach (var c in line)
            {
                if (DicCloses.ContainsValue(c))
                {
                    stack.Push(c);
                }
                else
                {
                    var open = stack.Pop();

                    if (open != DicCloses[c])
                    {
                        isError = true;
                        break;
                    }
                }
            }

            if (!isError)
            {
                var scoreLine = stack.Aggregate(0L, (acc, item) => acc * 5 + DicScores2[item]);
                scores.Add(scoreLine);
            }
        }

        return scores.Order().Skip(scores.Count / 2).First();
    }
}
