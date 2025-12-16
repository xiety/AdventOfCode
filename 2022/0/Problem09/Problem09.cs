using System.Diagnostics;
using System.Text.RegularExpressions;

using Advent.Common;

namespace A2022.Problem09;

public static class Solver
{
    [GeneratedTest<long>(13, 6339)]
    public static long RunA(string[] lines)
    {
        var steps = LoadData(lines);

        var visitedByTail = new HashSet<NonEuclideanPos>();

        var headPos = new NonEuclideanPos(0, 0);
        var tailPos = new NonEuclideanPos(0, 0);

        visitedByTail.Add(tailPos);

        foreach (var step in steps)
        {
            headPos += step;
            tailPos = CalculateNewPos(headPos, tailPos);
            visitedByTail.Add(tailPos);
        }

        return visitedByTail.Count;
    }

    [GeneratedTest<long>(36, 2541)]
    public static long RunB(string[] lines)
    {
        var steps = LoadData(lines);

        var visitedByTail = new HashSet<NonEuclideanPos>();

        const int ropeLength = 10;
        var tailList = Array.CreateAndInitialize(ropeLength, _ => new NonEuclideanPos(0, 0));

        visitedByTail.Add(tailList[^1]);

        foreach (var step in steps)
        {
            tailList[0] += step;

            foreach (var i in 1..ropeLength)
                tailList[i] = CalculateNewPos(tailList[i - 1], tailList[i]);

            visitedByTail.Add(tailList[^1]);
        }

        return visitedByTail.Count;
    }

    static NonEuclideanPos CalculateNewPos(NonEuclideanPos h, NonEuclideanPos t)
    {
        var diff = (h - t);

        return diff.AbnormalLength > 1
            ? t + diff.Direction
            : t;
    }

    static IEnumerable<NonEuclideanPos> LoadData(string[] lines)
    {
        var items = CompiledRegs.FromLinesItemRegex(lines);

        foreach (var item in items)
        {
            foreach (var i in item.Number)
            {
                yield return item.Dir switch
                {
                    "U" => new NonEuclideanPos(0, 1),
                    "D" => new NonEuclideanPos(0, -1),
                    "L" => new NonEuclideanPos(-1, 0),
                    "R" => new NonEuclideanPos(1, 0),
                };
            }
        }
    }
}

[DebuggerDisplay($"[X={{{nameof(X)}}}, Y={{{nameof(Y)}}}, AbnormalLength={{{nameof(AbnormalLength)}}}]")]
record NonEuclideanPos(int X, int Y)
{
    public NonEuclideanPos Direction
        => new(Normalize(X), Normalize(Y));

    public int AbnormalLength
        => Abs(X) > Abs(Y) ? Abs(X) : Abs(Y);

    static int Normalize(int n)
        => n switch { < 0 => -1, > 0 => 1, 0 => 0 };

    static int Abs(int n)
        => n switch { < 0 => -n, _ => n };

    public static NonEuclideanPos operator +(NonEuclideanPos a, NonEuclideanPos b)
        => new(a.X + b.X, a.Y + b.Y);

    public static NonEuclideanPos operator -(NonEuclideanPos a, NonEuclideanPos b)
        => new(a.X - b.X, a.Y - b.Y);
}

record Item(string Dir, int Number);

static partial class CompiledRegs
{
    [GeneratedRegex(@$"(?<{nameof(Item.Dir)}>\w) (?<{nameof(Item.Number)}>\d+)")]
    [MapTo<Item>]
    public static partial Regex ItemRegex();
}
