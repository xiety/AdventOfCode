using Advent.Common;

namespace A2023.Problem12;

public class Solver : IProblemSolver<long>
{
    public long RunA(string filename)
    {
        var lines = File.ReadAllLines(filename);
        return lines.AsParallel().Select(RunALine).Sum();
    }

    public long RunB(string filename)
    {
        var lines = File.ReadAllLines(filename);
        return lines.AsParallel().Select(RunBLine).Sum();
    }

    private static long RunALine(string line)
    {
        var n = line.IndexOf(' ');
        var pattern = line[..n].Select(a => a switch { '.' => 0, '#' => 1, '?' => 2 }).ToArray();
        var combos = line[(n + 1)..].Split(',').Select(int.Parse).ToArray();

        var calculator = new Calculator();
        var result = calculator.BruteForce(pattern, combos);

        return result;
    }

    public long RunBLine(string line)
    {
        var n = line.IndexOf(' ');
        var pattern = line[..n].Select(a => a switch { '.' => 0, '#' => 1, '?' => 2 }).ToArray();
        var combos = line[(n + 1)..].Split(',').Select(int.Parse).ToArray();

        pattern = Enumerable.Range(0, 4).Aggregate(pattern.AsEnumerable(), (p, _) => [.. p, 2, .. pattern]).ToArray();
        combos = Enumerable.Range(0, 4).Aggregate(combos.AsEnumerable(), (p, _) => [.. p, .. combos]).ToArray();

        var calculator = new Calculator();
        var result = calculator.BruteForce(pattern, combos);

        return result;
    }

    private Dictionary<string, long> LoadCache()
    {
        if (!File.Exists(@"d:\results3.txt"))
            return [];

        var lines = File.ReadAllLines(@"d:\results3.txt");

        return lines.Select(line =>
        {
            var splits = line.Split(';');
            return (splits[1], long.Parse(splits[2]));
        })
        .ToDictionary(a => a.Item1, a => a.Item2);
    }
}

public class Calculator
{
    public long BruteForce(int[] pattern, int[] combos)
    {
        var questions = pattern.FindAllIndexes(2).ToArray();
        var requiredOne = combos.Sum();
        var requiredZero = pattern.Length - requiredOne;
        var quantityOne = pattern.Count(a => a == 1);
        var quantityZero = pattern.Count(a => a == 0);

        var result = Recurse(
            0, questions.Length, questions, [requiredZero, requiredOne], [quantityZero, quantityOne], pattern, combos, 0, 0, 2);

        return result;
    }

    private readonly Dictionary<string, long> history = [];

    private long Recurse(
        int depth, int questionsLength, int[] questions, int[] required, int[] quantity, int[] pattern, int[] combos, int fromCombos, int fromIndex, int parentPredict)
    {
        if (depth < questionsLength)
        {
            var key = String.Join("", pattern[fromIndex..]) + $";{fromCombos}";

            if (history.TryGetValue(key, out var result))
            {
                return result;
            }
            else
            {
                var index = questions[depth];

                var ret = 0L;

                for (var g = 0; g <= 1; ++g)
                {
                    if (parentPredict != 2 && parentPredict != g)
                        continue;

                    if (quantity[g] < required[g])
                    {
                        pattern[index] = g;
                        quantity[g]++;

                        var (good, skipCombo, skipIndex, predict) = Check(pattern, combos, fromCombos, fromIndex,
                                depth + 1 < questionsLength ? depth + 1 : -1);

                        if (good)
                        {
                            ret += Recurse(
                                depth + 1, questionsLength, questions, required, quantity, pattern, combos, skipCombo, skipIndex, predict);
                        }

                        quantity[g]--;
                    }
                }

                pattern[index] = 2;

                history[key] = ret;

                return ret;
            }
        }
        else
        {
            return 1;
        }
    }

    public (bool, int, int, int) Check(int[] pattern, int[] combos, int fromCombos, int fromIndex, int nextQuestion)
    {
        var start = -1;
        var partsCombos = fromCombos;
        var requiredCombos = fromCombos;
        var lastIndex = fromIndex;
        var hit = false;

        for (var i = fromIndex; i < pattern.Length + 1; ++i)
        {
            if (i == pattern.Length || pattern[i] is 0 or 2)
            {
                var lastByUnknown = i < pattern.Length && pattern[i] is 2;
                var lastByLength = i == pattern.Length;

                if (start is not -1)
                {
                    var a = i - start;

                    var b = combos[requiredCombos];

                    if (lastByUnknown && a == b)
                        return (true, partsCombos, lastIndex, 0);

                    if (lastByUnknown && a < b)
                        return (true, partsCombos, lastIndex, 1);

                    if (lastByLength && a != b)
                        return (false, 0, 0, 0);

                    if (a != b)
                        return (false, 0, 0, 0);

                    lastIndex = i + 1;

                    partsCombos++;

                    if (lastByLength && a == b)
                        return (true, partsCombos, i, 0);

                    if (combos.Length < partsCombos)
                        return (false, 0, 0, 0);

                    requiredCombos++;

                    hit = true;
                    start = -1;
                }
                else
                {
                    if (hit)
                        lastIndex = i;

                    if (lastByLength || lastByUnknown)
                        return (true, partsCombos, lastIndex, 2);
                }
            }
            else if (pattern[i] is 1)
            {
                if (start is -1)
                    start = i;
            }
        }

        throw new Exception($"{String.Join("", pattern)}, [{String.Join(", ", combos)}], {fromCombos}, {fromIndex}, {nextQuestion}");
    }
}
