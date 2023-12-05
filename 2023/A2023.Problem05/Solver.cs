using System.Text.RegularExpressions;

using Advent.Common;

namespace A2023.Problem05;

public class Solver : IProblemSolver<long>
{
    public long RunA(string filename)
    {
        var chunks = File.ReadAllLines(filename).Split(String.Empty).Select(a => ParseChunk([..a])).ToArray();

        var seeds = chunks.Single(a => a.From == "");

        return seeds.Maps.Select(a => Recurse(chunks, "seed", a.TargetStart)).Min();
    }

    private long Recurse(Chunk[] chunks, string from, long fromValue)
    {
        if (from == "location")
            return fromValue;

        var chunk = chunks.Single(a => a.From == from);

        var target = chunk.Maps
            .FirstOrDefault(a => a.SourceStart < fromValue && a.SourceStart + a.Length >= fromValue);

        var result = fromValue;

        if (target is not null)
        {
            result = target.TargetStart + (fromValue - target.SourceStart);
        }

        return Recurse(chunks, chunk.To, result);
    }

    private Chunk ParseChunk(string[] lines)
    {
        var first = lines[0];

        if (first.StartsWith("seeds: "))
        {
            var splits = first.Split(' ').Skip(1).Select(long.Parse).ToArray();
            return new Chunk("", "seed", [..splits.Select(a => new ItemMap(a, a, 1))]);
        }
        else
        {
            var n1 = first.IndexOf('-');
            var n2 = first.LastIndexOf(' ');

            var from = first[..n1];
            var to = first[(n1 + 4)..n2];

            var maps = lines.Skip(1).Select(ParseMap).ToArray();

            return new Chunk(from, to, maps);
        }
    }

    private static ItemMap ParseMap(string a)
    {
        var splits = a.Split(' ').Select(long.Parse).ToArray();
        return new ItemMap(splits[0], splits[1], splits[2]);
    }
}

record ItemMap(long TargetStart, long SourceStart, long Length);
record Chunk(string From, string To, ItemMap[] Maps);
