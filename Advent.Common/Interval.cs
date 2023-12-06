﻿using System.Numerics;

namespace Advent.Common;

public static class Interval
{
    public static bool IsIntersect<T>(T fromA, T toA, T fromB, T toB)
        where T : INumber<T>
        => !((fromA > toB) || (toA < fromB));

    public static (T, T) Intersect<T>(T fromA, T toA, T fromB, T toB)
        where T : INumber<T>
    {
        if (fromA >= fromB && toA <= toB)
            return (fromA, toA);
        else if (fromA <= fromB && toA >= toB)
            return (fromB, toB);
        else if (fromA <= fromB && toA <= toB)
            return (fromB, toA);
        else if (fromA >= fromB && toA >= toB)
            return (fromA, toB);

        return (-T.One, -T.One);
    }
}
