using System.Text.RegularExpressions;

using Advent.Common;

namespace A2023.Problem02;

public class Solver : IProblemSolver<int>
{
    static readonly string[] colors = ["red", "green", "blue"];

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
        int[] cubes = [12, 13, 14];

        var games = LoadFile(filename);

        var result = games
            .Select(g => Enumerable.Range(0, colors.Length)
                .Select(i => g.Balls.Select(b => b[i]).Max()).Mul())
            .Sum();

        return result;
    }

    private Game[] LoadFile(string filename)
        => CompiledRegs
            .Regex()
            .FromFile<Step1>(filename)
            .Select(Parse)
            .ToArray();

    private Game Parse(Step1 step)
    {
        var balls = new List<int[]>();

        foreach (var part in step.GameData.Split("; "))
        {
            var data = new int[3];

            foreach (var bp in part.Split(", "))
            {
                var index = Array.FindIndex(colors, color => bp.EndsWith(" " + color));
                var n = bp.IndexOf(' ');
                var num = int.Parse(bp[..n]);
                data[index] = num;
            }

            balls.Add(data);
        }

        return new(step.GameNumber, balls.ToArray());
    }
}

record Step1(int GameNumber, string GameData);
record Game(int GameNumber, int[][] Balls);

static partial class CompiledRegs
{
    [GeneratedRegex(@$"^Game (?<{nameof(Step1.GameNumber)}>\d+): (?<{nameof(Step1.GameData)}>.*)$")]
    public static partial Regex Regex();
}
