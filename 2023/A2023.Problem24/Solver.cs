﻿using System.Text.RegularExpressions;

using Advent.Common;

namespace A2023.Problem24;

public class Solver : IProblemSolver<long>
{
    public long RunA(string filename)
    {
        var lines = CompiledRegs.Regex().FromFile<Line>(filename).ToArray();

        var isSample = Path.GetFileName(filename) == "sample.txt";

        var from = isSample ? 7 : 200000000000000L;
        var to = isSample ? 27 : 400000000000000L;

        return lines
            .EnumeratePairs()
            .Where(x => Check(@from, to, x.Item1, x.Item2))
            .Count();
    }

    static bool Check(long from, long to, Line line1, Line line2)
    {
        var intersect = Intersect2D(line1, line2);

        if (intersect.X >= from && intersect.X <= to
         && intersect.Y >= from && intersect.Y <= to)
        {
            var t1 = (intersect.X - line1.X) / line1.VX;
            var t2 = (intersect.X - line2.X) / line2.VX;

            if (t1 >= 0 && t2 >= 0)
                return true;
        }

        return false;
    }

    static (double X, double Y) Intersect2D(Line line1, Line line2)
    {
        var k1 = line1.VY / (double)line1.VX;
        var b1 = -line1.X * k1 + line1.Y;

        var k2 = line2.VY / (double)line2.VX;
        var b2 = -line2.X * k2 + line2.Y;

        var x = (b2 - b1) / (k1 - k2);
        var y = k1 * x + b1;

        return (x, y);
    }
}

record Line(long X, long Y, long Z, long VX, long VY, long VZ);

static partial class CompiledRegs
{
    [GeneratedRegex(@$"^(?<{nameof(Line.X)}>\d+),\s+(?<{nameof(Line.Y)}>\d+),\s+(?<{nameof(Line.Z)}>\d+)\s+@\s+(?<{nameof(Line.VX)}>-?\d+),\s+(?<{nameof(Line.VY)}>-?\d+),\s+(?<{nameof(Line.VZ)}>-?\d+)$")]
    public static partial Regex Regex();
}
