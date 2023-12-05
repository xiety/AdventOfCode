using System.Text.RegularExpressions;
using System.Xml;

using Advent.Common;

namespace A2023.Problem05;

public class Solver : IProblemSolver<long>
{
    public long RunA(string filename)
    {
        var lines = File.ReadAllLines(filename).Split(String.Empty).ToArray();
        var seeds = lines.First().First().Split(' ').Skip(1).Select(long.Parse);
        var chunks = lines.Skip(1).Select(a => ParseChunk([.. a])).ToArray();
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
        {
            result = target.TargetStart + (fromValue - target.SourceStart);
        }

        return RecurseA(chunks, chunk.To, result);
    }

    public long RunB(string filename)
    {
        var lines = File.ReadAllLines(filename).Split(String.Empty).ToArray();
        var seeds = lines.First().First().Split(' ').Skip(1).Select(long.Parse).Pairs(false).ToArray();
        var chunks = lines.Skip(1).Select(a => ParseChunk([.. a])).ToArray();

        return seeds.Select(a => RecurseB(chunks, "seed", a.Item1, a.Item2)).Min();
    }

    private long RecurseB(Chunk[] chunks, string from, long fromStart, long fromLength)
    {
        var fromEnd = fromStart + fromLength - 1;

        //Console.WriteLine($"{from} {fromStart} ({fromLength}) {fromEnd}");

        if (from == "location")
        {
            Console.WriteLine(fromStart);
            return fromStart;
        }

        var chunk = chunks.Single(a => a.From == from);

        var points = chunk.Maps.Select(a => a.SourceStart)
            .Concat(chunk.Maps.Select(a => a.SourceEnd))
            .Distinct().Order().ToArray();

        var parts = ToParts(fromStart, fromLength, points);

        var minResult = long.MaxValue;

        foreach (var part in parts)
        {
            var partEnd = part.Item1 + part.Item2 - 1;

            var target = chunk.Maps
                .SingleOrDefault(a => Interval.IsIntersect(a.SourceStart, a.SourceEnd, part.Item1, partEnd));

            var result = minResult;

            if (target is not null)
            {
                //fully outside
                if (target.SourceStart <= part.Item1 && target.SourceEnd >= partEnd)
                {
                    var d = part.Item1 - target.SourceStart;
                    result = RecurseB(chunks, chunk.To, target.TargetStart + d, part.Item2);
                }
                //fully inside
                else if (part.Item1 <= target.SourceStart && partEnd >= target.SourceEnd)
                {
                    result = RecurseB(chunks, chunk.To, target.TargetStart, target.Length);
                }
                //partly start
                else if (part.Item1 <= target.SourceStart && partEnd <= target.SourceEnd)
                {
                    result = RecurseB(chunks, chunk.To, target.TargetStart, target.Length - (target.SourceEnd - partEnd));
                }
                //partly end
                else
                {
                    var d = (part.Item1 - target.SourceStart);
                    result = RecurseB(chunks, chunk.To, target.TargetStart + d, target.Length - d);
                }
            }
            else
            {
                result = RecurseB(chunks, chunk.To, part.Item1, part.Item2);
            }

            if (result < minResult)
                minResult = result;
        }

        return minResult;
    }

    private (long, long)[] ToParts(long fromStart, long fromLength, long[] points)
    {
        if (fromLength == 1)
            return [(fromStart, fromLength)];

        long[] temp = [fromStart,
            .. points.Where(a => fromStart <= a && fromStart + fromLength - 1 >= a),
            (fromStart + fromLength - 1)];

        var pointsInside = temp.Distinct().Order().ToArray();

        return pointsInside.Pairs(true).Select(a => (a.Item1, a.Item2 - a.Item1)).ToArray();
    }

    private Chunk ParseChunk(string[] lines)
    {
        var first = lines[0];

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
        return new ItemMap(splits[0], splits[1], splits[2]);
    }
}

record ItemMap(long TargetStart, long SourceStart, long Length)
{
    public long TargetEnd => TargetStart + Length - 1;
    public long SourceEnd => SourceStart + Length - 1;
}

record Chunk(string From, string To, ItemMap[] Maps);
record ResultMap(long Start, long Length, long Value);
