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
}
