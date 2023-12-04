﻿using System.Numerics;

namespace System.Collections;

// Most significant first
public static class BitArrayExtensions
{
    public static byte GetByte(this BitArray @this, Range r)
    {
        var (offset, length) = r.GetOffsetAndLength(@this.Length);
        return @this.GetByte(offset, length);
    }

    public static byte GetByte(this BitArray @this, int offset, int length)
    {
        var result = (byte)0;

        for (var n = 0; n < length; ++n)
        {
            if (@this[n + offset])
                result |= (byte)(1 << (length - n - 1));
        }

        return result;
    }

    public static BigInteger GetBigInteger(this BitArray @this)
        => @this.GetBigInteger(0, @this.Count);

    public static BigInteger GetBigInteger(this BitArray @this, int offset, int length)
    {
        var result = (BigInteger)0;
        var slider = ((BigInteger)1) << (length - 1);

        for (var n = 0; n < length; ++n)
        {
            if (@this[n + offset])
                result |= slider;

            slider >>= 1;
        }

        return result;
    }

    public static BitArray MostSignificantFirst(byte[] array)
    {
        var temp = new BitArray(array);
        var bits = new BitArray(temp.Count);

        for (var i = 0; i < array.Length; ++i)
            for (var b = 0; b < 8; ++b)
                bits[i * 8 + b] = temp[i * 8 + 7 - b];

        return bits;
    }
}
