using Advent.Common;

namespace A2021.Problem24;

public class Solver : IProblemSolver<long>
{
    public long RunA(string filename)
    {
        var flow = new Flow();
        var flowResult = flow.Run(filename);

        Console.WriteLine(flowResult);

        //Parallel.For(0, 12, (a, state) => RunCpu(a, state));
        //RunCpu(0, 1);

        return -1;
    }

    public long RunCpu(int offset, int cpu)
    {
        var cnt = 0L;

        for (var tp = 9_999_999_999L - offset; tp >= 1_111_111_111L; tp -= cpu)
        {
            NoZero(ref tp);

            var a = tp.ToString();
            var t = long.Parse($"9{a[0]}999{a[1..]}");

            var z = Run(t);

            if (z == 0)
                return t;

            cnt++;
        }

        throw new("Not found");
    }

    private static void NoZero(ref long t)
    {
        var copy = t;
        var g = t;
        var d = 10L;

        if (t % 10 == 0)
        {
            t -= 1;
            g = t / 10;

            for (int w = 2; w <= 14-4; ++w)
            {
                if (g % 10 == 0)
                {
                    t -= d;
                }
                //else
                //    break;

                d *= 10;
                g = t / d;
            }
        }
    }

    static int Run(long input)
    {
        int w = 0, x = 0, y = 0, z = 0;
        var input_index = 10_000_000_000_000L;

        w = (int)(input % input_index);
        input_index /= 10;
        x *= 0;
        x += z;
        if (x < 0 || 26 <= 0)
            return -1;
        x %= 26;
        if (1 == 0)
            return -1;
        z /= 1;
        x += 14;
        x = (x == w) ? 1 : 0;
        x = (x == 0) ? 1 : 0;
        y *= 0;
        y += 25;
        y *= x;
        y += 1;
        z *= y;
        y *= 0;
        y += w;
        y += 12;
        y *= x;
        z += y;
        w = (int)(input % input_index);
        input_index /= 10;
        x *= 0;
        x += z;
        if (x < 0 || 26 <= 0)
            return -1;
        x %= 26;
        if (1 == 0)
            return -1;
        z /= 1;
        x += 11;
        x = (x == w) ? 1 : 0;
        x = (x == 0) ? 1 : 0;
        y *= 0;
        y += 25;
        y *= x;
        y += 1;
        z *= y;
        y *= 0;
        y += w;
        y += 8;
        y *= x;
        z += y;
        w = (int)(input % input_index);
        input_index /= 10;
        x *= 0;
        x += z;
        if (x < 0 || 26 <= 0)
            return -1;
        x %= 26;
        if (1 == 0)
            return -1;
        z /= 1;
        x += 11;
        x = (x == w) ? 1 : 0;
        x = (x == 0) ? 1 : 0;
        y *= 0;
        y += 25;
        y *= x;
        y += 1;
        z *= y;
        y *= 0;
        y += w;
        y += 7;
        y *= x;
        z += y;
        w = (int)(input % input_index);
        input_index /= 10;
        x *= 0;
        x += z;
        if (x < 0 || 26 <= 0)
            return -1;
        x %= 26;
        if (1 == 0)
            return -1;
        z /= 1;
        x += 14;
        x = (x == w) ? 1 : 0;
        x = (x == 0) ? 1 : 0;
        y *= 0;
        y += 25;
        y *= x;
        y += 1;
        z *= y;
        y *= 0;
        y += w;
        y += 4;
        y *= x;
        z += y;
        w = (int)(input % input_index);
        input_index /= 10;
        x *= 0;
        x += z;
        if (x < 0 || 26 <= 0)
            return -1;
        x %= 26;
        if (26 == 0)
            return -1;
        z /= 26;
        x += -11;
        x = (x == w) ? 1 : 0;
        x = (x == 0) ? 1 : 0;
        y *= 0;
        y += 25;
        y *= x;
        y += 1;
        z *= y;
        y *= 0;
        y += w;
        y += 4;
        y *= x;
        z += y;
        w = (int)(input % input_index);
        input_index /= 10;
        x *= 0;
        x += z;
        if (x < 0 || 26 <= 0)
            return -1;
        x %= 26;
        if (1 == 0)
            return -1;
        z /= 1;
        x += 12;
        x = (x == w) ? 1 : 0;
        x = (x == 0) ? 1 : 0;
        y *= 0;
        y += 25;
        y *= x;
        y += 1;
        z *= y;
        y *= 0;
        y += w;
        y += 1;
        y *= x;
        z += y;
        w = (int)(input % input_index);
        input_index /= 10;
        x *= 0;
        x += z;
        if (x < 0 || 26 <= 0)
            return -1;
        x %= 26;
        if (26 == 0)
            return -1;
        z /= 26;
        x += -1;
        x = (x == w) ? 1 : 0;
        x = (x == 0) ? 1 : 0;
        y *= 0;
        y += 25;
        y *= x;
        y += 1;
        z *= y;
        y *= 0;
        y += w;
        y += 10;
        y *= x;
        z += y;
        w = (int)(input % input_index);
        input_index /= 10;
        x *= 0;
        x += z;
        if (x < 0 || 26 <= 0)
            return -1;
        x %= 26;
        if (1 == 0)
            return -1;
        z /= 1;
        x += 10;
        x = (x == w) ? 1 : 0;
        x = (x == 0) ? 1 : 0;
        y *= 0;
        y += 25;
        y *= x;
        y += 1;
        z *= y;
        y *= 0;
        y += w;
        y += 8;
        y *= x;
        z += y;
        w = (int)(input % input_index);
        input_index /= 10;
        x *= 0;
        x += z;
        if (x < 0 || 26 <= 0)
            return -1;
        x %= 26;
        if (26 == 0)
            return -1;
        z /= 26;
        x += -3;
        x = (x == w) ? 1 : 0;
        x = (x == 0) ? 1 : 0;
        y *= 0;
        y += 25;
        y *= x;
        y += 1;
        z *= y;
        y *= 0;
        y += w;
        y += 12;
        y *= x;
        z += y;
        w = (int)(input % input_index);
        input_index /= 10;
        x *= 0;
        x += z;
        if (x < 0 || 26 <= 0)
            return -1;
        x %= 26;
        if (26 == 0)
            return -1;
        z /= 26;
        x += -4;
        x = (x == w) ? 1 : 0;
        x = (x == 0) ? 1 : 0;
        y *= 0;
        y += 25;
        y *= x;
        y += 1;
        z *= y;
        y *= 0;
        y += w;
        y += 10;
        y *= x;
        z += y;
        w = (int)(input % input_index);
        input_index /= 10;
        x *= 0;
        x += z;
        if (x < 0 || 26 <= 0)
            return -1;
        x %= 26;
        if (26 == 0)
            return -1;
        z /= 26;
        x += -13;
        x = (x == w) ? 1 : 0;
        x = (x == 0) ? 1 : 0;
        y *= 0;
        y += 25;
        y *= x;
        y += 1;
        z *= y;
        y *= 0;
        y += w;
        y += 15;
        y *= x;
        z += y;
        w = (int)(input % input_index);
        input_index /= 10;
        x *= 0;
        x += z;
        if (x < 0 || 26 <= 0)
            return -1;
        x %= 26;
        if (26 == 0)
            return -1;
        z /= 26;
        x += -8;
        x = (x == w) ? 1 : 0;
        x = (x == 0) ? 1 : 0;
        y *= 0;
        y += 25;
        y *= x;
        y += 1;
        z *= y;
        y *= 0;
        y += w;
        y += 4;
        y *= x;
        z += y;
        w = (int)(input % input_index);
        input_index /= 10;
        x *= 0;
        x += z;
        if (x < 0 || 26 <= 0)
            return -1;
        x %= 26;
        if (1 == 0)
            return -1;
        z /= 1;
        x += 13;
        x = (x == w) ? 1 : 0;
        x = (x == 0) ? 1 : 0;
        y *= 0;
        y += 25;
        y *= x;
        y += 1;
        z *= y;
        y *= 0;
        y += w;
        y += 10;
        y *= x;
        z += y;
        w = (int)(input % input_index);
        input_index /= 10;
        x *= 0;
        x += z;
        if (x < 0 || 26 <= 0)
            return -1;
        x %= 26;
        if (26 == 0)
            return -1;
        z /= 26;
        x += -11;
        x = (x == w) ? 1 : 0;
        x = (x == 0) ? 1 : 0;
        y *= 0;    //z * (25 * x + 1) = (-9 + w) * x
        y += 25;   //z * ((y + 25) * x + 1) = -(9 + w) * x
        y *= x;    //z * (y * x + 1) = -(9 + w) * x
        y += 1;    //z * (y + 1) = -(9 + w) * x
        z *= y;    //z * y == -(9 + w) * x
        y *= 0;    //z == -(9 + w) * x
        y += w;    //z == -(y + 9 + w) * x
        y += 9;    //z == -(y + 9) * x
        y *= x;    //z == -y * x
        z += y;    //z == -y
        return z;
    }

}
