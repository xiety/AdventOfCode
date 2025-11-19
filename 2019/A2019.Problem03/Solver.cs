using System.Diagnostics;

using Advent.Common;

namespace A2019.Problem03;

public class Solver : ISolver<int>
{
    public int RunA(string[] lines, bool isSample)
    {
        var (chain1, chain2) = LoadData(lines);
        var intersections = EnumerateIntersections(chain1, chain2);

        return intersections
            .Min(a => a.Pos.ManhattanLength);
    }

    public int RunB(string[] lines, bool isSample)
    {
        var (chain1, chain2) = LoadData(lines);
        var intersections = EnumerateIntersections(chain1, chain2);

        return intersections
            .Min(a => CalcDistance(chain1, a.Line1, a.Pos) + CalcDistance(chain2, a.Line2, a.Pos));
    }

    static int CalcDistance(Line[] chain, Line last, Pos intersection)
        => chain.TakeWhile(a => a != last).Append(new(last.From, intersection))
            .Sum(line => (line.From - line.To).ManhattanLength);

    static IEnumerable<Intersection> EnumerateIntersections(Line[] chain1, Line[] chain2)
        => from line1 in chain1
           from line2 in chain2
           let intersection = line1.Intersect(line2)
           where intersection is Pos p && p != Pos.Zero
           select new Intersection(line1, line2, intersection.Value);

    static (Line[], Line[]) LoadData(string[] lines)
    {
        var data = lines
            .ToArray(a => ToLines(a.Split(",").Select(Parse)).ToArray());

        return data is [var chain1, var chain2, ..] ? (chain1, chain2) : throw new();
    }

    static IEnumerable<Line> ToLines(IEnumerable<Pos> items)
    {
        var last = Pos.Zero;

        foreach (var item in items)
        {
            var next = last + item;
            yield return new(last, next);
            last = next;
        }
    }

    static Pos Parse(string text)
    {
        var d = text[0];
        var len = int.Parse(text[1..]);

        return d switch
        {
            'L' => new(-len, 0),
            'R' => new(len, 0),
            'U' => new(0, -len),
            'D' => new(0, len),
        };
    }
}

[DebuggerDisplay("From: {From} To: {To}")]
public class Line(Pos from, Pos to)
{
    public Pos From { get; } = from;
    public Pos To { get; } = to;

    public bool IsHorizontal => From.Y == To.Y;

    public Pos? Intersect(Line that)
        => (this.IsHorizontal, that.IsHorizontal) switch
        {
            (true, false) when IsBetween(this.From.X, this.To.X, that.From.X) && IsBetween(that.From.Y, that.To.Y, this.From.Y)
                => new(that.From.X, this.From.Y),
            (false, true) when IsBetween(this.From.Y, this.To.Y, that.From.Y) && IsBetween(that.From.X, that.To.X, this.From.X)
                => new(this.From.X, that.From.Y),
            _ => null,
        };

    static bool IsBetween(int p1, int p2, int m)
        => Math.Min(p1, p2) <= m && m <= Math.Max(p1, p2);
}

record Intersection(Line Line1, Line Line2, Pos Pos);
