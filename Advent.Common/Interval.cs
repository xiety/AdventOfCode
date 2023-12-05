using System.Numerics;

namespace Advent.Common;

public static class Interval
{
    public static bool IsIntersect<T>(T fromA, T toA, T fromB, T toB)
        where T : INumber<T>
        => !((fromA > toB) || (toA < fromB));
}
