#pragma warning disable CA5351 // Do Not Use Broken Cryptographic Algorithms

using System.Text;

namespace A2015.Problem04;

public static class Solver
{
    [GeneratedTest<int>(609043 + 1048970, 117946)]
    public static int RunA(string[] lines)
        => lines.Sum(a => Calc(a, "00000"));

    [GeneratedTest<int>(-1, 3938038)]
    public static int RunB(string[] lines, bool isSample)
        => isSample ? -1 : lines.Sum(a => Calc(a, "000000"));

    static int Calc(string text, string start)
    {
        var n = 1;

        do
        {
            var input = text + n.ToString();
            var h = CalcMD5(input);

            if (h.StartsWith(start))
                break;

            n++;
        }
        while (true);

        return n;
    }

    static string CalcMD5(string input)
    {
        var inputBytes = Encoding.UTF8.GetBytes(input);
        var hashBytes = System.Security.Cryptography.MD5.HashData(inputBytes);
        return String.Concat(hashBytes.Select(a => a.ToString("x2")));
    }
}
