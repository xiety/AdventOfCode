using System.Diagnostics;
using System.Text.RegularExpressions;

using Advent.Common;

namespace A2022.Problem09;

public class Solver : IProblemSolver<long>
{
    public long RunA(string filename)
    {
        var steps = LoadFile(filename);

        var visitedByTail = new HashSet<NonEuqlideanPos>();

        var headPos = new NonEuqlideanPos(0, 0);
        var tailPos = new NonEuqlideanPos(0, 0);

        visitedByTail.Add(tailPos);

        foreach (var step in steps)
        {
            headPos += step;
            tailPos = CalculateNewPos(headPos, tailPos);
            visitedByTail.Add(tailPos);
        }

        return visitedByTail.Count;
    }

    public long RunB(string filename)
    {
        var steps = LoadFile(filename);

        var visitedByTail = new HashSet<NonEuqlideanPos>();

        var ropeLength = 10;
        var tailList = ArrayEx.CreateAndInitialize(ropeLength, _ => new NonEuqlideanPos(0, 0));

        visitedByTail.Add(tailList[^1]);

        foreach (var step in steps)
        {
            tailList[0] += step;

            for (var i = 1; i < ropeLength; ++i)
                tailList[i] = CalculateNewPos(tailList[i - 1], tailList[i]);

            visitedByTail.Add(tailList[^1]);
        }

        return visitedByTail.Count;
    }

    static NonEuqlideanPos CalculateNewPos(NonEuqlideanPos h, NonEuqlideanPos t)
    {
        var diff = (h - t);

        return diff.AbnormalLength > 1
            ? t + diff.Direction
            : t;
    }

    static IEnumerable<NonEuqlideanPos> LoadFile(string fileName)
    {
        var lines = CompiledRegs.ItemRegex().FromFile<Item>(fileName);

        foreach (var item in lines)
        {
            for (var i = 0; i < item.Number; ++i)
            {
                yield return item.Dir switch
                {
                    "U" => new NonEuqlideanPos(0, 1),
                    "D" => new NonEuqlideanPos(0, -1),
                    "L" => new NonEuqlideanPos(-1, 0),
                    "R" => new NonEuqlideanPos(1, 0),
                    _ => throw new Exception()
                };
            }
        }
    }
}

[DebuggerDisplay($"[X={{{nameof(X)}}}, Y={{{nameof(Y)}}}, AbnormalLength={{{nameof(AbnormalLength)}}}]")]
record NonEuqlideanPos(int X, int Y)
{
    public NonEuqlideanPos Direction
        => new(Normalize(X), Normalize(Y));

    public int AbnormalLength
        => Abs(X) > Abs(Y) ? Abs(X) : Abs(Y);

    private static int Normalize(int n)
        => n switch { < 0 => -1, > 0 => 1, 0 => 0 };

    private static int Abs(int n)
        => n switch { < 0 => -n, _ => n };

    public static NonEuqlideanPos operator +(NonEuqlideanPos a, NonEuqlideanPos b)
        => new(a.X + b.X, a.Y + b.Y);

    public static NonEuqlideanPos operator -(NonEuqlideanPos a, NonEuqlideanPos b)
        => new(a.X - b.X, a.Y - b.Y);
}

public record Item(string Dir, int Number);

static partial class CompiledRegs
{
    [GeneratedRegex(@$"(?<{nameof(Item.Dir)}>\w) (?<{nameof(Item.Number)}>\d+)")]
    public static partial Regex ItemRegex();
}
