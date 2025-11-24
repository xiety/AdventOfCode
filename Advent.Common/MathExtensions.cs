namespace System;

public static class MathExtensions
{
    extension(Math)
    {
        public static int Mod(int x, int m)
        {
            var r = x % m;
            return r < 0 ? r + m : r;
        }

        public static long Lcm(IEnumerable<int> values)
        {
            var primes = values.Select(Primes);
            var list = new List<int>();

            foreach (var ps in primes)
                list.AddRange(ps.Where(a => !list.Contains(a)));

            return list.MulLong();
        }

        public static IEnumerable<int> Primes(int value)
        {
            var current = value;

            do
            {
                for (var i = 2; i <= current; ++i)
                {
                    if ((current % i) == 0)
                    {
                        yield return i;
                        current /= i;
                    }
                }
            }
            while (current > 1);
        }
    }
}
