﻿using Advent.Common;

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

    static long RunALine(string line)
    {
        var n = line.IndexOf(' ');
        var pattern = line[..n].ToArray(a => a switch { '.' => 0, '#' => 1, '?' => 2 });
        var combos = line[(n + 1)..].Split(',').ToArray(int.Parse);

        var calculator = new Calculator();
        var result = calculator.BruteForce(pattern, combos);

        return result;
    }

    static long RunBLine(string line)
    {
        var n = line.IndexOf(' ');
        var pattern = line[..n].ToArray(a => a switch { '.' => 0, '#' => 1, '?' => 2 });
        var combos = line[(n + 1)..].Split(',').ToArray(int.Parse);

        pattern = Enumerable.Range(0, 4).Aggregate(pattern, (p, _) => [.. p, 2, .. pattern]).ToArray();
        combos = Enumerable.Range(0, 4).Aggregate(combos, (p, _) => [.. p, .. combos]).ToArray();

        var calculator = new Calculator();
        var result = calculator.BruteForce(pattern, combos);

        return result;
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

    readonly Dictionary<string, long> history = [];

    long Recurse(
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

    static (bool, int, int, int) Check(int[] pattern, int[] combos, int fromCombos, int fromIndex, int nextQuestion)
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

        throw new($"{String.Join("", pattern)}, [{String.Join(", ", combos)}], {fromCombos}, {fromIndex}, {nextQuestion}");
    }
}
