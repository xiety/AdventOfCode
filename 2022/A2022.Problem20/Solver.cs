using System.Numerics;

using Advent.Common;

namespace A2022.Problem20;

public class Solver : IProblemSolver<long>
{
    public long RunA(string filename)
        => Run(filename, 1, 1);

    public long RunB(string filename)
        => Run(filename, 811589153, 10);

    static long Run(string filename, BigInteger key, int mixes)
    {
        var items = File.ReadAllLines(filename)
            .Select((a, i) => new Item(i, int.Parse(a) * key))
            .ToList();

        var total = items.Count;

        for (var m = 0; m < mixes; ++m)
        {
            for (var i = 0; i < total; ++i)
            {
                var oldPosition = items.FindIndex(a => a.Order == i);
                var item = items[oldPosition];
                var newPosition = Wrap(oldPosition + item.Value, total);

                items.RemoveAt(oldPosition);
                items.Insert(newPosition, item);
            }
        }

        var zeroPos = items.FindIndex(a => a.Value == 0);

        var n1 = WrapPos(zeroPos + 1000, total);
        var n2 = WrapPos(zeroPos + 2000, total);
        var n3 = WrapPos(zeroPos + 3000, total);

        var result = items[n1].Value + items[n2].Value + items[n3].Value;

        return (long)result;
    }

    static int Wrap(BigInteger position, int length)
    {
        var newPosition = Mod(position, length - 1);

        if (newPosition == 0)
            newPosition = length - 1;

        return newPosition;
    }

    static int WrapPos(BigInteger position, int length)
    {
        while (position > length - 1)
            position -= length;

        return (int)position;
    }

    static int Mod(BigInteger n, int d)
    {
        var result = n % d;

        if (result < 0)
            result += d;

        return (int)result;
    }
}

record struct Item(int Order, BigInteger Value);
