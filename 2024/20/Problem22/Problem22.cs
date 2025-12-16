using Advent.Common;

namespace A2024.Problem22;

public static class Solver
{
    [GeneratedTest<long>(37327623, 12664695565)]
    public static long RunA(string[] lines)
    {
        var items = lines.ToArray(long.Parse);

        for (var j = 0; j < items.Length; ++j)
            for (var i = 0; i < 2000; ++i)
                items[j] = Pseudo(items[j]);

        return items.Sum();
    }

    [GeneratedTest<long>(23, 1444)]
    public static long RunB(string[] lines)
    {
        var items = lines.ToArray(long.Parse).ToArray();
        var lists = new List<Dictionary<string, int>>();

        for (var j = 0; j < items.Length; ++j)
        {
            var dic = new Dictionary<string, int>();

            var queue = new Queue<int>();
            var n = items[j];
            var prev = LastDigit(n);

            for (var i = 0; i < 2000; ++i)
            {
                n = Pseudo(n);
                var m = LastDigit(n);
                var d = m - prev;

                if (queue.Count == 4)
                {
                    queue.Dequeue();
                    queue.Enqueue(d);

                    if (queue.Count == 4)
                    {
                        var key = queue.StringJoin(",");
                        dic.TryAdd(key, m);
                    }
                }
                else
                {
                    queue.Enqueue(d);
                }

                prev = m;
            }

            lists.Add(dic);
        }

        return lists.SelectMany(a => a)
            .GroupBy(a => a.Key)
            .Max(a => a.Sum(b => b.Value));
    }

    static int LastDigit(long n)
        => (int)(n % 10);

    static long Pseudo(long n)
    {
        n = Prune(Mix(n, n * 64));
        n = Prune(Mix(n, n / 32));
        n = Prune(Mix(n, n * 2048));
        return n;
    }

    static long Mix(long a, long b)
        => a ^ b;

    static long Prune(long n)
        => n % 16777216;
}
