using Advent.Common;

namespace A2023.Problem05;

public class Solver : IProblemSolver<long>
{
    public long RunA(string filename)
    {
        var parts = File.ReadAllLines(filename).Split(String.Empty).ToArray();
        var seeds = parts.First().First().Split(' ').Skip(1).Select(long.Parse);
        var chunks = parts.Skip(1).Select(ParseChunk).ToArray();
        return seeds.Select(a => RecurseA(chunks, "seed", a)).Min();
    }

    private long RecurseA(Chunk[] chunks, string from, long fromValue)
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

    public long RunB(string filename)
    {
        var parts = File.ReadAllLines(filename).Split(String.Empty).ToArray();
        var seeds = parts.First().First().Split(' ').Skip(1).Select(long.Parse)
            .Pairs(false).Select(a => (a.Item1, a.Item1 + a.Item2 - 1)).ToArray();
        var chunks = parts.Skip(1).Select(ParseChunk).ToArray();

        return seeds.Select(a => RecurseB(chunks, "seed", a.Item1, a.Item2)).Min();
    }

    private long RecurseB(Chunk[] chunks, string from, long fromStart, long fromEnd)
    {
        if (from == "location")
            return fromStart;

        var chunk = chunks.Single(a => a.From == from);

        var points = chunk.Maps.Select(a => a.SourceStart)
            .Concat(chunk.Maps.Select(a => a.SourceEnd))
            .Distinct().Order().ToArray();

        var parts = ToParts(fromStart, fromEnd, points);

        var minResult = parts
            .Select(a => CalculateB(chunks, chunk, a.Item1, a.Item2))
            .Min();

        return minResult;
    }

    private long CalculateB(Chunk[] chunks, Chunk chunk, long partStart, long partEnd)
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

    private (long, long)[] ToParts(long fromStart, long fromEnd, long[] points)
    {
        if (fromStart == fromEnd)
            return [(fromStart, fromEnd)];

        long[] temp = [fromStart,
            .. points.Where(a => fromStart <= a && fromEnd >= a),
            fromEnd];

        var pointsInside = temp.Distinct().Order().ToArray();

        return pointsInside.Pairs(true).ToArray();
    }

    private Chunk ParseChunk(IEnumerable<string> lines)
    {
        var first = lines.First();

        var n1 = first.IndexOf('-');
        var n2 = first.LastIndexOf(' ');

        var from = first[..n1];
        var to = first[(n1 + 4)..n2];

        var maps = lines.Skip(1).Select(ParseMap).ToArray();

        return new Chunk(from, to, maps);
    }

    private static ItemMap ParseMap(string a)
    {
        var splits = a.Split(' ').Select(long.Parse).ToArray();
        return new(splits[0], splits[1], splits[0] + splits[2] - 1, splits[1] + splits[2] - 1);
    }
}

record ItemMap(long TargetStart, long SourceStart, long TargetEnd, long SourceEnd);
record Chunk(string From, string To, ItemMap[] Maps);
record ResultMap(long Start, long Length, long Value);
