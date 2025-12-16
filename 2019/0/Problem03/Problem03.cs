namespace A2019.Problem03;

public static class Solver
{
    [GeneratedTest<int>(135, 627)]
    public static int RunA(string[] lines)
    {
        var (chain1, chain2) = LoadData(lines);
        var intersections = EnumerateIntersections(chain1, chain2);

        return intersections
            .Min(a => a.Pos.ManhattanLength);
    }

    [GeneratedTest<int>(410, 13190)]
    public static int RunB(string[] lines)
    {
        var (chain1, chain2) = LoadData(lines);
        var intersections = EnumerateIntersections(chain1, chain2);

        return intersections
            .Min(a => CalcDistance(chain1, a.Line1, a.Pos) + CalcDistance(chain2, a.Line2, a.Pos));
    }

    static int CalcDistance(Line[] chain, Line last, Pos intersection)
        => chain.TakeWhile(a => a != last).Append(new(last.Start, intersection))
            .Sum(line => (line.Max - line.Min).ManhattanLength);

    static IEnumerable<Intersection> EnumerateIntersections(Line[] chain1, Line[] chain2)
        => from line1 in chain1
           from line2 in chain2
           let intersection = Intersect(line1, line2)
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

    static Pos? Intersect(Line a, Line b)
        => (a.IsHorizontal, b.IsHorizontal) switch
        {
            (true, false) when IsBetween(a.Min.X, a.Max.X, b.Min.X) && IsBetween(b.Min.Y, b.Max.Y, a.Min.Y)
                => new(b.Min.X, a.Min.Y),
            (false, true) when IsBetween(a.Min.Y, a.Max.Y, b.Min.Y) && IsBetween(b.Min.X, b.Max.X, a.Min.X)
                => new(a.Min.X, b.Min.Y),
            _ => null,
        };

    static bool IsBetween(int p1, int p2, int m)
        => Math.Min(p1, p2) <= m && m <= Math.Max(p1, p2);

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

record Intersection(Line Line1, Line Line2, Pos Pos);
