using System.Text;
using System.Text.RegularExpressions;

namespace A2015.Problem08;

public static class Solver
{
    [GeneratedTest<int>(12, 1342)]
    public static int RunA(string[] lines)
        => lines.Sum(a => a.Length - Decode(a).Length);

    [GeneratedTest<int>(12, 1342)]
    public static int RunARegex(string[] lines)
        => lines.Sum(a => a.Length - DecodeRegex(a).Length);

    [GeneratedTest<int>(19, 2074)]
    public static int RunB(string[] lines)
        => lines.Sum(a => Encode(a).Length - a.Length);

    static string Decode(string input)
    {
        var sb = new StringBuilder();

        for (var i = 1; i < input.Length - 1; ++i)
        {
            var (c, advance) = input.AsSpan(i) switch
            {
                ['\\', '\\', ..] => ('\\', 1),
                ['\\', '"', ..] => ('"', 1),
                ['\\', 'x', var hi, var lo, ..] => (EncodeHex([hi, lo]), 3),
                _ => (input[i], 0),
            };

            sb.Append(c);
            i += advance;
        }

        return sb.ToString();
    }

    static string DecodeRegex(string input)
        => CompiledRegs.Escapes().Replace(input[1..^1], match =>
            match.Groups[1].Success
            ? EncodeHex(match.Groups[1].Value).ToString()
            : match.Groups[2].Value);

    static string Encode(string input) =>
        $"\"{input.Replace("\\", "\\\\").Replace("\"", "\\\"")}\"";

    static char EncodeHex(ReadOnlySpan<char> code)
        => (char)int.Parse(code, System.Globalization.NumberStyles.HexNumber);
}

static partial class CompiledRegs
{
    [GeneratedRegex(@"\\(?:x([0-9a-f]{2})|(\\|""))")]
    public static partial Regex Escapes();
}
