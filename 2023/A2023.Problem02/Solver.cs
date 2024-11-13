using System.Text.RegularExpressions;

using Advent.Common;

namespace A2023.Problem02;

public class Solver : IProblemSolver<int>
{
    static readonly string[] Colors = ["red", "green", "blue"];

    public int RunA(string filename)
    {
        int[] cubes = [12, 13, 14];

        var games = LoadFile(filename);

        var result = games
            .Where(g => !g.Balls.Any(b => cubes.Zip(b)
                .Any(tuple => tuple.First < tuple.Second)))
            .Sum(g => g.GameNumber);

        return result;
    }

    public int RunB(string filename)
    {
        var games = LoadFile(filename);

        var result = games
            .Select(g => Enumerable.Range(0, Colors.Length)
                .Select(i => g.Balls.Select(b => b[i]).Max()).Mul())
            .Sum();

        return result;
    }

    static Game[] LoadFile(string filename)
        => CompiledRegs
            .Regex()
            .FromFile<Step1>(filename)
            .Select(Parse)
            .ToArray();

    static Game Parse(Step1 step)
    {
        var balls = ParseData();

        return new(step.GameNumber, [.. balls]);

        IEnumerable<int[]> ParseData()
        {
            foreach (var part in step.GameData.Split("; "))
            {
                var data = new int[3];

                var parts = CompiledRegs.ColorRegex().FromLines<Step2>(part.Split(", "));

                foreach (var part2 in parts)
                {
                    var index = Array.IndexOf(Colors, part2.Color);
                    data[index] = part2.Num;
                }

                yield return data;
            }
        }
    }
}

record Step1(int GameNumber, string GameData);
record Step2(int Num, string Color);
record Game(int GameNumber, int[][] Balls);

static partial class CompiledRegs
{
    [GeneratedRegex(@$"^Game (?<{nameof(Step1.GameNumber)}>\d+): (?<{nameof(Step1.GameData)}>.*)$")]
    public static partial Regex Regex();

    [GeneratedRegex(@$"^(?<{nameof(Step2.Num)}>\d+) (?<{nameof(Step2.Color)}>\w+)$")]
    public static partial Regex ColorRegex();
}
