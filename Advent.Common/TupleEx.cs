namespace Advent.Common;

public static class TupleEx
{
    public static ValueTuple<T, T> FromArray2<T>(T[] array)
        => (array[0], array[1]);

    public static ValueTuple<T, T, T> FromArray3<T>(T[] array)
        => (array[0], array[1], array[2]);
}
