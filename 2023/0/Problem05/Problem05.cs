namespace A2023.Problem05;

public static class Solver
{
    [GeneratedTest<long>(35, 57075758)]
    public static long RunA(string[] lines)
    {
        var parts = lines.SplitBy(String.Empty).ToArray();
        var seeds = parts[0][0].Split(' ').Skip(1).Select(long.Parse);
        var chunks = parts.Skip(1).ToArray(ParseChunk);
        return seeds.Min(a => RecurseA(chunks, "seed", a));
    }

    [GeneratedTest<long>(46, 31161857)]
    public static long RunB(string[] lines)
    {
        var parts = lines.SplitBy(String.Empty).ToArray();
        var seeds = parts[0][0].Split(' ').Skip(1).ToArray(long.Parse)
            .Chunk(2).ToArray(a => (a[0], a[0] + a[1] - 1));
        var chunks = parts.Skip(1).ToArray(ParseChunk);

        return seeds.Min(a => RecurseB(chunks, "seed", a.Item1, a.Item2));
    }

    static long RecurseA(Chunk[] chunks, string from, long fromValue)
    {
        if (from == "location")
            return fromValue;

        var chunk = chunks.Single(a => a.From == from);

        var target = chunk.Maps
            .FirstOrDefault(a => a.SourceStart < fromValue && a.SourceEnd >= fromValue);

        var result = fromValue;

        if (target is not null)
            result = target.TargetStart + (fromValue - target.SourceStart);

        return RecurseA(chunks, chunk.To, result);
    }

    static long RecurseB(Chunk[] chunks, string from, long fromStart, long fromEnd)
    {
        if (from == "location")
            return fromStart;

        var chunk = chunks.Single(a => a.From == from);

        var points = chunk.Maps.Select(a => a.SourceStart)
            .Concat(chunk.Maps.Select(a => a.SourceEnd))
            .Distinct().Order().ToArray();

        var parts = ToParts(fromStart, fromEnd, points);

        return parts
            .Min(a => CalculateB(chunks, chunk, a.Item1, a.Item2));
    }

    static long CalculateB(Chunk[] chunks, Chunk chunk, long partStart, long partEnd)
    {
        var target = chunk.Maps
            .FirstOrDefault(a => Interval.IsIntersect(a.SourceStart, a.SourceEnd, partStart, partEnd));

        var targetStart = partStart;
        var targetEnd = partEnd;

        if (target is not null)
        {
            (targetStart, targetEnd) = Interval.Intersect(target.SourceStart, target.SourceEnd, partStart, partEnd);
            targetStart += target.TargetStart - target.SourceStart;
            targetEnd += target.TargetStart - target.SourceStart;
        }

        return RecurseB(chunks, chunk.To, targetStart, targetEnd);
    }

    static (long, long)[] ToParts(long fromStart, long fromEnd, long[] points)
    {
        if (fromStart == fromEnd)
            return [(fromStart, fromEnd)];

        long[] temp = [fromStart,
            .. points.Where(a => fromStart <= a && fromEnd >= a),
            fromEnd];

        var pointsInside = temp.Distinct().Order().ToArray();

        return pointsInside.Chain().ToArray();
    }

    static Chunk ParseChunk(IEnumerable<string> lines)
    {
        var array = lines.ToArray();

        var first = array[0];

        var n1 = first.IndexOf('-');
        var n2 = first.LastIndexOf(' ');

        var from = first[..n1];
        var to = first[(n1 + 4)..n2];

        var maps = array.Skip(1).ToArray(ParseMap);

        return new(from, to, maps);
    }

    static ItemMap ParseMap(string a)
    {
        var splits = a.Split(' ').ToArray(long.Parse);
        return new(splits[0], splits[1], splits[0] + splits[2] - 1, splits[1] + splits[2] - 1);
    }
}

record ItemMap(long TargetStart, long SourceStart, long TargetEnd, long SourceEnd);
record Chunk(string From, string To, ItemMap[] Maps);
