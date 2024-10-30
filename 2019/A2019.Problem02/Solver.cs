using Advent.Common;

namespace A2019.Problem02;

public class Solver : ISolver<int>
{
    public int RunA(string text, bool isSample)
    {
        var codes = LoadData(text);

        if (!isSample)
        {
            codes[1] = 12;
            codes[2] = 2;
        }

        return Interpret(codes, codes);
    }

    public int RunB(string text, bool isSample)
    {
        var codes = LoadData(text);

        var target = !isSample ? 19690720 : 100;

        for (var a = 0; a <= 99; ++a)
        {
            for (var b = 0; b <= 99; ++b)
            {
                codes[1] = a;
                codes[2] = b;

                var result = Interpret(codes, [.. codes]);

                if (result == target)
                    return a * 100 + b;
            }
        }

        throw new();
    }

    private static int Interpret(int[] codes, int[] memory)
    {
        var position = 0;

        do
        {
            var n = Get(ref codes, position);

            if (n == 1)
                Op(ref codes, ref memory, position, (a,b) => a + b);
            else if (n == 2)
                Op(ref codes, ref memory, position, (a, b) => a * b);
            else if (n == 99)
                break;

            position += 4;
        }
        while (true);

        return Get(ref memory, 0);
    }

    private static void Op(ref int[] codes, ref int[] memory, int position, Func<int, int, int> func)
    {
        var a = Get(ref memory, Get(ref codes, position + 1));
        var b = Get(ref memory, Get(ref codes, position + 2));
        Set(ref memory, Get(ref codes, position + 3), func(a, b));
    }

    static int Get(ref int[] m, int position)
    {
        Resize(ref m, position);
        return m[position];
    }

    static void Set(ref int[] m, int position, int value)
    {
        Resize(ref m, position);
        m[position] = value;
    }

    static void Resize(ref int[] m, int size)
    {
        if (m.Length <= size)
            Array.Resize(ref m, size + 1);
    }

    private static int[] LoadData(string filename)
        => filename.Split(Environment.NewLine).First().Split(",").Select(int.Parse).ToArray();
}
