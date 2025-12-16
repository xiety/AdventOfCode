using System.Text.RegularExpressions;

using Advent.Common;

namespace A2023.Problem02;

public static class Solver
{
    static readonly string[] Colors = ["red", "green", "blue"];

    [GeneratedTest<int>(8, 3059)]
    public static int RunA(string[] lines)
    {
        int[] cubes = [12, 13, 14];

        var games = LoadData(lines);

        return games
            .Where(g => !g.Balls.Any(b => cubes.Zip(b)
                .Any(tuple => tuple.First < tuple.Second)))
            .Sum(g => g.GameNumber);
    }

    [GeneratedTest<int>(2286, 65371)]
    public static int RunB(string[] lines)
        => LoadData(lines)
            .Sum(g => Enumerable.Range(0, Colors.Length)
                .Select(i => g.Balls.Max<int[], int>(b => b[i])).Mul());

    static Game[] LoadData(string[] lines)
        => CompiledRegs
            .FromLinesRegex(lines)
            .ToArray(Parse);

    static Game Parse(Step1 step)
    {
        var balls = ParseData();

        return new(step.GameNumber, [.. balls]);

        IEnumerable<int[]> ParseData()
        {
            foreach (var part in step.GameData.Split("; "))
            {
                var data = new int[3];

                var parts = CompiledRegs.FromLinesColorRegex(part.Split(", "));

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
    [MapTo<Step1>]
    public static partial Regex Regex();

    [GeneratedRegex(@$"^(?<{nameof(Step2.Num)}>\d+) (?<{nameof(Step2.Color)}>\w+)$")]
    [MapTo<Step2>]
    public static partial Regex ColorRegex();
}
