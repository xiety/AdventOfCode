using Advent.Common;

namespace A2023.Problem12;

public class Solver : IProblemSolver<long>
{
    public long RunA(string filename)
    {
        var lines = File.ReadAllLines(filename);
        return lines.Select(Process).Sum();
    }

    private long Process(string line)
    {
        var n = line.IndexOf(' ');
        var pattern = line[..n].Select(a => a switch { '.' => 0, '#' => 1, '?' => 2 }).ToArray();
        var combos = line[(n + 1)..].Split(',').Select(int.Parse).ToArray();

        return BruteForce(pattern, combos);
    }

    private int BruteForce(int[] pattern, int[] combos)
    {
        Console.WriteLine($"Start {String.Join("", pattern)} {(String.Join(", ", combos))}");

        var questions = pattern.Count(a => a == 2);
        var requiredOne = combos.Sum();
        var requiredZero = pattern.Length - requiredOne;
        var quantityOne = pattern.Count(a => a == 1);
        var quantityZero = pattern.Count(a => a == 0);

        var result =  Recurse(0, questions, requiredOne, requiredZero, quantityOne, quantityZero, pattern, combos);

        //Console.WriteLine($"  result: {result}");
        //Console.WriteLine();

        return result;
    }

    private int Recurse(
        int depth, int questions, int requiredOne, int requiredZero, int quantityOne, int quantityZero, int[] pattern, int[] combos)
    {
        //Console.WriteLine($"{depth} {String.Join("", pattern)}");

        var first = Array.IndexOf(pattern, 2);

        if (first >= 0)
        {
            var ret = 0;

            if (quantityOne < requiredOne)
            {
                pattern[first] = 1;

                if (Check(pattern, combos))
                    ret += Recurse(depth + 1, questions, requiredOne, requiredZero, quantityOne + 1, quantityZero, pattern, combos);
            }

            if (quantityZero < requiredZero)
            {
                pattern[first] = 0;

                if (Check(pattern, combos))
                    ret += Recurse(depth + 1, questions, requiredOne, requiredZero, quantityOne, quantityZero + 1, pattern, combos);
            }

            pattern[first] = 2;

            return ret;
        }
        else
        {
            //Console.WriteLine("  +1");
            return 1;
        }
    }

    private bool Check(int[] pattern, int[] combos)
    {
        var parts = new List<int>();
        var start = -1;

        for (var i = 0; i < pattern.Length + 1; ++i)
        {
            if (i == pattern.Length || pattern[i] is 0 or 2)
            {
                if (start is not -1)
                {
                    parts.Add(i - start);
                    start = -1;
                }

                if (i < pattern.Length && pattern[i] is 2)
                    break;
            }
            else if (pattern[i] is 1)
            {
                if (start is -1)
                    start = i;
            }
        }

        //var result = parts.Zip(combos).All(a => a.First <= a.Second);

        var result = true;

        for (var i = 0; i < Math.Max(parts.Count, combos.Length); ++i)
        {
            var a = i < parts.Count ? parts[i] : 0;
            var b = i < combos.Length ? combos[i] : 0;

            if (a > b)
            {
                result = false;
                break;
            }
        }

        return result;
    }

    private int[] ExpectedPattern(int[] combos, int[] pattern)
    {
        var expectedPattern = new int[pattern.Length];
        var fromX = 0;

        var list = new List<int[]>();

        for (var i = 0; i < combos.Length; ++i)
        {
            var line = new int[pattern.Length];

            var toX = pattern.Length - combos[(i + 1)..].Sum(a => a + 1) - 1;

            for (var x = 0; x < pattern.Length; ++x)
            {
                if (pattern[x] == 2 && x >= fromX && x <= toX)
                    line[x] = 1;
                else
                    line[x] = pattern[x];
            }

            fromX += combos[i] + 1;

            list.Add(line);
        }

        var output = new int[pattern.Length];

        for (var i = 0; i < pattern.Length; ++i)
        {
            if (list.All(a => a[i] == 1))
                output[i] = 1;
            else if (list.Any(a => a[i] == 1))
                output[i] = 2;
        }

        return output;
    }
}
