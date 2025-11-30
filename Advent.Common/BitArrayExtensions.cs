using System.Numerics;

namespace System.Collections;

// Most significant first
public static class BitArrayExtensions
{
    extension(BitArray @this)
    {
        public byte GetByte(Range r)
        {
            var (offset, length) = r.GetOffsetAndLength(@this.Length);
            return @this.GetByte(offset, length);
        }

        public byte GetByte(int offset, int length)
        {
            var result = (byte)0;

            for (var n = 0; n < length; ++n)
            {
                if (@this[n + offset])
                    result |= (byte)(1 << (length - n - 1));
            }

            return result;
        }

        public BigInteger GetBigInteger()
            => @this.GetBigInteger(0, @this.Count);

        public BigInteger GetBigInteger(int offset, int length)
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

        public ulong ToUInt64()
        {
            var result = (ulong)0;
            var slider = (uint)(1) << @this.Length;

            for (var n = 0; n < @this.Length; ++n)
            {
                if (@this[n])
                    result |= slider;

                slider >>= 1;
            }

            return result;
        }

        //public byte[] ToBytes()
        //{
        //    var numBytes = (@this.Length + 7) / 8;
        //    var bytes = new byte[numBytes];

        //    for (var i = 0; i < @this.Length; i++)
        //    {
        //        if (@this[i])
        //            bytes[i / 8] |= (byte)(1 << (i % 8));
        //    }

        //    return bytes;
        //}

        public byte[] ToBytes()
        {
            var bytes = new byte[(@this.Length + 7) / 8];
            @this.CopyTo(bytes, 0);
            return bytes;
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
}
