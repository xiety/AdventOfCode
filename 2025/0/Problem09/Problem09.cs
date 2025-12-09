using System.Text.RegularExpressions;

using Advent.Common;

namespace A2025.Problem09;

public static class Solver
{
    [GeneratedTest<long>(50, 4755064176)]
    public static long RunA(string[] lines)
    {
        var points = LoadData(lines);
        return points.EnumeratePairs()
            .Select(a => new Rect(a.First, a.Second))
            .Max(a => a.Area);
    }

    [GeneratedTest<long>(24, 1613305596)]
    public static long RunB(string[] lines)
    {
        var points = LoadData(lines);
        var edges = points.Append(points[0])
            .Chain()
            .ToArray(p => new Line(p.First, p.Second));

        return points.EnumeratePairs()
            .Select(p => new Rect(p.First, p.Second))
            .Where(rect => !edges.Any(edge => rect.Intersects(edge)))
            .Where(rect => IsInside(edges, rect.From))
            .Max(a => a.Area);
    }

    static bool IsInside(Line[] edges, Pos p)
        => edges.Count(a => a.IsVertical && a.Start.X > p.X && a.Min.Y <= p.Y && a.Max.Y > p.Y) % 2 == 1;

    static Pos[] LoadData(string[] lines)
        => lines.ToArray(Pos.Parse);
}
