using System.Text.RegularExpressions;

using Advent.Common;
namespace A2021.Problem19;

public static class Solver
{
    [GeneratedTest<long>(79, 408)]
    public static long RunA(string[] lines)
    {
        var scanners = LoadData(lines);

        var dic = CalcDic(scanners);

        var globalBeams = scanners[0].Beams.ToHashSet();

        foreach (var a in 1..scanners.Length)
        {
            var scanner = scanners[a];
            var transform = dic[a];

            foreach (var beam in scanner.Beams)
            {
                var rotated = ApplyTransformPos(beam, transform);
                globalBeams.Add(rotated);
            }
        }

        return globalBeams.Count;
    }

    [GeneratedTest<long>(3621, 13348)]
    public static long RunB(string[] lines)
    {
        var scanners = LoadData(lines);

        var dic = CalcDic(scanners);

        var max = int.MinValue;

        foreach (var a in (scanners.Length - 1))
        {
            foreach (var b in (a + 1)..scanners.Length)
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

    static Dictionary<int, Transform> CalcDic(Scanner[] scanners)
    {
        var dic = new Dictionary<int, Transform>
        {
            [0] = new(new(0, 0, 0), new(new(Axis.X, 1), new(Axis.Y, 1), new(Axis.Z, 1))),
        };

        var unknown = false;

        do
        {
            unknown = false;

            foreach (var sa in (scanners.Length - 1))
            {
                foreach (var sb in (sa + 1)..scanners.Length)
                {
                    var scannerA = scanners[sa];
                    var scannerB = scanners[sb];

                    foreach (var i in scannerA.Beams.Length)
                    {
                        var distancesI = CalcDistances(scannerA.Beams, i);

                        foreach (var j in scannerB.Beams.Length)
                        {
                            var distancesJ = CalcDistances(scannerB.Beams, j);

                            var intersection = distancesI
                                .IntersectBy(distancesJ.Select(a => a.distance), a => a.distance)
                                .ToArray();

                            if (intersection.Length > 11)
                                throw new();

                            if (intersection.Length == 11)
                            {
                                if (intersection.Distinct().Count() != 11)
                                    throw new();

                                var sameA1 = scannerA.Beams[i];
                                var sameB1 = scannerB.Beams[j];

                                var intersected1 = intersection[0];

                                var sameA2 = scannerA.Beams[intersected1.index];
                                var sameB2 = scannerB.Beams[distancesJ.First(a => a.distance == intersected1.distance).index];

                                var da = sameA1 - sameA2;
                                var db = sameB1 - sameB2;

                                if (dic.TryGetValue(sa, out var transformForParent))
                                {
                                    var convert = CalcAxisConvert(da, db);
                                    var sameB1Rotated = ApplyConvertPos(sameB1, convert);

                                    var transform = new Transform(sameA1 - sameB1Rotated, convert);

                                    var test = ApplyTransformPos(sameB1, transform);

                                    if (test != sameA1)
                                        throw new();

                                    transform = ApplyTransform(transform, transformForParent);

                                    dic.TryAdd(sb, transform);

                                    goto labelOut;
                                }
                                else if (dic.TryGetValue(sb, out var transformForParent2))
                                {
                                    var convert = CalcAxisConvert(db, da);
                                    var sameA1Rotated = ApplyConvertPos(sameA1, convert);

                                    var transform = new Transform(sameB1 - sameA1Rotated, convert);

                                    var test = ApplyTransformPos(sameA1, transform);

                                    if (test != sameB1)
                                        throw new();

                                    transform = ApplyTransform(transform, transformForParent2);

                                    dic.TryAdd(sa, transform);

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

    static Pos3 ApplyTransformPos(Pos3 v, Transform transform)
    {
        var rotated = ApplyConvertPos(v, transform.Rotate);
        return rotated + transform.Translate;
    }

    static Transform ApplyTransform(Transform convert, Transform parent)
    {
        var pX = convert.Rotate.Get(parent.Rotate.AxisX.Axis);
        var axisX = pX with { Direction = pX.Direction * parent.Rotate.AxisX.Direction };

        var pY = convert.Rotate.Get(parent.Rotate.AxisY.Axis);
        var axisY = pY with { Direction = pY.Direction * parent.Rotate.AxisY.Direction };

        var pZ = convert.Rotate.Get(parent.Rotate.AxisZ.Axis);
        var axisZ = pZ with { Direction = pZ.Direction * parent.Rotate.AxisZ.Direction };

        var rotate = ApplyConvertPos(convert.Translate, parent.Rotate);

        return new(parent.Translate + rotate, new(axisX, axisY, axisZ));
    }

    static Pos3 ApplyConvertPos(Pos3 v, AxisConvert parent)
    {
        var x = Get(parent.AxisX);
        var y = Get(parent.AxisY);
        var z = Get(parent.AxisZ);

        return new(x, y, z);

        int Get(Orientation o)
            => o.Axis switch
            {
                Axis.X => v.X * o.Direction,
                Axis.Y => v.Y * o.Direction,
                Axis.Z => v.Z * o.Direction,
            };
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

        throw new();
    }

    static (int index, double distance)[] CalcDistances(Pos3[] beams, int n)
        => beams.Select((item, index) => (item, index))
                .Where(a => a.index != n)
                .ToArray(a => (a.index, CalcDistance(a.item, beams[n])));

    static double CalcDistance(Pos3 a, Pos3 b)
        => (a.X - b.X) * (a.X - b.X) + (a.Y - b.Y) * (a.Y - b.Y) + (a.Z - b.Z) * (a.Z - b.Z);

    static Scanner[] LoadData(string[] lines)
    {
        var chunks = lines.SplitBy(String.Empty);
        return chunks.ToArray(a => new Scanner([.. a.Skip(1).Select(Pos3.Parse)]));
    }
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

enum Axis { X, Y, Z }
