using System.Numerics;

namespace System;

public static class MathExtensions
{
    extension(Math)
    {
        public static T Mod<T>(T x, T m)
            where T : INumber<T>
        {
            var r = x % m;
            return r < T.Zero ? r + m : r;
        }

        public static T GCD<T>(T a, T b)
            where T : INumber<T>
        {
            while (b != T.Zero)
                (a, b) = (b, a % b);

            return T.Abs(a);
        }

        public static T GCD<T>(params IEnumerable<T> numbers)
            where T : INumber<T>
            => numbers.Aggregate(GCD);

        public static T LCM<T>(T a, T b)
            where T : INumber<T>
        {
            if (a == T.Zero || b == T.Zero)
                return T.Zero;

            return T.Abs(a / GCD(a, b) * b);
        }

        public static T LCM<T>(params IEnumerable<T> numbers)
            where T : INumber<T>
            => numbers.Aggregate(LCM);

        public static T ModPow<T>(T value, T exponent, T modulus)
            where T : INumber<T>, IBitwiseOperators<T, T, T>, IShiftOperators<T, int, T>
        {
            var result = T.One;

            value = Math.Mod(value, modulus);

            while (exponent > T.Zero)
            {
                if ((exponent & T.One) == T.One)
                    result = Math.Mod(result * value, modulus);

                value = Math.Mod(value * value, modulus);

                exponent >>= 1;
            }

            return result;
        }
    }

    extension(Enumerable)
    {
        public static IEnumerable<T> Primes<T>(T maxExcl)
            where T : INumber<T>
        {
            var two = T.One + T.One;

            if (maxExcl <= two)
                yield break;

            yield return two;

            HashSet<T> notPrimes = [];

            for (var i = two + T.One; i < maxExcl; i += two)
            {
                if (!notPrimes.Contains(i))
                {
                    yield return i;

                    for (var j = i + i; j < maxExcl; j += i)
                        notPrimes.Add(j);
                }
            }
        }

        public static IEnumerable<T[]> BinaryCounting<T>(int n)
        {
            var comb = (long)Math.Pow(2, n);

            for (var i = 0L; i < comb; ++i)
            {
                var array = new T[n];

                for (var j = 0; j < n; ++j)
                    array[j] = (T)Convert.ChangeType((i & 1L << j) >> j, typeof(T));

                yield return array;
            }
        }
    }
}
