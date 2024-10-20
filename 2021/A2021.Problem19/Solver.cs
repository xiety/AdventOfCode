using System.Text.RegularExpressions;

using Advent.Common;

namespace A2021.Problem19;

public class Solver : IProblemSolver<long>
{
    public long RunA(string filename)
    {
        var scanners = LoadFile(filename);

        var dic = CalcDic(scanners);

        var globalBeams = scanners[0].Beams.ToHashSet();

        for (var a = 1; a < scanners.Length; ++a)
        {
            var scanner = scanners[a];
            var transform = dic[a];

            for (var i = 0; i < scanner.Beams.Length; ++i)
            {
                var beam = scanner.Beams[i];
                var rotated = ApplyTransformPos(beam, transform);

                globalBeams.Add(rotated);
            }
        }

        return globalBeams.Count;
    }

    public long RunB(string filename)
    {
        var scanners = LoadFile(filename);

        var dic = CalcDic(scanners);

        var max = Int32.MinValue;

        for (var a = 0; a < scanners.Length - 1; ++a)
        {
            for (var b = a + 1; b < scanners.Length; ++b)
            {
                var ta = dic[a].Translate;
                var tb = dic[b].Translate;

                var dist = Math.Abs(ta.X - tb.X)
                    + Math.Abs(ta.Y - tb.Y)
                    + Math.Abs(ta.Z - tb.Z);

                if (dist > max)
                    max = dist;
            }
        }

        return max;
    }

    private static Dictionary<int, Transform> CalcDic(Scanner[] scanners)
    {
        var dic = new Dictionary<int, Transform>
        {
            [0] = new(new(0, 0, 0), new(new(Axis.X, 1), new(Axis.Y, 1), new(Axis.Z, 1))),
        };

        var unknown = false;

        do
        {
            unknown = false;

            for (var a = 0; a < scanners.Length - 1; ++a)
            {
                for (var b = a + 1; b < scanners.Length; ++b)
                {
                    var scannerA = scanners[a];
                    var scannerB = scanners[b];

                    for (var i = 0; i < scannerA.Beams.Length; ++i)
                    {
                        var distancesI = CalcDistances(scannerA.Beams, i);

                        for (var j = 0; j < scannerB.Beams.Length; ++j)
                        {
                            var distancesJ = CalcDistances(scannerB.Beams, j);

                            var intersection = distancesI
                                .IntersectBy(distancesJ.Select(a => a.distance), a => a.distance)
                                .ToArray();

                            if (intersection.Length > 11)
                                throw new Exception("WE HAVE A PROBLEM");

                            if (intersection.Length == 11)
                            {
                                Console.WriteLine($"BEAM a={a} b={b} i={i} j={j} = {intersection.Length}");

                                if (intersection.Distinct().Count() != 11)
                                    throw new Exception("DISTINCT PROBLEM");

                                var sameA1 = scannerA.Beams[i];
                                var sameB1 = scannerB.Beams[j];

                                var intersected1 = intersection[0];

                                var sameA2 = scannerA.Beams[intersected1.index];
                                var sameB2 = scannerB.Beams[distancesJ.First(a => a.distance == intersected1.distance).index];

                                var da = sameA1 - sameA2;
                                var db = sameB1 - sameB2;

                                if (dic.TryGetValue(a, out var transformForParent))
                                {
                                    var convert = CalcAxisConvert(da, db);
                                    var sameB1Rotated = ApplyConvertPos(sameB1, convert);

                                    var transform = new Transform(sameA1 - sameB1Rotated, convert);

                                    var test = ApplyTransformPos(sameB1, transform);

                                    if (test != sameA1)
                                        throw new Exception();

                                    transform = ApplyTransform(transform, transformForParent);

                                    dic.TryAdd(b, transform);

                                    goto labelOut;
                                }
                                else if (dic.TryGetValue(b, out var transformForParent2))
                                {
                                    var convert = CalcAxisConvert(db, da);
                                    var sameA1Rotated = ApplyConvertPos(sameA1, convert);

                                    var transform = new Transform(sameB1 - sameA1Rotated, convert);

                                    var test = ApplyTransformPos(sameA1, transform);

                                    if (test != sameB1)
                                        throw new Exception();

                                    transform = ApplyTransform(transform, transformForParent2);

                                    dic.TryAdd(a, transform);

                                    goto labelOut;
                                }
                                else
                                {
                                    unknown = true;
                                }
                            }
                        }
                    }

                labelOut:
                    ;
                }
            }
        }
        while (unknown);

        return dic;
    }

    private static Scanner[] LoadFile(string filename)
    {
        var chunks = File.ReadAllLines(filename).Split(String.Empty);
        var scanners = chunks.Select(a => new Scanner(a.Skip(1).Select(CompiledRegs.Parse).ToArray())).ToArray();
        return scanners;
    }

    static Pos3 ApplyTransformPos(Pos3 v, Transform transform)
    {
        var rotated = ApplyConvertPos(v, transform.Rotate);
        return rotated + transform.Translate;
    }

    static Transform ApplyTransform(Transform convert, Transform parent)
    {
        var pX = convert.Rotate.Get(parent.Rotate.AxisX.Axis);
        var axisX = new Orientation(pX.Axis, pX.Direction * parent.Rotate.AxisX.Direction);

        var pY = convert.Rotate.Get(parent.Rotate.AxisY.Axis);
        var axisY = new Orientation(pY.Axis, pY.Direction * parent.Rotate.AxisY.Direction);

        var pZ = convert.Rotate.Get(parent.Rotate.AxisZ.Axis);
        var axisZ = new Orientation(pZ.Axis, pZ.Direction * parent.Rotate.AxisZ.Direction);

        var rotate = ApplyConvertPos(convert.Translate, parent.Rotate);

        return new(parent.Translate + rotate, new(axisX, axisY, axisZ));
    }

    static Pos3 ApplyConvertPos(Pos3 v, AxisConvert parent)
    {
        int Get(Orientation o)
            => o.Axis switch
            {
                Axis.X => v.X * o.Direction,
                Axis.Y => v.Y * o.Direction,
                Axis.Z => v.Z * o.Direction,
            };

        var x = Get(parent.AxisX);
        var y = Get(parent.AxisY);
        var z = Get(parent.AxisZ);

        return new(x, y, z);
    }

    static AxisConvert CalcAxisConvert(Pos3 a, Pos3 b)
        => new(FindOrientation(a.X, b), FindOrientation(a.Y, b), FindOrientation(a.Z, b));

    static Orientation FindOrientation(double dist, Pos3 v)
    {
        if (v.X == dist)
            return new(Axis.X, 1);

        if (v.X == -dist)
            return new(Axis.X, -1);

        if (v.Y == dist)
            return new(Axis.Y, 1);

        if (v.Y == -dist)
            return new(Axis.Y, -1);

        if (v.Z == dist)
            return new(Axis.Z, 1);

        if (v.Z == -dist)
            return new(Axis.Z, -1);

        throw new Exception("No axis");
    }

    static (int index, double distance)[] CalcDistances(Pos3[] beams, int n)
        => beams
        .Select((item, index) => (item, index))
        .Where(a => a.index != n)
        .Select(a => (a.index, CalcDistance(a.item, beams[n])))
        .ToArray();

    static double CalcDistance(Pos3 a, Pos3 b)
        => (a.X - b.X) * (a.X - b.X) + (a.Y - b.Y) * (a.Y - b.Y) + (a.Z - b.Z) * (a.Z - b.Z);
}

record Scanner(Pos3[] Beams);
readonly record struct Orientation(Axis Axis, int Direction);
readonly record struct Transform(Pos3 Translate, AxisConvert Rotate);
readonly record struct AxisConvert(Orientation AxisX, Orientation AxisY, Orientation AxisZ)
{
    public Orientation Get(Axis axis)
        => axis switch
        {
            Axis.X => AxisX,
            Axis.Y => AxisY,
            Axis.Z => AxisZ,
        };
}

enum Axis { X, Y, Z };

static partial class CompiledRegs
{
    [GeneratedRegex(@$"^(?<{nameof(Pos3.X)}>-?\d+),(?<{nameof(Pos3.Y)}>-?\d+),(?<{nameof(Pos3.Z)}>-?\d+)$")]
    private static partial Regex MapRegEx();

    public static Pos3 Parse(string text)
        => MapRegEx().MapTo<Pos3>(text);
}
