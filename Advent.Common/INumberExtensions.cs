namespace System.Numerics;

public static class INumberExtensions
{
    public static T GCD<T>(this T a, T b)
        where T : INumber<T>
    {
        if (a < T.Zero)
            a = -a;

        if (b < T.Zero)
            b = -b;

        while (a != T.Zero && b != T.Zero)
        {
            if (a > b)
                a %= b;
            else
                b %= a;
        }

        return a == T.Zero ? b : a;
    }

    public static T LCM<T>(T a, T b)
        where T : INumber<T>
        => T.Abs(a * b) / GCD(a, b);

    public static T LCM<T>(T[] numbers)
        where T : INumber<T>
    {
        var result = numbers[0];

        foreach (var num in numbers)
            result = LCM(result, num);

        return result;
    }
}
