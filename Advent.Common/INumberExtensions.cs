namespace System.Numerics;

public static class INumberExtensions
{
    extension<T>(T a)
        where T : INumber<T>
    {
        public T GCD(T b)
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

        public T LCM(T b)
            => T.Abs(a * b) / GCD(a, b);
    }

    public static T LCM<T>(params T[] numbers)
        where T : INumber<T>
    {
        var result = numbers[0];

        foreach (var num in numbers)
            result = LCM(result, num);

        return result;
    }
}
