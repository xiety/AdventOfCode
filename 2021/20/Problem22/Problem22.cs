#pragma warning disable CS0162 // Unreachable code detected
using System.Text.RegularExpressions;

using Advent.Common;

namespace A2021.Problem22;

public static class Solver
{
    [GeneratedTest<long>(590784, 533863)]
    public static long RunA(string[] lines)
    {
        var items = LoadData(lines);

        return Fors
            .For((-50, 51), (-50, 51), (-50, 51))
            .Select(a => new Pos3(a[0], a[1], a[2]))
            .Count(pos => items.Where(a => a.Rect.Intersects(pos)).Select(a => a.On).FirstOrDefault(false));
    }

    [GeneratedTest<long>(2758514936282235, 1261885414840992)]
    public static long RunB(string[] lines)
    {
        throw new NotImplementedException(); //too slow

        var items = LoadData(lines);

        var all = items.SelectMany(a => new[] { a.Rect.From, new Pos3(a.Rect.To.X + 1, a.Rect.To.Y + 1, a.Rect.To.Z + 1) })
            .Distinct()
            .ToArray();

        var allX = all.Select(a => a.X).Distinct().Order().ToArray();
        var allY = all.Select(a => a.Y).Distinct().Order().ToArray();
        var allZ = all.Select(a => a.Z).Distinct().Order().ToArray();

        var insideAll = items
            .SelectMany(a => Populate(a.Rect))
            .Distinct();

        return insideAll.AsParallel().Sum(@for =>
        {
            var (nx, ny, nz) = @for;

            var from = new Pos3(allX[nx], allY[ny], allZ[nz]);
            var to = new Pos3(allX[nx + 1] - 1, allY[ny + 1] - 1, allZ[nz + 1] - 1);

            var rect = new Rect3(from, to);

            var on = items.Where(a => a.Rect.Intersects(rect))
                .Select(a => a.On)
                .FirstOrDefault(false);

            return on ? rect.Volume : 0;
        });

        IEnumerable<(int, int, int)> Populate(Rect3 rect)
        {
            var nx1 = Array.IndexOf(allX, rect.From.X);
            var nx2 = Array.IndexOf(allX, rect.To.X + 1);

            var ny1 = Array.IndexOf(allY, rect.From.Y);
            var ny2 = Array.IndexOf(allY, rect.To.Y + 1);

            var nz1 = Array.IndexOf(allZ, rect.From.Z);
            var nz2 = Array.IndexOf(allZ, rect.To.Z + 1);

            for (var nx = nx1; nx <= Math.Min(nx2, allX.Length - 2); ++nx)
                for (var ny = ny1; ny <= Math.Min(ny2, allY.Length - 2); ++ny)
                    for (var nz = nz1; nz <= Math.Min(nz2, allZ.Length - 2); ++nz)
                        yield return (nx, ny, nz);
        }
    }

    static Item Convert(ItemRaw r)
        => new(r.Type == "on", new(new(r.FromX, r.FromY, r.FromZ), new(r.ToX, r.ToY, r.ToZ)));

    static Item[] LoadData(string[] lines)
        => CompiledRegs.FromLinesRegex(lines).Select(Convert).Reverse().ToArray();
}

record ItemRaw(string Type, int FromX, int ToX, int FromY, int ToY, int FromZ, int ToZ);
record Item(bool On, Rect3 Rect);

static partial class CompiledRegs
{
    [GeneratedRegex(@$"^(?<{nameof(ItemRaw.Type)}>(on|off)) x=(?<{nameof(ItemRaw.FromX)}>-?\d+)..(?<{nameof(ItemRaw.ToX)}>-?\d+),y=(?<{nameof(ItemRaw.FromY)}>-?\d+)..(?<{nameof(ItemRaw.ToY)}>-?\d+),z=(?<{nameof(ItemRaw.FromZ)}>-?\d+)..(?<{nameof(ItemRaw.ToZ)}>-?\d+)$")]
    [MapTo<ItemRaw>]
    public static partial Regex Regex();
}
