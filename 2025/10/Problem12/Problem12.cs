using System.Text.RegularExpressions;

using Advent.Common;

namespace A2025.Problem12;

public static class Solver
{
    [GeneratedTest<long>(2, 579)]
    public static long RunA(string[] lines)
    {
        var (shapes, regions) = LoadData(lines);
        return regions.Count(a => Fit(a, shapes));
    }

    static bool Fit(Region region, bool[][,] shapes)
    {
        var target = region.Nums.Sum();
        var patterns = PrecomputePatterns(shapes);
        var required = region.Nums.Select((a, i) => a * Count(shapes[i])).Sum();

        if (required > region.Width * region.Height)
            return false;

        return Recursion(0, new bool[region.Width, region.Height]);

        bool Recursion(int index, bool[,] space)
            => (
                from shape in patterns[Bucket(region.Nums, index)]
                from x in Enumerable.Range(0, region.Width - 2)
                from y in Enumerable.Range(0, region.Height - 2)
                where CanPut(space, shape, x, y)
                select (index + 1 == target || Recursion(index + 1, PutShape(space, shape, x, y)))
            ).Any(a => a);
    }

    static int Bucket(int[] segs, int idx)
        => segs.Accumulate(0, (acc, len) => acc + len)
            .Skip(1)
            .TakeWhile(cum => idx >= cum)
            .Count();

    static int Count(bool[,] map)
        => map.EnumeratePositionsOf(true).Count();

    static bool[][][,] PrecomputePatterns(bool[][,] shapes)
        => shapes.ToArray(shape => Enumerable.Range(0, 4).Accumulate(shape, (a, _) => a.RotatedRight())
            .DistinctBy(a => a.ToDump()).ToArray());

    static bool CanPut(bool[,] space, bool[,] shape, int x, int y)
    {
        for (var tx = 0; tx < shape.Width; ++tx)
            for (var ty = 0; ty < shape.Height; ++ty)
                if (shape[tx, ty] && space[tx + x, ty + y])
                    return false;
        return true;
    }

    static bool[,] PutShape(bool[,] space, bool[,] shape, int x, int y)
    {
        var copy = space.CloneArray();

        for (var tx = 0; tx < shape.Width; ++tx)
            for (var ty = 0; ty < shape.Height; ++ty)
                if (shape[tx, ty])
                    copy[tx + x, ty + y] = true;

        return copy;
    }

    static (bool[][,], Region[]) LoadData(string[] lines)
    {
        var parts = lines.SplitBy("").ToArray();
        var shapes = parts[..^1]
            .ToArray(a => MapData.ParseMap(a[1..], a => a == '#'));
        var regions = CompiledRegs.FromLinesRegionRegex(parts[^1]);
        return (shapes, regions);
    }
}

record Region(int Width, int Height, int[] Nums);

static partial class CompiledRegs
{
    [GeneratedRegex(@$"^(?<{nameof(Region.Width)}>\d+)x(?<{nameof(Region.Height)}>\d+)\:\s((?<{nameof(Region.Nums)}>\d+)\s?)+$")]
    [MapTo<Region>]
    public static partial Regex RegionRegex();
}
