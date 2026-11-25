namespace A2015.Problem20;

public static class Solver
{
    [GeneratedTest<int>(8, 831600)]
    public static int RunA(string[] lines)
        => Calc(int.Parse(lines[0]), 10, int.MaxValue);

    [GeneratedTest<int>(8, 831600)]
    public static int RunACheck(string[] lines)
    {
        var target = int.Parse(lines[0]);

        return Enumerable.InfiniteSequence(1, 1)
            .First(a => SumOfDivisors(a) * 10 >= target);
    }

    [GeneratedTest<int>(8, 884520)]
    public static int RunB(string[] lines)
        => Calc(int.Parse(lines[0]), 11, 50);

    static int Calc(int input, int mul, int lim)
    {
        var houses = new int[(input / mul) + 1];
        var result = int.MaxValue;

        foreach (var i in 1..houses.Length)
        {
            for (int j = i, k = 0; j < houses.Length && k < lim; j += i, ++k)
            {
                houses[j] += i * mul;
                if (houses[j] >= input && j < result)
                    result = j;
            }
        }

        return result;
    }

    static int SumOfDivisors(int n)
    {
        var sum = 1;

        Calc(2);

        for (var p = 3; (long)p * p <= n; p += 2)
            Calc(p);

        if (n > 1)
            sum *= (n + 1);

        return sum;

        void Calc(int d)
        {
            if (n % d == 0)
            {
                var q = 1;

                do
                {
                    n /= d;
                    q = (q * d) + 1;
                }
                while (n % d == 0);

                sum *= q;
            }
        }
    }
}
