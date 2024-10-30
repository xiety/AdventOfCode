namespace Advent.Common;

public static class StringExtensions
{
    public static string TrimLength(this string text, int n)
    {
        if (text.Length <= n)
            return text;

        const string ellipsis = "...";

        text = text[0..(n - ellipsis.Length)];

        return text + ellipsis;
    }
}
