namespace A2022.Problem25;

public static class SnafuConverter
{
    public static long ToDecimal(string snafu)
    {
        var result = 0L;
        var b = 1L;

        for (var i = 0; i < snafu.Length; ++i)
        {
            var c = snafu[^(i + 1)];
            var cv = digits[c];

            result += b * cv;

            b *= 5;
        }

        return result;
    }

    public static string ToSnafu(long dec)
    {
        var cur = dec;
        var result = "";
        var a = 0L;

        do
        {
            var d = cur / 5;
            var r = cur % 5;

            if (r > 2)
            {
                result = ToSnafuDigit(r - 5) + result;
                a = 1;
            }
            else
            {
                result = ToSnafuDigit(r) + result;
                a = 0;
            }

            cur = d + a;
        }
        while (cur != 0);

        return result;
    }

    static char ToSnafuDigit(long n)
        => digits.First(a => a.Value == n).Key;

    static readonly Dictionary<char, long> digits = new()
    {
        ['='] = -2,
        ['-'] = -1,
        ['0'] = 0,
        ['1'] = +1,
        ['2'] = +2,
    };
}
