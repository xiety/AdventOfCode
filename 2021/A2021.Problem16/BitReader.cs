using System.Collections;
using System.Numerics;

namespace A2021.Problem16;

public sealed class BitReader(byte[] array)
{
    readonly BitArray bits = BitArrayExtensions.MostSignificantFirst(array);

    //mutable

    public int CurrentOffset { get; private set; }

    public byte ReadToByte(int length)
    {
        var result = bits.GetByte(CurrentOffset, length);
        CurrentOffset += length;
        return result;
    }

    public bool ReadToBool()
    {
        var result = bits.Get(CurrentOffset);
        CurrentOffset++;
        return result;
    }

    public void Skip(int length)
    {
        CurrentOffset += length;
    }

    public BigInteger ReadToBigInteger(int length)
    {
        var result = bits.GetBigInteger(CurrentOffset, length);
        CurrentOffset += length;
        return result;
    }
}
