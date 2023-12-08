using System.Collections;
using System.Numerics;

namespace A2021.Problem16;

public sealed class BitReader(byte[] array)
{
    private readonly BitArray bits = BitArrayExtensions.MostSignificantFirst(array);

    //mutable
    private int currentOffest;

    public int CurrentOffset => currentOffest;

    public byte ReadToByte(int length)
    {
        var result = bits.GetByte(currentOffest, length);
        currentOffest += length;
        return result;
    }

    public bool ReadToBool()
    {
        var result = bits.Get(currentOffest);
        currentOffest++;
        return result;
    }

    public void Skip(int length)
    {
        currentOffest += length;
    }

    public BigInteger ReadToBigInteger(int length)
    {
        var result = bits.GetBigInteger(currentOffest, length);
        currentOffest += length;
        return result;
    }
}
