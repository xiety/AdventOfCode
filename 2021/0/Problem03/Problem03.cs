namespace A2021.Problem03;

public static class Solver
{
    [GeneratedTest<int>(198, 3374136)]
    public static int RunA(string[] lines)
    {
        var length = lines[0].Length;

        var output1 = "";
        var output2 = "";

        foreach (var i in length)
        {
            var numZero = lines.Select(a => a[i]).Count(a => a == '0');
            var numOne = lines.Select(a => a[i]).Count(a => a == '1');

            output1 += numZero >= numOne ? '0' : '1';
            output2 += numZero >= numOne ? '1' : '0';
        }

        var r1 = ToDec(output1);
        var r2 = ToDec(output2);

        return r1 * r2;
    }

    [GeneratedTest<int>(230, 4432698)]
    public static int RunB(string[] lines)
    {
        var length = lines[0].Length;

        var copy1 = lines.ToList();
        var copy2 = lines.ToList();

        foreach (var i in length)
        {
            var numZero1 = copy1.Select(a => a[i]).Count(a => a == '0');
            var numOne1 = copy1.Select(a => a[i]).Count(a => a == '1');

            if (numZero1 > numOne1)
            {
                if (copy1.Count > 1)
                    copy1.RemoveAll(a => a[i] != '0');
            }
            else
            {
                if (copy1.Count > 1)
                    copy1.RemoveAll(a => a[i] != '1');
            }

            var numZero2 = copy2.Select(a => a[i]).Count(a => a == '0');
            var numOne2 = copy2.Select(a => a[i]).Count(a => a == '1');

            if (numZero2 > numOne2)
            {
                if (copy2.Count > 1)
                    copy2.RemoveAll(a => a[i] != '1');
            }
            else
            {
                if (copy2.Count > 1)
                    copy2.RemoveAll(a => a[i] != '0');
            }
        }

        var r1 = ToDec(copy1[0]);
        var r2 = ToDec(copy2[0]);

        return r1 * r2;
    }

    static int ToDec(string n)
    {
        var ret = 0;
        var b = 1;

        for (var i = n.Length - 1; i >= 0; --i)
        {
            if (n[i] == '1')
                ret += b;

            b *= 2;
        }

        return ret;
    }
}
